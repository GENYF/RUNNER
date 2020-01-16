using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileCtrl : MonoBehaviour {

    public float speed = 5.0f;
    public GameObject bang;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Obstacle") && transform.position.x > 10)
        {
            Destroy(other.gameObject);
            Instantiate(bang, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
        if (other.CompareTag("Player"))
        {
            Instantiate(bang, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        transform.position += Vector3.right * speed * Time.deltaTime;
    }
}
