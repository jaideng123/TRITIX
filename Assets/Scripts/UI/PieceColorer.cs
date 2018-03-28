using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PieceColorer : MonoBehaviour
{

    private Button[] buttons;
    // Use this for initialization
    void Start()
    {
        buttons = GetComponentsInChildren<Button>();
    }

    public void SetButtonColor(Color color)
    {
        foreach (Button btn in buttons)
        {
            ColorBlock colors = btn.colors;
            colors.normalColor = color;
            btn.colors = colors;
        }
    }
}
