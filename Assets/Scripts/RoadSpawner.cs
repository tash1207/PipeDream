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

    void SpawnUpcomingPieces()
    {
        for (int i = 0; i < upcomingPieces.Length; i++)
        {
            RoadPiece roadPiece = Instantiate( 
                possibleRoadPieces[Random.Range(0, possibleRoadPieces.Length)],
                upcomingPiecesTiles[i].transform.position,
                Quaternion.identity,
                upcomingPiecesTiles[i].gameObject.transform);
            upcomingPieces[i] = roadPiece;
        }
    }

    public RoadPiece PlaceCurrentPiece()
    {
        RoadPiece currentRoadPiece = upcomingPieces[0];
        ShiftAllUpcomingPieces();

        return currentRoadPiece;
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

        int lastIndex = upcomingPieces.Length - 1;
        RoadPiece roadPiece = Instantiate(
                possibleRoadPieces[Random.Range(0, possibleRoadPieces.Length)],
                upcomingPiecesTiles[lastIndex].transform.position,
                Quaternion.identity,
                upcomingPiecesTiles[lastIndex].gameObject.transform);
        upcomingPieces[lastIndex] = roadPiece;
    }
}
