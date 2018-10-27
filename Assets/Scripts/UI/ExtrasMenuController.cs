using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExtrasMenuController : MonoBehaviour
{
    public Button nextEnvButton;
    public Button prevEnvButton;
    public Text envNameDisplay;
    public int currentBackdropIndex { get; private set; }
    private Backdrop currentBackdrop;

    void Start()
    {
        UpdateBackdropDisplay();
    }

    private void SetCurrentBackdrop(int index)
    {
        Managers.Backdrop.LoadBackdrop(Managers.Backdrop.availableBackdrops[index]);
        UpdateBackdropDisplay();
    }

    private void UpdateBackdropDisplay()
    {
        currentBackdrop = Managers.Backdrop.currentBackdrop;
        currentBackdropIndex = Managers.Backdrop.getBackdropIndex(currentBackdrop);
        if (currentBackdropIndex >= Managers.Backdrop.availableBackdrops.Length - 1)
        {
            nextEnvButton.interactable = false;
        }
        else
        {
            nextEnvButton.interactable = true;
        }
        if (currentBackdropIndex <= 0)
        {
            prevEnvButton.interactable = false;
        }
        else
        {
            prevEnvButton.interactable = true;
        }
        envNameDisplay.text = currentBackdrop.name;
    }


    public void IncrementEnv()
    {
        if (currentBackdropIndex >= Managers.Backdrop.availableBackdrops.Length - 1)
        {
            Debug.LogWarning("BackDrop Index is already at max");
            return;
        }
        SetCurrentBackdrop(currentBackdropIndex + 1);
    }
    public void DecrementEnv()
    {
        if (currentBackdropIndex <= 0)
        {
            Debug.LogWarning("BackDrop Index is already at min");
            return;
        }
        SetCurrentBackdrop(currentBackdropIndex - 1);
    }

}