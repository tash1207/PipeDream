using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance { get; private set; }

    [SerializeField] int width, height;
    
    [SerializeField] GridTile gridTile;
    [SerializeField] Transform gridTileContainer;
    [SerializeField] Transform upcomingPieceContainer;
    [SerializeField] RoadPiece[] possibleHubPieces;

    Dictionary<Vector2Int, GridTile> tiles;
    GridTile[] upcomingPiecesTiles = new GridTile[5];

    int upcomingPiecesLength = 5;
    int upcomingPiecesX = -2;

    RoadPiece startPiece;
    RoadPiece endPiece;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        GenerateUpcomingPiecesGrid();
        GenerateGrid();
    }

    void GenerateUpcomingPiecesGrid()
    {
        for (int y = 0; y < upcomingPiecesLength; y++)
        {
            GridTile spawnedTile = Instantiate(
                gridTile,
                new Vector3Int(upcomingPiecesX, y + 1, 0),
                Quaternion.identity,
                upcomingPieceContainer);
            spawnedTile.canHighlight = false;
            spawnedTile.SetHighlight(y != 0);
            upcomingPiecesTiles[y] = spawnedTile;
        }

        RoadSpawner.Instance.Init(upcomingPiecesTiles);
    }

    void GenerateGrid()
    {
        tiles = new Dictionary<Vector2Int, GridTile>();
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                GridTile spawnedTile = Instantiate(
                    gridTile,
                    new Vector3Int(x, y, 0),
                    Quaternion.identity,
                    gridTileContainer);
                tiles[new Vector2Int(x, y)] = spawnedTile;
            }
        }
        GenerateStartAndEnd();
    }

    void GenerateStartAndEnd()
    {
        int randomStartX = Random.Range(1, 3);
        int randomStartY = Random.Range(1, 3);
        Vector2Int startPos = new Vector2Int(randomStartX, randomStartY);

        int randomEndX = Random.Range(width - 3, width - 1);
        int randomEndY = Random.Range(height - 3, height - 1);
        Vector2Int endPos = new Vector2Int(randomEndX, randomEndY);

        GridTile startTile = GetTileAtPosition(startPos);
        startPiece = Instantiate(
            possibleHubPieces[Random.Range(0, possibleHubPieces.Length)],
            new Vector3(startPos.x, startPos.y, 0),
            Quaternion.identity,
            startTile.transform);
        startPiece.isStart = true;
        startTile.hasRoad = true;

        GridTile endTile = GetTileAtPosition(endPos);
        endPiece = Instantiate(
            possibleHubPieces[Random.Range(0, possibleHubPieces.Length)],
            new Vector3(endPos.x, endPos.y, 0),
            Quaternion.identity,
            endTile.transform);
        endPiece.isEnd = true;
        endTile.hasRoad = true;
    }

    public GridTile GetTileAtPosition(Vector2Int pos)
    {
        if (tiles.TryGetValue(pos, out var tile))
        {
            return tile;
        }

        return null;
    }
}
