using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

// AStar.cs
public class AStar
{
    private Tilemap groundTilemap;
    private Tilemap collisionTilemap;

    public AStar(Tilemap ground, Tilemap collision)
    {
        groundTilemap = ground;
        collisionTilemap = collision;
    }

    public List<Vector2> FindPath(Vector2 start, Vector2 end)
    {
        List<Node> openList = new List<Node>();
        HashSet<Node> closedList = new HashSet<Node>();

        Node startNode = new Node(new Vector3Int((int)start.x, (int)start.y, 0));
        Node endNode = new Node(new Vector3Int((int)end.x, (int)end.y, 0));

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

        Vector3Int[] directions = {
            new Vector3Int(0, 1, 0),
            new Vector3Int(0, -1, 0),
            new Vector3Int(1, 0, 0),
            new Vector3Int(-1, 0, 0)
        };

        foreach (var direction in directions)
        {
            Vector3Int neighborPosition = node.Position + direction;
            if (IsWalkable(neighborPosition))
            {
                neighbors.Add(new Node(neighborPosition));
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

        return distX + distY;
    }

    private List<Vector2> RetracePath(Node start, Node end)
    {
        List<Vector2> path = new List<Vector2>();
        Node currentNode = end;

        while (currentNode != start)
        {
            path.Add(new Vector2(currentNode.Position.x, currentNode.Position.y));
            currentNode = currentNode.Parent;
        }

        path.Reverse();
        return path;
    }
}
