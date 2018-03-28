using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardLayer : MonoBehaviour
{
    public Space[] spaces;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SpaceSelected(Space space)
    {
        int i = Array.IndexOf(spaces, space);
        Board board = transform.GetComponentInParent<Board>();
        if (board != null)
        {
            board.LayerSelected(this, transformLayerIndex(i));
        }
        else
        {
            Debug.LogError("No Board Object Found!");
        }
    }

    public Space GetSpace(Vector2Int coordinates)
    {
        int i = transformLayerCoordinate(coordinates);
        return spaces[i];
    }

    public Piece[][] GetLayerModel()
    {
        Piece[][] layer = {new Piece[3], new Piece[3], new Piece[3] };
        for (int i = 0; i < 9; i++)
        {
            Vector2Int v = transformLayerIndex(i);
            layer[v.x][v.y] = GetSpace(v).piece;
        }
        return layer;
    }

    private Vector2Int transformLayerIndex(int index)
    {
        return new Vector2Int(index % 3, Mathf.FloorToInt(index / 3));
    }

    private int transformLayerCoordinate(Vector2Int coordinates)
    {
        return coordinates.x + coordinates.y * 3;
    }
}
