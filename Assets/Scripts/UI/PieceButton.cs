using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PieceButton : MonoBehaviour
{

    public PieceType pieceType;
    public int matchedAlpha = 80;
    private Text quantityLabel;
    private Button button;
    private Image image;
    private int pieceValue;
    // Use this for initialization
    void Awake()
    {
        quantityLabel = GetComponentInChildren<Text>();
        button = GetComponent<Button>();
        image = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetQuantity(int value)
    {
        if (button)
        {
            SetActive(value > 0);
        }
        quantityLabel.text = value.ToString();
    }

    private void SetActive(bool active)
    {
        if (active)
        {
            button.interactable = true;
        }
        else
        {
            button.interactable = false;
        }
    }

    public void SetMatched(bool active)
    {
        if (image == null)
        {
            return;
        }
        Color c = image.color;
        if (active)
        {
            c.a = matchedAlpha;
        }
        else
        {
            c.a = 0;
        }
        image.color = c;
    }
}
