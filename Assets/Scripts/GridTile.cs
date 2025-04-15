using UnityEngine;

public class GridTile : MonoBehaviour
{
    [SerializeField] GameObject highlight;
    [SerializeField] GameObject[] roadPieces;

    public bool canHighlight = true;
    bool hasRoad;

    public void SetHighlight(bool enabled)
    {
        highlight.SetActive(enabled);
    }
    
    void OnMouseEnter()
    {
        if (canHighlight && !hasRoad)
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
        if (!hasRoad)
        {
            RoadPiece roadPiece = RoadSpawner.Instance.PlaceCurrentPiece();
            roadPiece.transform.position = transform.position;
            roadPiece.transform.parent = transform;
            highlight.SetActive(false);
            hasRoad = true;
        }
    }
}
