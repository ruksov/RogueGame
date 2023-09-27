using Rogue.Player;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Rogue.Hero
{
  public class HeroFactory
  {
    public Hero Create(HeroSO heroSO, Vector3 position)
    {
      GameObject heroObject = Object.Instantiate(heroSO.Prefab);

      var hero = heroObject.GetComponent<Hero>();
      hero.Init(heroSO);
      heroObject.transform.position = position;
      
      return hero;
    }
  }
}