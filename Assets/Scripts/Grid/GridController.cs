using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridController : MonoBehaviour
{
    [SerializeField] private int Width = 10, Height = 10;
    private WorldGrid grid;
    public GameObject black;

    private Vector3 mouseCellPos;          // The grid cell the mouse is currently in.
    private Vector3 prevMouseCellPosition;

    private Vector3 highlightPos;
    private Vector3 prevHighlightPos;

    bool mouseButtonDown = false;

    void Start()
    {
        grid = new WorldGrid(Width, Height);
        for (int i = 0; i < grid.Width; i++)
        {
            for (int j = 0; j < grid.Height; j++)
            {
                GameObject tileObj = new GameObject(i.ToString() + ", " + j.ToString());
                tileObj.transform.position = new Vector3(i - ((grid.Width - 1f) / 2), j - ((grid.Height - 1f) / 2), 0);
                SpriteRenderer tileBorderRenderer = tileObj.AddComponent<SpriteRenderer>();
                tileBorderRenderer.sortingLayerName = "Grid";
                tileObj.transform.SetParent(transform);

                print(grid.Width + " ," + grid.Height);
                GridTile t = grid.GetTile(i, j);
                if (t.TileObject == null)
                {
                    tileBorderRenderer.sprite = black.GetComponent<SpriteRenderer>().sprite;
                    tileBorderRenderer.color = new Color(0f,0f,0f,.25f);
                }
                //else
                //    tileBorderRenderer.sprite = black; 

                //t.RegisterCB((GridTile) => { OnGridCellUpdated(t, tileObj); });
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        //HighlightGridWhenMousedOver();

        if (Input.GetMouseButtonDown(0))
        {
            mouseButtonDown = true;
            // set mouse position
            mouseCellPos = GetMousePositionInGrid();
            highlightPos = mouseCellPos;
            prevMouseCellPosition = mouseCellPos;
            //HighlightGridWhenMousedOver();
            //GameObject tile = new GameObject();
            //tile.AddComponent<SpriteRenderer>().sprite = green;
            //tile.transform.position = MouseToTilePosition(mouseCellPos);
        }
        /*if (mouseButtonDown)
        {
            prevHighlightPos = highlightPos;
            highlightPos = GetMousePositionInGrid();
            for (int x = (int)prevMouseCellPosition.x; x <= (int)mouseCellPos.x; x++)
            {
                for (int y = (int)prevMouseCellPosition.y; y <= (int)mouseCellPos.y; y++)
                {
                    HighlightGridAtPos(new Vector3(x, y));
                }
            }
        }*/ 

        if (Input.GetMouseButtonUp(0))
        {
            mouseButtonDown = false;
            prevMouseCellPosition = mouseCellPos;
            mouseCellPos = GetMousePositionInGrid();

            if (prevMouseCellPosition.x > mouseCellPos.x)
            {
                Vector3 tmp = prevMouseCellPosition;
                prevMouseCellPosition = new Vector3(mouseCellPos.x, prevMouseCellPosition.y);
                mouseCellPos = new Vector3(tmp.x, mouseCellPos.y);
            }
            if (prevMouseCellPosition.y > mouseCellPos.y)
            {
                Vector3 tmp = prevMouseCellPosition;
                prevMouseCellPosition = new Vector3(prevMouseCellPosition.x, mouseCellPos.y);
                mouseCellPos = new Vector3(mouseCellPos.x, tmp.y);
            }
            for (int x = (int)prevMouseCellPosition.x; x <= (int)mouseCellPos.x; x++)
            {
                for (int y = (int)prevMouseCellPosition.y; y <= (int)mouseCellPos.y; y++)
                {
                    Debug.Log("positions: " + x + ", " + y);
                    GameObject tile = new GameObject();
                    //tile.AddComponent<SpriteRenderer>().sprite = green;
                    tile.transform.position = MouseToTilePosition(new Vector3(x, y));
                }
            }
            

            //GameObject tile = new GameObject();
            //tile.AddComponent<SpriteRenderer>().sprite = green;
            //tile.transform.position = MouseToTilePosition(mouseCellPos);
        }

        //prevMouseCellPosition = mouseCellPos;
    }

    void HighlightIfMouseDown()
    {
        
    }

    public Vector3 MouseToTilePosition(Vector3 mousePosition)
    {
        return mousePosition - new Vector3((grid.Width - 1f) / 2, (grid.Height - 1f) / 2, 0);
    }
    private Vector3 GetMousePositionInGrid()
    {
        Vector3 mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (grid.Width % 2 == 0)
            return new Vector3((Mathf.Ceil(mouse.x) + (grid.Width - 1) / 2), (Mathf.Ceil(mouse.y) + (grid.Height - 1) / 2));
        else
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

    }

    private bool MouseClickWasInGrid(Vector3 MouseCellPos)
    {
        return MouseCellPos.x >= 0 && MouseCellPos.x < grid.Width && MouseCellPos.y >= 0 && MouseCellPos.y < grid.Height;
    }

    /*void OnGridCellUpdated(GridTile tile, GameObject go)
    {
        if (tile.TileBorder == GridTile.Border.yellow)
        {
            go.GetComponent<SpriteRenderer>().sprite = yellow;
        }
        else
            go.GetComponent<SpriteRenderer>().sprite = black;

        //if (tile.TType == GridTile.TileType.green)
        //    go.GetComponent<SpriteRenderer>().sprite = green;
    }

    void HighlightGridAtPos(Vector3 gridPos)
    {
        if (grid.GetTile((int)gridPos.x, (int)gridPos.y).TileBorder != GridTile.Border.yellow)
        {
            grid.SetTileBorder((int)gridPos.x, (int)gridPos.y, GridTile.Border.yellow);
        }
    }
    void HighlightGridWhenMousedOver()
    {
        mouseCellPos = GetMousePositionInGrid();
        if (MouseClickWasInGrid(prevMouseCellPosition))
            grid.SetTileBorder((int)prevMouseCellPosition.x, (int)prevMouseCellPosition.y, GridTile.Border.black);
        if (MouseClickWasInGrid(mouseCellPos))
            grid.SetTileBorder((int)mouseCellPos.x, (int)mouseCellPos.y, GridTile.Border.yellow);
    }
    */
}
