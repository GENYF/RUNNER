using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeTime : MonoBehaviour {

    public float lifetime = 5.0f;

    private void Start()
    {
        Invoke("Clear_", lifetime);   
    }

    void Clear_()
    {
        Destroy(gameObject);
    }
}
