using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Space : MonoBehaviour
{
    private GameObject pieceObject;
    private Piece piece;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Triggered()
    {
        BoardLayer layer = transform.GetComponentInParent<BoardLayer>();
        if (layer != null)
        {
            layer.SpaceSelected(this.gameObject);
        }
        else
        {
            Debug.LogError("No Layer Object Found!");
        }
    }

    public void ApplyPiece(Piece newPiece)
    {
        piece = newPiece;
        if(pieceObject != null){
            Destroy(pieceObject);
        }
        pieceObject = Instantiate(Resources.Load("Pieces/Prefabs/"+piece.type.ToString()) as GameObject);
        pieceObject.transform.SetParent(this.transform, false);
        pieceObject.GetComponent<Renderer>().material = Resources.Load("Pieces/Materials/Piece-White") as Material;
    }

    public void ClearPiece(){
        Destroy(pieceObject);
    }
}
