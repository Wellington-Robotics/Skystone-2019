using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankMovement : MonoBehaviour
{
    float speed = 0.1f;
    int bulletSpeed = 60;
    int timer = 0;
    int chargeTime = 30;
    bool firstShot = false;

    public static int points = 0;

    GameObject enemy;
    GameObject bullet;

    // Start is called before the first frame update
    void Start()
    {
        enemy = Resources.Load("Enemy") as GameObject;
        bullet = Resources.Load("Bullet") as GameObject;

        Instantiate(enemy);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0) * speed;

        Vector3 difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        float angle = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;

        GameObject.Find("Tank Turret").transform.rotation = Quaternion.Euler(0f, 0f, angle - 90);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Instantiate(bullet);
            bullet.SetActive(true);
        }
    }
}
