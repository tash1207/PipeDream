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
    public bool isBorder { get; private set; }
    public bool isPlacingNewRoad;

    int replaceRoadCost = -20;

    public void SetHighlight(bool enabled)
    {
        highlight.SetActive(enabled);
    }

    public bool HasRoad()
    {
        return RoadPiece != null;
    }

    public void SetIsBorder()
    {
        isBorder = true;
        canHighlight = false;
        canPlaceRoad = false;
        GetComponent<SpriteRenderer>().enabled = false;
    }

    public void SetRoadPiece(RoadPiece rp)
    {
        RoadPiece = rp;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
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
        isPlacingNewRoad = true;
        ScoreKeeper.Instance.ModifyScore(replaceRoadCost);
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
        isPlacingNewRoad = false;
    }

    void PlaceRoad()
    {
        RoadPiece = RoadSpawner.Instance.PlaceCurrentPiece();
        RoadPiece.transform.position = transform.position;
        RoadPiece.transform.parent = transform;
        highlight.SetActive(false);
    }
}
