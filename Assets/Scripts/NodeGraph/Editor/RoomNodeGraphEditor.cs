using System;
using System.Linq;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

namespace Rogue.NodeGraph.Editor
{
  public class RoomNodeGraphEditor : EditorWindow
  {
    private static readonly Vector2 ms_nodeSize = new(160, 75);
    private const int mk_nodePadding = 25;
    private const int mk_nodeBorder = 12;

    private const int mk_linkThickness = 2;
    private const float mk_arrowLength = 20;
    private const int mk_arrowAngle = 30;

    private static RoomNodeGraph ms_graph;
    private static GUIStyle ms_nodeStyle;
    private static GUIStyle ms_selectedNodeStyle;

    private static Node ms_dragNode;
    private static Vector2 ms_dragNodeOffset;

    private static Node ms_dragLinkStartNode;
    private static Vector2 ms_dragLinkEndPosition;


    [MenuItem("Room Node Graph Editor", menuItem = "Tools/Room Node Graph Editor")]
    private static void OpenWindow()
    {
      GetWindow<RoomNodeGraphEditor>("Room Node Graph Editor");
    }

    [OnOpenAsset]
    private static bool OnOpenAsset(int instanceId, int line)
    {
      if(EditorUtility.InstanceIDToObject(instanceId) is not RoomNodeGraph graph)
        return false;

      OpenWindow();
      ms_graph = graph;
      return true;
    }

    private void Awake()
    {
      if(ms_nodeStyle == null)
        CreateNodeStyle();
    }

    private void OnDestroy()
    {
      if (ms_graph)
      {
        ClearSelectedNodes();
        SaveGraph();
      }

      ms_graph = null;
    }

    private void OnGUI()
    {
      if(ms_graph == null)
        return;

      ProcessEvent(Event.current);
      
      DrawDragLink();
      DrawLinks();
      DrawNodes();
    }

    private void ClearSelectedNodes()
    {
      foreach (Node node in ms_graph.Nodes.Where(node => node.IsSelected)) 
        node.IsSelected = false;
    }

    private static void ProcessEvent(Event currentEvent)
    {
      switch (currentEvent.type)
      {
        case EventType.MouseDown:
          ProcessMouseDownEvent(currentEvent);
          break;
        
        case EventType.MouseUp:
          ProcessMouseUpEvent(currentEvent);
          break;
        
        case EventType.MouseDrag:
          ProcessMouseDragEvent(currentEvent);
          break;
      }
    }

    private static void DrawDragLink()
    {
      if(ms_dragLinkStartNode != null)
        Handles.DrawDottedLine(ms_dragLinkStartNode.Transform.center, ms_dragLinkEndPosition, 5);
    }

    private static void DrawLinks()
    {
      foreach (Node node in ms_graph.Nodes)
      {
        foreach (string childId in node.ChildIds)
          DrawLink(node, ms_graph.IdToNode[childId]);
      }
    }

    private static void DrawLink(Node parent, Node child)
    {
      DrawLine(parent.Transform.center, child.Transform.center);
      DrawLinkArrow(parent.Transform.center, child.Transform.center);
    }

    private static void DrawLinkArrow(Vector2 parentCenter, Vector2 childCenter)
    {
      float distance = Vector2.Distance(parentCenter, childCenter);

      Vector2 linkDir = (parentCenter - childCenter).normalized;
      Vector2 leftArrowDir = Quaternion.AngleAxis(mk_arrowAngle, Vector3.forward) * linkDir;
      Vector2 rightArrowDir = Quaternion.AngleAxis(-mk_arrowAngle, Vector3.forward) * linkDir;

      Vector2 linkCenter = childCenter + linkDir * (distance / 2);
      DrawLine(linkCenter, linkCenter + leftArrowDir * mk_arrowLength);
      DrawLine(linkCenter, linkCenter + rightArrowDir * mk_arrowLength);
    }

    private static void DrawNodes()
    {
      foreach (Node node in ms_graph.Nodes)
        node.Draw(node.IsSelected ? ms_selectedNodeStyle : ms_nodeStyle);
    }

    private static void ProcessMouseDownEvent(Event currentEvent)
    {
      if (currentEvent.button == 0)
        ProcessLeftMouseDown(currentEvent.mousePosition);
      else if (currentEvent.button == 1) 
        ProcessRightMouseDown(currentEvent);
    }

    private static void ProcessLeftMouseDown(Vector2 mousePosition)
    {
      ms_dragNode = ms_graph.GetHoveredNode(mousePosition);

      if (ms_dragNode != null)
        StartDragNode(mousePosition);
      else
        DeselectAllNodes();
    }

    private static void ProcessRightMouseDown(Event currentEvent)
    {
      Node hoveredNode = ms_graph.GetHoveredNode(currentEvent.mousePosition);
      
      if(hoveredNode == null)
        ShowContextMenu(currentEvent.mousePosition);
      else
        StartDragLink(hoveredNode, currentEvent.mousePosition);
    }

    private static void ProcessMouseUpEvent(Event currentEvent)
    {
      if(currentEvent.button == 0)
        ProcessLeftMouseUpEvent();
      else if (currentEvent.button == 1)
        ProcessRightMouseUpEvent();
    }

    private static void ProcessLeftMouseUpEvent() => 
      ms_dragNode = null;

    private static void ProcessRightMouseUpEvent()
    {
      if (ms_dragLinkStartNode != null)
        StopDragLink();
    }

    private static void ProcessMouseDragEvent(Event currentEvent)
    {
      if (currentEvent.button == 0)
      {
        DragNode(currentEvent);
        DragGraph(currentEvent);
      }
      else if (currentEvent.button == 1)
        DragLink(currentEvent);
    }

