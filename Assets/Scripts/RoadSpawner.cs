using System.Collections.Generic;
using UnityEngine;

public class RoadSpawner : MonoBehaviour
{
    public static RoadSpawner Instance { get; private set; }

    [SerializeField] RoadPiece[] possibleRoadPieces;

    GridTile[] upcomingPiecesTiles;
    RoadPiece[] upcomingPieces = new RoadPiece[5];

    void Awake()
    {
        Instance = this;
    }

    public void Init(GridTile[] upcomingPiecesTiles)
    {
        this.upcomingPiecesTiles = upcomingPiecesTiles;
        SpawnUpcomingPieces();
    }

    public RoadPiece PlaceCurrentPiece()
    {
        RoadPiece currentRoadPiece = upcomingPieces[0];
        ShiftAllUpcomingPieces();

        return currentRoadPiece;
    }

    void SpawnUpcomingPieces()
    {
        for (int i = 0; i < upcomingPieces.Length; i++)
        {
            SpawnPieceAtIndex(i);
        }
    }

    void ShiftAllUpcomingPieces()
    {
        for (int i = 1; i < upcomingPieces.Length; i++)
        {
            // Move currently displayed upcoming road pieces.
            RoadPiece rp = upcomingPiecesTiles[i].gameObject.GetComponentInChildren<RoadPiece>();
            rp.transform.position = upcomingPiecesTiles[i-1].transform.position;
            rp.transform.parent = upcomingPiecesTiles[i-1].transform;

            upcomingPieces[i-1] = upcomingPieces[i];
        }

        SpawnPieceAtIndex(upcomingPieces.Length - 1);
    }

    void SpawnPieceAtIndex(int index)
    {
        RoadPiece roadPiece = Instantiate(
                possibleRoadPieces[Random.Range(0, possibleRoadPieces.Length)],
                upcomingPiecesTiles[index].transform.position,
                Quaternion.identity,
                upcomingPiecesTiles[index].gameObject.transform);
        upcomingPieces[index] = roadPiece;
    }
}
