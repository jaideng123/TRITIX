using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerName : MonoBehaviour
{

    public Color defaultColor;
    public Color activeColor;
    private bool _active;
    private Image image;
    private Text nameText;
    // Use this for initialization
    void Awake()
    {
        image = GetComponent<Image>();
        nameText = GetComponentInChildren<Text>();
        _active = false;
        image.color = defaultColor;
    }

    public void SetActive(bool active)
    {
        if (image == null)
        {
            StartCoroutine(SetActiveWhenReady(active));
            return;
        }
        _active = active;
        if (_active)
        {
            image.color = activeColor;
            nameText.color = defaultColor;
        }
        if (!_active)
        {
            image.color = defaultColor;
            nameText.color = activeColor;
        }
    }

    private IEnumerator SetActiveWhenReady(bool value)
    {
        while (image == null)
        {
            yield return null;
        }
        _active = value;
        if (_active)
        {
            image.color = activeColor;
        }
        if (!_active)
        {
            image.color = defaultColor;
        }

    }

    public void SetName(string name)
    {
        nameText.text = name;
    }



}
