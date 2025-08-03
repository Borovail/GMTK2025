using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class Barrier : MonoBehaviour
{
    [HideInInspector] public UnityEvent Exited;
    private BoxCollider _сollider;
    private float _enter;
    private ParticleSystem _fog;
    private void Awake()
    {
        _сollider = GetComponent<BoxCollider>();
        _fog = GetComponentInChildren<ParticleSystem>();
    }

    public void SetTrigger(bool value)
    {
        Debug.Log(value);
        _сollider.isTrigger = value;
        if (!value)
            _fog.Play();
        else
            _fog.Stop(true, ParticleSystemStopBehavior.StopEmitting);
    }


    protected void OnTriggerEnter(Collider other)
    {
        Vector3 dir = other.transform.position - transform.position;
        _enter = Vector3.Dot(dir, transform.forward);
    }

    protected void OnTriggerExit(Collider other)
    {
        Vector3 dir = other.transform.position - transform.position;
        float exit = Vector3.Dot(dir, transform.forward);

        if (exit * _enter >= 0) return;

        SetTrigger(false);
        Exited?.Invoke();
    }

}
