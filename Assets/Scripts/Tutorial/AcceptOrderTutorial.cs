using System;
using UnityEngine;
using UnityEngine.TextCore;

public class AcceptOrderTutorial : ArrowTutorialStep
{
    public AcceptOrderTutorial(Customer customer) : base(customer)
    {
        ArrowPosition = customer.transform.position + new Vector3(0, 0, 1);
        ArrowRotation = Quaternion.Euler(0, 90, 90);
        TextForCurrentAction = "To accept an order press E button";
        TextForNextAction = "Look at the recipe behind you";
    }

    public override void EndStep()
    {
        base.EndStep();
        Obj.GetComponent<BoxCollider>().enabled = false;
        CircleManager.Instance.SetBarrier(false);
    }

    public override bool IsStepCompleted()
    {
        return ((Customer)Obj).IsOrderAccepted;
    }
}
