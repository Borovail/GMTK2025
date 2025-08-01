using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Net.NetworkInformation;
using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;

public class CircleManager : MonoBehaviour
{
    [SerializeField] private Customer _customerPrefab;
    [SerializeField] private Barrier[] _barriers;
    [SerializeField] private Transform[] _customerPositions;
    [SerializeField] private int _customersOnSide;

    private List<Customer>[] _customers;

    private int _circleCount;
    private int _index => Convert.ToInt32(_circleCount % 2 != 0);

    private void Start()
    {
        _customers = new List<Customer>[2];
        for (int i = 0; i < 2; i++)
            _customers[i] = new List<Customer>(_customersOnSide);

        SpawnCustomer();
    }

    private void SpawnCustomer()
    {
        var customer = Instantiate(_customerPrefab, _customerPositions[_index].position, _customerPositions[_index].rotation);
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
            SpawnCustomer();
        else
            if (_customers[_index].All(c => c.IsOrderAccepted))
            _barriers[_index].SetTrigger(true);
    }

    private void OnDisable()
    {
        foreach (var barrier in _barriers) barrier.Exited.RemoveListener(OnBarrierExited);
    }
}
