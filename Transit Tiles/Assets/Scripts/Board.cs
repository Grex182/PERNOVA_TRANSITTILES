using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
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
    [SerializeField] private float yPositionOffset;

    private Passenger[,] passengers;
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
        if (Physics.Raycast(ray, out info, 100, LayerMask.GetMask("Tile", "Hover", "MovableSpot")))
        {
            //Get the indexes of the tile the player hits
            Vector2Int hitPosition = LookupTileIndex(info.transform.gameObject);

            //If hovering over tile after not hovering any tiles
            if (currentHover == -Vector2Int.one)
            {
                currentHover = hitPosition;
                tiles[hitPosition.x, hitPosition.y].layer = LayerMask.NameToLayer("Hover");
            }

            //If already hovering tile, change previous one
            if (currentHover != hitPosition)
            {
                tiles[currentHover.x, currentHover.y].layer = (ContainsValidMove(ref availableMoves, currentHover)) ? LayerMask.NameToLayer("MovableSpot") : LayerMask.NameToLayer("Tile");
                currentHover = hitPosition;
                tiles[hitPosition.x, hitPosition.y].layer = LayerMask.NameToLayer("Hover");
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

                bool validMove = MoveTo(currentlyDragging, hitPosition.x, hitPosition.y);

                //go back to previous position
                if (!validMove)
                {
                    currentlyDragging.SetPosition(GetTileCenter(previousPosition.x, previousPosition.y));
                }

                currentlyDragging = null;
                RemoveMovableTiles();
            }
        }
        else
        {
            if (currentHover != -Vector2Int.one)
            {
                tiles[currentHover.x, currentHover.y].layer = (ContainsValidMove(ref availableMoves, currentHover)) ? LayerMask.NameToLayer("MovableSpot") : LayerMask.NameToLayer("Tile");
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
                tiles[x, y] = GenerateSingleTile(tileSize, x, y);
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
        tileObject.AddComponent<BoxCollider>();

        return tileObject;
    }

    //Spawning Pieces
    private void SpawnAllPieces()
    {
        passengers = new Passenger[tileCountX, tileCountY];

        passengers[0, 0] = SpawnSinglePiece(PassengerType.Pawn);
        passengers[0, 3] = SpawnSinglePiece(PassengerType.Pawn);
    }

    private Passenger SpawnSinglePiece(PassengerType type)
    {
        Passenger passenger = Instantiate(prefabs[(int)type - 1], transform).GetComponent<Passenger>();
        passenger.transform.localScale = Vector3.one;

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
            tiles[availableMoves[i].x, availableMoves[i].y].layer = LayerMask.NameToLayer("MovableSpot");
        }
    }
    private void RemoveMovableTiles()
    {
        for (int i = 0; i < availableMoves.Count; i++)
        {
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