using App.Scripts.Scenes.SceneChess.Features.ChessField.GridMatrix;
using App.Scripts.Scenes.SceneChess.Features.ChessField.Types;
using System.Collections.Generic;
using UnityEngine;

public abstract class Figure
{
    protected List<Vector2Int> _moves = new();
    protected ChessUnitType _name;
    protected Vector2Int[] _direction;
    protected int[,] _matrixDirection;
    protected int _mul;
    protected Vector2Int[] _baseModulStep;
    public ChessUnitType Name { get => _name; set => _name = value; }
    public List<Vector2Int> Moves { get => _moves; set => _moves = value; }
    public int MovesCount { get => _moves.Count; }

    public Figure(ChessUnitType name)
    { 
        _name = name;
    }
    public List<Vector2Int> GetAviableMoves(Vector2Int pos, LinkedList<Vector2Int> chain, ChessGrid grid)
    {
        _moves = new List<Vector2Int>();

        foreach (var step in _baseModulStep)
        {
            for (int i = 0; i < _mul; i++)
            {
                for (int z = 0; z < _direction.Length; z++)
                {
                    var rr = new Vector2Int(step.x * _direction[z].x, step.y * _direction[z].y) * (i + 1);
                    var newStep = pos + rr;

                    if (ValidateTargetStep(newStep, chain, grid) && !_moves.Contains(rr)) _moves.Add(rr);
                }
            } 
        }
        return _moves; 
    }

    public bool ValidateTargetStep(Vector2Int pos, LinkedList<Vector2Int> chain, ChessGrid grid)
    {
        
        //если позиция за пределами экрана
        if (pos.x < 0 || pos.x > 7 || pos.y < 0 || pos.y > 7) return false;

        //если на пути другая фигура (кроме коня)
        if (Name != ChessUnitType.Knight && IsCrossingPath(pos, chain, grid)) return false;

        //если на позиции другая фигура
        //if (grid.Get(pos) != null) return false;

        //если это "обратый" ход
        if (chain.Last.Previous != null && chain.Last.Previous.Value == pos) return false;

        return true;
    }
    private bool IsCrossingPath(Vector2Int pos, LinkedList<Vector2Int> chain, ChessGrid grid)
    {
        Vector2Int vectorPath = chain.Last.Value - pos;
        float pathLen = vectorPath.magnitude;

        foreach (var f in grid.Pieces)
        {
            //проверка на самого себя
            if (f.CellPosition == chain.First.Value) continue;
            //todo возможно есть способ лучше, но с ходу я не придумал, точка лежит на прямой, если разбивает отрезки
            var vector1 = (pos - f.CellPosition).magnitude;
            var vector2 = (f.CellPosition - chain.Last.Value).magnitude;
            if (Mathf.Approximately(pathLen, vector2 + vector1)) return true;
        }
        return false;
    }
}

public class Pon : Figure
{
    
    public Pon(ChessUnitType name) : base(name)
    {
        _baseModulStep = new[] { new Vector2Int(0, 1) };
        //todo добавить костыль для пешки
        _direction = new[] {new Vector2Int(0,1)};
        _mul = 1; 

    }
}
public class Knight : Figure
{
    public Knight(ChessUnitType name) : base (name)
    {
        _baseModulStep = new[] { new Vector2Int(2, 1), new Vector2Int(1, 2) };
        _direction = new[] { new Vector2Int(1, 1), new Vector2Int(-1, 1), new Vector2Int(1, -1), new Vector2Int(-1, -1) };
        _mul = 1;
    }
}

public class Rook : Figure
{
    public Rook(ChessUnitType name) : base(name)
    {
        _baseModulStep = new[] { new Vector2Int(1, 0), new Vector2Int(0, 1) };
        _direction = new[] { new Vector2Int(1, 1), new Vector2Int(-1, -1)};
        _mul = 7;
    }
}

public class Bishop : Figure
{
    public Bishop(ChessUnitType name) : base(name)
    {
        _baseModulStep = new[] { new Vector2Int(1, 1) };
        _direction = new[] { new Vector2Int(1, 1), new Vector2Int(-1, 1), new Vector2Int(1, -1), new Vector2Int(-1, -1) };
        _mul = 7;
    }
}

public class Queen : Figure
{
    public Queen(ChessUnitType name) : base(name)
    {
        _baseModulStep = new[] { new Vector2Int(1, 1), new Vector2Int(1, 0), new Vector2Int(0, 1) };
        _direction = new[] { new Vector2Int(1, 1), new Vector2Int(-1, 1), new Vector2Int(1, -1), new Vector2Int(-1, -1) };
        _mul = 7;
    }
}
public class King : Figure
{
    public King(ChessUnitType name) : base(name)
    {
        _baseModulStep = new[] { new Vector2Int(1, 1), new Vector2Int(1, 0), new Vector2Int(0, 1) };
        _direction = new[] { new Vector2Int(1, 1), new Vector2Int(-1, 1), new Vector2Int(1, -1), new Vector2Int(-1, -1) };
        _mul = 1;
    }
}