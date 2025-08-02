using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CustomerUI : MonoBehaviour, IDropHandler
{
    private Customer _customer;
    public void Initialize(Customer customer)
    {
        _customer = customer;
    }

    public void OnDrop(PointerEventData eventData)
    {
        var glass = eventData.pointerDrag.GetComponent<Glass>();
        glass.Reset();
        _customer.CompleteOrder(glass.Cocktail);
        glass.Draggable.IsDropSuccessful = true;
    }

}