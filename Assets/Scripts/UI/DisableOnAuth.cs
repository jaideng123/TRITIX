using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisableOnAuth : MonoBehaviour
{
    [SerializeField]
    private bool enable;

    [SerializeField]
    private bool fade;
    [SerializeField]
    private bool hide;
    private Button _button;
    // Use this for initialization
    void Start()
    {
        _button = GetComponent<Button>();
        SetVisibility();
    }

    // Update is called once per frame
    void Update()
    {
        SetVisibility();
    }

    private void SetVisibility()
    {
        if (Managers.Auth.loggedIn)
        {
            if (fade)
            {
                _button.interactable = enable;
            }
            if (hide)
            {
                Debug.Log(enable);
                transform.localScale = Vector3.zero;

            }
        }
        else
        {
            if (fade)
            {
                _button.interactable = !enable;
            }
            if (hide)
            {
                gameObject.SetActive(!enable);
            }
        }
    }
}
