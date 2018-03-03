using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardController : MonoBehaviour
{

    [SerializeField]
    private GameObject boardObject;
    private Board board;
    // Use this for initialization
    void Start()
    {
        board = boardObject.GetComponent<Board>();
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                Piece p = new Piece();
                p.type = PieceType.FLAT;
                board.applyPiece(p, new Vector3Int(i, j, 0));
            }
        }
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                Piece p = new Piece();
                p.type = PieceType.ROUND;
                board.applyPiece(p, new Vector3Int(i, j, 1));
            }
        }
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                Piece p = new Piece();
                p.type = PieceType.POINT;
                board.applyPiece(p, new Vector3Int(i, j, 2));
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
