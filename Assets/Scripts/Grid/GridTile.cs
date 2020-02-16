using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;


public class GridTile //: MonoBehaviour
{
    float x, y; // position of the tile in the grid
    private GameObject tileObject, borderObject;
    public GameObject TileObject
    {
        get
        {
            return tileObject;
        }
        set
        {
            GameObject old = tileObject;
            tileObject = value;

            if (old != tileObject)
            {
                //UpdateGrid();
            }
        }
    }
    
    public GameObject BorderObject
    {
        get
        {
            return borderObject;
        }
        set
        {
            GameObject old = borderObject;
            borderObject = value;

            if (old != tileObject)
            {
                //UpdateGrid();
            }
        }
    }

    public GridTile()
    {
        tileObject = null;
        borderObject = null;
    }
}
