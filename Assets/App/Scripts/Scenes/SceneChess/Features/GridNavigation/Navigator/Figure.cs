using App.Scripts.Scenes.SceneChess.Features.ChessField.GridMatrix;
using App.Scripts.Scenes.SceneChess.Features.ChessField.Types;
using System.Collections.Generic;
using UnityEngine;

public abstract class Figure
{
    protected List<Vector2Int> _moves = new();

    protected Vector2Int[] _direction;
    public Vector2Int[] Direction => _direction;

    protected int _mul;
    public int Mul => _mul;

    protected Vector2Int[] _baseModulStep;
    public Vector2Int[] BaseStep => _baseModulStep;

    public List<Vector2Int> Moves { get => _moves; set => _moves = value; }
    public int MovesCount { get => _moves.Count; }
}

public class Pon : Figure
{
    
    public Pon()
    {
        _baseModulStep = new[] { new Vector2Int(0, 1) };
        //todo добавить костыль для пешки, не нашел как узнать ее цвет, т.е. черная пешка не сможет ходить
        _direction = new[] {new Vector2Int(0,1)};
        _mul = 1; 

    }
}
public class Knight : Figure
{
    public Knight()
    {
        _baseModulStep = new[] { new Vector2Int(2, 1), new Vector2Int(1, 2) };
        _direction = new[] { new Vector2Int(1, 1), new Vector2Int(-1, 1), new Vector2Int(1, -1), new Vector2Int(-1, -1) };
        _mul = 1;
    }
}

public class Rook : Figure
{
    public Rook()
    {
        _baseModulStep = new[] { new Vector2Int(1, 0), new Vector2Int(0, 1) };
        _direction = new[] { new Vector2Int(1, 1), new Vector2Int(-1, -1)};
        _mul = 7;
    }
}
public class Bishop : Figure
{
    public Bishop()
    {
        _baseModulStep = new[] { new Vector2Int(1, 1) };
        _direction = new[] { new Vector2Int(1, 1), new Vector2Int(-1, 1), new Vector2Int(1, -1), new Vector2Int(-1, -1) };
        _mul = 7;
    }
}
public class Queen : Figure
{
    public Queen()
    {
        _baseModulStep = new[] { new Vector2Int(1, 1), new Vector2Int(1, 0), new Vector2Int(0, 1) };
        _direction = new[] { new Vector2Int(1, 1), new Vector2Int(-1, 1), new Vector2Int(1, -1), new Vector2Int(-1, -1) };
        _mul = 7;
    }
}
public class King : Figure
{
    public King()
    {
        _baseModulStep = new[] { new Vector2Int(1, 1), new Vector2Int(1, 0), new Vector2Int(0, 1) };
        _direction = new[] { new Vector2Int(1, 1), new Vector2Int(-1, 1), new Vector2Int(1, -1), new Vector2Int(-1, -1) };
        _mul = 1;
    }
}