using UnityEngine;
using UnityEngine.Events;

public class Barrier : MonoBehaviour
{
    public UnityEvent Exited;
    private BoxCollider _сollider;
    private float _enter;
    private void Awake()
    {
        _сollider = GetComponent<BoxCollider>();
    }


    private void Update()
    {
        GetComponent<MeshRenderer>().enabled = _сollider.isTrigger;
    }

    public void SetTrigger(bool value)
    {
        _сollider.isTrigger = value;
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

        Debug.Log("Выход");
        _сollider.isTrigger = false;
        Exited?.Invoke();
    }

}
