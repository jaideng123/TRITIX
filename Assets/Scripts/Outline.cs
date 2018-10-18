using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Outline : MonoBehaviour
{

    private MeshRenderer _renderer;
    private bool _enabled;
    // Use this for initialization
    void Start()
    {
        _renderer = GetComponent<MeshRenderer>();
        _renderer.enabled = _enabled;
    }

    // Update is called once per frame
    void Update()
    {
        if (_renderer != null && _renderer.enabled != _enabled)
        {
            _renderer.enabled = _enabled;
        }
    }

    public void SetVisible(bool visible)
    {
        _enabled = visible;
    }


}
