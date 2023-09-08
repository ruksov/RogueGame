using System;
using UnityEngine.Serialization;

namespace Rogue.Settings
{
  [Serializable]
  public class DungeonBuilderSettings
  {
    public int FirstLevelIndex = 0;
    public int MaxGraphBuildAttempts = 1000;
    public int MaxDungeonBuildAttempts = 10;
  }
}