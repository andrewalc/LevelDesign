using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedProjectiles : MonoBehaviour
{
    public GameObject projectile;
    public float delay;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(FiringSequence());
    }

    private IEnumerator FiringSequence()
    {
        yield return new WaitForSeconds(delay);

        var friend = Instantiate(projectile, transform);

        StartCoroutine(FiringSequence());
    }
}
