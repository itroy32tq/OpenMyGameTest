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

        /// <summary>
        /// countIteration оценочная переменная, что бы понимать сколько итераций необходимо что бы найти путь
        /// большинство фигур справляется быстро до 500 итераций, однако для короля с проходом через все поле понадобилось
        /// порядка 8000 итераций
        /// </summary>
        private readonly int countIteration = 2048*4;
        public static LinkedList<LinkedList<Vector2Int>> ChainContener { get; set; }

        public LinkedList<Vector2Int> FinalChain { get; set; }

        public List<Vector2Int> FindPath(ChessUnitType unit, Vector2Int from, Vector2Int to, ChessGrid grid)
        {
            //напиши реализацию не меняя сигнатуру функции

            InitChainContener(from);

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

        /// <summary>
        /// Создание контейнера цепочек с единственным списком из начального положения
        /// </summary>
        /// <param name="pos">стартовое положение фигуры</param>
        public void InitChainContener(Vector2Int pos)
        {
            ChainContener = new();
            ChainContener.AddFirst(new LinkedList<Vector2Int>(new List<Vector2Int>() { pos }));
        }

        /// <summary>
        /// Метод определения какой класс создать исходя и значения
        /// enum ChessUnitType
        /// </summary>
        /// <param name="unit">тип фигуры</param>
        /// <returns></returns>
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