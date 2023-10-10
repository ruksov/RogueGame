using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Rogue.Dungeon.Rooms
{
  public class RoomFactory
  {
    private readonly IObjectResolver m_container;

    public RoomFactory(IObjectResolver container)
    {
      m_container = container;
    }

    public RoomInstance CreateRoomInstance(Room room, Transform parent)
    {
      var roomObjectPosition = new Vector3(
        room.WorldBounds.position.x - room.Template.lowerBounds.x, 
        room.WorldBounds.position.y - room.Template.lowerBounds.y);
        
      GameObject roomObject = m_container.Instantiate(room.Template.prefab, roomObjectPosition, Quaternion.identity, parent);

      var roomInstance = roomObject.GetComponent<RoomInstance>();
      roomInstance.Initialize(room);
      room.Instance = roomInstance;

      return roomInstance;
    }
  }
}