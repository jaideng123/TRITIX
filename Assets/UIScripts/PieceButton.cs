using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PieceButton : MonoBehaviour
{

    public PieceType pieceType;
    private Text quantityLabel;
    private Button button;
    private int pieceValue;
    // Use this for initialization
    void Start()
    {
        quantityLabel = GetComponentInChildren<Text>();
        button = GetComponent<Button>();
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
}
