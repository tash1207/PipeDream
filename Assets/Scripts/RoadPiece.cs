using System.Collections.Generic;
using UnityEngine;

public class RoadPiece : MonoBehaviour
{
    public enum Node
    {
        Up,
        Down,
        Left,
        Right,
        Empty,
    }

    public List<Node> nodes;

    public bool isStart;
    public bool isEnd;

    public bool EnterRoad(Node enterNode)
    {
        bool hasEnterNode = nodes.Contains(enterNode);
        nodes.Remove(enterNode);

        return hasEnterNode;
    }

    public Node GetExit()
    {
        if (nodes.Count == 1)
        {
            return nodes[0];
        }
        return Node.Empty;
    }
}
