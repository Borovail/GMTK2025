using UnityEngine;

public class PlayerInteractor : MonoBehaviour
{
    public void Update()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out var hit))
        {
            if (hit.collider.TryGetComponent<Customer>(out var customer))
            {
                if (Input.GetMouseButtonDown(0))
                    customer.AcceptOrder();

                if (Input.GetMouseButtonDown(1))
                    PlayerUI.Instance.OpenCocktailMenu(customer);
            }
            if (hit.collider.TryGetComponent<ResourceProvider>(out var provider))
            {
                if (Input.GetKeyDown(KeyCode.Alpha1))
                    PlayerUI.Instance.Take(1, provider.Resource);
                if (Input.GetKeyDown(KeyCode.Alpha2))
                    PlayerUI.Instance.Take(2, provider.Resource);

            }
        }
    }
}
