using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Path : MonoBehaviour
{
    public List<Transform> path;

    float speed = 2f;
    int index = 0;
    Vector3 offset;
  
    void Start()
    {
        offset = Vector3.zero;
        path = new List<Transform>();
        index = 0;

        GetPath();

        // get intial rotation
        Vector3 distance = (path[index].position - transform.position).normalized;
        float angle = (Mathf.Atan2(distance.y, distance.x) * Mathf.Rad2Deg);
        transform.rotation = Quaternion.Euler(0f, 0f, angle-90);
    }

    void Update()
    {
        if (index <= path.Count)
        {
            if (transform.position != path[index].position + offset)
            {        
                transform.position = Vector3.MoveTowards(transform.position, path[index].position, speed * Time.deltaTime);
            }
            else
            {
                index++;

                // get and set the new rotation
                Vector3 distance = (path[index].position - transform.position).normalized;
                float angle = (Mathf.Atan2(distance.y, distance.x) * Mathf.Rad2Deg);
                transform.rotation = Quaternion.Euler(0f, 0f, angle-90);
                transform.position = Vector3.MoveTowards(transform.position, path[index].position, speed * Time.deltaTime);
            }
        } 
    }

    void GetPath()
    {
        GameObject _path = GameObject.Find("Path");
        for(int i = 0; i < _path.transform.childCount; i++)
        {
            path.Add(_path.transform.GetChild(i));
        }
    }
}
