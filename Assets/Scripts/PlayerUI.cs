using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using StarterAssets;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerUI : Singleton<PlayerUI>
{
    [SerializeField] private List<UIDraggable> _hands = new();
    [SerializeField] private GameObject ServingUI;
    [SerializeField] private CustomerUI CustomerUI;
    [SerializeField] private PlayerInput _playerInput;
    [SerializeField] private Text _text;

    public bool IsCocktailMenuOpen;

    public void Start()
    {
        ServingUI.SetActive(false);
    }

    public void Take(int index, Resource resource)
    {
        _hands[index - 1].Initialize(resource);
    }

    public void Text(bool show = true, string text = "")
    {
        _text.gameObject.SetActive(show);
        _text.text = text;
    }

    public IEnumerable<UIDraggable> GetHandsWithResource()
       => _hands.Where(h => h.Resource != null);

    public void OpenCocktailMenu(Customer customer)
    {
        _playerInput.enabled = false;
        ServingUI.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        CustomerUI.Initialize(customer);
        IsCocktailMenuOpen = true;
    }

    public void CloseCocktailMenu()
    {
        ServingUI.SetActive(false);
        _playerInput.enabled = true;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // private void Update()
    // {
    //     if (Input.GetKeyDown(KeyCode.E))
    //     {
    //         _playerInput.enabled = false;
    //         ServingUI.SetActive(true);
    //         Cursor.lockState = CursorLockMode.None;
    //     }
    //     if (Input.GetKeyDown(KeyCode.R))
    //     {
    //         ServingUI.SetActive(false);
    //         _playerInput.enabled = true;
    //         Cursor.lockState = CursorLockMode.Locked;
    //     }
    // }
}
