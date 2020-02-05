using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.Events;

public class TagEventTrigger : MonoBehaviour
{
    public string detectionTag;
    
    public bool enableEnterEvents;
    public bool fireEnterOnce;
    public bool hasFiredEnterOnce;
    public UnityEvent triggerEnterEvents;
    
    public bool enableExitEvents;
    public bool fireExitOnce;
    public bool hasFiredExitOnce;
    public UnityEvent triggerExitEvents;

    private void OnTriggerEnter(Collider other)
    {
        if (!enableEnterEvents) return;
        if (!other.CompareTag(detectionTag)) return; // Tag only
        if (fireEnterOnce && hasFiredEnterOnce) return; // Only Invoke once if the flag is set
        triggerEnterEvents.Invoke();
        hasFiredEnterOnce = true;
    }
    private void OnTriggerExit(Collider other)
    {
        if (!enableExitEvents) return;
        if (!other.CompareTag(detectionTag)) return; // Tag only
        if (fireExitOnce && hasFiredExitOnce) return; // Only Invoke once if the flag is set
        triggerExitEvents.Invoke();
        hasFiredExitOnce = true;
    }
}