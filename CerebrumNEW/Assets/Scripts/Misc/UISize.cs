using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class UISize : MonoBehaviour
{
    private RectTransform objTransform;
    private Vector3 originalSize;
    private RectTransform o;
    // Start is called before the first frame update
    void Start()
    {
        o = transform.GetComponent<RectTransform>();
        originalSize = o.localScale;
    }

    public void Grow()
    {
        o.DOScale(originalSize, 0.5f);
    }

    public void Shrink()
    {
        o.DOScale(Vector3.zero, 0.5f);
    }
}
