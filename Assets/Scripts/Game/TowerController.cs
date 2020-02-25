using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerController : MonoBehaviour
{
    public GameObject projectile;
    List<GameObject> allProjectiles;
    List<GameObject> enemiesInRange;
    public GameObject enemyLockedOnto; // public in case we want enemies that force a tower to lock onto them. (multiplayer?)
    private float timer = 0f;
    public float timeToWait = 1f;

    void Start()
    {
        enemyLockedOnto = null;
        enemiesInRange = new List<GameObject>();
    }

    void Update()
    {
        if (enemiesInRange[0] == null && enemiesInRange.Count > 0)
        {
            enemiesInRange.RemoveAt(0);
            int num = Random.Range(0, enemiesInRange.Count - 1);
            enemyLockedOnto = enemiesInRange[num];
        }
        else if (enemiesInRange != null)
        {
            //int num = Random.Range(0, enemiesInRange.Count - 1);
            enemyLockedOnto = enemiesInRange[0];
        }

        if (enemyLockedOnto != null)
        {
            if (timer >= timeToWait)
            {
                timer = 0;
                // create a projectile
                GameObject proj = Instantiate(projectile, transform.position, Quaternion.identity);
                proj.GetComponent<Projectile>().SetTarget(enemyLockedOnto);
            }
            else
            {
                timer += Time.deltaTime;
            }
        }
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy")
        {
            enemiesInRange.Add(collision.gameObject);
            /*if (enemyLockedOnto == null)
            {
                enemyLockedOnto = collision.gameObject;
                Debug.Log("Enemy Locked on to: " + enemyLockedOnto);
            } */
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        enemiesInRange.Remove(collision.gameObject);
    }
}
