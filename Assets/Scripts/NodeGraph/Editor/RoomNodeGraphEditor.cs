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
      DrawNodes();
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

    private static void DrawNodes()
    {
      foreach (Node node in ms_graph.Nodes)
        node.Draw(ms_nodeStyle);
    }

    private static void ProcessMouseDownEvent(Event currentEvent)
    {
      switch (currentEvent.button)
      {
        case 0:
          ProcessLeftMouseDown(currentEvent.mousePosition);
          break;
        
        case 1:
          ProcessRightMouseDown(currentEvent);
          break;
      }
    }

    private static void ProcessLeftMouseDown(Vector2 mousePosition)
    {
      ms_selectedNode = GetHoveredNode(mousePosition);
      
      if(ms_selectedNode != null)
        ms_nodeDragOffset = ms_selectedNode.Transform.position - mousePosition;
    }

    private static void ProcessRightMouseDown(Event currentEvent)
    {
      if(!IsNodeHovered(currentEvent.mousePosition))
        ShowContextMenu(currentEvent.mousePosition);
    }

    private static void ProcessMouseUpEvent(Event currentEvent)
    {
      if(currentEvent.button == 0)
        ProcessLeftMouseUpEvent();
    }

    private static void ProcessLeftMouseUpEvent()
    {
      ms_selectedNode = null;
    }

    private static void ProcessMouseDragEvent(Event currentEvent)
    {
      if(ms_selectedNode != null)
        DragNode(currentEvent);
    }

    private static Node GetHoveredNode(Vector2 mousePosition) => 
      ms_graph.Nodes.Find(node => node.Transform.Contains(mousePosition));

    private static bool IsNodeHovered(Vector2 mousePosition) => 
      GetHoveredNode(mousePosition) != null;

    private static void ShowContextMenu(Vector2 mousePosition)
    {
      var contextMenu = new GenericMenu();
      
      contextMenu.AddItem(new GUIContent("Create Room Node"), false, CreateRoomNode, mousePosition);
      contextMenu.AddItem(new GUIContent("Delete All Nodes"), false, DeleteAllNodes);
      
      contextMenu.ShowAsContext();
    }

    private static void CreateRoomNode(object mousePositionObj)
    {
      var mousePosition = (Vector2) mousePositionObj;
      ms_graph.Nodes.Add(new Node(new Rect(mousePosition, ms_nodeSize)));
    }

    private static void DeleteAllNodes() => 
      ms_graph.Nodes.Clear();

    private static void DragNode(Event currentEvent)
    {
      ms_selectedNode.Transform.position = currentEvent.mousePosition + ms_nodeDragOffset;
      GUI.changed = true;
    }

    private static void SaveGraph()
    {
      EditorUtility.SetDirty(ms_graph);
      AssetDatabase.SaveAssetIfDirty(ms_graph);
    }
  }
}