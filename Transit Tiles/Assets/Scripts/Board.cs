using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TileType
{
    Train = 0,
    Chair = 1,
    Platform = 2
}

public class Board : MonoBehaviour
{
    //Spawns unavailable tiles
    private readonly HashSet<Vector2Int> occupiedTilesAtStart = new HashSet<Vector2Int>
    {
        new Vector2Int(11, 0),  new Vector2Int(11, 1), new Vector2Int(11, 2),  new Vector2Int(11, 3), new Vector2Int(11, 4),    new Vector2Int(17, 3),  new Vector2Int(17, 5),  new Vector2Int(5, 3),
        new Vector2Int(10, 5),  new Vector2Int(3, 4),  new Vector2Int(3, 5),   new Vector2Int(2, 5),  new Vector2Int(11, 5),    new Vector2Int(16, 3),  new Vector2Int(16, 5),  new Vector2Int(4, 3),
        new Vector2Int(9, 5),   new Vector2Int(2, 2),  new Vector2Int(2, 3),   new Vector2Int(2, 4),  new Vector2Int(1, 5),     new Vector2Int(15, 3),  new Vector2Int(15, 5),  new Vector2Int(6, 4),
        new Vector2Int(8, 5),   new Vector2Int(1, 2),  new Vector2Int(1, 3),   new Vector2Int(1, 4),  new Vector2Int(0, 5),     new Vector2Int(14, 3),  new Vector2Int(14, 5),  new Vector2Int(5, 4),
        new Vector2Int(3, 2),   new Vector2Int(3, 3),  new Vector2Int(0, 2),   new Vector2Int(0, 3),  new Vector2Int(17, 2),    new Vector2Int(13, 3),  new Vector2Int(13, 5),  new Vector2Int(4, 4),
        new Vector2Int(6, 0),   new Vector2Int(6, 1),  new Vector2Int(17, 0),  new Vector2Int(17, 1), new Vector2Int(16, 2),    new Vector2Int(12, 3),  new Vector2Int(12, 5),
        new Vector2Int(5, 0),   new Vector2Int(5, 1),  new Vector2Int(16, 0),  new Vector2Int(16, 1), new Vector2Int(15, 2),    new Vector2Int(17, 4),  new Vector2Int(5, 5),
        new Vector2Int(4, 0),   new Vector2Int(4, 1),  new Vector2Int(15, 0),  new Vector2Int(15, 1), new Vector2Int(14, 2),    new Vector2Int(16, 4),  new Vector2Int(4, 5),
        new Vector2Int(3, 0),   new Vector2Int(3, 1),  new Vector2Int(14, 0),  new Vector2Int(14, 1), new Vector2Int(13, 2),    new Vector2Int(15, 4),  new Vector2Int(6, 2),
        new Vector2Int(2, 0),   new Vector2Int(2, 1),  new Vector2Int(13, 0),  new Vector2Int(13, 1), new Vector2Int(12, 2),    new Vector2Int(14, 4),  new Vector2Int(5, 2),
        new Vector2Int(1, 0),   new Vector2Int(1, 1),  new Vector2Int(12, 0),  new Vector2Int(12, 1), new Vector2Int(7, 5),     new Vector2Int(13, 4),  new Vector2Int(4, 2),                                                                                   
        new Vector2Int(0, 0),   new Vector2Int(0, 1),  new Vector2Int(0, 4),   new Vector2Int(0, 5),  new Vector2Int(6, 5),     new Vector2Int(12, 4),  new Vector2Int(6, 3),
    };

