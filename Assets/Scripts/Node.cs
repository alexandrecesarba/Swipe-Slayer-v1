using UnityEngine;

public class Node
{
    public Vector3Int Position { get; set; }
    public Node Parent { get; set; }
    public float GCost { get; set; } // Custo do início até o nó atual
    public float HCost { get; set; } // Custo heurístico do nó atual até o objetivo
    public float FCost { get { return GCost + HCost; } } // Custo total (G + H)

    public Node(Vector3Int position)
    {
        Position = position;
    }
}
