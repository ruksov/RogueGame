using System.Collections.Generic;
using Rogue.Dungeon;
using UnityEngine;

namespace Dungeon
{
  public class Room
  {
    public string Id;
    public RoomTemplateSO Template;
    public RectInt GridTransform;
    public List<string> Childs = new();
    public string Parent;
    public List<Doorway> Doorways = new();
    public bool IsPositioned = false;
    public bool IsLit = false;
    public bool IsCleared = false;
    public bool IsVisited = false;
    public RoomInstance Instance;
  }
}