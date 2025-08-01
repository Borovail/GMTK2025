using System.Collections.Generic;
using JetBrains.Annotations;
using StarterAssets;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerUI : Singleton<PlayerUI>
{
    [SerializeField] private UIDraggable[] _hands = new UIDraggable[2];
    [SerializeField] private GameObject ServingUI;
    [SerializeField] private CustomerUI CustomerUI;
    [SerializeField] private PlayerInput _playerInput;

    public void Take(int index, Resource resource)
    {
        _hands[index - 1].Initialize(resource);
    }

    public void OpenCocktailMenu(Customer customer)
    {
        _playerInput.enabled = false;
        ServingUI.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        CustomerUI.Initialize(customer);
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
