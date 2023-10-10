using System.Collections.Generic;
using System.Linq;
using Rogue.Dungeon.Data;
using Rogue.Dungeon.Rooms;
using Rogue.NodeGraph;
using Rogue.Settings;
using Rogue.Utilities;
using UnityEditor;
using UnityEngine;

namespace Rogue.Dungeon
{
  public class DungeonBuilder
  {
    private const string mk_rootObjectName = "Dungeon";
    
    private readonly DungeonBuilderSettings m_builderSettings;
    private readonly RoomFactory m_roomFactory;
    private readonly RoomsContainer m_roomsContainer;

    private DungeonLevelSO m_dungeonLevelSO;

    public DungeonBuilder(GameSettingsSO settings, RoomFactory roomFactory, RoomsContainer roomsContainer)
    {
      m_roomFactory = roomFactory;
      m_roomsContainer = roomsContainer;
      m_builderSettings = settings.DungeonBuilderSettings;
    }

    public bool Build(DungeonLevelSO dungeonLevelSO)
    {
      m_dungeonLevelSO = dungeonLevelSO;
      
      int dungeonBuildAttempts = 0;
      bool dungeonBuildSuccess = false;
      
      while (!dungeonBuildSuccess && dungeonBuildAttempts < m_builderSettings.MaxDungeonBuildAttempts)
      {
        dungeonBuildSuccess = Build_Internal();
        ++dungeonBuildAttempts;
      }
      
      return dungeonBuildSuccess;
    }

    private bool Build_Internal()
    {
      RoomNodeGraphSO graphSO = m_dungeonLevelSO.RandomGraph();

      int graphBuildAttempts = 0;
      bool graphBuildSuccess = false;
      while (!graphBuildSuccess && graphBuildAttempts < m_builderSettings.MaxGraphBuildAttempts)
      {
        m_roomsContainer.Clear();
        m_roomsContainer.CreateRootObject();
        
        graphBuildSuccess = BuildGraph(graphSO);
        ++graphBuildAttempts;
      }

      if (graphBuildSuccess)
        InstantiateRoomGameObjects(); 

      return graphBuildSuccess;
    }

    private bool BuildGraph(RoomNodeGraphSO graphSO)
    {
      Queue<Node> nodesToCreate = new();

      Node entrance = graphSO.FirstNodeOf(ENodeType.Entrance);
      if (entrance == null)
      {
        Debug.LogError($"Failed to build graph {graphSO.name}, there is no Entrance node");
        return false;
      }
      
      nodesToCreate.Enqueue(entrance);
      while (nodesToCreate.Count > 0)
      {
        Node roomNode = nodesToCreate.Dequeue();
        
        if (!CreateRoom(roomNode))
          return false;

        foreach (Node childNode in graphSO.ChildNodes(roomNode)) 
          nodesToCreate.Enqueue(childNode);
      }

      return true;
    }

    private void InstantiateRoomGameObjects()
    {
      foreach (Room room in m_roomsContainer.Rooms.Values) 
        m_roomFactory.CreateRoomInstance(room, m_roomsContainer.RootTransform);
    }

    private bool CreateRoom(Node roomNode)
    {
      Room room = null;
      
      if (roomNode.Type == ENodeType.Entrance)
      {
        RoomTemplateSO roomTemplateSO = m_dungeonLevelSO.RandomRoomTemplateOf(roomNode.Type);
        room = CreateRoomFromTemplate(roomTemplateSO, roomNode);
      }
      else if (TryGetParentRoom(roomNode, out Room parentRoom))
      {
        room = CreateRoomFromParentRoom(roomNode, parentRoom);
      }

      if (room == null)
        return false;
      
      AddRoom(room);
      return true;
    }

    private static Room CreateRoomFromTemplate(RoomTemplateSO roomTemplateSO, Node roomNode)
    {
      var room = new Room
      {
        Id = roomNode.Id,
        Template = roomTemplateSO,
        ChildIds = roomNode.ChildIds.ToList(),
        ParentId = roomNode.ParentIds.Count == 0 ? new GUID() : roomNode.ParentIds[0],
        WorldBounds = new RectInt
        {
          position = roomTemplateSO.lowerBounds,
          size = roomTemplateSO.upperBounds - roomTemplateSO.lowerBounds
        },
      };

      foreach (Doorway doorway in roomTemplateSO.doorwayList) 
        room.Doorways.Add(new Doorway(doorway));

      if (roomNode.Type == ENodeType.Entrance)
        room.IsVisited = true;
      
      return room;
    }

    private Room CreateRoomFromParentRoom(Node roomNode, Room parentRoom)
    {
      List<Doorway> availableParentDoorways = parentRoom.AvailableDoorways().ToList();
      
      while (availableParentDoorways.Count > 0)
      {
        Doorway parentDoorway = availableParentDoorways.PopRandomItem();
        
        RoomTemplateSO roomTemplateSO = m_dungeonLevelSO.RandomRoomTemplateFor(roomNode, parentDoorway);

        Room room = CreateRoomFromTemplate(roomTemplateSO, roomNode);

        if (PlaceRoom(room, parentDoorway, parentRoom))
          return room;
      }
      
      return null;
    }

    private bool PlaceRoom(Room room, Doorway parentDoorway, Room parentRoom)
    {
      Doorway doorway = room.OppositeDoorway(parentDoorway);

      if (doorway == null)
        return false;

      Vector2Int worldDoorwayPosition = 
        parentRoom.WorldBounds.position 
        - parentRoom.Template.lowerBounds 
        + parentDoorway.position 
        + parentDoorway.orientation.Direction();
      
      room.WorldBounds.position = worldDoorwayPosition - doorway.position + room.Template.lowerBounds;

      if (OverlapWithRooms(room))
        return false;

      parentDoorway.IsConnected = true;
      doorway.IsConnected = true;
      return true;
    }

    private bool OverlapWithRooms(Room roomToTest) => 
      m_roomsContainer.Rooms.Values.Any(room => room.WorldBounds.Overlaps(roomToTest.WorldBounds));

    private void AddRoom(Room room)
    {
      room.IsPositioned = true;
      m_roomsContainer.Rooms.Add(room.Id, room);
    }

    private bool TryGetParentRoom(Node roomNode, out Room parentRoom) => 
      m_roomsContainer.Rooms.TryGetValue(roomNode.ParentIds[0], out parentRoom);

    public Room RoomOfType(ENodeType type) => 
      m_roomsContainer.Rooms.First(pair => pair.Value.Template.Type == type).Value;
  }
}