using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardController : MonoBehaviour
{

    [SerializeField]
    private GameObject boardObject;
    private Board board;
    [SerializeField]
    private GameObject flat_1;
    [SerializeField]
    private GameObject point_1;
    [SerializeField]
    private GameObject round_1;
    [SerializeField]
    private GameObject wild_1;
    [SerializeField]
    private GameObject flat_2;
    [SerializeField]
    private GameObject point_2;
    [SerializeField]
    private GameObject round_2;
    [SerializeField]
    private GameObject wild_2;
    // Use this for initialization
    void Start()
    {
        board = boardObject.GetComponent<Board>();
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                board.applyPiece(point_2, new Vector3Int(i, j, 0));
            }
        }
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                board.applyPiece(round_1, new Vector3Int(i, j, 1));
            }
        }
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                board.applyPiece(flat_2, new Vector3Int(i, j, 2));
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
