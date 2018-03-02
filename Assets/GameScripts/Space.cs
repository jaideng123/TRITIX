using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Space : MonoBehaviour
{

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

    public void ApplyPiece(GameObject piecePrefab)
    {
        GameObject piece = Instantiate(piecePrefab);
        piece.transform.SetParent(this.transform, false);

    }
}
