using System;
using UnityEngine;

public class ViewRecipeTutorial : ArrowTutorialStep
{

    public ViewRecipeTutorial(Recipe recipe) : base(recipe)
    {
        ArrowPosition = recipe.transform.position - Vector3.left ;
        ArrowRotation = Quaternion.Euler(0, 0, -90);
        Text = "All the recipes are shown here. Memorize the one you need and press E to continue";
    }

    public override bool IsStepCompleted()
    {
        return ((Recipe)Obj).IsViewed;
    }
}