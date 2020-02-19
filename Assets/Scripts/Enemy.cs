using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed = 1f;
    public Transform exitToMoveTo;

    public float health = 2f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        print("health: " + health);
        if (health <= 0)
        {
            Destroy(gameObject);
        }
        transform.Translate(Vector3.left * speed * Time.deltaTime);
    }

    public void onHit(float dmg)
    {
        health -= dmg;
    }
}
