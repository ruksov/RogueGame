using UnityEditor;
using UnityEngine;

namespace Rogue.NodeGraph.Editor
{
  [CustomEditor(typeof(RoomNodeGraphSO))]
  public class RoomNodeGraphEditor : UnityEditor.Editor
  {
    public override void OnInspectorGUI()
    {
      base.OnInspectorGUI();

      if (GUILayout.Button("Regenerate IDs"))
      {
        var graph = (RoomNodeGraphSO) target;

        graph.IdToNode.Clear();
        
        foreach (Node node in graph.Nodes)
        {
          node.Id = GUID.Generate();
          graph.IdToNode[node.Id] = node;
        }
      }
    }
  }
}