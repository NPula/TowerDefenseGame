﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject entity;
    public float timer = 3;
    private float timeToWait = 0;

    // Start is called before the first frame update
    void Start()
    {
                
    }

    // Update is called once per frame
    void Update()
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
