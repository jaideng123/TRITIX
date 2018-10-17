using System.Collections.Specialized;
using UnityEngine;
public class BoardCoordinates
{
    public int layer, column, row;

    public BoardCoordinates(int layer, int column, int row)
    {
        this.layer = layer;
        this.column = column;
        this.row = row;
    }

    public BoardCoordinates()
    {
        layer = 0;
        column = 0;
        row = 0;
    }

    public override string ToString()
    {
        return string.Format("[{0},{1},{2}]", layer, column, row);
    }
}