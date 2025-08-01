using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Glass : MonoBehaviour, IDropHandler
{
    [SerializeField] private Sprite _defaultSprite;
    [HideInInspector] public Cocktail Cocktail;
    private Image _image;
    public UIDraggable Draggable;


    private void Awake()
    {
        _image = GetComponent<Image>();
        Draggable = GetComponent<UIDraggable>();
    }

    public void Reset()
    {
        _image.sprite = _defaultSprite;
    }

    public void OnDrop(PointerEventData eventData)
    {
        var shaker = eventData.pointerDrag.GetComponent<Shaker>();
        Cocktail = shaker.Cocktail;
        _image.sprite = Cocktail.sprite;
        shaker.Draggable.IsDropSuccessful = true;
    }
}