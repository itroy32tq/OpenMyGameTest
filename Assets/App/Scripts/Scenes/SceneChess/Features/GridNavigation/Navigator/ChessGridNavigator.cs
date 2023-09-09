using App.Scripts.Scenes.SceneChess.Features.ChessField.GridMatrix;
using App.Scripts.Scenes.SceneChess.Features.ChessField.Piece;
using App.Scripts.Scenes.SceneChess.Features.ChessField.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace App.Scripts.Scenes.SceneChess.Features.GridNavigation.Navigator
{
    public class ChessGridNavigator : IChessGridNavigator, IChainBilder
    {
        private Figure figure;
        //todo за 500 итерация справляется, но нужно оптимизировать алгоритм и добавить защиту от обратного хода
        //сократил с 512 до 380, думаю достаточно done
        private int countIteration = 100000;
        public List<LinkedList<Vector2Int>> ChainContener { get; set; }

        public Vector2Int FinalPosition { get; set; }

        public LinkedList<Vector2Int> FinalChain { get; set; }

        public ChessGrid Grid { get; set; }

        public IEnumerable<ChessUnit>  GridFigure {get; set;}

        public List<Vector2Int> FindPath(ChessUnitType unit, Vector2Int from, Vector2Int to, ChessGrid grid)
        {
            //напиши реализацию не меняя сигнатуру функции
            FinalPosition = to;
            
            Grid = grid;

            GridFigure = Grid.Pieces;

            figure = DetectFigure(unit);

            //создаем хранилище для цепочек
            InitChainContener(from);

            var chaine = ChainContener.FirstOrDefault();
            int i = 0;

            while (i <= countIteration)
            {
                FinalChain = AddUnitToChain(chaine);
                if (FinalChain != null) return FinalChain.ToList();
                chaine = ChainContener.FirstOrDefault();
                i++;
            }
            return null;
        }

        public int ValidateTargetPos(Vector2Int pos, LinkedList<Vector2Int> chain)
        {

            //если позиция за пределами экрана
            if (pos.x < 0 || pos.x > 7 || pos.y < 0 || pos.y > 7) return -1;

            //если на пути другая фигура (кроме коня)
            if (figure.Name != ChessUnitType.Knight && IsCrossingPath(pos, chain)) return -2;

            //если на позиции другая фигура
            if (Grid.Get(pos) != null) return -3;

            //если это "обратый" ход
            if (chain.Last.Previous != null && chain.Last.Previous.Value == pos) return  -4;

            //если целевая позиция найдена и мир спасен
            if (pos == FinalPosition) return 1;

            return 0;
        }

        private bool IsCrossingPath(Vector2Int pos, LinkedList<Vector2Int> chain)
        {
            Vector2Int vectorPath = chain.Last.Value - pos;
            float pathLen = vectorPath.magnitude;
            
            foreach (var f in GridFigure)
            {
                //проверка на самого себя
                if (f.CellPosition == chain.First.Value) continue;
                //todo возможно есть способ лучше, но с ходу я не придумал, точка лежит на прямой, если разбивает отрезки
                var vector1 = (pos - f.CellPosition).magnitude;
                var vector2 = (f.CellPosition - chain.Last.Value).magnitude;
                if (Mathf.Approximately(pathLen, vector2 + vector1))  return true;
            }
            return false;
        }
        public void InitChainContener(Vector2Int pos)
        {
            ChainContener = new()
            {
                new LinkedList<Vector2Int>(new List<Vector2Int>() { pos })
            };

        }
        public LinkedList<Vector2Int> AddUnitToChain(LinkedList<Vector2Int> chain)
        {
            figure.Moves = figure.GetAviableMoves(chain.Last.Value);
            
            for (int i = 0; i < figure.MovesCount; i++)
            {
                //возможно это костыль и можно лучше, но пока оставлю так
                LinkedList<Vector2Int> original_chain = new LinkedList<Vector2Int>(chain);

                var unit = original_chain.Last.Value + figure.Moves[i];

                var res = ValidateTargetPos(unit ,chain);

                switch (res)
                {
                    case 0:
                        original_chain.AddLast(unit);
                        ChainContener.Add(original_chain);
                        break;
                    case 1:
                        original_chain.AddLast(unit);
                        return original_chain;
                }
            }
            ChainContener.RemoveAt(0);
            return null;

        }

        private Figure DetectFigure(ChessUnitType unit)
        {
            switch (unit)
            {
                case ChessUnitType.Pon:
                    return new Pon(unit);
                case ChessUnitType.Knight:
                    return new Knight(unit);
                case ChessUnitType.Rook:
                    return new Rook(unit);
                case ChessUnitType.Bishop:
                    return new Bishop(unit);
                case ChessUnitType.Queen:
                    return new Bishop(unit);
            }
            return null;
        }

    }
}