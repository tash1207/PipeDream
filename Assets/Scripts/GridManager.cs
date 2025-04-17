using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance { get; private set; }

    [Header("Settings")]
    [SerializeField] int width, height;
    [SerializeField] float iceCreamTruckMoveDelay = 5f;
    [SerializeField] float iceCreamTruckMoveSpeed = 0.1f;
    
    [Header("References")]
    [SerializeField] GridTile gridTile;
    [SerializeField] Transform gridTileContainer;
    [SerializeField] Transform upcomingPieceContainer;
    [SerializeField] RoadPiece[] possibleHubPieces;
    [SerializeField] IceCreamTruck iceCreamTruckPrefab;
    [SerializeField] AudioSource backgroundMusic;

    [SerializeField] GameObject gameOverCanvas;
    [SerializeField] GameObject youWinCanvas;
    [SerializeField] TMP_Text gameOverScoreText;
    [SerializeField] TMP_Text youWinScoreText;

    Dictionary<Vector2Int, GridTile> tiles;
    GridTile[] upcomingPiecesTiles = new GridTile[5];

    int upcomingPiecesLength = 5;
    int upcomingPiecesX = -2;

    public bool canPlaceRoad = true;

    RoadPiece startPiece;
    RoadPiece endPiece;
    IceCreamTruck iceCreamTruck;

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
            GridTile spawnedTile =
                GenerateTileAtPoint(upcomingPiecesX, y + 1, upcomingPieceContainer);
            spawnedTile.canHighlight = false;
            spawnedTile.canPlaceRoad = false;
            spawnedTile.SetHighlight(y != 0);
            upcomingPiecesTiles[y] = spawnedTile;
        }

        RoadSpawner.Instance.Init(upcomingPiecesTiles);
    }

    void GenerateGrid()
    {
        tiles = new Dictionary<Vector2Int, GridTile>();
        for (int x = -1; x < width + 1; x++)
        {
            for (int y = -1; y < height + 1; y++)
            {
                GridTile spawnedTile = GenerateTileAtPoint(x, y, gridTileContainer);
                tiles[new Vector2Int(x, y)] = spawnedTile;
                if (x == -1 || y == -1 || x == width || y == height)
                {
                    spawnedTile.SetIsBorder();
                }
            }
        }
        GenerateStartAndEnd();
    }

    GridTile GenerateTileAtPoint(int x, int y, Transform parent)
    {
        return Instantiate(
            gridTile,
            new Vector3Int(x, y, 0),
            Quaternion.identity,
            parent);
    }

    void GenerateStartAndEnd()
    {
        // Spawn at 2, 2 for easiest level.
        int randomStartX = Random.Range(2, 3);
        int randomStartY = Random.Range(2, 3);
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
        startTile.SetRoadPiece(startPiece);
        startTile.canPlaceRoad = false;

        iceCreamTruck = Instantiate(
            iceCreamTruckPrefab,
            new Vector3(startPos.x, startPos.y, 0),
            Quaternion.identity);
        iceCreamTruck.SetSpeed(iceCreamTruckMoveSpeed);
        StartCoroutine(MoveIceCreamTruck());

        GridTile endTile = GetTileAtPosition(endPos);
        endPiece = Instantiate(
            possibleHubPieces[Random.Range(0, possibleHubPieces.Length)],
            new Vector3(endPos.x, endPos.y, 0),
            Quaternion.identity,
            endTile.transform);
        endPiece.isEnd = true;
        endTile.SetRoadPiece(endPiece);
        endTile.canPlaceRoad = false;
    }

    IEnumerator MoveIceCreamTruck()
    {
        yield return new WaitForSecondsRealtime(iceCreamTruckMoveDelay);
        iceCreamTruck.MoveFrom(startPiece);
    }

    public void SpeedUp()
    {
        iceCreamTruck.SpeedUp();
        backgroundMusic.pitch = 1.07f;
    }

    public void ShowGameOver()
    {
        gameOverScoreText.text = "SCORE : " + ScoreKeeper.Instance.GetScoreBeforeThisLevel();
        gameOverCanvas.SetActive(true);
    }

    public void ShowWinScreen()
    {
        youWinScoreText.text = "SCORE : " + ScoreKeeper.Instance.GetScore();
        youWinCanvas.SetActive(true);
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
