using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UIElements;

public class WaterDash : MonoBehaviour
{
    public float chargeRate;
    public float travelTime;
    public GameObject targetPrefab;
    private GameObject _instantiatedPrefab;
    private float _chargeDistance;

    private void Start()
    {
        _chargeDistance = 2;
        _instantiatedPrefab = null;
    }

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            _chargeDistance += Time.deltaTime * chargeRate;
            if (_instantiatedPrefab == null)
            {
                _instantiatedPrefab = Instantiate(targetPrefab, GetDashDestination(), Quaternion.identity);
            }
            else
            {
                _instantiatedPrefab.transform.position = GetDashDestination();
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            Destroy(_instantiatedPrefab);
            _instantiatedPrefab = null;
            Dash();
        }
    }

    private Vector3  GetDashDestination()
    {
        return transform.position + transform.forward.normalized * _chargeDistance;
    }

    private void Dash()
    {
        transform.DOMove(GetDashDestination(), travelTime);
        _chargeDistance = 2;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(GetDashDestination(), travelTime);
    }
}
