using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideableDrawer : MonoBehaviour
{
    public float duration = 1f;
    private float hiddenOffset;
    public bool visible { get; private set; }
    private Vector3 targetPos;
    // Use this for initialization
    void Start()
    {
        var rectTransform = GetComponent<RectTransform>();
        hiddenOffset = rectTransform.rect.height;
        Vector3 pos = transform.position;
        pos.y -= hiddenOffset;
        transform.position = pos;
        targetPos = pos;
        visible = false;
    }

    // Update is called once per frame
    void Update()
    {
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