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

    private static RoomNodeGraph ms_graph;
    private static GUIStyle ms_nodeStyle;

    private static Node ms_selectedNode;
    private static Vector2 ms_nodeDragOffset;

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
      SaveGraph();
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
      foreach (NodeLink link in ms_graph.Links)
        Handles.DrawLine(link.First.Transform.center, link.Second.Transform.center);
    }

    private static void DrawNodes()
    {
      foreach (Node node in ms_graph.Nodes)
        node.Draw(ms_nodeStyle);
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
      ms_selectedNode = ms_graph.GetHoveredNode(mousePosition);
      
      if(ms_selectedNode != null)
        ms_nodeDragOffset = ms_selectedNode.Transform.position - mousePosition;
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
      ms_selectedNode = null;

    private static void ProcessRightMouseUpEvent()
    {
      if (ms_dragLinkStartNode != null)
        StopDragLink();
    }

    private static void ProcessMouseDragEvent(Event currentEvent)
    {
      if(currentEvent.button == 0)
        DragNode(currentEvent);
      else if (currentEvent.button == 1)
        DragConnection(currentEvent);
    }

    private static void ShowContextMenu(Vector2 mousePosition)
    {
      var contextMenu = new GenericMenu();
      
      contextMenu.AddItem(new GUIContent("Create Room Node"), false, CreateRoomNode, mousePosition);
      contextMenu.AddItem(new GUIContent("Delete All Nodes"), false, DeleteAllNodes);
      
      contextMenu.ShowAsContext();
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
    }

    private static void CreateRoomNode(object mousePositionObj)
    {
      var mousePosition = (Vector2) mousePositionObj;
      var node = new Node(new Rect(mousePosition, ms_nodeSize));
      
      ms_graph.Nodes.Add(node);
      ms_graph.IdToNode[node.Id] = node;
    }

    private static void DeleteAllNodes()
    {
      ms_graph.DeleteAllNodes();
    }

    private static void CreateLink(Node linkStartNode, Node linkEndNode)
    {
      if (linkStartNode.Id == linkEndNode.Id ||
          linkStartNode.Links.Any(id => id == linkEndNode.Id))
        return;
      
      linkStartNode.Links.Add(linkEndNode.Id);
      linkEndNode.Links.Add(linkStartNode.Id);
      
      ms_graph.Links.Add(new NodeLink(linkStartNode, linkEndNode));
    }
    
    private static void DragNode(Event currentEvent)
    {
      ms_selectedNode.Transform.position = currentEvent.mousePosition + ms_nodeDragOffset;
      GUI.changed = true;
    }

    private static void DragConnection(Event currentEvent)
    {
      ms_dragLinkEndPosition += currentEvent.delta;
      GUI.changed = true;
    }

    private static void SaveGraph()
    {
      EditorUtility.SetDirty(ms_graph);
      AssetDatabase.SaveAssetIfDirty(ms_graph);
    }
  }
}