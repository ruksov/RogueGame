using System;
using Rogue.Hero;

namespace Rogue.Save
{
  [Serializable]
  public class PlayerSaveData
  {
    public string Name;
    public HeroSO Hero;
  }
}