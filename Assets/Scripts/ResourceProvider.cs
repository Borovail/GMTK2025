using UnityEngine;
using UnityEngine.Events;

public class ResourceProvider : Lookable
{
    public Resource Resource;
    [HideInInspector] public bool IsTaken;
}
