using System;
using UnityEngine;

public class ViewRecipeTutorial : ArrowTutorialStep
{

    public ViewRecipeTutorial(Recipe recipe) : base(recipe)
    {
        ArrowPosition = recipe.transform.position - new Vector3(0,0.6f,1.2f);
        ArrowRotation = Quaternion.Euler(0, -90, 90);
        TextForCurrentAction = "All the recipes are shown here. Memorize the one you need and press E to continue";
    }

    public override void StartStep()
    {
        Obj.gameObject.SetActive(true);
        base.StartStep();
    }

    public override bool IsStepCompleted()
    {
        return ((Recipe)Obj).IsViewed;
    }
}