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
        if (health <= 0)
        {
            Destroy(gameObject);
        }
        transform.Translate(Vector3.left * speed * Time.deltaTime);

        // When the enemy leaves the screen destroy it.
        Vector2 screenPosition = Camera.main.WorldToScreenPoint(transform.position);
        if (screenPosition.y > Screen.height || screenPosition.y < 0)
            Destroy(this.gameObject);
        else if (screenPosition.x > Screen.width || screenPosition.x < 0)
            Destroy(this.gameObject);
    }

    public void Hit(float dmg)
    {
        health -= dmg;
    }
}
