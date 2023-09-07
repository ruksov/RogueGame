using System;
using System.Collections.Generic;
using System.Linq;
using Rogue.Dungeon.Data;
using Rogue.Utilities;
using UnityEditor;
using UnityEngine;

namespace Rogue.Dungeon
{
  [Serializable]
  public class Room
  {
    public GUID Id;
    public RoomTemplateSO Template;
    public RectInt WorldBounds;
    public List<GUID> ChildIds = new();
    public GUID ParentId;
    public List<Doorway> Doorways = new();
    public bool IsPositioned = false;
    public bool IsLit = false;
    public bool IsCleared = false;
    public bool IsVisited = false;
    public RoomInstance Instance;

    public Doorway OppositeDoorway(Doorway doorway) => 
      Doorways.Find(d => d.orientation == doorway.orientation.Opposite());

    public IEnumerable<Doorway> AvailableDoorways() =>
      Doorways.Where(doorway => !doorway.IsConnected);
  }
}