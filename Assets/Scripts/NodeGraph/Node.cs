using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

namespace Rogue.NodeGraph
{
  [Serializable]
  public class Node
  {
    public NodeType Type;
    public Rect Transform;

    public Node(Rect transform) 
      : this(NodeType.None, transform)
    {
    }
    
    public Node(NodeType type, Rect transform) 
    {
      Type = type;
      Transform = transform;
    }

    public void Draw(GUIStyle style)
    {
      GUILayout.BeginArea(Transform, style);
      var type = (NodeType) EditorGUILayout.EnumPopup(Type);

      GUILayout.EndArea();

      if(Type != type)
        Type = type;
    }
  }
}