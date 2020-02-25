using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class GridEditor : MonoBehaviour
{


    public GameObject gridImage;
    public GameObject highLightImage;
    public GameObject[] tower;
    public GameObject towerToPlace;
    public GameObject grass;

    public string fileName = "map.dat";


    public GridTile[,] tiles;

    public int width, height = 10;
    private int prevWidth, prevHeight;

    //public int Width { get => width; set => width = value; }
    //public int height { get => height; set => height = value; }

    public void CreateGrid(int _width = 10, int _height = 10)
    {
        this.width = _width;
        this.height = _height;
        tiles = new GridTile[_width, _height];

        Debug.Log(width);
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                Vector3 position = new Vector3(i - ((width - 1f) / 2), j - ((height - 1f) / 2), 0);
                tiles[i, j] = new GridTile();
                tiles[i, j].TileObject = null;
                tiles[i, j].TileBorder = Instantiate(gridImage, position, Quaternion.identity);
                tiles[i, j].MapTile = null;
                
            }
        }
    }
    private void Start()
    {
        prevWidth = width;
        prevHeight = height;
        CreateGrid(width, height);
        //DisplayGrid();
    }

    void Resize(int _width, int _height)
    {
        Debug.Log("Resizing");
        for (int i = 0; i < prevWidth; i++)
        {
            for (int j = 0; j < prevHeight; j++)
            {
                Destroy(tiles[i, j].TileBorder);
            }
        }
        CreateGrid(_width, _height);
    }


    void Update()
    {
        prevWidth = width;
        prevHeight = height;
        //towerToPlace = tower[GameManager.instance.towerSelection];
        //HighlightGridWhenMousedOver();
        if (tiles == null)
        {
            CreateGrid(width, height);
        }

        if (prevWidth != width || prevHeight != height)
        {
            Resize(width, height);
        }
       

        Vector2 mouseCellPos;
        if (Input.GetMouseButtonDown(0))
        {
            mouseCellPos = GetMousePositionInGrid();
            if (MouseIsInGrid(mouseCellPos))
            {
                //SetTileType((int)mouseCellPos.x, (int)mouseCellPos.y, highLightImage);
                SetTileType((int)mouseCellPos.x, (int)mouseCellPos.y, GameManager.instance.currentTile);
            }
        } 

        if (Input.GetMouseButtonDown(1))
        {
            MapToFile();
        }
    }


    void DisplayGrid()
    {
        // Create the main grid for renderering all objects.
        //grid = new WorldGrid(Width, height);
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                // Create the inital game objects used for rendering the grid and the towers
                GameObject tileBlock = new GameObject(i.ToString() + ", " + j.ToString());
                GameObject tileBorder = new GameObject("Border");
                GameObject tileObject = new GameObject("Tile");
                GameObject tileMap = new GameObject("Map");

                // Set the position of the grid cells on the screen. (Centered)
                tileBorder.transform.position = new Vector3(i - ((width - 1f) / 2), j - ((height - 1f) / 2), 0);
                tileObject.transform.position = new Vector3(i - ((width - 1f) / 2), j - ((height - 1f) / 2), 0);
                tileMap.transform.position = new Vector3(i - ((width - 1f) / 2), j - ((height - 1f) / 2), 0);

                // Create the sprite renderers for holding the sprites
                SpriteRenderer tileBorderRenderer = tileBorder.AddComponent<SpriteRenderer>();
                SpriteRenderer tileObjRenderer = tileObject.AddComponent<SpriteRenderer>();
                SpriteRenderer tileMapRenderer = tileMap.AddComponent<SpriteRenderer>();

                // Set the layers rendering order. (Might not be necessary in this case.)
                tileBorderRenderer.sortingLayerName = "Grid Border";
                tileObjRenderer.sortingLayerName = "Grid Object";

                // Just used for organizing game objects in the editor.
                tileBorder.transform.SetParent(tileBlock.transform);
                tileObject.transform.SetParent(tileBlock.transform);
                tileMap.transform.SetParent(tileBlock.transform);
                tileBlock.transform.SetParent(transform);

                GridTile t = GetTile(i, j);
                if (t.TileBorder == null)
                {
                    // Set the current tile grid sprite
                    t.TileBorder = gridImage;

                    //tileMapRenderer.sprite = tiles[i, j].MapTile.GetComponent<SpriteRenderer>().sprite;
                    // Render initial border tiles to the screen (Make them black and slightly transparent)
                    
                    tileBorderRenderer.sprite = t.TileBorder.GetComponent<SpriteRenderer>().sprite;
                    tileBorderRenderer.GetComponent<SpriteRenderer>().color = new Color(0f, 0f, 0f, .25f);
                }

                // Pass the grid objects created to update function so we can update the grid later. Register the callback.
                t.RegisterCB((GridTile) => { OnGridCellUpdated(t, tileBorder, tileObject); });
            }
        }
    }
    public Vector3 GridToScreenPosition(int x, int y)
    {
        return new Vector3(x - ((width - 1f) / 2), y - ((height - 1f) / 2), 0);
    }

    public Vector3 MouseToTilePosition(Vector3 mousePosition)
    {
        return mousePosition - new Vector3((width - 1f) / 2, (height - 1f) / 2, 0);
    }
    private Vector3 GetMousePositionInGrid()
    {
        Vector3 mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (width % 2 == 0 && height % 2 == 0)
            return new Vector3((Mathf.Ceil(mouse.x) + (width - 1) / 2), (Mathf.Ceil(mouse.y) + (height - 1) / 2));
        else if (width % 2 != 0 && height % 2 != 0)
        {
            // add .5 necessary in this case since the mouse position can be above and below whole
            // number values. example: a block on position 0,0 has the values [-.5, .5] inside it.
            // to ensure that the values in this case takes the whole number which is 0 we can 
            // simply add the mouse position + .5 to guantee that the position with .Floor method
            // will always return the number for the current grid cell. if x = .4 then -.4 + .5 = .1
            // Floor(.1) would == 0 or the whole number value for that tile.  
            // (if we didnt add .5 then Floor(-.4) would == -1 which would be incorrect).
            return new Vector3(Mathf.Floor(mouse.x + .5f) + (width - 1) / 2, Mathf.Floor(mouse.y + .5f) + (height - 1) / 2);
        }
        else if (width % 2 == 0 && height % 2 != 0)
        {
            return new Vector3((Mathf.Ceil(mouse.x) + (width - 1) / 2), Mathf.Floor(mouse.y + .5f) + (height - 1) / 2);
        }
        else if (width % 2 != 0 && height % 2 == 0)
        {
            return new Vector3(Mathf.Floor(mouse.x + .5f) + (width - 1) / 2, Mathf.Ceil(mouse.y) + (height - 1) / 2);
        }
        else
        {
            Debug.Log("Test Game Grid because it is probably broken!");
            return new Vector3((Mathf.Ceil(mouse.x) + (width - 1) / 2), (Mathf.Ceil(mouse.y) + (height - 1) / 2));
        }

    }

    private bool MouseIsInGrid(Vector3 MouseCellPos)
    {
        return MouseCellPos.x >= 0 && MouseCellPos.x < width && MouseCellPos.y >= 0 && MouseCellPos.y < height;
    }



    public GridTile GetTile(int x, int y)
    {
        return tiles[x, y];
    }

    public void SetTileType(int x, int y, GameObject type)
    {
        tiles[x, y].TileObject = Instantiate(type, GridToScreenPosition(x, y), Quaternion.identity);
    }

    public void SetTileBorder(int x, int y, GameObject border)
    {
        tiles[x, y].TileBorder = border;
    }

    public void SetMapTile(int x, int y, GameObject mapTileType)
    {   
        tiles[x, y].MapTile = mapTileType;
    }

    void OnGridCellUpdated(GridTile tile, GameObject goBorder, GameObject tileObj)
    {
        //goTile.GetComponent<SpriteRenderer>().sprite = tower[0].GetComponent<SpriteRenderer>().sprite;
        //if (grid.GetTile((int)tileObj.transform.position.x, (int)tileObj.transform.position.y).TileObject != tileObj)
        //{
        tileObj = Instantiate(towerToPlace, tileObj.transform.position, Quaternion.identity);
        //}
         //Debug.Log(goTile);
    }

    void MapToFile()
    {
        char[] c = new char[7];
        c[0] = '(';
        c[1] = 'C';
        c[2] = 'l';
        c[3] = 'o';
        c[4] = 'n';
        c[5] = 'e';
        c[6] = ')';
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (tiles[i, j].TileObject != null)
                {
                    //string t = tiles[i, j].TileObject.name.Trim(c);

                    File.AppendAllText("assets/resources/map.txt", tiles[i, j].TileObject.name.Trim(c) + " ");
                }
                else
                {
                    File.AppendAllText("assets/resources/map.txt", "null ");
                }
            }
        }
    }
}
