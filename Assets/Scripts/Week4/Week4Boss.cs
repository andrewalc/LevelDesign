using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Week4Boss : MonoBehaviour
{

    public UnityEvent onCrystalEnter;

    private List<GameObject> collidedCrystals;
    // Start is called before the first frame update
    void Start()
    {
     collidedCrystals = new List<GameObject>();   
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Crystal") && !collidedCrystals.Contains(other.gameObject))
        {
            collidedCrystals.Add(other.gameObject);
            onCrystalEnter.Invoke();
        }
    }
}
