using App.Scripts.Scenes.SceneChess.Features.ChessField.Types;
using System.Collections.Generic;
using UnityEngine;

public abstract class Figure
{
    protected List<Vector2Int> _moves = new();
    protected ChessUnitType _name;
    protected int[] _direction;
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
    public List<Vector2Int> GetAviableMoves(Vector2Int pos)
    {
        
        foreach (var step in _baseModulStep)
        {
            for (int i = 0; i < _mul; i++)
                for (int j = 0; j < _mul; j++)
                {
                    for (int z = 0; z < _direction.Length; z++)
                    {
                        var v = step * _matrixDirection[i, j];
                        if (v.sqrMagnitude != 0) _moves.Add(v);
                    }
                    
                }
        }
        return _moves; 
    }
    protected static int[,] MatrixCreator(int[] value)
    {
        int[,] result = new int[2, 2];
        result[0,0] = value[0];
        result[0,1] = value[1];
        result[1,0] = value[2];
        result[1, 1] = value[3];
        return result;
    }
}

public class Pon : Figure
{
    
    public Pon(ChessUnitType name) : base(name)
    {
        _baseModulStep = new[] { new Vector2Int(0, 1) };
        //todo добавить костыль для пешки
        _direction = new[] {0,0,1,0};
        _matrixDirection = MatrixCreator(_direction);
        _mul = 1; 

    }
}
public class Knight : Figure
{
    public Knight(ChessUnitType name) : base (name)
    {
        _baseModulStep = new[] { new Vector2Int(2, 1), new Vector2Int(1, 2) };
        _direction = new[] { 1, -1, 1, -1 };
        _matrixDirection = MatrixCreator(_direction);
        _mul = 1;
    }
}

public class Rook : Figure
{
    public Rook(ChessUnitType name) : base(name)
    {
        
    }
}

public class Bishop : Figure
{
    public Bishop(ChessUnitType name) : base(name)
    {
       
    }
}

public class Queen : Figure
{
    public Queen(ChessUnitType name) : base(name)
    {
       
    }
}