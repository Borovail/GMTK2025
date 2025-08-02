using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] private Transform _arrow;
    [SerializeField] private Recipe _recipe;
    [SerializeField] private List<ResourceProvider> _resourceProviders;
    [SerializeField] private Shaker _shaker;
    [SerializeField] private Glass _glass;
    [SerializeField] private CustomerUI _customerUi;

    [SerializeField] private GameObject[] _objectsToActivate;

    private List<ITutorialStep> steps = new();
    private int currentStepIndex = 0;
    private Customer _customer;

    void Start()
    {
        ArrowTutorialStep.Arrow = _arrow;
        // заполнить список шагов, например:
        _customer = CircleManager.Instance.GetPendingOrderCustomer();
        steps.Add(new AcceptOrderTutorial(_customer));
        steps.Add(new ViewRecipeTutorial(_recipe));
        steps.Add(new CollectIngredientsTutorial(_resourceProviders));
        steps.Add(new OpenCocktailMenuTutorial(_customer));
        steps.Add(new GhostTutorialStep(PlayerUI.Instance.GetHandsWithResource(), _shaker.transform));
        steps.Add(new ShakerTutorial(_shaker));
        steps.Add(new GhostTutorialStep(new List<UIDraggable>() { _shaker.Draggable }, _glass.transform));
        steps.Add(new GhostTutorialStep(new List<UIDraggable>() { _glass.Draggable }, _customerUi.transform));
        StartCoroutine(RunTutorial());
    }

    private IEnumerator RunTutorial()
    {
        while (currentStepIndex < steps.Count)
        {
            var step = steps[currentStepIndex];
            step.StartStep();
            // ждём пока игрок не выполнит шаг:
            yield return new WaitUntil(() => step.IsStepCompleted());
            step.EndStep();
            currentStepIndex++;
            yield return null; // на следующий кадр
        }
        OnTutorialCompleted();
    }

    public void ReplayFrom(int stepIndex)
    {
        // можно сбросить состояние шагов ниже stepIndex и запустить корутину с нужного места
        currentStepIndex = stepIndex;
        StartCoroutine(RunTutorial());
    }

    private void OnTutorialCompleted()
    {
        foreach (var obj in _objectsToActivate)
        {
            obj.SetActive(true);
        }

        CircleManager.Instance.SetBarrier(true);
    }
}