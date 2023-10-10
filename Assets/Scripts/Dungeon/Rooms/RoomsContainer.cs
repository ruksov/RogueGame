using System;
using System.Collections.Generic;
using System.Linq;
using Rogue.NodeGraph;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Rogue.Dungeon.Rooms
{
  public class RoomsContainer
  {
    private const string mk_rootObjectName = "Dungeon";
    
    public Room CurrentRoom;
    public Action CurrentRoomChanged;
    
    public Dictionary<GUID, Room> Rooms = new();
    public Transform RootTransform;
    
    public void CreateRootObject()
    {
      var root = new GameObject(mk_rootObjectName);
      RootTransform = root.transform;
    }
    
    public void Clear()
    {
      if (RootTransform == null)
        return;
      
      Object.Destroy(RootTransform.gameObject);
      Rooms.Clear();
    }

    public void SetCurrentRoom(Room room)
    {
      if (CurrentRoom == room)
        return;

      CurrentRoom = room;
      CurrentRoomChanged?.Invoke();
      
      Debug.Log($"Current room changed {room.Instance.gameObject.name}");
    }

    public Room RoomOfType(ENodeType type) => 
      Rooms.Values.First(r => r.Template.Type == type);
  }
}