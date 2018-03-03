using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Space : MonoBehaviour
{
    private GameObject _pieceObject;
    private Piece _piece;
    private GameObject _touchBox;
    private bool _active;
    // Use this for initialization
    void Start()
    {
        _touchBox = GetComponentInChildren<TouchTarget>().gameObject;
        _piece = null;
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
        _piece = newPiece;
        if (_pieceObject != null)
        {
            Destroy(_pieceObject);
        }
        _pieceObject = Instantiate(Resources.Load("Pieces/Prefabs/" + _piece.type.ToString()) as GameObject);
        _pieceObject.transform.SetParent(this.transform, false);
        _pieceObject.GetComponent<Renderer>().material = Resources.Load("Pieces/Materials/Piece-White") as Material;
    }

    public void ClearPiece()
    {
        Destroy(_pieceObject);
    }

    public void SetActive(bool active)
    {
        if (active)
        {
            _touchBox.GetComponent<Renderer>().material = Resources.Load("Spaces/Materials/SpaceTargetActive") as Material;
        }
        else
        {
            _touchBox.GetComponent<Renderer>().material = Resources.Load("Spaces/Materials/SpaceTarget") as Material;
        }
        _active = active;
    }
}
