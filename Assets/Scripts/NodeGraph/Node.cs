using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Rogue.NodeGraph
{
  [Serializable]
  public class Node : ISerializationCallbackReceiver
  {
    private const int mk_maxChildCorridors = 3;

    public ENodeType Type;

    [NonSerialized] public GUID Id;
    [NonSerialized] public List<GUID> ParentIds = new();
    [NonSerialized] public List<GUID> ChildIds = new();

    public Rect Transform;

    [NonSerialized]
    public bool IsSelected;

    [SerializeField] [HideInInspector] private string m_id;
    [SerializeField] [HideInInspector] private string[] m_parentIds;
    [SerializeField] [HideInInspector] private string[] m_childIds;

    public Node(Rect transform) 
      : this(ENodeType.None, transform)
    {
    }

    public Node(ENodeType type, Rect transform)
    {
      Id = GUID.Generate();
      Type = type;
      Transform = transform;
    }

    public bool CanAddChild(Node child)
    {
      return Id != child.Id &&
             Type != ENodeType.None &&
             child.Type != ENodeType.None &&
             child.Type != ENodeType.Entrance &&
             !ChildIds.Contains(child.Id) &&
             !ParentIds.Contains(child.Id) &&
             CanBeParentFor(child);
    }

    private bool CanBeParentFor(Node child)
    {
      return (Type == ENodeType.Corridor && child.Type != ENodeType.Corridor && ChildIds.Count == 0 && child.ParentIds.Count == 0) ||
             (Type != ENodeType.Corridor && child.Type == ENodeType.Corridor && child.ParentIds.Count == 0 && ChildIds.Count < mk_maxChildCorridors);
    }

    public void AddChild(Node child)
    {
      ChildIds.Add(child.Id);
      child.ParentIds.Add(Id);
    }

    public void Draw(GUIStyle style)
    {
      GUILayout.BeginArea(Transform, style);
      
      if(Type == ENodeType.Entrance || ParentIds.Count > 0)
        EditorGUILayout.LabelField(Type.ToString());
      else
      {
        var type = (ENodeType) EditorGUILayout.EnumPopup(Type);
        
        if(Type != type)
          Type = type;
      }
      
      GUILayout.EndArea();
    }

    public void OnBeforeSerialize()
    {
      m_id = Id.ToString();
      m_parentIds = ParentIds?.Select(id => id.ToString()).ToArray();
      m_childIds = ChildIds?.Select(id => id.ToString()).ToArray();
    }

    public void OnAfterDeserialize()
    {
      Id = new GUID(m_id);
      ParentIds = m_parentIds?.Select(id => new GUID(id)).ToList();
      ChildIds = m_childIds?.Select(id => new GUID(id)).ToList();
    }
  }
}