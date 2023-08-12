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
    public List<string> ParentIds = new();
    public List<string> ChildIds = new();

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

    public bool CanAddChild(Node child)
    {
      return Id != child.Id &&
             !ChildIds.Contains(child.Id) &&
             !ParentIds.Contains(child.Id);
    }

    public void AddChild(Node child)
    {
      ChildIds.Add(child.Id);
      child.ParentIds.Add(child.Id);
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