using System;
using System.Collections.Generic;
using Rogue.Dungeon;
using UnityEngine;
using UnityEngine.Serialization;

namespace Rogue.Settings
{
  [Serializable]
  public class DungeonBuilderSettings
  {
    public int FirstLevelIndex = 0;
    [FormerlySerializedAs("MaxRebuildAttemptsForGraph")] public int MaxGraphBuildAttempts = 1000;
    public int MaxDungeonBuildAttempts = 10;
  }
  
  [CreateAssetMenu(fileName = "Settings", menuName = "Rogue/Settings")]
  public class GameSettingsSO : ScriptableObject
  {
    public DungeonBuilderSettings DungeonBuilderSettings;
  }
}