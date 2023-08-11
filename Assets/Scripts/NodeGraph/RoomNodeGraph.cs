using System.Collections.Generic;
using UnityEngine;

namespace Rogue.NodeGraph
{
  [CreateAssetMenu(fileName = "RoomNodeGraph_", menuName = "Rogue/RoomNodeGraph/Graph", order = 0)]
  public class RoomNodeGraph : ScriptableObject
  {
    public List<Node> Nodes = new();
  }
}