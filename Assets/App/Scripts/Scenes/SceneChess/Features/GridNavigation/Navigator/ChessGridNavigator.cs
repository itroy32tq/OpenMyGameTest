using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using App.Scripts.Scenes.SceneChess.Features.ChessField.GridMatrix;
using App.Scripts.Scenes.SceneChess.Features.ChessField.Types;
using UnityEngine;

namespace App.Scripts.Scenes.SceneChess.Features.GridNavigation.Navigator
{
    public class ChessGridNavigator : IChessGridNavigator
    {
        private Figure figure = null;
        private bool isMoveSearchProcessing = true;
        List<Vector2Int> path = new List<Vector2Int>();
        private List<LinkedList<Vector2Int>> allPathList;

        public List<Vector2Int> FindPath(ChessUnitType unit, Vector2Int from, Vector2Int to, ChessGrid grid)
        {
            //напиши реализацию не меняя сигнатуру функции

            figure = DetectFigure(unit, to);

            //создаем хранилище для цепочек
            figure.InitChainContener(from);


            int i = 0;
            var chaine = figure.ChainContener.FirstOrDefault();
            LinkedList<Vector2Int> res;
            while (i <= 20)
            {
                res = figure.AddUnitToChain(chaine);
                if (res != null) return res.ToList();
                chaine = figure.ChainContener.FirstOrDefault();
                i++;
            }
            return null;
        }

        private Figure DetectFigure(ChessUnitType unit, Vector2Int curPoint)
        {
            switch (unit)
            {
                case ChessUnitType.Pon:
                    return new Pon(curPoint);
                case ChessUnitType.Knight:
                    return new Knight(curPoint);
            }
            return null;
        }
        
    }
}