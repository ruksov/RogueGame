using UnityEngine;

namespace Rogue.Hero.Components
{
  public class HeroHealth : MonoBehaviour
  {
    public int DefaultHealth;
    public int CurrentHealth;

    public void SetDefaultHealth(int value)
    {
      DefaultHealth = value;
      CurrentHealth = value;
    }
  }
}