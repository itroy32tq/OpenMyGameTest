using App.Scripts.Scenes.SceneFillwords.Features.FillwordModels;
using System;
using UnityEngine;


/// <summary>
/// класс предназначен расчетов размера сетки для модели и 
/// интерпретации индексов из файла уровня в координаты сетки
/// </summary>
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
    public Vector2Int GetCoordinate(int index)
    {
        return new Vector2Int(DivisionUp(index + 1, _size.x), RemainderIn(index + 1, _size.x));
    }

    /// <summary>
    /// метод был написан для заполнения ячеек модели пробельным символом, для
    /// того что бы можно отображать в сетки уровни, где количество индексо меньше ее размера
    /// </summary>
    /// <param name="model"></param>
    public void Initialize(GridFillWords model)
    {
        for (int i = 0; i < Size.x; i++)
            for (int j = 0; j < Size.x; j++)
            {
                CharGridModel tChar = new(' ');
                model.Set(i, j, tChar);
            }
    }
}
