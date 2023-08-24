using System;
using System.Collections.Generic;
using Rogue.Dungeon;
using Rogue.Dungeon.Data;
using UnityEngine;
using UnityEngine.Serialization;

namespace Rogue.Settings
{
  [Serializable]
  public class Materials
  {
    public Material DimmedMaterial;
  }

  [Serializable]
  public class GameplayAssets
  {
    public List<DungeonLevelSO> Levels;
  }
  
  [CreateAssetMenu(fileName = "GameResources", menuName = "Rogue/Resources")]
  public class GameResourcesSO : ScriptableObject
  {
    public Materials Materials;
    [FormerlySerializedAs("Assets")] public GameplayAssets GameplayAssets;
  }
}