    //Apply TrainTiles tag on tiles at the start
    private readonly HashSet<Vector2Int> tagTrainTilesAtStart = new HashSet<Vector2Int>
    {
        new Vector2Int(11, 7),  new Vector2Int(11, 8),  new Vector2Int(11, 9),  new Vector2Int(11, 10), new Vector2Int(11, 11),
        new Vector2Int(10, 7),  new Vector2Int(10, 8),  new Vector2Int(10, 9),  new Vector2Int(10, 10), new Vector2Int(10, 11),
        new Vector2Int(9, 7),   new Vector2Int(9, 8),   new Vector2Int(9, 9),   new Vector2Int(9, 10),  new Vector2Int(9, 11),
        new Vector2Int(8, 7),   new Vector2Int(8, 8),   new Vector2Int(8, 9),   new Vector2Int(8, 10),  new Vector2Int(8, 11),
        new Vector2Int(7, 7),   new Vector2Int(7, 8),   new Vector2Int(7, 9),   new Vector2Int(7, 10),  new Vector2Int(7, 11),
        new Vector2Int(6, 7),   new Vector2Int(6, 8),   new Vector2Int(6, 9),   new Vector2Int(6, 10),  new Vector2Int(6, 11),
        new Vector2Int(5, 7),   new Vector2Int(5, 8),   new Vector2Int(5, 9),   new Vector2Int(5, 10),  new Vector2Int(5, 11),
        new Vector2Int(4, 7),   new Vector2Int(4, 8),   new Vector2Int(4, 9),   new Vector2Int(4, 10),  new Vector2Int(4, 11),
        new Vector2Int(3, 7),   new Vector2Int(3, 8),   new Vector2Int(3, 9),   new Vector2Int(3, 10),  new Vector2Int(3, 11),
        new Vector2Int(2, 7),   new Vector2Int(2, 8),   new Vector2Int(2, 9),   new Vector2Int(2, 10),  new Vector2Int(2, 11),
        new Vector2Int(1, 7),   new Vector2Int(1, 8),   new Vector2Int(1, 9),   new Vector2Int(1, 10),  new Vector2Int(1, 11),
        new Vector2Int(0, 7),   new Vector2Int(0, 8),   new Vector2Int(0, 9),   new Vector2Int(0, 10),  new Vector2Int(0, 11),
    };

    private readonly HashSet<Vector2Int> tagExitTilesAtStart = new HashSet<Vector2Int>
    {
        new Vector2Int(9, 4),   new Vector2Int(10, 4),
        new Vector2Int(9, 3),   new Vector2Int(10, 3),
        new Vector2Int(9, 2),   new Vector2Int(10, 2),
        new Vector2Int(9, 1),   new Vector2Int(10, 1),
        new Vector2Int(9, 0),   new Vector2Int(10, 0),
    };

    private readonly HashSet<Vector2Int> tagEntranceTilesAtStart = new HashSet<Vector2Int>
    {
        new Vector2Int(8, 4),   new Vector2Int(7, 4),
        new Vector2Int(8, 3),   new Vector2Int(7, 3),
        new Vector2Int(8, 2),   new Vector2Int(7, 2),
        new Vector2Int(8, 1),   new Vector2Int(7, 1),
        new Vector2Int(8, 0),   new Vector2Int(7, 0),
    };

    private readonly HashSet<Vector2Int> chairTilesAtStart = new HashSet<Vector2Int>
    {
        new Vector2Int(11, 7),   new Vector2Int(4, 5),
        new Vector2Int(10, 7),   new Vector2Int(4, 4),
        new Vector2Int(9, 7),   new Vector2Int(4, 3),
        new Vector2Int(8, 7),   new Vector2Int(4, 2),
    };

    [Header("Art")]
    [SerializeField] private Material tileMaterial;
    [SerializeField] private float dragOffset = 1.25f;

    [Header("Tile Settings")]
    [SerializeField] private float tileSize = 1f;
    [SerializeField] private float gapSize = 0.1f;
    [SerializeField] private float yOffset = 0.2f;
    [SerializeField] private Vector3 boardCenter = Vector3.zero;

    [Header("Prefabs & Materials")]
    [SerializeField] private GameObject[] prefabs;
    [SerializeField] private GameObject platformTile;
    [SerializeField] private GameObject chairTile;
    [SerializeField] private GameObject trainTile;
    [SerializeField] private float yOffsetFloorTile;
    [SerializeField] private float yPositionOffset;

    private Passenger[,] passengers;
    [SerializeField] private List<GameObject> platformTiles = new List<GameObject>();
    private Passenger currentlyDragging;
    private List<Vector2Int> availableMoves = new List<Vector2Int>();
    [SerializeField] private int tileCountX = 8;
    [SerializeField] private int tileCountY = 8;
    private GameObject[,] tiles;
    private Camera currentCamera;
    private Vector2Int currentHover;
    private Vector3 bounds;

    private void Awake()
    {
        GenerateAllTiles(tileSize, tileCountX, tileCountY);

        SpawnAllPieces();

        PositionAllPieces();
    }

    private void Start()
    {
        StationManager.instance.Board = this;
    }

