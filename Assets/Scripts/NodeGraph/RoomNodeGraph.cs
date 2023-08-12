using System.Collections.Generic;
using UnityEngine;

namespace Rogue.NodeGraph
{
  [CreateAssetMenu(fileName = "RoomNodeGraph_", menuName = "Rogue/RoomNodeGraph/Graph", order = 0)]
  public class RoomNodeGraph : ScriptableObject, ISerializationCallbackReceiver
  {
    public List<Node> Nodes = new();
    public List<NodeLink> Links = new();
    
    public readonly Dictionary<string, Node> IdToNode = new();
    
#if UNITY_EDITOR
    
    public Node GetHoveredNode(Vector2 mousePosition) => 
      Nodes.Find(node => node.Transform.Contains(mousePosition));

    public bool IsNodeHovered(Vector2 mousePosition) => 
      GetHoveredNode(mousePosition) != null;

#endif

    public void DeleteAllNodes()
    {
      Links.Clear();
      IdToNode.Clear();
      Nodes.Clear();
    }
    
    public void OnBeforeSerialize()
    {
    }

    public void OnAfterDeserialize()
    {
      foreach (Node node in Nodes) 
        IdToNode[node.Id] = node;
    }
  }
}