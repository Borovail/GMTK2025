using UnityEngine;

public class OpenCocktailMenuTutorial : ArrowTutorialStep
{
    public OpenCocktailMenuTutorial(Customer customer) : base(customer)
    {
        ArrowPosition = customer.transform.position + new Vector3(0, 0, 1);
        ArrowRotation = Quaternion.Euler(0, 90, 90);
        TextForCurrentAction = "To make a cocktail press E button";
    }

    public override void StartStep()
    {
        Obj.GetComponent<BoxCollider>().enabled = true;
        base.StartStep();
    }

    public override bool IsStepCompleted()
    {
        return PlayerUI.Instance.IsCocktailMenuOpen;
    }
}