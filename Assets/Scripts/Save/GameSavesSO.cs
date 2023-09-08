using UnityEngine;

namespace Rogue.Save
{
  [CreateAssetMenu(fileName = "Game Saves", menuName = "Rogue/Game Saves")]
  public class GameSavesSO : ScriptableObject
  {
    public PlayerSaveData PlayerSave;
  }
}