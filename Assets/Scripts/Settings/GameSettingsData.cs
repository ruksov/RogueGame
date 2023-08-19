using System;
using System.Collections.Generic;
using UnityEngine;

namespace Rogue.Settings
{
  [Serializable]
  public class LevelBuilderSettings
  {
    public List<DungeonLevelSO> Levels;
    public int FirstLevelIndex = 0;
  }
  
  [CreateAssetMenu(fileName = "Settings", menuName = "Rogue/Settings")]
  public class GameSettingsData : ScriptableObject
  {
    public LevelBuilderSettings LevelBuilder;
  }
}