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
    public float minXAngle = -75f;
    public float maxXAngle = 80f;
    private float _rotY;
    private float _rotX;
    private float _currentY;
    private float _currentX;
    private Vector3 _offset;
    private Rigidbody _rigidBody;

    private bool _paused;

    void Awake()
    {
        Messenger<bool>.AddListener(GameEvent.GAME_PAUSED, OnGamePaused);
    }

    void OnDestroy()
    {
        Messenger<bool>.RemoveListener(GameEvent.GAME_PAUSED, OnGamePaused);
    }

    // Use this for initialization
    void Start()
    {
        _rotY = transform.eulerAngles.y;
        _rotX = transform.eulerAngles.x;
        _currentY = transform.eulerAngles.y;
        _currentX = transform.eulerAngles.x;
        _offset = target.position - transform.position;
        SetAngle(35, 45);
    }

    public void SetAngle(float rotX, float rotY)
    {
        _rotX = rotX;
        _rotY = rotY;
        Quaternion rotation = Quaternion.Euler(_rotX, _rotY, 0);
        transform.position = target.position - (rotation * _offset);
        transform.LookAt(target);
    }

    private void OnGamePaused(bool paused)
    {
        _paused = paused;
    }

    void LateUpdate()
    {
        if (_paused)
        {
            return;
        }
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
                horInput = Input.GetAxis("Mouse X") * 3;
                vertInput = Input.GetAxis("Mouse Y") * 3;
            }
        }

        _rotY += horInput * rotSpeed;
        _rotX += vertInput * rotSpeed * -1;
        _rotX = Mathf.Clamp(_rotX, minXAngle, maxXAngle);
        _currentY += (_rotY - _currentY) * smoothingScale;
        _currentX += (_rotX - _currentX) * smoothingScale;
        Quaternion rotation = Quaternion.Euler(_currentX, _currentY, 0);
        Vector3 targetPos = target.position - (rotation * _offset);
        transform.position = targetPos;

        transform.LookAt(target);
    }
    // Update is called once per frame
    void Update()
    {

    }
}
