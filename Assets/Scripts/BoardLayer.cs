using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardLayer : MonoBehaviour
{
    public Space[] spaces;

    public void SpaceSelected(Space space)
    {
        int spaceIndex = Array.IndexOf(spaces, space);
        Board board = transform.GetComponentInParent<Board>();
        if (board != null)
        {
            int row = transformIndexToRow(spaceIndex);
            int column = transformIndexToColumn(spaceIndex);
            board.LayerSelected(this, row, column);
        }
        else
        {
            Debug.LogError("No Board Object Found!");
        }
    }

    public Space GetSpace(int row, int column)
    {
        return spaces[row + (column * 3)];
    }

    private int transformIndexToRow(int index)
    {
        return index % 3;
    }
    private int transformIndexToColumn(int index)
    {
        return Mathf.FloorToInt(index / 3);
    }
}
