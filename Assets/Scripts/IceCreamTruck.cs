using UnityEngine;

public class IceCreamTruck : MonoBehaviour
{
    public float MoveSpeed = 0.1f;

    bool isMoving;
    Vector3 moveTarget;

    RoadPiece.Node enteringDirection;

    public void SpeedUp()
    {
        MoveSpeed = 2f;
    }
    
    public void MoveTo(Vector2Int cell)
    {
        isMoving = true;
        moveTarget = new Vector3(cell.x, cell.y, 0);
    }

    public void MoveFrom(RoadPiece roadPiece)
    {
        isMoving = true;
        moveTarget = GetExitMoveTargetFromRoadPiece(roadPiece);
    }

    Vector3 GetExitMoveTargetFromRoadPiece(RoadPiece roadPiece)
    {
        Vector3 rpTransform = roadPiece.transform.position;
        switch (roadPiece.GetExit())
        {
            case RoadPiece.Node.Up:
                enteringDirection = RoadPiece.Node.Down;
                return new Vector3(rpTransform.x, rpTransform.y + 1, 0);
            case RoadPiece.Node.Down:
                enteringDirection = RoadPiece.Node.Up;
                return new Vector3(rpTransform.x, rpTransform.y - 1, 0);
            case RoadPiece.Node.Left:
                enteringDirection = RoadPiece.Node.Right;
                return new Vector3(rpTransform.x - 1, rpTransform.y, 0);
            case RoadPiece.Node.Right:
                enteringDirection = RoadPiece.Node.Left;
                return new Vector3(rpTransform.x + 1, rpTransform.y, 0);
            case RoadPiece.Node.Empty:
                // Game over!
                Debug.Log("No exit");
                enteringDirection = RoadPiece.Node.Empty;
                return new Vector3(rpTransform.x, rpTransform.y, 0);
        }
        return new Vector3(rpTransform.x, rpTransform.y, 0);
    }

    void Update()
    {
        if (isMoving)
        {
            transform.position =
                Vector3.MoveTowards(transform.position, moveTarget, MoveSpeed * Time.deltaTime);
        
            if (transform.position == moveTarget)
            {
                isMoving = false;

                // Probably want to do this check earlier instead of when we're in the
                // center of the tile that might not have a road on it.
                GridTile currentTile = GridManager.Instance.GetTileAtPosition(
                    new Vector2Int(Mathf.RoundToInt(transform.position.x),
                        Mathf.RoundToInt(transform.position.y)));
                
                if (currentTile != null && currentTile.HasRoad())
                {
                    Debug.Log("Current tile has road");
                    if (currentTile.RoadPiece.EnterRoad(enteringDirection))
                    {
                        Debug.Log("Current tile can be entered");
                        if (currentTile.RoadPiece.isEnd)
                        {
                            Debug.Log("You win!");
                        }
                        else
                        {
                            MoveFrom(currentTile.RoadPiece);
                        }
                    }
                    else
                    {
                        Debug.Log("Current tile cannot be entered");
                    }
                }
                else
                {
                    Debug.Log("Current tile not found or has no road");
                }
            }
        }
    }
}
