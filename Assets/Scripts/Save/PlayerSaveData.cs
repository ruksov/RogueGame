using System;
using Rogue.Player;

namespace Rogue.Save
{
  [Serializable]
  public class PlayerSaveData
  {
    public string Name;
    public HeroSO Hero;
  }
}