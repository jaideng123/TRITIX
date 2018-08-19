using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenuController : MonoBehaviour
{
    [SerializeField]
    private Button logoutButton;

    // Use this for initialization
    void Start()
    {
        SetInteractableElements();
    }

    private void SetInteractableElements()
    {
        logoutButton.gameObject.SetActive(Managers.Auth.loggedIn);
    }

    // Update is called once per frame
    void Update()
    {
        SetInteractableElements();
    }
    public void LogOut()
    {
        Managers.Auth.LogOut();
    }
}