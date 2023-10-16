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

        // Vizinhos ortogonais
        Vector3Int[] directions = {
            new Vector3Int(0, 1, 0),  // Acima
            new Vector3Int(0, -1, 0), // Abaixo
            new Vector3Int(1, 0, 0),  // Direita
            new Vector3Int(-1, 0, 0)  // Esquerda
        };

        foreach (var direction in directions)
        {
            Vector3Int neighborPosition = new Vector3Int(node.Position.x + direction.x, node.Position.y + direction.y, node.Position.z);
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

        return distX + distY; // Custo de 1 para movimentos ortogonais
    }

    private List<Vector3Int> RetracePath(Node start, Node end)
    {
        List<Vector3Int> path = new List<Vector3Int>();
        Node currentNode = end;

        Node previousNode = null; // Adicionado para rastrear o nó anterior

        while (currentNode != start)
        {
            path.Add(currentNode.Position);

            // Se houver um nó anterior, desenhe uma linha entre o nó atual e o nó anterior
            if (previousNode != null)
            {
                Vector3 currentPos = groundTilemap.GetCellCenterWorld(currentNode.Position);
                Vector3 previousPos = groundTilemap.GetCellCenterWorld(previousNode.Position);
                Debug.DrawLine(currentPos, previousPos, Color.magenta, 5f); // A linha será vermelha e durará 5 segundos
            }

            previousNode = currentNode; // Atualize o nó anterior
            currentNode = currentNode.Parent;
        }

        path.Reverse();
        return path;
    }

}
