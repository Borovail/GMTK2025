using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
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
    [SerializeField] private FirstPersonController _playerController;
    [SerializeField] private Text _text;
    [SerializeField] private Text _clientsServed;

    [Header("UI Groups (CanvasGroup for fade)")]
    [SerializeField] private CanvasGroup overlayPanel;
    [SerializeField] private CanvasGroup initialMessageGroup;
    [SerializeField] private CanvasGroup okButtonGroup;

    [Header("Durations")]
    [SerializeField] private float panelFadeDuration = 1f;
    [SerializeField] private float messageFadeDuration = 0.5f;
    [SerializeField] private float messageDisplayDuration = 2f;
    [SerializeField] private float statsFadeDuration = 0.5f;

    public bool IsCocktailMenuOpen;
    private Sequence _sequence;

    private void Update()
    {
        _clientsServed.text = $"Clients served {CircleManager.Instance.CustomersServed}/{CircleManager.Instance.MaxCustomersCount}";
    }

    protected override void Awake()
    {
        base.Awake();
        // initial states
        overlayPanel.alpha = 0;
        initialMessageGroup.alpha = 0;
        okButtonGroup.alpha = 0;

        // disable interactive until shown
        overlayPanel.interactable = false;
        okButtonGroup.interactable = false;
    }
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
        Cursor.lockState = CursorLockMode.None;
        Vector3 dirRot = customer.transform.position - _playerController.CinemachineCameraTarget.transform.position;
        Quaternion rot = Quaternion.LookRotation(dirRot);
        _playerController.CinemachineCameraTarget.transform.rotation = rot;
        Vector3 dirPos = -customer.transform.forward;
        _playerController.transform.position = customer.transform.position + dirPos * 3.2f;
        ServingUI.SetActive(true);
        CustomerUI.Initialize(customer);
        IsCocktailMenuOpen = true;
    }

    public void CloseCocktailMenu()
    {
        ServingUI.SetActive(false);
        _playerInput.enabled = true;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void ShowEndMenu()
    {
        _playerController.enabled = false;
        Cursor.lockState = CursorLockMode.None;
        _sequence?.Kill();

        // Activate panel gameObjects if needed
        overlayPanel.gameObject.SetActive(true);
        initialMessageGroup.gameObject.SetActive(true);
        okButtonGroup.gameObject.SetActive(true);

        _sequence = DOTween.Sequence()
            // 1. Fade in overlay panel to full black
            .Append(overlayPanel.DOFade(1f, panelFadeDuration))
            // 2. Fade in initial message
            .Append(initialMessageGroup.DOFade(1f, messageFadeDuration))
            // 3. Wait while message is visible
            .AppendInterval(messageDisplayDuration)
            // 5. Fade in stats container
            .Append(okButtonGroup.DOFade(1f, messageFadeDuration))
            // 8. Enable interactions on OK button
            .AppendCallback(() => okButtonGroup.interactable = true)
            ;

        // Subscribe OK button click
        var okButton = okButtonGroup.GetComponentInChildren<Button>();
        if (okButton != null)
        {
            okButton.onClick.RemoveAllListeners();
            okButton.onClick.AddListener(OnOkClicked);
        }
    }

    private void OnOkClicked()
    {
        Application.Quit();
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
