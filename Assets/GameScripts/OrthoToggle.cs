using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(MatrixBlender))]
public class OrthoToggle : MonoBehaviour
{
    public float fov = 60f,
                        near = .3f,
                        far = 1000f,
                        orthographicSize = 5f,
                        transitionTime = .5f,
                        smoothingFactor = 1f;
    [SerializeField]
    private GameObject top;
    [SerializeField]
    private GameObject mid;
    [SerializeField]
    private Transform targetPosition;
    [SerializeField]
    private Transform viewTarget;
    private float aspect;
    private MatrixBlender _matrixBlender;
    private OrbitCamera _orbitCamera;
    private bool _active;
    // Use this for initialization

    void Awake()
    {
        Messenger.AddListener(GameEvent.TOGGLE_VIEW, OnToggleView);
    }
    void OnDestroy()
    {
        Messenger.RemoveListener(GameEvent.TOGGLE_VIEW, OnToggleView);
    }
    void Start()
    {
        aspect = (float)Screen.width / (float)Screen.height;
        _matrixBlender = GetComponent<MatrixBlender>();
        _orbitCamera = GetComponent<OrbitCamera>();

    }

    // Update is called once per frame
    void Update()
    {
        if (_active)
        {
            this.transform.position += (targetPosition.position - transform.position) * smoothingFactor;
            this.transform.LookAt(viewTarget);

        }
    }

    private void OnToggleView()
    {
        if (!_active)
        {
            Vector3 midPos = mid.transform.position;
            midPos.z += 2;
            mid.transform.position = midPos;
            Vector3 topPos = top.transform.position;
            topPos.z += 4;
            top.transform.position = topPos;
            _orbitCamera.enabled = false;
            _matrixBlender.BlendToMatrix(Matrix4x4.Ortho(-orthographicSize * aspect, orthographicSize * aspect, -orthographicSize, orthographicSize, near, far), transitionTime);
        }
        if (_active)
        {
            Vector3 midPos = mid.transform.position;
            midPos.z -= 2;
            mid.transform.position = midPos;
            Vector3 topPos = top.transform.position;
            topPos.z -= 4;
            top.transform.position = topPos;
            _orbitCamera.enabled = true;
            _orbitCamera.moveToAngle(0, 0);
            _matrixBlender.BlendToMatrix(Matrix4x4.Perspective(fov, aspect, near, far), transitionTime);
        }
        _active = !_active;
    }
}
