using UnityEngine;

namespace Rogue.Settings
{
  [CreateAssetMenu(fileName = "Settings", menuName = "Rogue/Settings")]
  public class GameSettingsSO : ScriptableObject
  {
    public DungeonBuilderSettings DungeonBuilderSettings;
  }
}