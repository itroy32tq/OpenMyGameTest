using App.Scripts.Scenes.SceneChess.Features.ChessField.GridMatrix;
using App.Scripts.Scenes.SceneChess.Features.GridNavigation.Navigator;
using System.Collections.Generic;
using UnityEngine;

public class ChainPathService
{
    private Figure _figure;
    private ChessGrid _grid;
    private Vector2Int _finalPos;
    private LinkedList<Vector2Int> _chain;

    public ChainPathService(Figure figure, ChessGrid grid, Vector2Int finalPos)
    { 
        _figure = figure; _grid = grid; _finalPos = finalPos;
    }
    private Vector2Int SelfPos => _chain.Last.Value;
    private LinkedListNode<Vector2Int> PreviousNode => _chain.Last.Previous;
    private Vector2Int OriginalPos => _chain.First.Value;

    /// <summary>
    /// Центральный метод по созданию цепочек ходов, он получился немного монстроузным в плане вложненности
    /// но при любых других вариациях у меня получались лишние циклы по обходу вариантов. В данной реализации
    /// я получая валидную цепочку могу сразу проверить что она финальная и прервать обход
    /// </summary>
    /// <param name="chain">целевая цепочка ходов</param>
    /// <returns></returns>
    public LinkedList<Vector2Int> MakeChainList(LinkedList<Vector2Int> chain)
    {
        List<Vector2Int> moves = new ();
        _chain = chain;

        foreach (var step in _figure.BaseStep)
        {
            for (int i = 0; i < _figure.Mul; i++)
            {
                for (int z = 0; z < _figure.Direction.Length; z++)
                {
                    Vector2Int newStep = new Vector2Int(step.x * _figure.Direction[z].x, step.y * _figure.Direction[z].y) * (i + 1);

                    Vector2Int newPos = SelfPos + newStep;

                    if (ValidateTargetStep(newPos) && !moves.Contains(newStep))
                    {
                        moves.Add(newStep);
                        LinkedList<Vector2Int> IterListChain = new(chain);
                        IterListChain.AddLast(newPos);
                        if (IterListChain.Last.Value == _finalPos) return IterListChain;
                        else ChessGridNavigator.ChainContener.AddLast(IterListChain);
                    } 
                }
            }
        }
        return null;
    }
    /// <summary>
    /// Метод валидации последнего элемента цепочки (последнего хода)
    /// </summary>
    /// <param name="pos">вероятный послений элемент цепочки</param>
    /// <returns></returns>
    public bool ValidateTargetStep(Vector2Int pos)
    {
        if (IsCrossingBoardBound(pos)) return false;

        if (IsCrossingPath(pos)) return false;

        if (IsReverseStep(pos)) return false;

        return true;
    }
    /// <summary>
    /// Метод проверки наличия другой фигуры на пути, я проверяю исходя из того
    /// что если вектор пути проходит через позицию с другой фигрой, то это позиция
    /// разбивает вектор на две равные части
    /// </summary>
    /// <param name="pos">вероятный послений элемент цепочки</param>
    /// <returns></returns>
    private bool IsCrossingPath(Vector2Int pos)
    {
        Vector2Int vectorPath = SelfPos - pos;

        float pathLen = vectorPath.magnitude;

        foreach (var figure in _grid.Pieces)
        {
            //проверка на самого себя
            if (figure.CellPosition == OriginalPos) continue;

            float vector1 = (pos - figure.CellPosition).magnitude;
            float vector2 = (figure.CellPosition - SelfPos).magnitude;
            if (Mathf.Approximately(pathLen, vector2 + vector1)) return true;
        }
        return false;
    }
    /// <summary>
    /// Метод проверки выхода фигуры за пределы доски
    /// </summary>
    /// <param name="pos">вероятный послений элемент цепочки</param>
    /// <returns></returns>
    private bool IsCrossingBoardBound(Vector2Int pos)
    {
        return pos.x < 0 || pos.x > 7 || pos.y < 0 || pos.y > 7;
    }
    /// <summary>
    /// Метод проверки на "обратные" ход - мы не возвращаемся в обратный шаг цепочки
    /// </summary>
    /// <param name="pos">вероятный послений элемент цепочки</param>
    /// <returns></returns>
    private bool IsReverseStep(Vector2Int pos)
    {
        return PreviousNode != null && PreviousNode.Value == pos;
    }

}
