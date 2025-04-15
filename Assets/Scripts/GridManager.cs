using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridManager : MonoBehaviour
{
    [SerializeField] int width, height;
    [SerializeField] GridTile gridTile;

    [SerializeField] Tilemap cellTilemap;
    [SerializeField] Tile cellTile;

    Dictionary<Vector2Int, GridTile> tiles;

    void Start()
    {
        GenerateGrid();
    }

    void GenerateGrid()
    {
        tiles = new Dictionary<Vector2Int, GridTile>();
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                //cellTilemap.SetTile(new Vector3Int(x, y, 0), cellTile);
                GridTile spawnedTile = Instantiate(gridTile, new Vector3Int(x, y, 0), Quaternion.identity);
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
