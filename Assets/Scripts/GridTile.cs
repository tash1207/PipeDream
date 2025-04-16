using System.Collections;
using UnityEngine;

public class GridTile : MonoBehaviour
{
    [SerializeField] GameObject highlight;
    [SerializeField] ParticleSystem boomEffectPrefab;
    [SerializeField] GameObject[] roadPieces;

    public bool canHighlight = true;
    public RoadPiece RoadPiece { get; private set; }

    public bool canPlaceRoad = true;

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

    void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Ice cream truck has entered new tile");
        if (collision.gameObject.TryGetComponent<IceCreamTruck>(out IceCreamTruck truck))
        {
            truck.OnEnteredNewTile(this);
        }
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
        if (!canPlaceRoad || !GridManager.Instance.canPlaceRoad) { return; }

        if (!HasRoad())
        {
            PlaceRoad();
        }
        else
        {
            StartCoroutine(ReplaceRoad());
        }
    }

    IEnumerator ReplaceRoad()
    {
        GridManager.Instance.canPlaceRoad = false;
        canPlaceRoad = false;
        ParticleSystem boomEffect =
            Instantiate(boomEffectPrefab, transform.position, Quaternion.identity);
        Destroy(boomEffect.gameObject, boomEffect.main.duration);

        yield return new WaitForSecondsRealtime(0.5f);
        RoadPiece.gameObject.SetActive(false);

        yield return new WaitForSecondsRealtime(0.5f);
        PlaceRoad();
        RoadPiece.gameObject.SetActive(true);
        canPlaceRoad = true;
        GridManager.Instance.canPlaceRoad = true;
    }

    void PlaceRoad()
    {
        RoadPiece = RoadSpawner.Instance.PlaceCurrentPiece();
        RoadPiece.transform.position = transform.position;
        RoadPiece.transform.parent = transform;
        highlight.SetActive(false);
    }
}
