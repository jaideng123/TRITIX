using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceDrawer : MonoBehaviour
{

    public float hiddenOffset;
    private bool visible;
    // Use this for initialization
    void Start()
    {
        Vector3 pos = transform.position;
        pos.y -= hiddenOffset;
        transform.position = pos;
        visible = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Open()
    {
        if (!visible)
        {
            Vector3 pos = transform.position;
            pos.y += hiddenOffset;
            transform.position = pos;
        }
        visible = true;
    }

    public void Close()
    {
        if (visible)
        {
            Vector3 pos = transform.position;
            pos.y -= hiddenOffset;
            transform.position = pos;
        }
        visible = false;
    }
}
