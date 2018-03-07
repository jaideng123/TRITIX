using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideablePanel : MonoBehaviour
{

    public bool active { get; private set; }

    public void SetActive(bool active)
    {
        this.gameObject.SetActive(active);
        this.active = active;
    }
}
