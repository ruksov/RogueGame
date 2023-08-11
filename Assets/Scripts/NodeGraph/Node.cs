using System;
using UnityEditor;
using UnityEngine;

namespace Rogue.NodeGraph
{
  [Serializable]
  public class Node
  {
    public NodeType Type;

    [SerializeField]
    private Rect m_transform;

    public Node(Rect transform) 
      : this(NodeType.None, transform)
    {
    }
    
    public Node(NodeType type, Rect transform) 
    {
      Type = type;
      m_transform = transform;
    }

    public void Draw(GUIStyle style)
    {
      GUILayout.BeginArea(m_transform, style);
      var type = (NodeType) EditorGUILayout.EnumPopup(Type);

      if(Type != type)
        Type = type;
      
      GUILayout.EndArea();
    }
  }
}