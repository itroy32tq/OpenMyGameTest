using App.Scripts.Scenes.SceneChess.Features.ChessField.Types;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Numerics;
using UnityEngine;
using static UnityEditor.PlayerSettings;


public abstract class Figure: IChainFinder
{
    private ChessUnitType figureName;
    public ChessUnitType FigureName { get => figureName; set => figureName = value; }

    protected Vector2Int[] _moves;

    protected int _mulKoef;
    public int MulKoef => _mulKoef;

    public Vector2Int LastPos { get; set; }
    public Vector2Int FinalPosition { get; set; }
    public List<LinkedList<Vector2Int>> ChainContener { get ; set ; }
    public Vector2Int[] Moves { get => _moves; set => _moves = value; }
    public int MovesCount { get => _moves.Length;  }

    public Figure(Vector2Int position)
    { 
        FinalPosition = position;
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

        for (int i = 0; i < MovesCount; i++)
        {
            LinkedList<Vector2Int> original_chain = new LinkedList<Vector2Int>(chain);

            var unit = original_chain.Last.Value + Moves[i];

            var res = ValidationChain(unit);

            switch (res)
            {
                case 1:
                    original_chain.AddLast(unit);
                    ChainContener.Add(original_chain);
                    break;
                case 0:
                    original_chain.AddLast(unit);
                    Debug.Log("!!!!!!!!!!!!!!!!");
                    return original_chain;
            }
        }
        ChainContener.RemoveAt(0);
        return null;

    }

    public int ValidationChain(Vector2Int unit)
    {
        if (unit == FinalPosition) return 0;

        if (unit.x < 0 || unit.x > 7 || unit.y < 0 || unit.y > 7) return -1;
        
        return 1;
    }
    public LinkedList<Vector2Int> GetVariantMoves(Vector2Int point)
    {
        LinkedList <Vector2Int> variants = new();
        foreach (var move in Moves)
        { 
            var vector = point + move;
            variants.AddLast(vector);
        }
        return null;
    }
    public void SaveChainInContener() { }
}

public class Pon : Figure
{
    public Pon(Vector2Int position) : base(position)
    {
        
        _moves = new Vector2Int[]
        {
            new Vector2Int(0,1)
        };
        _mulKoef = 1;
    }
}
public class Knight : Figure
{
    public Knight(Vector2Int position) : base(position)
    {
        _moves = new Vector2Int[]
        {
            new Vector2Int(-1,-2),
            new Vector2Int(1,-2),
            new Vector2Int(-1,2),
            new Vector2Int(1,2),
            new Vector2Int(-2,1),
            new Vector2Int(-2,-1),
            new Vector2Int(2,1),
            new Vector2Int(2,-1),

        };
        _mulKoef = 1;
    }
}