using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardLayer : MonoBehaviour
{
    public GameObject[] spaces;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SpaceSelected(GameObject space)
    {
        int i = Array.IndexOf(spaces, space);
        Board board = transform.GetComponentInParent<Board>();
        if (board != null)
        {
            board.LayerSelected(this.gameObject, transformLayerIndex(i));
        }
        else
        {
            Debug.LogError("No Board Object Found!");
        }
    }

    public GameObject GetSpace(Vector2Int coordinates)
    {
        int i = transformLayerCoordinate(coordinates);
        return spaces[i];
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
