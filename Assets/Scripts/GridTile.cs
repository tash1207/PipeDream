using UnityEngine;

public class GridTile : MonoBehaviour
{
    [SerializeField] GameObject highlight;
    [SerializeField] GameObject[] roadPieces;

    public bool canHighlight = true;
    public RoadPiece RoadPiece { get; private set; }

    public void SetHighlight(bool enabled)
    {
        highlight.SetActive(enabled);
    }

    public bool HasRoad()
    {
        return RoadPiece != null;
    }

    public void SetRoadPiece(RoadPiece rp)
    {
        RoadPiece = rp;
    }
    
    void OnMouseEnter()
    {
        if (canHighlight && !HasRoad())
        {
            highlight.SetActive(true);
        }
    }

    void OnMouseExit()
    {
        if (canHighlight)
        {
            highlight.SetActive(false);
        }
    }

    void OnMouseDown()
    {
        if (!HasRoad())
        {
            RoadPiece = RoadSpawner.Instance.PlaceCurrentPiece();
            RoadPiece.transform.position = transform.position;
            RoadPiece.transform.parent = transform;
            highlight.SetActive(false);
        }
    }
}
