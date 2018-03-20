using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class OrbitCameraStatic : MonoBehaviour
{
    [SerializeField] private Transform target;
    public float rotSpeedX = 1.5f;
    public float rotSpeedY = 0f;
    private float _rotY;
    private float _rotX;
    private Vector3 _offset;
    // Use this for initialization
    void Start()
    {
        _rotY = transform.eulerAngles.y;
        _rotX = transform.eulerAngles.x;
        _offset = target.position - transform.position;
    }

    public void SetAngle(float rotX, float rotY)
    {
        _rotX = rotX;
        _rotY = rotY;
        Quaternion rotation = Quaternion.Euler(_rotX, _rotY, 0);
        transform.position = target.position - (rotation * _offset);
        transform.LookAt(target);
    }

    void LateUpdate()
    {
        _rotY += rotSpeedY * Time.deltaTime;
        _rotX += rotSpeedX * Time.deltaTime;

        Quaternion rotation = Quaternion.Euler(_rotX, _rotY, 0);
        Vector3 targetPos = target.position - (rotation * _offset);
        transform.position = targetPos;

        transform.LookAt(target);
    }
    // Update is called once per frame
    void Update()
    {

    }
}
