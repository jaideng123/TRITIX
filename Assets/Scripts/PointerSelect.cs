using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Camera))]
public class PointerSelect : MonoBehaviour
{
    private Camera _camera;
    private Vector2 pastPosition;

    private const float epsilon = 10f;

    // Use this for initialization
    void Start()
    {
        _camera = GetComponent<Camera>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            if (Input.touchCount == 0)
            {
                pastPosition = Input.mousePosition;
            }
            else
            {
                pastPosition = Input.GetTouch(0).position;
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            Vector2 currentPosition;
            if (Input.touchCount > 0)
            {
                currentPosition = Input.GetTouch(0).position;
            }
            else
            {
                currentPosition = Input.mousePosition;
            }

            if (TouchMoved(pastPosition, currentPosition))
            {
                Debug.LogWarning("TOUCH SKIPPED");
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

    private bool DeltaChanged(Vector2 delta, float epsilon)
    {
        if (Mathf.Max(Mathf.Abs(delta.x), Mathf.Abs(delta.y)) > epsilon)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private bool TouchMoved(Vector2 initial, Vector2 current)
    {
        if (Mathf.Abs(initial.x - current.x) > epsilon)
        {
            return true;
        }
        else if (Mathf.Abs(initial.y - current.y) > epsilon)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
