using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    public BoardLayer[] layers;

    public void LayerSelected(BoardLayer layer, int row, int column)
    {
        int i = Array.IndexOf(layers, layer);
        BoardCoordinates fullCoord = new BoardCoordinates(i, row, column);
        Messenger<BoardCoordinates>.Broadcast(GameEvent.SPACE_SELECTED, fullCoord);
    }

    public Space GetSpace(BoardCoordinates coordinates)
    {
        BoardLayer layer = layers[coordinates.layer];
        Space space = layer.GetSpace(coordinates.column, coordinates.row);
        return space;
    }

    public Space[] GetAllSpaces()
    {
        List<Space> spaces = new List<Space>();
        foreach (BoardLayer layer in layers)
        {
            foreach (Space space in layer.spaces)
            {
                spaces.Add(space);
            }
        }
        return spaces.ToArray();
    }
}
