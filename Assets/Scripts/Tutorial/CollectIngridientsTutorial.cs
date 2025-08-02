

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
        ArrowPosition = Obj.transform.position + Vector3.up;
        Text = $"Press {index} button to take ingredient";
    }

    public override bool IsStepCompleted()
    {
        if (((ResourceProvider)Obj).IsTaken && index < _resourceProviders.Count)
        {
            EndStep();
            Obj = _resourceProviders[index++];
            Text = $"Press {index} button to take ingredient";
            ArrowPosition = Obj.transform.position + Vector3.up;
            StartStep();
            return false;
        }

        return _resourceProviders.Count == index && ((ResourceProvider)Obj).IsTaken;
    }
}