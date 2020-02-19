using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UIElements;

public class WaterDash : MonoBehaviour
{
    public float chargeRate;
    public float maxChargeDistance;
    public float travelTime;
    public GameObject targetPrefab;
    private GameObject _instantiatedPrefab;
    private float _chargeDistance;
    private bool _inWater;

    private void Start()
    {
        _instantiatedPrefab = null;
        _chargeDistance = 2;
    }

    private void Update()
    {
        if (_inWater)
        {
            // Charge distance while holding
            if (Input.GetMouseButton(0))
            {
                _chargeDistance = Math.Min(maxChargeDistance,  _chargeDistance + Time.deltaTime * chargeRate);
                UpdateTarget();
            }
            // Dash on Release
            if (Input.GetMouseButtonUp(0))
            {
                DestroyTarget();
                Dash();
            }
        }
    }

    private void Dash()
    {
        transform.DOMove(GetDashDestination(), travelTime);
        _chargeDistance = 2;
    }

    private Vector3  GetDashDestination()
    {
        return transform.position + transform.forward.normalized * _chargeDistance;
    }
    
    private void UpdateTarget()
    {
        if (_instantiatedPrefab == null)
        {
            _instantiatedPrefab = Instantiate(targetPrefab, GetDashDestination(), Quaternion.identity);
        }
        else
        {
            _instantiatedPrefab.transform.position = GetDashDestination();
        }
    }

    private void DestroyTarget()
    {
        Destroy(_instantiatedPrefab);
        _instantiatedPrefab = null;
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Water"))
        {
            _inWater = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Water"))
        {
            _inWater = false;
            _chargeDistance = 2;
            DestroyTarget();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(GetDashDestination(), travelTime);
    }
}
