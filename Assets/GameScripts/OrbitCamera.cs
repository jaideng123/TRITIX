using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class OrbitCamera : MonoBehaviour
{
    [SerializeField] private Transform target;

    public float rotSpeed = 1.5f;
    [Range(0, 1)]
    public float smoothingScale = .05f;
    public float drag = .3f;
    public float maxVelocity = 30f;
    public float minXAngle = -75f;
    public float maxXAngle = 80f;
    private float _rotY;
    private float _yVelocity;
    private float _rotX;
    private float _xVelocity;
    private Vector3 _offset;
    private Rigidbody _rigidBody;

    // Use this for initialization
    void Start()
    {
        _rotY = transform.eulerAngles.y;
        _rotX = transform.eulerAngles.x;
        _offset = target.position - transform.position;
    }

    public void moveToAngle(float rotX, float rotY)
    {
        _rotX = rotX;
        _rotY = rotY;
        Quaternion rotation = Quaternion.Euler(_rotX, _rotY, 0);
        transform.position = target.position - (rotation * _offset);
        _xVelocity = 0;
        _yVelocity = 0;
        transform.LookAt(target);
    }

    void LateUpdate()
    {
        float horInput = Input.GetAxis("Horizontal") * -1;
        float vertInput = Input.GetAxis("Vertical");
        if (horInput == 0 && vertInput == 0)
        {
            if (Input.touchCount > 0)
            {
                if (Input.GetTouch(0).phase == TouchPhase.Moved && !EventSystem.current.IsPointerOverGameObject())
                {
                    // Get movement of the finger since last frame
                    Vector2 touchDeltaPosition = Input.GetTouch(0).deltaPosition;
                    horInput = (touchDeltaPosition.x / Screen.width) * 100;
                    vertInput = (touchDeltaPosition.y / Screen.height) * 100;
                }
                else if (Input.GetTouch(0).phase == TouchPhase.Stationary)
                {
                    horInput = .00000001f;
                    vertInput = .00000001f;
                }
            }
            else if (Input.GetMouseButton(0) && !EventSystem.current.IsPointerOverGameObject())
            {
                horInput = Input.GetAxis("Mouse X") * 3 + .00000001f;
                vertInput = Input.GetAxis("Mouse Y") * 3 + .00000001f;
            }
        }
        if (horInput != 0 || vertInput != 0)
        {
            _yVelocity = Mathf.Clamp(horInput * rotSpeed, -maxVelocity, maxVelocity);
            _xVelocity = Mathf.Clamp(vertInput * rotSpeed, -maxVelocity, maxVelocity);
        }
        _rotY += _yVelocity * Time.deltaTime;
        _rotX += _xVelocity * Time.deltaTime;
        _rotX = Mathf.Clamp(_rotX, minXAngle, maxXAngle);

        Quaternion rotation = Quaternion.Euler(_rotX, _rotY, 0);
        Vector3 targetPos = target.position - (rotation * _offset);
        transform.position += (targetPos - transform.position) * smoothingScale * Time.timeScale;

        transform.LookAt(target);
        float yDirection = _yVelocity < 0 ? -1 : 1;
        float xDirection = _xVelocity < 0 ? -1 : 1;
        _yVelocity = Mathf.Max(Mathf.Abs(_yVelocity) - drag * Time.deltaTime, 0);
        _yVelocity *= yDirection;
        _xVelocity = Mathf.Max(Mathf.Abs(_xVelocity) - drag * Time.deltaTime, 0);
        _xVelocity *= xDirection;
    }
    // Update is called once per frame
    void Update()
    {

    }
}
