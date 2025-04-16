using UnityEngine;

public class IceCreamTruck : MonoBehaviour
{
    public float MoveSpeed = 0.1f;

    bool isMoving;
    Vector3 moveTarget;

    RoadPiece.Node enteringDirection;
    GridTile nextTile;

    public void SpeedUp()
    {
        MoveSpeed = 2f;
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

                if (nextTile != null)
                {
                    MoveFrom(nextTile.RoadPiece);
                }
                
            }
        }
    }

    public void OnEnteredNewTile(GridTile currentTile)
    {
        if (currentTile.HasRoad())
        {
            if (currentTile.RoadPiece.EnterRoad(enteringDirection))
            {
                if (currentTile.RoadPiece.isEnd)
                {
                    Debug.Log("You win!");
                }
                else
                {
                    nextTile = currentTile;
                }
            }
            else
            {
                Debug.Log("Current tile cannot be entered");
                isMoving = false;
            }
        }
        else
        {
            Debug.Log("Current tile has no road");
            isMoving = false;
        }
    }
}
