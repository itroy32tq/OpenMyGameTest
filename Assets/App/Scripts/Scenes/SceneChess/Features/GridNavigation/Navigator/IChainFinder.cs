using App.Scripts.Scenes.SceneChess.Features.ChessField.GridMatrix;
using App.Scripts.Scenes.SceneChess.Features.ChessField.Piece;
using System.Collections.Generic;
using UnityEngine;

public interface IChainMaker
{
    List<LinkedList<Vector2Int>> ChainContener { get; set; }
    LinkedList<Vector2Int> FinalChain { get; set; }
    ChessGrid Grid { get; set; }
    IEnumerable<ChessUnit> GridFigure { get; set; }
    void AddUnitToChain() { }
}
