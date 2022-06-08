using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class destroyObject : MonoBehaviour
{
    float delay = 2.5f;

    private void Start()
    {
        Invoke("Destroy", delay);
    }

    void Destroy()
    {
        Destroy(gameObject);
    }
}
