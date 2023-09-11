using System;
using UnityEngine;

public class FillWordsModelService 
{
    private Vector2Int _size;

    public Vector2Int Size => _size;
    public int ScalarSize => _size.x*_size.y;

    public FillWordsModelService(int index) 
    { 
        _size.x = _size.y = (int)Math.Ceiling(Math.Sqrt(index));
    }
    private int DivisionUp(int numerator, int denominator)
    {
        if (numerator % denominator == 0) return numerator / denominator;
        else return numerator / denominator + 1;
    }
    private int RemainderIn(int numerator, int denominator)
    {
        if (numerator % denominator == 0) return denominator;
        else return numerator % denominator;
    }
    public Vector2Int Get—oordinate(int index)
    {
        return new Vector2Int(DivisionUp(index + 1, _size.x), RemainderIn(index + 1, _size.x));
    }
}
