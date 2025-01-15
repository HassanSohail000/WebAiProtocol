using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class UIZoom : MonoBehaviour, IScrollHandler
{
    private Vector3 initalScale;
    private float zoomSpeed = 0.1f;
    [SerializeField]
    private float maxZoom = 10f;

    private void Awake()
    {
        initalScale = transform.localScale;
    }

    public void OnScroll(PointerEventData eventData)
    {
        var delta = Vector3.one * (eventData.scrollDelta.y * zoomSpeed);
        var desiredScale = transform.localScale + delta;

        desiredScale = ClampDesiredScale(desiredScale);
        transform.localScale = desiredScale;
    }


    private Vector3 ClampDesiredScale(Vector3 desiredScale)
    {
        desiredScale = Vector3.Max(initalScale, desiredScale);
        desiredScale = Vector3.Min(initalScale * maxZoom, desiredScale);
        return desiredScale;
    }
}
