using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridController : MonoBehaviour
{
    [SerializeField] private int Width = 10, Height = 10;
    private WorldGrid grid;
    public GameObject gridImage;
    public GameObject highLightImage;
    public GameObject[] tower;

    private Vector3 mouseCellPos;          // The grid cell the mouse is currently in.

    void Start()
    {
        highLightImage = Instantiate(highLightImage, transform.position, Quaternion.identity);
        highLightImage.SetActive(false);
        grid = new WorldGrid(Width, Height);
        for (int i = 0; i < grid.Width; i++)
        {
            for (int j = 0; j < grid.Height; j++)
            {
                GameObject tileBlock = new GameObject(i.ToString() + ", " + j.ToString());
                GameObject tileBord = new GameObject("Border");
                GameObject tileObj = new GameObject("Tile");
                
                tileBord.transform.position = new Vector3(i - ((grid.Width - 1f) / 2), j - ((grid.Height - 1f) / 2), 0);
                tileObj.transform.position = new Vector3(i - ((grid.Width - 1f) / 2), j - ((grid.Height - 1f) / 2), 0);
                
                SpriteRenderer tileBorderRenderer = tileBord.AddComponent<SpriteRenderer>();
                SpriteRenderer TileObjRenderer    = tileObj.AddComponent<SpriteRenderer>();
                //tileBorderRenderer.sortingLayerName = "Grid";
                
                tileBord.transform.SetParent(tileBlock.transform);
                tileObj.transform.SetParent(tileBlock.transform);
                tileBlock.transform.SetParent(transform);

                print(grid.Width + " ," + grid.Height);
                GridTile t = grid.GetTile(i, j);
                if (t.TileBorder == null)
                {
                    t.TileBorder = gridImage;
                    //t.TileBorder.GetComponent<SpriteRenderer>().color = new Color(0f, 0f, 0f, .25f);
                    tileBorderRenderer.sprite = t.TileBorder.GetComponent<SpriteRenderer>().sprite;
                    tileBorderRenderer.GetComponent<SpriteRenderer>().color = new Color(0f, 0f, 0f, .25f);
                    //Instantiate(gridImage, tileObj.transform.position, Quaternion.identity);
                }

                t.RegisterCB((GridTile) => { OnGridCellUpdated(t, tileBord, tileObj); });
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        HighlightGridWhenMousedOver();

        if (Input.GetMouseButtonDown(0))
        {
            mouseCellPos = GetMousePositionInGrid();
            grid.SetTileType((int)mouseCellPos.x, (int)mouseCellPos.y, tower[0]);
            //GameObject tile = new GameObject();
            //tile.AddComponent<SpriteRenderer>().sprite = tower[0].GetComponent<SpriteRenderer>().sprite;
            //tile.transform.position = MouseToTilePosition(mouseCellPos);
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

    void OnGridCellUpdated(GridTile tile, GameObject goBorder, GameObject goTile)
    {
        //if (tile.TileObject == tower[0])
        //{
        //goBorder.GetComponent<SpriteRenderer>().sprite = gridImage.GetComponent<SpriteRenderer>().sprite;
        goTile.GetComponent<SpriteRenderer>().sprite = tower[0].GetComponent<SpriteRenderer>().sprite;
        Debug.Log(goTile);
        //go.GetComponent<SpriteRenderer>().sprite = ;
        //}
        //else
        //    go.GetComponent<SpriteRenderer>().sprite = black; */

        //if (tile.TType == GridTile.TileType.green)
        //    go.GetComponent<SpriteRenderer>().sprite = green;
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
