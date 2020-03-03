using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    public GameObject[] mapTiles;
    public GameObject currentTile;
    public int towerSelection = -1;
    
    public enum Gamestate {edit, test, play}
    public Gamestate gameState = Gamestate.play;

    public string map = "assets/resources/map.txt";

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

    public void ChangeTower(int towerID)
    {
        towerSelection = towerID;
    }

    public void ChangeMapTile(string name)
    {
        for (int i = 0; i < mapTiles.Length; i++)
        {
            if (mapTiles[i].name == name)
            {
                currentTile = mapTiles[i];
                i = mapTiles.Length;
            }
        }

        Debug.Log(currentTile);
    }
}
