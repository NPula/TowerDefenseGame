using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed = 1f;
    //public Transform exitToMoveTo;
    
    private float offsetArea = 50f; // for allowing enemies to spawn off the map a little

    public float health = 1f;

    private void Update()
    {
        if (health <= 0)
        {
            Destroy(gameObject);
        }

        DestroyWhenLeavePlayArea();
    }

    public void Hit(float dmg)
    {
        health -= dmg;
    }

    private void DestroyWhenLeavePlayArea()
    {
        // When the enemy leaves the screen destroy it.
        Vector2 screenPosition = Camera.main.WorldToScreenPoint(transform.position);
        if (screenPosition.y > Screen.height + offsetArea || screenPosition.y < 0 - offsetArea)
        {
            Destroy(this.gameObject);
        }
        else if (screenPosition.x > Screen.width + offsetArea || screenPosition.x < 0 - offsetArea)
        {
            Destroy(this.gameObject);
        }
    }
}
