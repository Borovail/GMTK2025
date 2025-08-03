using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Net.NetworkInformation;
using System.Numerics;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class CircleManager : Singleton<CircleManager>
{
    [SerializeField] private Customer _customerPrefab;
    [SerializeField] private Barrier[] _barriers;
    [SerializeField] private List<Transform> _customerPositions;
    [SerializeField] private int _customersOnSide = 5;
    [SerializeField] private List<CustomerData> CustomerData;
    [SerializeField] private List<Customer>[] _customers;
    public int MaxCustomersCount = 20;
    public int CustomersCount;
    public int CustomersServed;

    private int _circleCount;
    private int _index => Convert.ToInt32(_circleCount % 2 != 0);

    public Customer GetPendingOrderCustomer() =>
     _customers[_index].FirstOrDefault(c => c.IsOrderAccepted == false);

    protected override void Awake()
    {
        base.Awake();
        _customers = new List<Customer>[2];

        for (int i = 0; i < _customers.Length; i++)
            _customers[i] = new List<Customer>();

        SpawnCustomer(CustomerData[0]);
    }


    private void SpawnCustomer(CustomerData customerData)
    {
        var index = _customerPositions.Count / 2 * _index + _customers[_index].Count;
        var customer = Instantiate(_customerPrefab, _customerPositions[index].position, _customerPositions[index].rotation);
        customer.Initialize(customerData, 3);
        customer.AddComponent<BoxCollider>();
        _customers[_index].Add(customer);
        customer.Id = _customers[_index].IndexOf(customer);
        customer.OrderAccepted.AddListener(OnOrderAccepted);
        customer.OrderCompleted.AddListener(OnOrderCompleted);
        CustomersCount++;
    }

    private void OnOrderCompleted(int Id)
    {
        var customer = _customers[_index][Id];
        customer.OrderAccepted.RemoveListener(OnOrderAccepted);
        customer.OrderCompleted.RemoveListener(OnOrderCompleted);
        _customers[_index].RemoveAt(Id);
        CustomersServed++;
        if (CustomersCount == MaxCustomersCount)
            PlayerUI.Instance.ShowEndMenu();
    }

    private void OnOrderAccepted()
    {
        _barriers[_index].SetTrigger(true);
    }

    public void SetBarrier(bool value)
    {
        _barriers[_index].SetTrigger(value);
    }

    private void OnEnable()
    {
        foreach (var barrier in _barriers) barrier.Exited.AddListener(OnBarrierExited);
    }

    private void OnBarrierExited()
    {
        for (int i = 0; i < 2; i++)
        {
            var snapshot = _customers[i].ToList();
            snapshot.ForEach(customer => customer.DecreaseCircleCount());
        }

        _circleCount++;
        SpawnCustomers();
    }

    private void SpawnCustomers()
    {

        if (_customers[_index].Count < _customersOnSide && CustomersCount < MaxCustomersCount)
        {
            var bound = UnityEngine.Random.Range(1, 4);
            for (int i = 0; i < bound; i++)
                SpawnCustomer(CustomerData[UnityEngine.Random.Range(0, CustomerData.Count)]);
        }
        else
           if (_customers[_index].All(c => c.IsOrderAccepted))
            _barriers[_index].SetTrigger(true);
    }

    private void OnDisable()
    {
        foreach (var barrier in _barriers) barrier.Exited.RemoveListener(OnBarrierExited);
    }
}
