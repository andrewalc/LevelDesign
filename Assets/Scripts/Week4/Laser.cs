using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityEditor;
using UnityEngine;

public class Laser : MonoBehaviour
{
    public float speed;
    [Range(0f, 20f)]
    public float lifetime;
    public int bouncesRemaining;

    private void Start()
    {
        if (lifetime != 0f)
        {
            Destroy(gameObject, lifetime);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Keep moving forward
        transform.position += speed * Time.deltaTime * transform.forward;
    }

    private void OnCollisionEnter(Collision other)
    {
        bouncesRemaining--;
        if (bouncesRemaining == 0) {
            Destroy(gameObject);
            return;
        } 
        
//        if (other.transform.CompareTag("Mirror"))
//        {
//            // If mirror, reflect to the mirror's current normal
//            transform.rotation = Quaternion.LookRotation(other.contacts[0].normal);
//
//        }
//        else
//        {
            // Reflect off of any other collision
            Vector3 reflectionVector = Vector3.Reflect(transform.forward, other.contacts[0].normal);
            reflectionVector = Vector3.Scale(new Vector3(1, 0, 1), reflectionVector).normalized; // Scale to X-Z plane
            transform.rotation = Quaternion.LookRotation(reflectionVector);
//        }

    }

    private void DrawPredictedReflectionPattern(Vector3 position, Vector3 direction, int reflectionsRemaining)
    {
        if (reflectionsRemaining == 0) {
            return;
        }

        Vector3 startingPosition = position;

        Ray ray = new Ray(position, direction);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 1000))
        {
            direction = Vector3.Reflect(direction, hit.normal);
            direction =  Vector3.Scale(new Vector3(1, 0, 1), direction).normalized; // Scale to X-Z plane
            position = hit.point;
        }
        else
        {
            position += direction * 1000;
        }

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(startingPosition, position);

        DrawPredictedReflectionPattern(position, direction, reflectionsRemaining - 1);
    }

    private void OnDrawGizmos()
    {
//        Gizmos.DrawLine(transform.position, transform.position + (transform.forward.normalized * reflectionRange));
        Handles.color = Color.red;
        Handles.ArrowHandleCap(0, this.transform.position + this.transform.forward * 0.25f, this.transform.rotation, 0.5f, EventType.Repaint);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(this.transform.position, transform.localScale.x);



        DrawPredictedReflectionPattern(this.transform.position + this.transform.forward * 0.75f, this.transform.forward, bouncesRemaining);
    }
}
