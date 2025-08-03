using UnityEngine;
using UnityEngine.Events;

public class PlayerInteractor : MonoBehaviour
{

    private Lookable _lookable;

    public void Update()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out var hit) && hit.collider.TryGetComponent<Lookable>(out var lookable))
        {
            _lookable?.ObjectLookedAway?.Invoke();

            _lookable = lookable;
            _lookable.ObjectLookedAt?.Invoke();

            if (_lookable is Customer customer)
            {
                if (Input.GetKeyDown(KeyCode.E) && customer.IsOrderAccepted)
                    PlayerUI.Instance.OpenCocktailMenu(customer);

                if (Input.GetKeyDown(KeyCode.E) && !customer.IsOrderAccepted)
                    customer.AcceptOrder();

            }
            else if (_lookable is ResourceProvider provider)
            {
                if (Input.GetKeyDown(KeyCode.Alpha1))
                {
                    AudioManager.Instance.PlaySfx(AudioManager.Instance.Ingridient);
                    PlayerUI.Instance.Take(1, provider.Resource);
                    provider.IsTaken = true;
                }
                if (Input.GetKeyDown(KeyCode.Alpha2))
                {
                    AudioManager.Instance.PlaySfx(AudioManager.Instance.Ingridient);
                    PlayerUI.Instance.Take(2, provider.Resource);
                    provider.IsTaken = true;
                }
                if (Input.GetKeyDown(KeyCode.Alpha3))
                {
                    AudioManager.Instance.PlaySfx(AudioManager.Instance.Ingridient);
                    PlayerUI.Instance.Take(3, provider.Resource);
                    provider.IsTaken = true;
                }
                if (Input.GetKeyDown(KeyCode.Alpha4))
                {
                    AudioManager.Instance.PlaySfx(AudioManager.Instance.Ingridient);
                    PlayerUI.Instance.Take(4, provider.Resource);
                    provider.IsTaken = true;
                }
            }
            else if (_lookable is Recipe recipe)
            {
                if (Input.GetKeyDown(KeyCode.E) && !recipe.IsViewed)
                    recipe.IsViewed = true;
            }
        }
        else
            _lookable?.ObjectLookedAway?.Invoke();
    }
}