    private static void ShowContextMenu(Vector2 mousePosition)
    {
      var contextMenu = new GenericMenu();
      
      contextMenu.AddItem(new GUIContent("Create Room Node"), false, CreateRoomNode, mousePosition);
      contextMenu.AddSeparator("");
      contextMenu.AddItem(new GUIContent("Delete All Nodes"), false, DeleteAllNodes);
      contextMenu.AddItem(new GUIContent("Delete Selected Nodes"), false, DeleteSelectedNodes);
      contextMenu.AddItem(new GUIContent("Delete Selected Nodes Links"), false, DeleteSelectedNodesLinks);
      
      contextMenu.ShowAsContext();
    }


    private static void DeselectAllNodes()
    {
      foreach (Node node in ms_graph.Nodes.Where(node => node.IsSelected))
        node.IsSelected = false;

      GUI.changed = true;
    }

    private static void DeleteSelectedNodes()
    {
      foreach (Node node in ms_graph.Nodes.Where(node => node.IsSelected))
      {
        DeleteLinksFor(node);
        ms_graph.IdToNode.Remove(node.Id);
      }

      ms_graph.Nodes.RemoveAll(node => node.IsSelected);

      GUI.changed = true;
    }

    private static void DeleteSelectedNodesLinks()
    {
      foreach (Node node in ms_graph.Nodes.Where(node => node.IsSelected)) 
        DeleteLinksFor(node);

      GUI.changed = true;
    }

    private static void DeleteLinksFor(Node node)
    {
      foreach (string parentId in node.ParentIds)
        ms_graph.IdToNode[parentId].ChildIds.Remove(node.Id);

      foreach (string childId in node.ChildIds)
        ms_graph.IdToNode[childId].ParentIds.Remove(node.Id);

      node.ParentIds.Clear();
      node.ChildIds.Clear();
    }

    private static void StartDragLink(Node hoveredNode, Vector2 mousePosition)
    {
      ms_dragLinkStartNode = hoveredNode;
      ms_dragLinkEndPosition = mousePosition;
    }

    private static void StopDragLink()
    {
      Node linkEndNode = ms_graph.GetHoveredNode(ms_dragLinkEndPosition);
      if (linkEndNode != null) 
        CreateLink(ms_dragLinkStartNode, linkEndNode);
      
      ms_dragLinkStartNode = null;
      GUI.changed = true;
    }

    private static void CreateNodeStyle()
    {
      ms_nodeStyle = new GUIStyle {
        normal = {
          background = EditorGUIUtility.Load("node1") as Texture2D,
          textColor = Color.white
        },
        padding = new RectOffset(mk_nodePadding, mk_nodePadding, mk_nodePadding, mk_nodePadding),
        border = new RectOffset(mk_nodeBorder, mk_nodeBorder, mk_nodeBorder, mk_nodeBorder)
      };
      
      ms_selectedNodeStyle = new GUIStyle {
        normal = {
          background = EditorGUIUtility.Load("node1 on") as Texture2D,
          textColor = Color.white
        },
        padding = new RectOffset(mk_nodePadding, mk_nodePadding, mk_nodePadding, mk_nodePadding),
        border = new RectOffset(mk_nodeBorder, mk_nodeBorder, mk_nodeBorder, mk_nodeBorder)
      };
    }

    private static void CreateRoomNode(object mousePositionObj)
    {
      var mousePosition = (Vector2) mousePositionObj;

      if (ms_graph.Nodes.Count == 0) 
        CreateRoomNode(NodeType.Entrance, mousePosition + Vector2.left * 300);

      CreateRoomNode(NodeType.None, mousePosition);
    }

    private static void CreateRoomNode(NodeType type, Vector2 position)
    {
      var node = new Node(type, new Rect(position, ms_nodeSize));
      ms_graph.AddNode(node);
    }

    private static void DeleteAllNodes()
    {
      ms_graph.DeleteAllNodes();
    }

    private static void CreateLink(Node linkStartNode, Node linkEndNode)
    {
      if(linkStartNode.CanAddChild(linkEndNode))
        linkStartNode.AddChild(linkEndNode);
    }

    private static void StartDragNode(Vector2 mousePosition)
    {
      ms_dragNode.IsSelected = !ms_dragNode.IsSelected;
      ms_dragNodeOffset = ms_dragNode.Transform.position - mousePosition;

      GUI.changed = true;
    }

    private static void DragNode(Event currentEvent)
    {
      if (ms_dragNode == null)
        return;
      
      ms_dragNode.Transform.position = currentEvent.mousePosition + ms_dragNodeOffset;
      GUI.changed = true;
    }

    private static void DragGraph(Event currentEvent)
    {
      if (ms_graph.IsNodeHovered(currentEvent.mousePosition))
        return;
      
      foreach (Node node in ms_graph.Nodes) 
        node.Transform.position += currentEvent.delta;

      GUI.changed = true;
    }

    private static void DragLink(Event currentEvent)
    {
      if (ms_dragLinkStartNode == null)
        return;
      
      ms_dragLinkEndPosition += currentEvent.delta;
      GUI.changed = true;
    }

    private static void SaveGraph()
    {
      EditorUtility.SetDirty(ms_graph);
      AssetDatabase.SaveAssetIfDirty(ms_graph);
    }

    private static void DrawLine(Vector2 start, Vector2 end) =>
      Handles.DrawBezier(start, end, start, end, Color.white, null, mk_linkThickness);
  }
}