    private void Update()
    {
        //For putting in the highlighting part of the board
        if (!currentCamera)
        {
            currentCamera = Camera.main;
            return;
        }

        RaycastHit info;
        Ray ray = currentCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out info, 100, LayerMask.GetMask("Tile", "Hover", "MovableSpot", "Occupied")))
        {
            //Get the indexes of the tile the player hits
            Vector2Int hitPosition = LookupTileIndex(info.transform.gameObject);

            //If hovering over tile after not hovering any tiles
            if (currentHover == -Vector2Int.one)
            {
                currentHover = hitPosition;

                if (tiles[hitPosition.x, hitPosition.y].layer == LayerMask.NameToLayer("Occupied") && currentlyDragging != null/* && passengers[currentHover.x, currentHover.y] == null*//* && passengers[currentHover.x, currentHover.y] == null*/) //uncomment the && part if tile under passenger should turn green and not stay red
                {
                    tiles[hitPosition.x, hitPosition.y].layer = LayerMask.NameToLayer("Occupied");
                }
                else if (tiles[hitPosition.x, hitPosition.y].layer == LayerMask.NameToLayer("Occupied") && passengers[hitPosition.x, hitPosition.y] == null)
                {
                    tiles[hitPosition.x, hitPosition.y].layer = LayerMask.NameToLayer("Occupied");
                }
                else
                {
                    tiles[hitPosition.x, hitPosition.y].layer = LayerMask.NameToLayer("Hover");
                }
            }

            //If already hovering tile, change previous one
            if (currentHover != hitPosition)
            {
                if (ContainsValidMove(ref availableMoves, currentHover))
                {
                    tiles[currentHover.x, currentHover.y].layer = LayerMask.NameToLayer("MovableSpot");
                }
                else if (passengers[currentHover.x, currentHover.y] != null || tiles[currentHover.x, currentHover.y].layer == LayerMask.NameToLayer("Occupied"))
                {
                    tiles[currentHover.x, currentHover.y].layer = LayerMask.NameToLayer("Occupied");
                }
                else if (tiles[currentHover.x, currentHover.y].layer == LayerMask.NameToLayer("Occupied") && passengers[currentHover.x, currentHover.y] == null)
                {
                    tiles[currentHover.x, currentHover.y].layer = LayerMask.NameToLayer("Occupied");
                }
                else if (tiles[hitPosition.x, hitPosition.y].layer == LayerMask.NameToLayer("Unavailable"))
                {
                    tiles[currentHover.x, currentHover.y].layer = LayerMask.NameToLayer("Unavailable");
                }
                else
                {
                    tiles[currentHover.x, currentHover.y].layer = LayerMask.NameToLayer("Tile");
                }

                currentHover = hitPosition;

                if (tiles[hitPosition.x, hitPosition.y].layer == LayerMask.NameToLayer("Occupied") && currentlyDragging != null/* && passengers[currentHover.x, currentHover.y] == null*//* && passengers[currentHover.x, currentHover.y] == null*/) //uncomment the && part if tile under passenger should turn green and not stay red
                {
                    tiles[hitPosition.x, hitPosition.y].layer = LayerMask.NameToLayer("Occupied");
                }
                else if (tiles[hitPosition.x, hitPosition.y].layer == LayerMask.NameToLayer("Occupied") && passengers[hitPosition.x, hitPosition.y] == null)
                {
                    tiles[hitPosition.x, hitPosition.y].layer = LayerMask.NameToLayer("Occupied");
                }
                else
                {
                    tiles[hitPosition.x, hitPosition.y].layer = LayerMask.NameToLayer("Hover");
                }
            }

            //If press down on mouse
            if (Input.GetMouseButtonDown(0))
            {
                if (passengers[hitPosition.x, hitPosition.y] != null)
                {
                    currentlyDragging = passengers[hitPosition.x, hitPosition.y];

                    //Get list of where passenger can go
                    availableMoves = currentlyDragging.GetAvailableMoves(ref passengers, tileCountX, tileCountY);
                    CreateMovableTiles();
                }
            }

            //If releasing mouse button
            if (currentlyDragging != null && Input.GetMouseButtonUp(0))
            {
                Vector2Int previousPosition = new Vector2Int(currentlyDragging.currentX, currentlyDragging.currentY);

                tiles[previousPosition.x, previousPosition.y].layer = LayerMask.NameToLayer("Tile");

                bool validMove = MoveTo(currentlyDragging, hitPosition.x, hitPosition.y);

/*                if (currentlyDragging.type == PassengerType.Bulky)
                {
                    tiles[previousPosition.x - 1, previousPosition.y].layer = LayerMask.NameToLayer("Tile");

                    if (currentlyDragging.currentX - 1 >= 0 && currentlyDragging.currentX - 1 < tiles.GetLength(0) && currentlyDragging.currentY >= 0 && currentlyDragging.currentY < tiles.GetLength(1))
                    {
                        tiles[currentlyDragging.currentX - 1, currentlyDragging.currentY].layer = LayerMask.NameToLayer("Occupied");
                    }
                    else
                    {
                        validMove = false;
                    }
                }*/

                //go back to previous position
                if (!validMove)
                {
/*                    if (currentlyDragging.type == PassengerType.Bulky)
                    {
                        currentlyDragging.SetPosition(GetTileCenter(previousPosition.x, previousPosition.y));
                        tiles[previousPosition.x, previousPosition.y].layer = LayerMask.NameToLayer("Occupied");
                        tiles[previousPosition.x - 1, previousPosition.y].layer = LayerMask.NameToLayer("Occupied");
                        tiles[currentlyDragging.currentX, currentlyDragging.currentY].layer = LayerMask.NameToLayer("Tile");
                        Debug.Log("Changed the position of bulky person back to previous position");
                    }*/

                    currentlyDragging.SetPosition(GetTileCenter(previousPosition.x, previousPosition.y));
                }

                RemoveMovableTiles();

                tiles[currentlyDragging.currentX, currentlyDragging.currentY].layer = LayerMask.NameToLayer("Hover");

                currentlyDragging = null;
            }
        }
        else
        {
            //if going out of bounds, change previous tile
            if (currentHover != -Vector2Int.one)
            {
                if (ContainsValidMove(ref availableMoves, currentHover))
                {
                    tiles[currentHover.x, currentHover.y].layer = LayerMask.NameToLayer("MovableSpot");
                }
                else if (passengers[currentHover.x, currentHover.y] != null)
                {
                    tiles[currentHover.x, currentHover.y].layer = LayerMask.NameToLayer("Occupied");
                    //Debug.Log("Set tile to occupied");
                }
                else if (passengers[currentHover.x, currentHover.y] == null && tiles[currentHover.x, currentHover.y].layer == LayerMask.NameToLayer("Occupied"))
                {
                    tiles[currentHover.x, currentHover.y].layer = LayerMask.NameToLayer("Occupied");
                    //Debug.Log("OK NOW ITS WORKING I THINK");
                }
                else
                {
                    tiles[currentHover.x, currentHover.y].layer = LayerMask.NameToLayer("Tile");
                    //Debug.Log("Tile has been set back to just being tile");
                }
                currentHover = -Vector2Int.one;
            }

            if (currentlyDragging && Input.GetMouseButtonUp(0))
            {
                currentlyDragging.SetPosition(GetTileCenter(currentlyDragging.currentX, currentlyDragging.currentY));
                currentlyDragging = null;
                RemoveMovableTiles();
            }
        }

        //IF dragging a piece
        if (currentlyDragging)
        {
            Plane horizontalPlane = new Plane(Vector3.up, Vector3.up * yOffset);
            float distance = 0.0f;
            if (horizontalPlane.Raycast(ray, out distance))
                currentlyDragging.SetPosition(ray.GetPoint(distance) + Vector3.up * dragOffset);

/*            Plane horizontalPlane = new Plane(Vector3.up, Vector3.up * yOffset);
            float distance = 0.0f;
            if (horizontalPlane.Raycast(ray, out distance))
            {
                RaycastHit hit;
                Ray hoverRay = currentCamera.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(hoverRay, out hit, 100, LayerMask.GetMask("MovableSpot")))
                {
                    Vector2Int movePos = LookupTileIndex(hit.transform.gameObject);

                    // Snap to center of the valid MovableSpot tile
                    currentlyDragging.SetPosition(GetTileCenter(movePos.x, movePos.y) + Vector3.up * dragOffset);
                }
                else
                {
                    // Optionally keep the piece at its original position if not over MovableSpot
                    currentlyDragging.SetPosition(GetTileCenter(currentlyDragging.currentX, currentlyDragging.currentY) + Vector3.up * dragOffset);
                }
            }*/
        }
    }

    //Generates the board
    private void GenerateAllTiles(float tileSize, int tileCountX, int tileCountY)
    {
        yOffset += transform.position.y;
        bounds = new Vector3((tileCountX / 2) * tileSize, 0, (tileCountX / 2) * tileSize) + boardCenter;

        tiles = new GameObject[tileCountX, tileCountY];
        for (int x = 0; x < tileCountX; x++)
        {
            for (int y = 0; y < tileCountY; y++)
            {
                Vector2Int tilePos = new Vector2Int(x, y);
                tiles[x, y] = GenerateSingleTile(tileSize, x, y);

                if (occupiedTilesAtStart.Contains(tilePos))
                {
                    tiles[x, y].layer = LayerMask.NameToLayer("Unavailable");
                }

                if (tagTrainTilesAtStart.Contains(tilePos))
                {
                    tiles[x, y].tag = "TrainTile";
                }
                else if (tagExitTilesAtStart.Contains(tilePos))
                {
                    tiles[x, y].tag = "ExitTile";
                    platformTiles.Add(tiles[x, y]);
                }
                else if (tagEntranceTilesAtStart.Contains(tilePos))
                {
                    tiles[x, y].tag = "EntranceTile";
                    platformTiles.Add(tiles[x, y]);
                }

                Instantiate(platformTile, new Vector3(GetTileCenter(x, y).x, yOffsetFloorTile, GetTileCenter(x, y).z), Quaternion.Euler(-90, 0, 0));
            }
        }
    }

    private GameObject GenerateSingleTile(float tileSize, int x, int y)
    {
        GameObject tileObject = new GameObject(string.Format("X:{0}, Y:{1}", x, y));
        tileObject.transform.parent = transform;

        Mesh mesh = new Mesh();
        tileObject.AddComponent<MeshFilter>().mesh = mesh;
        tileObject.AddComponent<MeshRenderer>().material = tileMaterial;

        float gapOffsetX = x * (tileSize + gapSize);
        float gapOffsetY = y * (tileSize + gapSize);

        Vector3[] vertices = new Vector3[4];
        vertices[0] = new Vector3(gapOffsetX, yOffset, gapOffsetY) - bounds;
        vertices[1] = new Vector3(gapOffsetX, yOffset, gapOffsetY + tileSize) - bounds;
        vertices[2] = new Vector3(gapOffsetX + tileSize, yOffset, gapOffsetY) - bounds;
        vertices[3] = new Vector3(gapOffsetX + tileSize, yOffset, gapOffsetY + tileSize) - bounds;

        int[] tris = new int[] { 0, 1, 2, 1, 3, 2 };

        mesh.vertices = vertices;
        mesh.triangles = tris;
        mesh.RecalculateNormals();

        tileObject.layer = LayerMask.NameToLayer("Tile");
        BoxCollider boxCollider = tileObject.AddComponent<BoxCollider>();
        boxCollider.isTrigger = true;

        return tileObject;
    }

    //Spawning Pieces
    private void SpawnAllPieces()
    {
        passengers = new Passenger[tileCountX, tileCountY];

        //the tiles[0, 0] part should be equal to the line before it, like if passengers[0, 3], then afterwards the tiles one should be tiles[0, 3] so that when it spawns, the tile below it has its layer set to "Occupied"
        passengers[8, 4] = SpawnSinglePiece(PassengerType.Standard);
        tiles[8, 4].layer = LayerMask.NameToLayer("Occupied");
        passengers[7, 2] = SpawnSinglePiece(PassengerType.Standard);
        tiles[7, 2].layer = LayerMask.NameToLayer("Occupied");
        passengers[7, 4] = SpawnSinglePiece(PassengerType.Standard);
        tiles[7, 4].layer = LayerMask.NameToLayer("Occupied");
        passengers[8, 1] = SpawnSinglePiece(PassengerType.Standard);
        tiles[8, 1].layer = LayerMask.NameToLayer("Occupied");
        passengers[8, 0] = SpawnSinglePiece(PassengerType.Bulky);
        tiles[8, 0].layer = LayerMask.NameToLayer("Occupied");
        tiles[7, 0].layer = LayerMask.NameToLayer("Occupied");
    }

    private Passenger SpawnSinglePiece(PassengerType type)
    {
        Passenger passenger = Instantiate(prefabs[(int)type - 1]).GetComponent<Passenger>();
        //passenger.transform.localScale = Vector3.one;
        passenger.transform.SetParent(transform);

        passenger.type = type;

        return passenger;
    }

    //Positioning
    private void PositionAllPieces() //snaps all the pieces where they're supposed to be (useful for spawning pieces at the start of game or round)
    {
        for (int x = 0; x < tileCountX; x++)
        {
            for (int y = 0; y < tileCountY; y++)
            {
                if (passengers[x, y] != null)
                {
                    PositionSinglePiece(x, y, true);
                }
            }
        }
    }

    private void PositionSinglePiece(int x, int y, bool force = false)
    {
        passengers[x, y].currentX = x;
        passengers[x, y].currentY = y;
        passengers[x, y].SetPosition(GetTileCenter(x, y), force);
    }

    private Vector3 GetTileCenter(int x, int y)
    {
        float xPos = x * (tileSize + gapSize);
        float zPos = y * (tileSize + gapSize);

        return new Vector3(xPos, yOffset + yPositionOffset, zPos) - bounds + new Vector3(tileSize / 2, 0, tileSize / 2);
    }

    private void CreateMovableTiles()
    {
        for (int i = 0; i < availableMoves.Count; i++)
        {
            if (tiles[availableMoves[i].x, availableMoves[i].y].layer == LayerMask.NameToLayer("Unavailable") || tiles[availableMoves[i].x, availableMoves[i].y].layer == LayerMask.NameToLayer("Occupied"))
            {
                continue; //Skips the tile that is unavailable and continues with the rest
            }

            tiles[availableMoves[i].x, availableMoves[i].y].layer = LayerMask.NameToLayer("MovableSpot");
        }
    }
    private void RemoveMovableTiles()
    {
        for (int i = 0; i < availableMoves.Count; i++)
        {
            if (tiles[availableMoves[i].x, availableMoves[i].y].layer == LayerMask.NameToLayer("Unavailable") || tiles[availableMoves[i].x, availableMoves[i].y].layer == LayerMask.NameToLayer("Occupied"))
            {
                continue; //Skips the tile that is unavailable and continues with the rest
            }

            tiles[availableMoves[i].x, availableMoves[i].y].layer = LayerMask.NameToLayer("Tile");
        }

        availableMoves.Clear();
    }

    //Operations
    private bool ContainsValidMove(ref List<Vector2Int> moves, Vector2 pos)
    {
        for (int i = 0; i < moves.Count; i++)
        {
            if (moves[i].x == pos.x && moves[i].y == pos.y)
            {
                return true;
            }
        }

        return false;
    }

    private bool MoveTo(Passenger passenger, int x, int y)
    {
        if (!ContainsValidMove(ref availableMoves, new Vector2(x, y)))
        {
            return false;
        }

        //Block movement if the tile's layer is "Unavailable"
        if (tiles[x, y].layer == LayerMask.NameToLayer("Unavailable") || tiles[x, y].layer == LayerMask.NameToLayer("Occupied"))
        {
            return false;
        }

        Vector2Int previousPosition = new Vector2Int(passenger.currentX, passenger.currentY);

        //Is there another piece on target position?
        if (passengers[x, y] != null)
        {
            //op means other passenger
            Passenger op = passengers[x, y];

            return false;
        }

        passengers[x, y] = passenger;
        passengers[previousPosition.x, previousPosition.y] = null;

        PositionSinglePiece(x, y);

        return true;
    }

    public void DisablePlatformTiles()
    {
        foreach (var tile in platformTiles)
        {
            tile.layer = LayerMask.NameToLayer("Unavailable");
        }
    }

    public void EnablePlatformTiles()
    {
        foreach (var tile in platformTiles)
        {
            tile.layer = LayerMask.NameToLayer("Tile");
        }
    }

    private Vector2Int LookupTileIndex(GameObject hitInfo)
    {
        for (int x = 0; x < tileCountX; x++)
        {
            for (int y = 0; y < tileCountY; y++)
            {
                if (tiles[x, y] == hitInfo)
                {
                    return new Vector2Int(x, y);
                }
            }
        }

        return -Vector2Int.one; //INvalid
    }

    //Possible Debug stuff (Might need to move to GameManager?)
}