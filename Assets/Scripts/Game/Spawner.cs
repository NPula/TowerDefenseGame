using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject entity;
    public float timer = 3;
    private float timeToWait = 0;

    void Update()
    {
        if (GameManager.instance.gameState == GameManager.Gamestate.play)
        {
            if (timeToWait >= timer)
            {
                Instantiate(entity, transform.position, Quaternion.identity);
                timeToWait = 0;
            }
            else
            {
                timeToWait += Time.deltaTime;
            }
        }
    }
}
