using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityEditor;
using UnityEngine;

public class Laser : MonoBehaviour
{
    public float speed;
    [Range(0f, 35f)]
    public float lifetime;
    public int bouncesRemaining;
    public LayerMask reflectionSurface;
    // When enabled, laser will bounce in direction of collision/ray surface's 'normal vector
    // without accounting for its original angle of collision.
    public bool normalVectorReflectionMode; 

    private void Start()
    {
        if (Math.Abs(lifetime) > 0f)
        {
            Destroy(gameObject, lifetime);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        RaycastReflection();
        // Keep moving forward
        transform.position += speed * Time.deltaTime * transform.forward;
    }
    
    
    private void RaycastReflection()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 1f, reflectionSurface))
        {
            bouncesRemaining--;
            if (!hit.transform.CompareTag("Mirror") || bouncesRemaining == 0) {
                Destroy(gameObject);
                return;
            }
            // Reflect at the mirrors reflection point for consistency
            MirrorControl mirrorControl = hit.transform.GetComponent<MirrorControl>();
            transform.position = mirrorControl.reflectionPoint.position;
            // Reflection vector depends on the mode set in the inspector
            Vector3 reflectionVector = normalVectorReflectionMode ? hit.normal : Vector3.Reflect(transform.forward, hit.normal);
            // If the verticle angle is neutral, scale to the X-Z plane
            if (mirrorControl.IsVerticleNeutral())
            {
                reflectionVector = Vector3.Scale(new Vector3(1, 0, 1), reflectionVector).normalized;
            }
            // Finally, set the rotation
            transform.rotation = Quaternion.LookRotation(reflectionVector);
        }
    }
    
    /**
     * In case our raycast fails, collision will reflect. A physical reflection is not guaranteed to match the editor
     * Gizmo preview.
     */
    private void OnCollisionEnter(Collision other)
    {
        bouncesRemaining--;
        if (!other.transform.CompareTag("Mirror") || bouncesRemaining == 0) {
            Destroy(gameObject);
            return;
        }
        // Reflect at the mirrors reflection point for consistency
        MirrorControl mirrorControl = other.transform.GetComponent<MirrorControl>();
        transform.position = mirrorControl.reflectionPoint.position;
        // Reflection vector depends on the mode set in the inspector
        Vector3 reflectionVector = normalVectorReflectionMode
            ? other.contacts[other.contacts.Length - 1].normal
            : Vector3.Reflect(transform.forward, other.contacts[other.contacts.Length - 1].normal);
        // If the verticle angle is neutral, scale to the X-Z plane
        if (mirrorControl.IsVerticleNeutral())
        {
            reflectionVector = Vector3.Scale(new Vector3(1, 0, 1), reflectionVector).normalized;
        }
        // Finally, set the rotation
        transform.rotation = Quaternion.LookRotation(reflectionVector);
    }


    /**
     * Recursive function for rendering a predictive editor view reflection line Gizmo. 
     * World of Zero: https://www.youtube.com/watch?v=GttdLYKEJAM
     * == Modified to match laser parameters and scale to the X-Z plane. ==
     */
    private void DrawPredictedReflectionPattern(Vector3 position, Vector3 direction, int reflectionsRemaining)
    {
        if (reflectionsRemaining == 0) {
            return;
        }
        Vector3 startingPosition = position;
        Ray ray = new Ray(position, direction);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 1000, reflectionSurface))
        {
            MirrorControl mirrorControl = hit.transform.GetComponent<MirrorControl>();
            direction = normalVectorReflectionMode ? hit.normal : Vector3.Reflect(direction, hit.normal);
            if (mirrorControl.IsVerticleNeutral())
            {
                direction =  Vector3.Scale(new Vector3(1, 0, 1), direction).normalized; // Scale to X-Z plane
            }
            position = hit.point;
        }
        else
        {
            position += direction * 1000;
        }

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(startingPosition, position);
        if (hit.transform != null)
        {
            position = hit.transform.GetComponent<MirrorControl>().reflectionPoint.position; // reposition for angle consistency
        }

        DrawPredictedReflectionPattern(position, direction, reflectionsRemaining - 1);
    }

    private void OnDrawGizmos()
    { 
        Handles.color = Color.red;
        Handles.ArrowHandleCap(0, this.transform.position + this.transform.forward * 0.25f, this.transform.rotation, 0.5f, EventType.Repaint);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(this.transform.position, transform.localScale.x);
        Gizmos.color = Color.cyan;
        Gizmos.DrawRay(transform.position, transform.forward);
        DrawPredictedReflectionPattern(this.transform.position + this.transform.forward * 0.75f, this.transform.forward, bouncesRemaining);
    }
}
