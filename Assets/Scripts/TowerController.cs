using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerController : MonoBehaviour
{
    public GameObject projectile;
    List<GameObject> allProjectiles;
    List<GameObject> enemiesInRange;
    public GameObject enemyLockedOnto; // public in case we want enemies that force a tower to lock onto them. (multiplayer?)
    //public float timer = 1000f;
    public float timeToWait = 1f;

    // Start is called before the first frame update
    void Start()
    {
        enemyLockedOnto = null;
        enemiesInRange = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        /*if (enemiesInRange[0] == null && enemiesInRange.Count > 0)
        {
            enemiesInRange.RemoveAt(0);
            foreach (GameObject proj in allProjectiles)
            {
                proj.GetComponent<Projectile>().SetTarget(enemiesInRange[0]);
            }
        }
        else */
            //enemyLockedOnto = enemiesInRange[0];

        // instantiate the projectile
        if (enemyLockedOnto != null)
        {
            timeToWait -= Time.deltaTime;
            if (timeToWait <= 0.0f)
            {
                timeToWait = 1f;
                // create a projectile
                GameObject proj = Instantiate(projectile, transform.position, Quaternion.identity);
                proj.GetComponent<Projectile>().SetTarget(enemyLockedOnto);
                //allProjectiles.Add(proj);
            }
        }
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy")
        {
            //Debug.Log("Triggered!!!!!");
            // only add if the instance 
            //enemiesInRange.Add(collision.gameObject);
            //if (enemyLockedOnto == null)
            //{
                enemyLockedOnto = collision.gameObject;
            //}
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
    }
}
