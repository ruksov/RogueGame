using Rogue.NodeGraph;
using UnityEngine;

namespace Rogue.Dungeon.Data
{
  [CreateAssetMenu(fileName = "Corridor_", menuName = "Rogue/RoomTemplates/Corridor")]
  public class CorridorTemplateSO : RoomTemplateSO
  {
    public ECorridorDir Direction;

    private void Reset() => 
      Type = ENodeType.Corridor;
  }
}