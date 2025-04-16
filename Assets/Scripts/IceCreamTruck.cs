using UnityEngine;

public class IceCreamTruck : MonoBehaviour
{
    public float MoveSpeed = 0.1f;

    bool isMoving;
    Vector3 moveTarget;

    public void MoveTo(Vector2Int cell)
    {
        isMoving = true;
        moveTarget = new Vector3(cell.x, cell.y, 0);
    }

    public void MoveFrom(RoadPiece roadPiece)
    {
        isMoving = true;
        moveTarget = GetMoveTargetFromRoadPiece(roadPiece);
    }

    Vector3 GetMoveTargetFromRoadPiece(RoadPiece roadPiece)
    {
        Vector3 rpTransform = roadPiece.transform.position;
        switch (roadPiece.GetExit())
        {
            case RoadPiece.Node.Up:
                return new Vector3(rpTransform.x, rpTransform.y + 1, 0);
            case RoadPiece.Node.Down:
                return new Vector3(rpTransform.x, rpTransform.y - 1, 0);
            case RoadPiece.Node.Left:
                return new Vector3(rpTransform.x - 1, rpTransform.y, 0);
            case RoadPiece.Node.Right:
                return new Vector3(rpTransform.x + 1, rpTransform.y, 0);
            case RoadPiece.Node.Empty:
                // Game over!
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
            }
        }
    }
}
