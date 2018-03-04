using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{

    [SerializeField]
    private PieceDrawer pieceDrawer;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnToggleView()
    {
        Messenger.Broadcast(GameEvent.TOGGLE_VIEW);
    }
}
