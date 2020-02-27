using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapUIController : MonoBehaviour
{
    [SerializeField]
    private GameObject map;
    private float startingTimeScale;

    // Start is called before the first frame update
    void Start()
    {
        map.SetActive(false);
        startingTimeScale = Time.timeScale;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            map.SetActive(true);
            Time.timeScale = 0;
        }

        if (Input.GetKeyUp(KeyCode.Tab))
        {
            map.SetActive(false);
            Time.timeScale = startingTimeScale;
        }
    }
}
