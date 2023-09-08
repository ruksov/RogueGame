using UnityEngine;

namespace Rogue.Resources
{
  [CreateAssetMenu(fileName = "GameResources", menuName = "Rogue/Resources")]
  public class GameResourcesSO : ScriptableObject
  {
    public Materials Materials;
    public GameplayAssets GameplayAssets;
  }
}