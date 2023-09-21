using Rogue.Player;
using UnityEngine;

namespace Rogue.Hero
{
  public class HeroFactory
  {
    public GameObject Create(HeroSO heroSO, Vector3 position)
    {
      GameObject heroObject = Object.Instantiate(heroSO.Prefab);
      
      heroObject.GetComponent<Hero>().Init(heroSO);
      heroObject.transform.position = position;
      
      return heroObject;
    }
  }
}