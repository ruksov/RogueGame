using System.Collections.Generic;
using UnityEngine;

namespace Rogue.NodeGraph
{
  [CreateAssetMenu(fileName = "RoomNodeGraph_", menuName = "Rogue/RoomNodeGraph/Graph")]
  public class RoomNodeGraph : ScriptableObject, ISerializationCallbackReceiver
  {
    public List<Node> Nodes = new();
    public readonly Dictionary<string, Node> IDToNode = new();
    
#if UNITY_EDITOR
    
    public Node GetHoveredNode(Vector2 mousePosition) => 
      Nodes.Find(node => node.Transform.Contains(mousePosition));

    public bool IsNodeHovered(Vector2 mousePosition) => 
      GetHoveredNode(mousePosition) != null;

#endif

    public void AddNode(Node node)
    {
      Nodes.Add(node);
      IDToNode[node.Id] = node;
    }    
    
    public void DeleteAllNodes()
    {
      IDToNode.Clear();
      Nodes.Clear();
    }
    
    public void OnBeforeSerialize()
    {
    }

    public void OnAfterDeserialize()
    {
      foreach (Node node in Nodes) 
        IDToNode[node.Id] = node;
    }
  }
}