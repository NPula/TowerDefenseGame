using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.Tilemaps;
using UnityEngine.EventSystems;

public class GridController : MonoBehaviour
{
    //[SerializeField] 
    private int Width = 10, Height = 10;
    private WorldGrid grid;
    public GameObject gridImage;
    public GameObject highLightImage;
    public GameObject[] tower;
    private GameObject towerToPlace;
    private GameObject ObjectInHand;
    public GameObject grass;

    public enum GridPos { Centered, AlignedToTilemap }
    GridPos gridPos;
    public Tilemap tMap;

    public GameObject[] mapTiles;

    private GridEditor gridEditor;

    //public string map;
    public string[] map = null;

   
    private Vector3 mouseCellPos;          // The grid cell the mouse is currently in.


    void Start()
    {
        //map = File.ReadAllText(GameManager.instance.map).Split(' ');
        Debug.Log(GameManager.instance.gameState);
        if (GameManager.instance.gameState == GameManager.Gamestate.play)
        {
            //map = File.ReadAllText("assets/resources/map.txt").Split(' '); 

            towerToPlace = tower[GameManager.instance.towerSelection];
            highLightImage = Instantiate(highLightImage, transform.position, Quaternion.identity);
            ObjectInHand = new GameObject("asdfasf");
            ObjectInHand.AddComponent<SpriteRenderer>().sortingLayerName = "Grid Object";
            ObjectInHand.SetActive(false);
            highLightImage.SetActive(false);

            // Create the main grid for renderering all objects.
            int k = 0;
            Width = tMap.size.x;
            Height = tMap.size.y;
            grid = new WorldGrid(Width, Height);
            GameObject fmap = new GameObject("Map"); // organizing map components in the editor
            for (int i = 0; i < grid.Width; i++)
            {
                for (int j = 0; j < grid.Height; j++)
                {
                    // Create the inital game objects used for rendering the grid and the towers
                    GameObject tileBlock = new GameObject(i.ToString() + ", " + j.ToString());
                    GameObject tileBorder = new GameObject("Border");
                    GameObject tileObject = new GameObject("Tile");
                    GameObject tileMap = new GameObject("Map");

                    
                    // Set the position of the grid cells on the screen. (Centered)
                    tileBorder.transform.position = new Vector3(i - ((grid.Width - 1f) / 2), j - ((grid.Height - 1f) / 2), 0);
                    tileObject.transform.position = new Vector3(i - ((grid.Width - 1f) / 2), j - ((grid.Height - 1f) / 2), 0);
                    tileMap.transform.position = new Vector3(i - ((grid.Width - 1f) / 2), j - ((grid.Height - 1f) / 2), 0); 

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

                    GridTile t = grid.GetTile(i, j);
                    if (t.TileBorder == null)
                    {
                        // Set the current tile grid sprite
                        t.TileBorder = gridImage;

                        //tileMapRenderer.sprite = gridEditor.map[i,j].transform.GetComponent<SpriteRenderer>().sprite;
                        // Render initial border tiles to the screen (Make them black and slightly transparent)
                        //tileBorderRenderer.sprite = t.TileBorder.GetComponent<SpriteRenderer>().sprite;

                        if (map.Length != 0)
                        {
                            Debug.Log("map: " + map);
                            if (map[k].ToString() != null)
                            {
                                GameObject go = new GameObject(i.ToString() + ", " + j.ToString());
                                go.AddComponent<SpriteRenderer>();
                                go.transform.SetParent(fmap.transform);
                                go.GetComponent<SpriteRenderer>().sortingLayerName = "Grid Border";
                                /*for (int l = 0; l < mapTiles.Length; l++)
                                {
                                    if (map[k].ToString() == mapTiles[l].name)
                                    {
                                        go = mapTiles[l];
                                    }
                                }*/

                                //GameObject fmap = new GameObject("Map");
                                for (int l = 0; l < GameManager.instance.mapTiles.Length; l++)
                                {
                                    if (map[k].ToString() == GameManager.instance.mapTiles[l].name)
                                    {
                                        go = GameManager.instance.mapTiles[l];
                                    }
                                }
                                tileBorderRenderer.sprite = go.GetComponent<SpriteRenderer>().sprite;
                            }
                            k++;
                        }
                    }

                    // Pass the grid objects created to update function so we can update the grid later. Register the callback.
                    t.RegisterCB((GridTile) => { OnGridCellUpdated(t, tileBorder, tileObject); });
                }
            }
        }
        else // enter edit mode
        {
            gameObject.AddComponent<GridEditor>().gridImage = this.gridImage;
            gameObject.GetComponent<GridEditor>().highLightImage = this.highLightImage;
            gameObject.GetComponent<GridEditor>().width = Width;
            gameObject.GetComponent<GridEditor>().height = Height;
        }
    }

