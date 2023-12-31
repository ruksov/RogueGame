using System.Collections.Generic;
using System.Linq;
using Rogue.Dungeon;
using Rogue.Dungeon.Data;
using Rogue.Dungeon.Rooms;
using Rogue.Hero;
using Rogue.Input;
using Rogue.NodeGraph;
using Rogue.Resources;
using Rogue.Settings;
using UnityEngine;
using VContainer.Unity;

namespace Rogue.GameStateMachine
{
  public class GameStateMachine : IStartable, ITickable
  {
    private readonly DungeonBuilderSettings m_settings;
    private readonly List<DungeonLevelSO> m_levels;
    private readonly DungeonBuilder m_dungeonBuilder;
    private readonly HeroProvider m_heroProvider;
    private readonly InputService m_inputService;
    private readonly RoomsContainer m_roomsContainer;


    [HideInInspector]
    public EGameState State = EGameState.None;

    public GameStateMachine(GameSettingsSO settings, GameResourcesSO resources, DungeonBuilder dungeonBuilder, HeroProvider heroProvider, InputService inputService, RoomsContainer roomsContainer)
    {
      m_dungeonBuilder = dungeonBuilder;
      m_heroProvider = heroProvider;
      m_inputService = inputService;
      m_roomsContainer = roomsContainer;
      m_settings = settings.DungeonBuilderSettings;
      m_levels = resources.GameplayAssets.Levels;
    }

    public void Start() => 
      State = EGameState.InitResources;

    public void Tick()
    {
      HandleGameStates();

      if (UnityEngine.Input.GetKeyDown(KeyCode.R))
        State = EGameState.Restart;
    }
      
    private void HandleGameStates()
    {
      switch (State)
      {
        case EGameState.None:
          break;
        
        case EGameState.InitResources:
          InitDungeonLevelSOs();
          State = EGameState.CreateDungeon;
          break;
          
        case EGameState.CreateDungeon:
          State = CreateDungeonLevel(m_settings.FirstLevelIndex) ? 
            EGameState.CreateHero : 
            EGameState.None;
          break;
        
        case EGameState.CreateHero:
          m_heroProvider.CreateHero();
          m_inputService.EnableGameplayInput();
          m_roomsContainer.SetCurrentRoom(m_roomsContainer.RoomOfType(ENodeType.Entrance));
          
          State = EGameState.PlayingLevel;
          break;
        
        case EGameState.Restart:
          m_heroProvider.DestroyHero();
          m_roomsContainer.Clear();
          
          State = EGameState.CreateDungeon;
          break;
      }
    }

    private void InitDungeonLevelSOs()
    {
      foreach (DungeonLevelSO dungeonLevelSO in m_levels)
        dungeonLevelSO.IdToRoomTemplate = dungeonLevelSO.RoomTemplates.ToDictionary(t => t.Id);
    }

    private bool CreateDungeonLevel(int levelIndex)
    {
      if (m_dungeonBuilder.Build(m_levels[levelIndex]))
      {
        Debug.Log($"Dungeon {m_levels[levelIndex].name} was created");
        return true;
      }
      
      Debug.LogError($"Failed to create dungeon {m_levels[levelIndex].name}");
      return false;
    }
  }
}