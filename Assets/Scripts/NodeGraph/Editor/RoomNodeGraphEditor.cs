using System;
using UnityEditor;
using UnityEngine;

namespace Rogue.NodeGraph.Editor
{
  public class RoomNodeGraphEditor : EditorWindow
  {
    [MenuItem("Room Node Graph Editor", menuItem = "Tools/Room Node Graph Editor")]
    private static void OpenWindow()
    {
      GetWindow<RoomNodeGraphEditor>("Room Node Graph Editor");
    }

    private void OnGUI()
    {
      Debug.Log("OnGUI");
    }
  }
}