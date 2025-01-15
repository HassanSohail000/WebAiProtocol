using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class RectModifier : MonoBehaviour
{
    public RectTransform[] Children;
    public float Offset;

    public ScrollRect srcollRect;

    //public static Action<bool> OnAdjustSize;
    //private float defaultHeight;

    private void OnEnable()
    {
        //OnAdjustSize += AdjustSize;
    }
    private void OnDisable()
    {
        //OnAdjustSize -= AdjustSize;
    }

    public void AdjustSize(bool isSearchPanel)
    {
        if (Children.Length > 0 && !isSearchPanel)
        {
            float TotalHeight = 0;
            foreach (RectTransform rectTransform in Children)
            {
                if (rectTransform.gameObject.activeInHierarchy)
                {
                    TotalHeight += rectTransform.rect.height;
                }
            }
            //if (TotalHeight > defaultHeight)
            //{
            //    MyRect.sizeDelta = new Vector2(MyRect.sizeDelta.x, (TotalHeight) + Offset);
            //}
            srcollRect.verticalNormalizedPosition = 1;

        }
        else
        {
            //MyRect.sizeDelta = new Vector2(MyRect.sizeDelta.x, defaultHeight);
            srcollRect.verticalNormalizedPosition = 1;
            Debug.LogWarning("No Children Found to scale against or Rect Transform not found");
        }
    }
}
