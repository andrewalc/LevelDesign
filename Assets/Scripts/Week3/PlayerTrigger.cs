using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTrigger : MonoBehaviour
{
    public bool fireOnce;
    public bool trigger;
    public bool hasFired;

    public bool Active()
    {
        return fireOnce ? hasFired : trigger;
    }
    public void LateUpdate()
    { 
        trigger = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (fireOnce)
            {
                if (!hasFired)
                {
                    print("FIRED");
                    hasFired = true;
                    trigger = true;
                }
            } else
            {
                trigger = true;
            }
        }
    }
}