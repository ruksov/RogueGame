using System;
using UnityEngine;
using VContainer;

namespace Rogue.Dungeon.Rooms
{
  public class CurrentRoomChanger : MonoBehaviour
  {
    private RoomInstance m_roomInstance;
    private RoomsContainer m_roomsContainer;

    [Inject]
    public void Construct(RoomsContainer roomsContainer)
    {
      m_roomsContainer = roomsContainer;
    }

    private void Awake() => 
      m_roomInstance = GetComponent<RoomInstance>();

    private void OnTriggerEnter2D(Collider2D other)
    {
      m_roomsContainer.SetCurrentRoom(m_roomInstance.Room);
    }
  }
}