using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class PathFinder : MonoBehaviour
{
    public List<Vector2> PathToTarget;
    List<Node> CheckedNodes = new List<Node>();
    List<Node> WaitingNodes = new List<Node>();

    public Target Target;
    public TargetObject targetObject;
    public Enemy targetEnemy;
    public bool cleared = false;
    public LayerMask SolidLayer;


    void Update()
    {
        if (Target != null)
        {
            PathToTarget = GetPath(Target.transform.position);
        }
        else if (Target == null)
        {
            if (FindAnyObjectByType<Enemy>() != null)
            {
                Target = FindAnyObjectByType<Enemy>();
            }
            else if (FindAnyObjectByType<TargetObject>() != null)
            {
                Target = FindAnyObjectByType<TargetObject>();
            }
            else
            {
                return;
            }
        }
    }

    public List<Vector2> GetPath(Vector2 target)
    {
        PathToTarget = new List<Vector2>();
        CheckedNodes = new List<Node>();
        WaitingNodes = new List<Node>();
        if (!cleared)
        {
            var Target = targetEnemy;
        }
        else
        {
            var Target = targetObject;
        }

        Vector2 StartPosition = new Vector2(Mathf.Round(transform.position.x), Mathf.Round(transform.position.y));
        Vector2 TargetPosition = new Vector2(Mathf.Round(Target.transform.position.x), Mathf.Round(Target.transform.position.y));

        if (StartPosition == TargetPosition) return PathToTarget;

        Node startNode = new Node(0, StartPosition, TargetPosition, null);
        CheckedNodes.Add(startNode);
        WaitingNodes.AddRange(GetNeighbourNodes(startNode));

        while (WaitingNodes.Count > 0 && CheckedNodes.Count < 1000)
        {
            Node nodeToCheck = WaitingNodes.Where(x => x.F == WaitingNodes.Min(y => y.F)).FirstOrDefault();

            if (Vector2.Distance(nodeToCheck.Position, TargetPosition) < 0.5f)
            {
                return CalculatePathFromNode(nodeToCheck);
            }

            var walkable = !Physics2D.OverlapCircle(nodeToCheck.Position, 0.25f, SolidLayer);
            if (!walkable)
            {
                WaitingNodes.Remove(nodeToCheck);
                CheckedNodes.Add(nodeToCheck);
            }
            else if (walkable)
            {
                WaitingNodes.Remove(nodeToCheck);
                if (!CheckedNodes.Where(x => x.Position == nodeToCheck.Position).Any())
                {
                    CheckedNodes.Add(nodeToCheck);
                    WaitingNodes.AddRange(GetNeighbourNodes(nodeToCheck));
                }
            }
        }

        return PathToTarget;
    }

    public List<Vector2> CalculatePathFromNode(Node node)
    {
        var path = new List<Vector2>();
        Node currentNode = node;
        int it = 0;
        while (currentNode.PreviousNode != null)
        {
            it += 1;
            path.Add(new Vector2(currentNode.Position.x, currentNode.Position.y));
            if (it == 1)
            {
                path.Remove(new Vector2(currentNode.Position.x, currentNode.Position.y));
            }
            currentNode = currentNode.PreviousNode;
        }

        return path;
    }

    List<Node> GetNeighbourNodes(Node node)
    {
        var Neighbours = new List<Node>();
        Neighbours.Add(new Node(node.G + 1, new Vector2(node.Position.x + 1, node.Position.y), node.TargetPosition, node));
        Neighbours.Add(new Node(node.G + 1, new Vector2(node.Position.x - 1, node.Position.y), node.TargetPosition, node));
        Neighbours.Add(new Node(node.G + 1, new Vector2(node.Position.x, node.Position.y + 1), node.TargetPosition, node));
        Neighbours.Add(new Node(node.G + 1, new Vector2(node.Position.x, node.Position.y - 1), node.TargetPosition, node));
        return Neighbours;
    }
    void OnDrawGizmos()
    {
        if (Target != null)
        {
            foreach (var item in CheckedNodes)
            {
                //Gizmos.color = Color.green;
                //Gizmos.DrawSphere(new Vector2(item.Position.x, item.Position.y), 0.05f);
            }
            if (PathToTarget != null)
            {

                foreach (var item in PathToTarget)
                {
                    Gizmos.color = Color.red;
                    Gizmos.DrawSphere(new Vector2(item.x, item.y), 0.1f);
                }
            }
        }
    }

}

public class Node
{
    public Vector2 Position;
    public Vector2 TargetPosition;
    public Node PreviousNode;
    public int F; // F = G + H (Расстояние от старта до цели)
    public int G; // Расстояние от старта до ноды
    public int H; // Расстояние от ноды до цели


    public Node(int g, Vector2 nodePosition, Vector2 targetPosition, Node previousNode)
    {
        Position = nodePosition;
        TargetPosition = targetPosition;
        PreviousNode = previousNode;
        G = g;
        H = (int)Mathf.Abs(targetPosition.x - Position.x) + (int)Mathf.Abs(targetPosition.y - Position.y);
        F = G + H;
    }
}
