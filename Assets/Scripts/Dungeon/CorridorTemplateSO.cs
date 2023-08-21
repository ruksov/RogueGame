using Rogue.NodeGraph;
using UnityEngine;

namespace Rogue.Dungeon
{
  [CreateAssetMenu(fileName = "Corridor_", menuName = "Rogue/RoomTemplates/Corridor")]
  public class CorridorTemplateSO : RoomTemplateSO
  {
    public ECorridorDir Direction;

    private void Reset() => 
      Type = NodeType.Corridor;
  }
}