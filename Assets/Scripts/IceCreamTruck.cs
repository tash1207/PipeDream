using UnityEngine;

public class IceCreamTruck : MonoBehaviour
{
    [SerializeField] GameObject iceCreamCone;
    
    float moveSpeed = 0.1f;
    bool isMoving;
    Vector3 moveTarget;
    int iceCreamValue = 10;

    RoadPiece.Node enteringDirection;
    GridTile nextTile;
    bool hasStarted;

    bool showWinScreen;

    public void SetSpeed(float speed)
    {
        moveSpeed = speed;
    }

    public void SpeedUp()
    {
        moveSpeed = 2.2f;
    }

    public void MoveFrom(RoadPiece roadPiece)
    {
        isMoving = true;
        moveTarget = GetExitMoveTargetFromRoadPiece(roadPiece);
    }

    public Quaternion GetMovingDirectionRotation(RoadPiece roadPiece)
    {
        switch (roadPiece.GetExit())
        {
            case RoadPiece.Node.Up:
                return Quaternion.identity;
            case RoadPiece.Node.Down:
                return Quaternion.Euler(0, 0, 180);
            case RoadPiece.Node.Left:
                return Quaternion.Euler(0, 0, 90);
            case RoadPiece.Node.Right:
                return Quaternion.Euler(0, 0, -90);
        }
        return Quaternion.identity;
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
                Vector3.MoveTowards(transform.position, moveTarget, moveSpeed * Time.deltaTime);
        
            if (transform.position == moveTarget)
            {
                isMoving = false;

                if (showWinScreen)
                {
                    GridManager.Instance.ShowWinScreen();
                }

                if (nextTile != null)
                {
                    Instantiate(iceCreamCone, transform.position, iceCreamCone.transform.rotation);
                    ScoreKeeper.Instance.ModifyScore(iceCreamValue);

                    transform.rotation = GetMovingDirectionRotation(nextTile.RoadPiece);
                    MoveFrom(nextTile.RoadPiece);
                }
            }
        }
    }

    public void OnEnteredNewTile(GridTile currentTile)
    {
        if (currentTile.isBorder)
        {
            Debug.Log("Crashed into border");
            GameOver();
            return;
        }
        currentTile.canPlaceRoad = false;
        if (currentTile.HasRoad())
        {
            if (currentTile.RoadPiece.isStart && !hasStarted)
            {
                nextTile = currentTile;
                hasStarted = true;
            }
            else if (currentTile.RoadPiece.EnterRoad(enteringDirection))
            {
                if (currentTile.RoadPiece.isEnd)
                {
                    Debug.Log("You win!");
                    nextTile = null;
                    GridManager.Instance.canPlaceRoad = false;
                    showWinScreen = true;
                }
                else
                {
                    nextTile = currentTile;
                }
            }
            else
            {
                Debug.Log("Current tile cannot be entered");
                GameOver();
            }
        }
        else
        {
            Debug.Log("Current tile has no road");
            GameOver();
        }
    }

    void GameOver()
    {
        isMoving = false;
        GridManager.Instance.canPlaceRoad = false;
        GridManager.Instance.ShowGameOver();
    }
}
