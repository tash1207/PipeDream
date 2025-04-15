using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridManager : MonoBehaviour
{
    [SerializeField] int width, height;
    [SerializeField] GridTile gridTile;

    [SerializeField] Transform gridTileContainer;
    [SerializeField] Transform upcomingPieceContainer;

    Dictionary<Vector2Int, GridTile> tiles;
    GridTile[] upcomingPiecesTiles = new GridTile[5];

    int upcomingPiecesLength = 5;
    int upcomingPiecesX = -2;

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
