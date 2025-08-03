using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Customer : Lookable
{
    [SerializeField] private int _circlesCount = 3;
    [SerializeField] private Text _circleCounterUi;
    [SerializeField] private Image _cocktailImage;
    public UnityEvent OrderAccepted;
    public UnityEvent<int> OrderCompleted;

    public bool IsOrderAccepted;
    public int Id;

    private Cocktail _cocktail;
    private SpriteRenderer _spriteRenderer;

    public void Initialize(CustomerData customerData, int circlesCount)
    {
        _circlesCount = circlesCount;
        _spriteRenderer.sprite = customerData.Sprite;
        _cocktail = customerData.Cocktail;
        _cocktailImage.sprite = _cocktail.sprite;
    }

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }
    public Sprite GetSprite() => _spriteRenderer.sprite;


    public void AcceptOrder()
    {
        _cocktailImage.gameObject.SetActive(true);
        _circleCounterUi.gameObject.SetActive(true);
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
    }

    public void CompleteOrder(Cocktail cocktail)
    {
        if (cocktail == _cocktail)
        {
            Debug.Log("Order completed");
            OrderCompleted?.Invoke(Id);
        }
        else
            FailOrder();

        StartCoroutine(CloseCocktailMenuAfter(1));
    }

    private IEnumerator CloseCocktailMenuAfter(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        PlayerUI.Instance.CloseCocktailMenu();
        Destroy(gameObject);
    }

}
