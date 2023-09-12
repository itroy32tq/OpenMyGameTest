using App.Scripts.Scenes.SceneChess.Features.ChessField.GridMatrix;
using App.Scripts.Scenes.SceneChess.Features.ChessField.Types;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace App.Scripts.Scenes.SceneChess.Features.GridNavigation.Navigator
{
    public class ChessGridNavigator : IChessGridNavigator
    {
        private Figure figure;
        private ChainPathService chainPathService;

        //большинство фигур справляется быстро до 500 итераций, однако для короля с проходом через все поле понадобилось
        //порядка 8000 итераций
        private readonly int countIteration = 2048*4;
        public static LinkedList<LinkedList<Vector2Int>> ChainContener { get; set; }

        public LinkedList<Vector2Int> FinalChain { get; set; }

        public List<Vector2Int> FindPath(ChessUnitType unit, Vector2Int from, Vector2Int to, ChessGrid grid)
        {
            //напиши реализацию не меняя сигнатуру функции

            //создаем хранилище для цепочек
            InitChainContener(from);

            //определяем фигуру и ее параметры
            figure = DetectFigure(unit);

            //передаем все для обработки в сервис
            chainPathService = new(figure, grid, to);

            LinkedList<Vector2Int> chain = ChainContener.FirstOrDefault();

            int i = 0;

            while (i <= countIteration)
            {
                FinalChain = chainPathService.MakeChainList(chain);

                if (FinalChain != null) return FinalChain.ToList();

                ChainContener.RemoveFirst();
                chain = ChainContener.FirstOrDefault();
                i++;
            }

            //не очень понятно зачем по условию возвращать null, в частности
            //если двигать слона на белое поле, сцена зависает
            return null;
        }

        public void InitChainContener(Vector2Int pos)
        {
            ChainContener = new();
            ChainContener.AddFirst(new LinkedList<Vector2Int>(new List<Vector2Int>() { pos }));
        }

        private Figure DetectFigure(ChessUnitType unit)
        {
            switch (unit)
            {
                case ChessUnitType.Pon:
                    return new Pon();
                case ChessUnitType.Knight:
                    return new Knight();
                case ChessUnitType.Rook:
                    return new Rook();
                case ChessUnitType.Bishop:
                    return new Bishop();
                case ChessUnitType.Queen:
                    return new Queen();
                case ChessUnitType.King:
                    return new King();
            }
            return null;
        }

    }
}