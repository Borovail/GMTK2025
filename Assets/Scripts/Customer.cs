using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Customer : MonoBehaviour
{
    [SerializeField] private int _circlesCount = 3;
    [SerializeField] private Text _circleCounterUi;
    public UnityEvent OrderAccepted;
    public UnityEvent<int> OrderCompleted;

    public bool IsOrderAccepted;
    public int Id;

    public void AcceptOrder()
    {
        _circleCounterUi.text = _circlesCount.ToString();
        OrderAccepted?.Invoke();
        IsOrderAccepted = true;
    }

    public void DecreaseCircleCount()
    {
        if (_circlesCount == 1)
        {
            FailOrder();
            return;
        }

        _circlesCount--;
        _circleCounterUi.text = _circlesCount.ToString();
    }

    private void FailOrder()
    {
        Debug.Log("Order failed");
        OrderCompleted?.Invoke(Id);
        Destroy(gameObject);
    }

    public void CompleteOrder()
    {
        Debug.Log("Order completed");
        OrderCompleted?.Invoke(Id);
        Destroy(gameObject);
    }

}
