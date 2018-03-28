using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Camera))]
public class PointerSelect : MonoBehaviour
{
    public float timeToDrag = .5f;
    private Camera _camera;
    private float _startTime = 0f;

    // Use this for initialization
    void Start()
    {
        _camera = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            if (Input.touchCount == 0 && (Input.GetAxis("Mouse Y") > 0 || Input.GetAxis("Mouse X") > 0))
            {
                return;
            }
            _startTime = Time.time;
        }
        if (Input.GetMouseButtonUp(0) && Time.time - _startTime < timeToDrag)
        {
            if (Input.touchCount > 0 && Input.GetTouch(0).deltaPosition != Vector2.zero)
            {
                return;
            }

            RaycastHit hit;
            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                hit.transform.SendMessage("Triggered", SendMessageOptions.DontRequireReceiver);
            }
        }
    }
}
