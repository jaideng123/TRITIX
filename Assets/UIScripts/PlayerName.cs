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
    void Start()
    {
        image = GetComponent<Image>();
        nameText = GetComponentInChildren<Text>();
        _active = false;
        image.color = defaultColor;
    }

    public void SetActive(bool active)
    {
        _active = active;
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
