using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed = 1f;
    public Transform exitToMoveTo;

    public float health = 2f;

    void Update()
    {
        print("health: " + health);
        if (health <= 0)
        {
            Destroy(gameObject);
        }
        transform.Translate(Vector3.left * speed * Time.deltaTime);
    }

    public void Hit(float dmg)
    {
        health -= dmg;
    }
}
