using System.Collections.Generic;
using System.Linq;
using GameManager;
using Rogue.Dungeon;
using Rogue.Dungeon.Data;
using Rogue.Hero;
using Rogue.NodeGraph;
using Rogue.Resources;
using Rogue.Save;
using Rogue.Settings;
using UnityEngine;
using VContainer.Unity;
using Object = UnityEngine.Object;

namespace Rogue.GameManager
{
  public class GameManager : IStartable, ITickable
  {
    private readonly DungeonBuilderSettings m_settings;
    private readonly List<DungeonLevelSO> m_levels;
    private readonly DungeonBuilder m_dungeonBuilder;
    private readonly GameSavesSO m_saves;

    [HideInInspector]
    public EGameState State = EGameState.None;

    private readonly HeroFactory m_heroFactory;
    private GameObject m_hero;

    public GameManager(GameSettingsSO settings, GameResourcesSO resources, DungeonBuilder dungeonBuilder, GameSavesSO saves, HeroFactory heroFactory)
    {
      m_dungeonBuilder = dungeonBuilder;
      m_saves = saves;
      m_heroFactory = heroFactory;
      m_settings = settings.DungeonBuilderSettings;
      m_levels = resources.GameplayAssets.Levels;
    }

    public void Start() => 
      State = EGameState.InitResources;

    public void Tick()
    {
      HandleGameStates();

      if (Input.GetKeyDown(KeyCode.R))
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
            EGameState.CreatePlayer : 
            EGameState.None;
          
          break;
        
        case EGameState.CreatePlayer:
          m_hero = m_heroFactory.Create( m_saves.PlayerSave.Hero, InitSpawnPlayerPosition());
          State = EGameState.PlayingLevel;
          break;
        
        case EGameState.Restart:
          Object.Destroy(m_hero);
          m_dungeonBuilder.ClearDungeon();
          
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

    private Vector3 InitSpawnPlayerPosition() => 
      m_dungeonBuilder.RoomOfType(ENodeType.Entrance).RandomWorldSpawnPosition();
  }
}