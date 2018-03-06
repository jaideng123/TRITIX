using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    public BoardLayer[] layers;
    // Use this for initialization
    void Start()
    {
    }
    // Update is called once per frame
    void Update()
    {

    }
    public void LayerSelected(BoardLayer layer, Vector2Int coordinates)
    {
        int i = Array.IndexOf(layers, layer);
        Vector3Int fullCoord = new Vector3Int(coordinates.x, coordinates.y, i);
        Debug.Log("Space Selected: " + fullCoord);
        Messenger<Vector3Int>.Broadcast(GameEvent.SPACE_SELECTED, fullCoord);
    }
    public Piece[][][] GetBoardModel()
    {
        Piece[][][] board = new Piece[3][][];
        for (int i = 0; i < 3; i++)
        {
            board[i] = layers[i].GetLayerModel();
        }
        return board;

    }
    public Space GetSpace(Vector3Int coordinates)
    {
        BoardLayer layer = layers[coordinates.z];
        Space space = layer.GetSpace(new Vector2Int(coordinates.x, coordinates.y));
        return space;
    }
}
