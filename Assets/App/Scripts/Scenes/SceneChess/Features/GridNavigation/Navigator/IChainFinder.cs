using App.Scripts.Scenes.SceneChess.Features.ChessField.GridMatrix;
using System.Collections.Generic;
using UnityEngine;

public interface IChainBilder
{
    List<LinkedList<Vector2Int>> ChainContener { get; set; }
    Vector2Int FinalPosition { get; set; }
    LinkedList<Vector2Int> FinalChain { get; set; }
    ChessGrid Grid { get; set; }
    void InitChainContener() { }
    int ValidateTargetPos(Vector2Int pos, LinkedList<Vector2Int> chain) { return 0; }
    void AddUnitToChain() { }
}
