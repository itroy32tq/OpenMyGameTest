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
    /// ����������� ����� �� �������� ������� �����, �� ��������� ������� ������������ � ����� ������������
    /// �� ��� ����� ������ ��������� � ���� ���������� ������ ����� �� ������ ���������. � ������ ����������
    /// � ������� �������� ������� ���� ����� ��������� ��� ��� ��������� � �������� �����
    /// </summary>
    /// <param name="chain">������� ������� �����</param>
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
    /// ����� ��������� ���������� �������� ������� (���������� ����)
    /// </summary>
    /// <param name="pos">��������� �������� ������� �������</param>
    /// <returns></returns>
    public bool ValidateTargetStep(Vector2Int pos)
    {
        if (IsCrossingBoardBound(pos)) return false;

        if (IsCrossingPath(pos)) return false;

        if (IsReverseStep(pos)) return false;

        return true;
    }
    /// <summary>
    /// ����� �������� ������� ������ ������ �� ����, � �������� ������ �� ����
    /// ��� ���� ������ ���� �������� ����� ������� � ������ ������, �� ��� �������
    /// ��������� ������ �� ��� ������ �����
    /// </summary>
    /// <param name="pos">��������� �������� ������� �������</param>
    /// <returns></returns>
    private bool IsCrossingPath(Vector2Int pos)
    {
        Vector2Int vectorPath = SelfPos - pos;

        float pathLen = vectorPath.magnitude;

        foreach (var figure in _grid.Pieces)
        {
            //�������� �� ������ ����
            if (figure.CellPosition == OriginalPos) continue;

            float vector1 = (pos - figure.CellPosition).magnitude;
            float vector2 = (figure.CellPosition - SelfPos).magnitude;
            if (Mathf.Approximately(pathLen, vector2 + vector1)) return true;
        }
        return false;
    }
    /// <summary>
    /// ����� �������� ������ ������ �� ������� �����
    /// </summary>
    /// <param name="pos">��������� �������� ������� �������</param>
    /// <returns></returns>
    private bool IsCrossingBoardBound(Vector2Int pos)
    {
        return pos.x < 0 || pos.x > 7 || pos.y < 0 || pos.y > 7;
    }
    /// <summary>
    /// ����� �������� �� "��������" ��� - �� �� ������������ � �������� ��� �������
    /// </summary>
    /// <param name="pos">��������� �������� ������� �������</param>
    /// <returns></returns>
    private bool IsReverseStep(Vector2Int pos)
    {
        return PreviousNode != null && PreviousNode.Value == pos;
    }

}
