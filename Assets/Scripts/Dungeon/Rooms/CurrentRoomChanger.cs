using UnityEngine;
using VContainer;

namespace Rogue.Dungeon.Rooms
{
  public class CurrentRoomChanger : MonoBehaviour
  {
    public RoomInstance RoomInstance;
    
    private RoomsContainer m_roomsContainer;

    [Inject]
    public void Construct(RoomsContainer roomsContainer)
    {
      m_roomsContainer = roomsContainer;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
      m_roomsContainer.SetCurrentRoom(RoomInstance.Room);
    }
  }
}