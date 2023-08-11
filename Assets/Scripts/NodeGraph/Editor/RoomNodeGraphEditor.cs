using System;
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

    private void OnDestroy() => 
      ms_graph = null;

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
          ProcessMouseEvent(currentEvent);
          break;
      }
    }

    private static void DrawNodes()
    {
      foreach (Node node in ms_graph.Nodes)
        node.Draw(ms_nodeStyle);

      GUI.changed = true;
    }

    private static void ProcessMouseEvent(Event currentEvent)
    {
      if(currentEvent.button == 1)
        ShowContextMenu(currentEvent.mousePosition);
    }

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
      
      AssetDatabase.SaveAssets();
    }

    private static void DeleteAllNodes() => 
      ms_graph.Nodes.Clear();
  }
}