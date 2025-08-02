using System;
using UnityEngine;

public class AcceptOrderTutorial : ArrowTutorialStep
{
    public AcceptOrderTutorial(Customer customer) : base(customer)
    {
        ArrowPosition = customer.transform.position - Vector3.left * 2;
        ArrowRotation = Quaternion.Euler(0, 0, -90);
        Text = "To accept an order press E button";
    }

    public override bool IsStepCompleted()
    {
        CircleManager.Instance.SetBarrier(false);
        return ((Customer)Obj).IsOrderAccepted;
    }
}
