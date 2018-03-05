using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    public GameObject[] layers;
    // Use this for initialization
    void Start()
    {
    }
    // Update is called once per frame
    void Update()
    {

    }
    public void LayerSelected(GameObject layer, Vector2Int coordinates)
    {
        int i = Array.IndexOf(layers, layer);
        Vector3Int fullCoord = new Vector3Int(coordinates.x, coordinates.y, i);
        Debug.Log("Space Selected: " + fullCoord);
        Messenger<Vector3Int>.Broadcast(GameEvent.SPACE_SELECTED, fullCoord);
    }
    public Piece[,,] GetBoardModel()
    {
        Piece[,,] board = new Piece[3, 3, 3];
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                for (int k = 0; k < 3; k++)
                {
                    Vector3Int v = new Vector3Int(j, k, i);
                    board[i, j, k] = GetSpace(v).piece;
                }
            }
        }
        return board;

    }
    public Space GetSpace(Vector3Int coordinates)
    {
        BoardLayer layer = layers[coordinates.z].GetComponent<BoardLayer>();
        Space space = layer.GetSpace(new Vector2Int(coordinates.x, coordinates.y)).GetComponent<Space>();
        return space;
    }
}
