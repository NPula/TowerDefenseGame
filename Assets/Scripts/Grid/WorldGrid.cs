using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldGrid
{
    GridTile[,] tiles;

    private int width, height;
    public int Width { get => width; set => width = value; }
    public int Height { get => height; set => height = value; }

    public WorldGrid(int _width = 10, int _height = 10)
    {
        this.width = _width;
        this.height = _height;
        tiles = new GridTile[_width, _height];

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                tiles[i, j] = new GridTile();
                tiles[i, j].TileObject = null;
                Debug.Log("added!");
            }
        }
    }

    public GridTile GetTile(int x, int y)
    {
        return tiles[x, y];
    }

    public void SetTileType(int x, int y, GameObject type)
    {
        tiles[x, y].TileObject = type;
    }

    public void SetTileBorder(int x, int y, GameObject border)
    {
        tiles[x, y].BorderObject = border;
    }
}