    private void CreateMap()
    {
        for (int i = 0; i < grid.Width; i++)
        {
            for (int j = 0; j < grid.Height; j++)
            {
                grid.SetTileType(i, j, grass);
            }
        }
    }

    void Update()
    {
        if (GameManager.instance.gameState == GameManager.Gamestate.play)
        {
            bool objectToPlaceInHand = true;
            if (GameManager.instance.towerSelection > 0)
            {
                towerToPlace = tower[GameManager.instance.towerSelection];
                objectToPlaceInHand = true;
            }
            if (objectToPlaceInHand)
            {
                ObjectInHand.GetComponent<SpriteRenderer>().sprite = towerToPlace.GetComponent<SpriteRenderer>().sprite;
                ObjectInHand.SetActive(true);
                ObjectInHand.transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Debug.Log("Mouse Position: " + Camera.main.ScreenToWorldPoint(Input.mousePosition));
                //Instantiate(towerToPlace, Input.mousePosition, Quaternion.identity);
            }
            HighlightGridWhenMousedOver();

            if (Input.GetMouseButtonDown(0))
            {
                if (EventSystem.current.IsPointerOverGameObject())
                    return;
                mouseCellPos = GetMousePositionInGrid();
                if (MouseIsInGrid(mouseCellPos))
                {
                    grid.SetTileType((int)mouseCellPos.x, (int)mouseCellPos.y, towerToPlace);
                }
            }
        }
    }

    public Vector3 MouseToTilePosition(Vector3 mousePosition)
    {
        return mousePosition - new Vector3((grid.Width - 1f) / 2, (grid.Height - 1f) / 2, 0);
    }
    private Vector3 GetMousePositionInGrid()
    {
        Vector3 mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (grid.Width % 2 == 0 && grid.Height % 2 == 0)
            return new Vector3((Mathf.Ceil(mouse.x) + (grid.Width - 1) / 2), (Mathf.Ceil(mouse.y) + (grid.Height - 1) / 2));
        else if (grid.Width % 2 != 0 && grid.Height % 2 != 0)
        {
            // add .5 necessary in this case since the mouse position can be above and below whole
            // number values. example: a block on position 0,0 has the values [-.5, .5] inside it.
            // to ensure that the values in this case takes the whole number which is 0 we can 
            // simply add the mouse position + .5 to guantee that the position with .Floor method
            // will always return the number for the current grid cell. if x = .4 then -.4 + .5 = .1
            // Floor(.1) would == 0 or the whole number value for that tile.  
            // (if we didnt add .5 then Floor(-.4) would == -1 which would be incorrect).
            return new Vector3(Mathf.Floor(mouse.x + .5f) + (grid.Width - 1) / 2, Mathf.Floor(mouse.y + .5f) + (grid.Height - 1) / 2);
        }
        else if (grid.Width % 2 == 0 && grid.Height % 2 != 0)
        {
            return new Vector3((Mathf.Ceil(mouse.x) + (grid.Width - 1) / 2), Mathf.Floor(mouse.y + .5f) + (grid.Height - 1) / 2);
        }
        else if (grid.Width % 2 != 0 && grid.Height % 2 == 0)
        {
            return new Vector3(Mathf.Floor(mouse.x + .5f) + (grid.Width - 1) / 2, Mathf.Ceil(mouse.y) + (grid.Height - 1) / 2);
        }
        else
        {
            Debug.Log("Test Game Grid because it is probably broken!");
            return new Vector3((Mathf.Ceil(mouse.x) + (grid.Width - 1) / 2), (Mathf.Ceil(mouse.y) + (grid.Height - 1) / 2));
        }

    }

    private bool MouseIsInGrid(Vector3 MouseCellPos)
    {
        return MouseCellPos.x >= 0 && MouseCellPos.x < grid.Width && MouseCellPos.y >= 0 && MouseCellPos.y < grid.Height;
    }

    void OnGridCellUpdated(GridTile tile, GameObject goBorder, GameObject tileObj)
    {
        tileObj = Instantiate(towerToPlace, tileObj.transform.position, Quaternion.identity);
        tileObj.GetComponent<SpriteRenderer>().sortingLayerName = "Grid Object";
    }

    void HighlightGridWhenMousedOver()
    {
        mouseCellPos = GetMousePositionInGrid();
        highLightImage.transform.position = MouseToTilePosition(mouseCellPos);
        if (MouseIsInGrid(mouseCellPos))
        {
            highLightImage.SetActive(true);
        }
        else
        {
            highLightImage.SetActive(false);
        }
    }

}
