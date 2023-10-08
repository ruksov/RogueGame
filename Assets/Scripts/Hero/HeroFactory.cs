using Rogue.Hero.Components;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Rogue.Hero
{
  public class HeroFactory
  {
    private readonly IObjectResolver m_container;

    public HeroFactory(IObjectResolver container)
    {
      m_container = container;
    }

    public GameObject Create(HeroSO heroSO, Vector3 position)
    {
      GameObject heroObject = m_container.Instantiate(heroSO.Prefab);

      heroObject.transform.position = position;
      
      heroObject.GetComponent<HeroHealth>().SetDefaultHealth(heroSO.Health);
      heroObject.GetComponent<HeroMove>().Speed = heroSO.MoveSpeed;

      return heroObject;
    }
  }
}