using App.Scripts.Scenes.SceneChess.Features.ChessField.GridMatrix;
using App.Scripts.Scenes.SceneChess.Features.ChessField.Piece;
using App.Scripts.Scenes.SceneChess.Features.ChessField.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace App.Scripts.Scenes.SceneChess.Features.GridNavigation.Navigator
{
    public class ChessGridNavigator : IChessGridNavigator, IChainMaker
    {
        private Figure figure;

        //большинство фигур справляется быстро до 500 итераций, однако для короля с проходом через все поле понадобилось
        //порядка 8000 итераций
        private readonly int countIteration = 2048*4;
        public List<LinkedList<Vector2Int>> ChainContener { get; set; }

        public Vector2Int FinalPosition { get; set; }

        public LinkedList<Vector2Int> FinalChain { get; set; }

        public ChessGrid Grid { get; set; }

        public IEnumerable<ChessUnit>  GridFigure {get; set;}

        public List<Vector2Int> FindPath(ChessUnitType unit, Vector2Int from, Vector2Int to, ChessGrid grid)
        {
            //напиши реализацию не меняя сигнатуру функции
            
            Grid = grid;

            FinalPosition = to;

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

        public bool ChekVictory(Vector2Int pos)
        {

            //если целевая позиция найдена и мир спасен
            if (pos == FinalPosition) return true;

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
            figure.Moves = figure.GetAviableMoves(chain.Last.Value, chain, Grid);
            
            for (int i = 0; i < figure.MovesCount; i++)
            {
                //возможно это костыль и можно лучше, но пока оставлю так
                LinkedList<Vector2Int> original_chain = new(chain);

                var unit = original_chain.Last.Value + figure.Moves[i];

                if (ChekVictory(unit))
                {
                    original_chain.AddLast(unit);
                    return original_chain;
                }
                else
                {
                    original_chain.AddLast(unit);
                    ChainContener.Add(original_chain);
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
                    return new Queen(unit);
                case ChessUnitType.King:
                    return new King(unit);
            }
            return null;
        }

    }
}