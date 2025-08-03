

using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CollectIngredientsTutorial : ArrowTutorialStep
{
    private List<ResourceProvider> _resourceProviders;

    private int index;

    public CollectIngredientsTutorial(List<ResourceProvider> resourceProviders)
    {
        _resourceProviders = resourceProviders;
        Obj = _resourceProviders[index++];
        ArrowPosition = Obj.transform.position + new Vector3(0, 0, 1);
        ArrowRotation = Quaternion.Euler(0, 90, 90);
        TextForCurrentAction = $"Press {index} button to take ingredient";
    }

    public override void StartStep()
    {
        Obj.gameObject.SetActive(true);
        base.StartStep();
    }

    public override void EndStep()
    {
        Obj.gameObject.SetActive(false);
        base.EndStep();
    }

    public override bool IsStepCompleted()
    {
        if (((ResourceProvider)Obj).IsTaken && index < _resourceProviders.Count)
        {
            EndStep();
            Obj = _resourceProviders[index++];
            TextForCurrentAction = $"Press {index} button to take ingredient";
            ArrowPosition = Obj.transform.position + new Vector3(0, 0, -1);
            ArrowRotation = Quaternion.Euler(0, -90, 90);
            TextForNextAction = "Go to customer";
            StartStep();
            return false;
        }

        return _resourceProviders.Count == index && ((ResourceProvider)Obj).IsTaken;
    }
}