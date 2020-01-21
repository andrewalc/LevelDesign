using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedProjectileBehvior : MonoBehaviour
{
    public float force;
    public Rigidbody rb;
    public float lifetime;
    // Start is called before the first frame update
    void Start()
    {
        rb.AddForce(transform.forward * force, ForceMode.Impulse);
        StartCoroutine(Lifetime());
    }

    private IEnumerator Lifetime()
    {
        yield return new WaitForSeconds(lifetime);

        Destroy(gameObject);
    }
}
