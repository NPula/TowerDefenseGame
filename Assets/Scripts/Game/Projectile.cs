using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 1f;

    private GameObject target;
    private TowerController tower;
    public float dmg = 1;
    Vector3 move;
    
    void Update()
    {
        MoveProjectile();

        // When the projectile leaves the screen destroy it.
        Vector2 screenPosition = Camera.main.WorldToScreenPoint(transform.position);
        if (screenPosition.y > Screen.height || screenPosition.y < 0)
            Destroy(this.gameObject);
        else if (screenPosition.x > Screen.width || screenPosition.x < 0)
            Destroy(this.gameObject);
    }

    private void MoveProjectile()
    {
        if (target != null)
        {
            Debug.Log(target.transform.position);
            // move the projectile    
            Vector2 calcHeading = target.transform.position - transform.position;
            move = calcHeading.normalized;
            //Debug.Log("Move: " + move);
            //Debug.Log("target: " + target);
            transform.position += move * speed * Time.deltaTime;
        }
        else
        {
            //Debug.Log("move: " + move);
            Vector3 m = move;
            transform.position += m * speed * Time.deltaTime;
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
            //Debug.Log("Hit Enemy");
            collision.GetComponent<Enemy>().Hit(dmg);
            Destroy(gameObject);
        }
    }
}
