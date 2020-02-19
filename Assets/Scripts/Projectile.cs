using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 1f;

    private GameObject target;
    private TowerController tower;
    public float dmg = 1;
    Vector2 move;

    private void Start()
    {
    }
    void Update()
    {
        Debug.Log("Target: " + target);
        if (target != null)
        {
            // move the projectile    
            move = Vector2.MoveTowards(transform.position, target.transform.position, speed * Time.deltaTime);
            transform.position = move;
        }
        else
        {
            Debug.Log("move: " + move);
            transform.Translate(move);
        }
    }

    public void GetTarget()
    {
        target = tower.enemyLockedOnto;
    }

    public void SetTarget(GameObject _target)
    {
        target = _target;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy")
        {
            Debug.Log("Hit Enemy");
            collision.GetComponent<Enemy>().onHit(dmg);
            Destroy(gameObject);
        }
    }
}
