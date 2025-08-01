using UnityEngine;

public class PlayerInteractor : MonoBehaviour
{
    public void Update()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out var hit) && hit.collider.TryGetComponent<Customer>(out var customer))
        {
            if (Input.GetMouseButtonDown(0))
                customer.AcceptOrder();

            if (Input.GetMouseButtonDown(1))
                customer.CompleteOrder();
        }
    }
}
