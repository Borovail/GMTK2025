using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Net.NetworkInformation;
using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;

public class CircleManager : Singleton<CircleManager>
{
    [SerializeField] private Customer _customerPrefab;
    [SerializeField] private Barrier[] _barriers;
    [SerializeField] private Transform[] _customerPositions;
    [SerializeField] private int _customersOnSide;
    [SerializeField] private CustomerData CustomerData;

    [SerializeField] private List<Customer>[] _customers;

    private int _circleCount;
    private int _index => Convert.ToInt32(_circleCount % 2 != 0);

    public Customer GetPendingOrderCustomer() =>
     _customers[_index].FirstOrDefault(c => c.IsOrderAccepted == false);

    protected override void Awake()
    {
        base.Awake();
        _customers = new List<Customer>[2];
        for (int i = 0; i < 2; i++)
            _customers[i] = new List<Customer>(_customersOnSide);

        SpawnCustomer(CustomerData);
    }

    private void SpawnCustomer(CustomerData customerData)
    {
        var customer = Instantiate(_customerPrefab, _customerPositions[_index].position, _customerPositions[_index].rotation);
        customer.Initialize(customerData);
        _customers[_index].Add(customer);
        customer.Id = _customers[_index].IndexOf(customer);
        customer.OrderAccepted.AddListener(OnOrderAccepted);
        customer.OrderCompleted.AddListener(OnOrderCompleted);
    }

    private void OnOrderCompleted(int Id)
    {
        var customer = _customers[_index][Id];
        customer.OrderAccepted.RemoveListener(OnOrderAccepted);
        customer.OrderCompleted.RemoveListener(OnOrderCompleted);
        _customers[_index].RemoveAt(Id);
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
        if (_customers[_index].Count < _customersOnSide)
            SpawnCustomer(CustomerData);
        else
            if (_customers[_index].All(c => c.IsOrderAccepted))
            _barriers[_index].SetTrigger(true);
    }

    private void OnDisable()
    {
        foreach (var barrier in _barriers) barrier.Exited.RemoveListener(OnBarrierExited);
    }
}
