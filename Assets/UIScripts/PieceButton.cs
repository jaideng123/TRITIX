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
        if (value <= 0)
        {
            button.interactable = false;
        }
        else
        {
            button.interactable = true;
        }
        quantityLabel.text = value.ToString();
    }
}
