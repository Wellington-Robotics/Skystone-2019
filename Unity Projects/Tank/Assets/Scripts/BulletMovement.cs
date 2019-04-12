using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMovement : MonoBehaviour
{
    float speed = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = GameObject.Find("Tank Turret").transform.position;
        transform.rotation = GameObject.Find("Tank Turret").transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.up * speed;
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
