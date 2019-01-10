using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideableDrawer : MonoBehaviour
{
    public float duration = 1f;
    public float hiddenOffset;
    public float scaleFactor;
    public bool visible { get; private set; }
    private Vector3 targetPos;
    // Use this for initialization
    void Start()
    {
        hiddenOffset = calculatOffset();
        var rectTransform = GetComponent<RectTransform>();
        Vector3 pos = rectTransform.position;
        pos.y -= hiddenOffset;
        transform.position = pos;
        targetPos = pos;
        visible = false;
    }

    private float calculatOffset()
    {
        var rectTransform = GetComponent<RectTransform>();
        Canvas mainCanvas = GameObject.Find("UI Canvas").GetComponent<Canvas>();
        scaleFactor = mainCanvas.scaleFactor;
        float offset = (rectTransform.rect.height / 2) + 10;
        offset *= scaleFactor * 3;
        return offset;
    }

    public void Open()
    {
        if (!visible)
        {
            Vector3 pos = transform.position;
            pos.y += hiddenOffset;
            targetPos = pos;
            StartCoroutine(SmoothMove(transform.position, targetPos, duration));
        }
        visible = true;
    }

    public void Close()
    {
        if (visible)
        {
            Vector3 pos = transform.position;
            pos.y -= hiddenOffset;
            targetPos = pos;
            StartCoroutine(SmoothMove(transform.position, targetPos, duration));
        }
        visible = false;
    }

    private IEnumerator SmoothMove(Vector3 startpos, Vector3 endpos, float seconds)
    {
        float t = 0f;
        while (t <= 1.0)
        {
            t += Time.deltaTime / seconds;
            transform.position = Vector3.Lerp(startpos, endpos, Mathf.SmoothStep(0.0f, 1.0f, t));
            yield return null;
        }
    }

}