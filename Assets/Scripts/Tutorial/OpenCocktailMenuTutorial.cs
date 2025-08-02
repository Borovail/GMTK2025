using UnityEngine;

public class OpenCocktailMenuTutorial : ArrowTutorialStep
{
    public OpenCocktailMenuTutorial(Customer customer) : base(customer)
    {
        ArrowPosition = customer.transform.position - Vector3.left * 2;
        ArrowRotation = Quaternion.Euler(0, 0, -90);
        Text = "To make a cocktail press E button";
    }

    public override bool IsStepCompleted()
    {
        return PlayerUI.Instance.IsCocktailMenuOpen;
    }
}