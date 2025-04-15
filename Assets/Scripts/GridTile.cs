using UnityEngine;

public class GridTile : MonoBehaviour
{
    [SerializeField] GameObject highlight;
    [SerializeField] GameObject[] roadPieces;

    bool hasRoad;

    void OnMouseEnter()
    {
        if (!hasRoad)
        {
            highlight.SetActive(true);   
        }
    }

    void OnMouseExit()
    {
        highlight.SetActive(false);
    }

    void OnMouseDown()
    {
        if (!hasRoad)
        {
            GameObject roadPiece = roadPieces[Random.Range(0, roadPieces.Length)];
            Instantiate(roadPiece, transform.position, Quaternion.identity);
            highlight.SetActive(false);
            hasRoad = true;
        }
    }
}
