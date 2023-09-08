using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IChainFinder
{
    List<LinkedList<Vector2Int>> ChainContener { get; set; }

    Vector2Int[] Moves { get; set; }

    int MovesCount { get;}
    void InitChainContener() { }
    void AddUnitToChain() { }
    LinkedList<Vector2Int> GetVariantMoves(Vector2Int point) {return null;}

    void SaveChainInContener() {}
}
