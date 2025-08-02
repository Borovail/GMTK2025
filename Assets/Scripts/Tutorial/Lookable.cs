using UnityEngine;
using UnityEngine.Events;

public class Lookable : MonoBehaviour
{
    [HideInInspector] public UnityEvent ObjectLookedAt = new();
    [HideInInspector] public UnityEvent ObjectLookedAway = new();
}
