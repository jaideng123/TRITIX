using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(MatrixBlender))]
public class OrthoCamera : MonoBehaviour
{
    public float fov = 60f,
                        near = .3f,
                        far = 1000f,
                        orthographicSize = 5f,
                        transitionTime = .5f,
                        smoothingFactor = 1f;
    [SerializeField]
    private Transform viewTarget;
    private float aspect;
    private MatrixBlender _matrixBlender;
    private Transform activeTargetPosition;
    // Use this for initialization

    void Awake()
    {
    }
    void OnDestroy()
    {
    }
    void Start()
    {
        aspect = (float)Screen.width / (float)Screen.height;
        _matrixBlender = GetComponent<MatrixBlender>();

    }

    // Update is called once per frame
    void Update()
    {
        if (activeTargetPosition)
        {
            this.transform.position += (activeTargetPosition.position - transform.position) * smoothingFactor;
            this.transform.LookAt(viewTarget);
        }
    }

    public void Activate(Transform targetPosition)
    {
        activeTargetPosition = targetPosition;
        _matrixBlender.BlendToMatrix(Matrix4x4.Ortho(-orthographicSize * aspect, orthographicSize * aspect, -orthographicSize, orthographicSize, near, far), transitionTime);
    }

    public void Disable()
    {
        activeTargetPosition = null;
        _matrixBlender.BlendToMatrix(Matrix4x4.Perspective(fov, aspect, near, far), transitionTime);
    }
}
