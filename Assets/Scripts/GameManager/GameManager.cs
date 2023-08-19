using System.Collections.Generic;
using Misc;
using UnityEngine;

namespace GameManager
{
  [DisallowMultipleComponent]
  public class GameManager : SingletonMonoBehaviour<GameManager>
  {
    public List<DungeonLevelSO> Levels;
    public int CurrentLevelIndex = 0;
    
    [HideInInspector]
    public EGameState State = EGameState.None;

    private void Start() => 
      State = EGameState.GameStarted;

    private void Update()
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
          PlayDungeonLevel(CurrentLevelIndex);
          State = EGameState.PlayingLevel;
          break;
      }
    }

    private void PlayDungeonLevel(int levelIndex)
    {
    }
  }
}