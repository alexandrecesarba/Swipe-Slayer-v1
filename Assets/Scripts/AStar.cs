using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class AStar
{
    private Tilemap groundTilemap;
    private Tilemap collisionTilemap;

    public AStar(Tilemap ground, Tilemap collision)
    {
        groundTilemap = ground;
        collisionTilemap = collision;
    }

    public List<Vector3Int> FindPath(Vector3Int start, Vector3Int end)
    {
        List<Node> openList = new List<Node>();
        HashSet<Node> closedList = new HashSet<Node>();

        Node startNode = new Node(start);
        Node endNode = new Node(end);

        openList.Add(startNode);

        while (openList.Count > 0)
        {
            Node currentNode = openList[0];
            for (int i = 1; i < openList.Count; i++)
            {
                if (openList[i].FCost < currentNode.FCost || 
                    openList[i].FCost == currentNode.FCost && openList[i].HCost < currentNode.HCost)
                {
                    currentNode = openList[i];
                }
            }

            openList.Remove(currentNode);
            closedList.Add(currentNode);

            if (currentNode.Position == endNode.Position)
            {
                return RetracePath(startNode, currentNode);
            }

            foreach (Node neighbor in GetNeighbors(currentNode))
            {
                if (closedList.Contains(neighbor))
                    continue;

                float newGCost = currentNode.GCost + GetDistance(currentNode, neighbor);
                if (newGCost < neighbor.GCost || !openList.Contains(neighbor))
                {
                    neighbor.GCost = newGCost;
                    neighbor.HCost = GetDistance(neighbor, endNode);
                    neighbor.Parent = currentNode;

                    if (!openList.Contains(neighbor))
                        openList.Add(neighbor);
                }
            }
        }

        return null; // No path found
    }

    private List<Node> GetNeighbors(Node node)
    {
        List<Node> neighbors = new List<Node>();

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0)
                    continue;

                Vector3Int neighborPosition = new Vector3Int(node.Position.x + x, node.Position.y + y, node.Position.z);
                if (IsWalkable(neighborPosition))
                {
                    neighbors.Add(new Node(neighborPosition));
                }
            }
        }

        return neighbors;
    }

    private bool IsWalkable(Vector3Int position)
    {
        return groundTilemap.GetTile(position) != null && collisionTilemap.GetTile(position) == null;
    }

    private float GetDistance(Node a, Node b)
    {
        int distX = Mathf.Abs(a.Position.x - b.Position.x);
        int distY = Mathf.Abs(a.Position.y - b.Position.y);

        if (distX > distY)
            return 1.4f * distY + 1.0f * (distX - distY);
        return 1.4f * distX + 1.0f * (distY - distX);
    }

    private List<Vector3Int> RetracePath(Node start, Node end)
    {
        List<Vector3Int> path = new List<Vector3Int>();
        Node currentNode = end;

        while (currentNode != start)
        {
            path.Add(currentNode.Position);
            currentNode = currentNode.Parent;
        }

        path.Reverse();
        return path;
    }
}
