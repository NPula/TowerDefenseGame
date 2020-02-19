using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    public int towerSelection;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(instance);
        }
    }
    private void Start()
    {
    }
    public void ChangeTower(int towerID)
    {
        towerSelection = towerID;
    }
}
