using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchTarget : MonoBehaviour
{

    public void Triggered()
    {
        transform.parent.transform.SendMessage("Triggered", SendMessageOptions.DontRequireReceiver);
    }
}
