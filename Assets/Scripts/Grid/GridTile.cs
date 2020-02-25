﻿using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;


public class GridTile
{
    public float x, y; // position of the tile in the grid
    Action<GridTile> cb;
    public bool tileInUse = false;
    private GameObject tileObject, tileBorder, mapTile;
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
                // Update Grid;
                cb(this);
            }
        }
    }
    
    public GameObject TileBorder
    {
        get
        {
            return tileBorder;
        }
        set
        {
            GameObject old = tileBorder;
            tileBorder = value;

            if (old != tileObject)
            {
                // Update Grid;
                cb(this);
            }
        }
    }

    public GameObject MapTile
    {
        get
        {
            return mapTile;
        }
        set
        {
            GameObject old = mapTile;
            mapTile = value;

            if (old != mapTile)
            {
                cb(this);
            }
        }
    }

    public GridTile()
    {
        tileObject = null;
        tileBorder = null;
    }

    public void RegisterCB(Action<GridTile> cb)
    {
        this.cb += cb;
    }
}
