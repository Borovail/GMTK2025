using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
public class UIDraggable : MonoBehaviour,
    IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Image _image;
    private Canvas _canvas;
    [HideInInspector] public Vector3 PositionAfterDrag;
    [HideInInspector] public Resource Resource;

    [HideInInspector] public bool IsDropSuccessful;

    public void Initialize(Resource resource)
    {
        Resource = resource;
        var color = _image.color;
        color.a = 255;
        _image.color = color;
        _image.sprite = resource.sprite;
        _image.SetNativeSize();
    }

    private void Awake()
    {
        _image = GetComponent<Image>();
        _canvas = GetComponentInParent<Canvas>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        PositionAfterDrag = transform.position;
        transform.SetAsLastSibling();
        _image.raycastTarget = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.localPosition += (Vector3)eventData.delta / _canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // var pointerData = new PointerEventData(EventSystem.current) {  position = eventData.position };
        // var results = new List<RaycastResult>();
        // _canvas.GetComponent<GraphicRaycaster>().Raycast(pointerData, results);

        transform.position = PositionAfterDrag;
        _image.raycastTarget = true;
    }

    public void DropSuccessful()
    {
        var color = _image.color;
        color.a = 0;
        _image.color = color;
        IsDropSuccessful = true;
    }
}
