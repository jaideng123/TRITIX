using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{


    private bool _orthoActive;
    private OrbitCamera orbitCamera;
    private OrthoCamera orthoCamera;
    [SerializeField]
    private Camera _camera;
    [SerializeField]
    private GameObject top;
    [SerializeField]
    private GameObject mid;
    [SerializeField]
    private Transform[] snapPositions;
    private Transform activeSnapPosition;

    void Awake()
    {
        Messenger.AddListener(GameEvent.TOGGLE_VIEW, OnToggleView);
    }
    void OnDestroy()
    {
        Messenger.RemoveListener(GameEvent.TOGGLE_VIEW, OnToggleView);
    }

    // Use this for initialization
    void Start()
    {
        orbitCamera = _camera.GetComponent<OrbitCamera>();
        orthoCamera = _camera.GetComponent<OrthoCamera>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnToggleView()
    {
        if (_orthoActive)
        {
            float direction = activeSnapPosition.transform.position.z < 0 ? -1 : 1;
            MoveLayers(direction);
            orthoCamera.Disable();
            activeSnapPosition = null;
            orbitCamera.enabled = true;
            orbitCamera.SetAngle(30, 0);
        }
        else
        {
            orbitCamera.enabled = false;
            activeSnapPosition = FindNearestPosition(snapPositions, _camera.transform);
            float direction = activeSnapPosition.transform.position.z < 0 ? 1 : -1;
            MoveLayers(direction);
            orthoCamera.Activate(activeSnapPosition);
        }
        _orthoActive = !_orthoActive;
    }

    private void MoveLayers(float direction)
    {
        Vector3 midPos = mid.transform.position;
        midPos.z += 2 * direction;
        mid.transform.position = midPos;
        Vector3 topPos = top.transform.position;
        topPos.z += 4 * direction;
        top.transform.position = topPos;
    }

    private Transform FindNearestPosition(Transform[] positions, Transform basePosition)
    {
        Transform closest = positions[0];
        float closestDist = float.MaxValue;
        foreach (Transform pos in positions)
        {
            float dist = Vector3.Distance(basePosition.position, pos.position);
            Debug.Log(dist);
            if (dist < closestDist)
            {
                closest = pos;
                closestDist = dist;
            }
        }
        Debug.Log(closest);
        return closest;
    }
}
