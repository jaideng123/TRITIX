﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardController : MonoBehaviour
{

    [SerializeField]
    private GameObject boardObject;
    private Board board;
    private Space selectedDestSpace;
    private Space selectedOriginSpace;
    private int currentPlayer = 1;
    // Use this for initialization
    void Awake()
    {
        Messenger<Vector3Int>.AddListener(GameEvent.SPACE_SELECTED, OnSpaceSelected);
    }
    void OnDestroy()
    {
        Messenger<Vector3Int>.RemoveListener(GameEvent.SPACE_SELECTED, OnSpaceSelected);
    }
    void Start()
    {
        board = boardObject.GetComponent<Board>();
        Player p = new Player();
        p.id = "Jaiden";
        p.pieceMaterialName = "Piece-White";
        Managers.Player.SetPlayer(1, p);
        p = new Player();
        p.id = "Sydney";
        p.pieceMaterialName = "Piece-Black";
        Managers.Player.SetPlayer(2, p);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnSpaceSelected(Vector3Int coordinates)
    {
        if (selectedDestSpace != null)
        {
            selectedDestSpace.SetActive(false);
        }
        selectedDestSpace = board.GetSpace(coordinates);
        selectedDestSpace.SetActive(true);
    }
}