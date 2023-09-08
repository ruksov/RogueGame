using UnityEngine;

namespace Rogue.Hero
{
  public class Health : MonoBehaviour
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