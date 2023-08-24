using System;
using System.Collections.Generic;
using System.Linq;
using GameManager;
using Rogue.Dungeon;
using Rogue.Dungeon.Data;
using Rogue.Settings;
using UnityEngine;
using VContainer.Unity;

namespace Rogue.GameManager
{
  public class GameManager : IStartable, ITickable
  {
    private readonly DungeonBuilderSettings m_settings;
    private readonly List<DungeonLevelSO> m_levels;
    private readonly DungeonBuilder m_dungeonBuilder;

    [HideInInspector]
    public EGameState State = EGameState.None;

    public GameManager(GameSettingsSO settings, GameResourcesSO resources, DungeonBuilder dungeonBuilder)
    {
      m_dungeonBuilder = dungeonBuilder;
      m_settings = settings.DungeonBuilderSettings;
      m_levels = resources.GameplayAssets.Levels;
    }

    public void Start() => 
      State = EGameState.GameStarted;

    public void Tick()
    {
      HandleGameStates();

      if (Input.GetKeyDown(KeyCode.R))
        State = EGameState.GameStarted;
    }
      
    private void HandleGameStates()
    {
      switch (State)
      {
        case EGameState.None:
          break;
        
        case EGameState.GameStarted:
          InitDungeonLevelSOs();
          PlayDungeonLevel(m_settings.FirstLevelIndex);
          State = EGameState.PlayingLevel;
          break;
      }
    }

    private void InitDungeonLevelSOs()
    {
      foreach (DungeonLevelSO dungeonLevelSO in m_levels)
        dungeonLevelSO.IdToRoomTemplate = dungeonLevelSO.RoomTemplates.ToDictionary(t => t.Id);
    }

    private void PlayDungeonLevel(int levelIndex)
    {
      if(m_dungeonBuilder.Build(m_levels[levelIndex]))
        Debug.Log($"Dungeon {m_levels[levelIndex].name} was created");
      else
        Debug.LogError($"Failed to create dungeon {m_levels[levelIndex].name}");
    }
  }
}