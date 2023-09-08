using Rogue.Player;
using UnityEngine;

namespace Rogue.Hero
{
  public class Hero : MonoBehaviour
  {
    public HeroSO HeroData;
    public Health Health;
    public SpriteRenderer SpriteRenderer;
    public Animator Animator;

    public void Init(HeroSO heroData)
    {
      HeroData = heroData;
      
      Health.SetDefaultHealth(HeroData.Health);
    }
  }
}