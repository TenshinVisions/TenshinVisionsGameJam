using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyPathFinder : MonoBehaviour
{
    public List<Vector2> PathToTarget;
    List<EnemyNode> CheckedNodes = new List<EnemyNode>();
    List<EnemyNode> WaitingNodes = new List<EnemyNode>();
    public Hero Target;
    public LayerMask SolidLayer;

    void Start()
    {
        if (FindAnyObjectByType<Hero>() != null)
            {
                Target = FindAnyObjectByType<Hero>();
            }
    }

    void Update()
    {
        if (Target != null)
            PathToTarget = GetPath(Target.transform.position);
    }

    public List<Vector2> GetPath(Vector2 target)
    {
        PathToTarget = new List<Vector2>();
        CheckedNodes = new List<EnemyNode>();
        WaitingNodes = new List<EnemyNode>();

        Vector2 StartPosition = new Vector2(Mathf.Round(transform.position.x), Mathf.Round(transform.position.y));
        Vector2 TargetPosition = new Vector2(Mathf.Round(Target.transform.position.x), Mathf.Round(Target.transform.position.y));

        if (StartPosition == TargetPosition) return PathToTarget;

        EnemyNode startNode = new EnemyNode(0, StartPosition, TargetPosition, null);
        CheckedNodes.Add(startNode);
        WaitingNodes.AddRange(GetNeighbourNodes(startNode));

        while (WaitingNodes.Count > 0 && CheckedNodes.Count < 20)
        {
            EnemyNode nodeToCheck = WaitingNodes.Where(x => x.F == WaitingNodes.Min(y => y.F)).FirstOrDefault();

            if (nodeToCheck.Position == TargetPosition)
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

    public List<Vector2> CalculatePathFromNode(EnemyNode node)
    {
        var path = new List<Vector2>();
        EnemyNode currentNode = node;
        int it = 0;
        while (currentNode.PreviousNode != null)
        {
            it += 1;
            path.Add(new Vector2(currentNode.Position.x, currentNode.Position.y));
            currentNode = currentNode.PreviousNode;
        }
        return path;
    }

    List<EnemyNode> GetNeighbourNodes(EnemyNode node)
    {
        var Neighbours = new List<EnemyNode>();
        Neighbours.Add(new EnemyNode(node.G + 1, new Vector2(node.Position.x + 1, node.Position.y), node.TargetPosition, node));
        Neighbours.Add(new EnemyNode(node.G + 1, new Vector2(node.Position.x - 1, node.Position.y), node.TargetPosition, node));
        Neighbours.Add(new EnemyNode(node.G + 1, new Vector2(node.Position.x, node.Position.y + 1), node.TargetPosition, node));
        Neighbours.Add(new EnemyNode(node.G + 1, new Vector2(node.Position.x, node.Position.y - 1), node.TargetPosition, node));
        return Neighbours;
    }
    void OnDrawGizmos()
    {
        if (Target != null)
        {
            foreach (var item in CheckedNodes)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawSphere(new Vector2(item.Position.x, item.Position.y), 0.04f);
            }
            if (PathToTarget != null)
            {

                foreach (var item in PathToTarget)
                {
                    Gizmos.color = Color.blue;
                    Gizmos.DrawSphere(new Vector2(item.x, item.y), 0.08f);
                }
            }
        }
    }

}

public class EnemyNode
{
    public Vector2 Position;
    public Vector2 TargetPosition;
    public EnemyNode PreviousNode;
    public int F; // F = G + H (Расстояние от старта до цели)
    public int G; // Расстояние от старта до ноды
    public int H; // Расстояние от ноды до цели


    public EnemyNode(int g, Vector2 nodePosition, Vector2 targetPosition, EnemyNode previousNode)
    {
        Position = nodePosition;
        TargetPosition = targetPosition;
        PreviousNode = previousNode;
        G = g;
        H = (int)Mathf.Abs(targetPosition.x - Position.x) + (int)Mathf.Abs(targetPosition.y - Position.y);
        F = G + H;
    }
}
