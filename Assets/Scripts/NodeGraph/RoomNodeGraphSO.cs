using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Rogue.NodeGraph
{
  [CreateAssetMenu(fileName = "RoomNodeGraph_", menuName = "Rogue/RoomNodeGraph")]
  public class RoomNodeGraphSO : ScriptableObject, ISerializationCallbackReceiver
  {
    //[HideInInspector]
    public List<Node> Nodes = new();
    public Dictionary<GUID, Node> IdToNode = new();
    
#if UNITY_EDITOR
    
    public Node GetHoveredNode(Vector2 mousePosition) => 
      Nodes.Find(node => node.Transform.Contains(mousePosition));

    public bool IsNodeHovered(Vector2 mousePosition) => 
      GetHoveredNode(mousePosition) != null;

#endif

    public Node FirstNodeOf(ENodeType type) => 
      Nodes.First(node => node.Type == type);

    public IEnumerable<Node> ChildNodes(Node node) => 
      node.ChildIds.Select(id => IdToNode[id]);

    public void AddNode(Node node)
    {
      Nodes.Add(node);
      IdToNode[node.Id] = node;
    }    
    
    public void DeleteAllNodes()
    {
      IdToNode.Clear();
      Nodes.Clear();
    }
    
    public void OnBeforeSerialize()
    {
    }

    public void OnAfterDeserialize() => 
      IdToNode = Nodes.ToDictionary(node => node.Id);
  }
}