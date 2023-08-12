using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Rogue.NodeGraph
{
  [Serializable]
  public class Node
  {
    public string Id;
    public NodeType Type;
    public Rect Transform;
    public List<string> Links = new();

    public Node(Rect transform) 
      : this(NodeType.None, transform)
    {
    }
    
    public Node(NodeType type, Rect transform)
    {
      Id = Guid.NewGuid().ToString();
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