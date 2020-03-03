using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerController : MonoBehaviour
{
    public GameObject projectile;
    private List<GameObject> enemiesInRange;
    public GameObject enemyLockedOnto; 

    private float timer = 0f;
    public float timeToWait = 1f;

    void Start()
    {
        enemyLockedOnto = null;
        enemiesInRange = new List<GameObject>();
    }

    void Update()
    {
        // only run if the is something in the list.
        if (enemiesInRange.Count > 0)
        {
            if (enemiesInRange[0] == null)
            {
                enemiesInRange.RemoveAt(0);
                int num = Random.Range(0, enemiesInRange.Count - 1);
                enemyLockedOnto = enemiesInRange[num];
            }
            else
            {
                enemyLockedOnto = enemiesInRange[0];
            }

            Vector3 distance = (enemiesInRange[0].transform.position - transform.position).normalized;
            float angle = (Mathf.Atan2(distance.y, distance.x) * Mathf.Rad2Deg);
            transform.rotation = Quaternion.Euler(0f, 0f, 90 + angle);
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
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        enemiesInRange.Remove(collision.gameObject);
    }
}
