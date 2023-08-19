using Rogue.Settings;
using UnityEngine;
using VContainer.Unity;

namespace GameManager
{
  public class GameManager : IStartable, ITickable
  {
    private readonly LevelBuilderSettings m_settings;
    
    [HideInInspector]
    public EGameState State = EGameState.None;

    public GameManager(GameSettingsData settings)
    {
      m_settings = settings.LevelBuilder;
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
          PlayDungeonLevel(m_settings.FirstLevelIndex);
          State = EGameState.PlayingLevel;
          break;
      }
    }

    private void PlayDungeonLevel(int levelIndex)
    {
      Debug.Log($"play level {m_settings.Levels[levelIndex].Name}");
    }
  }
}