using UnityEngine;
using VContainer;

namespace Rogue.Hero.Components
{
  public class HeroFollow : MonoBehaviour
  {
    private HeroProvider m_heroProvider;

    [Inject]
    private void Construct(HeroProvider heroProvider) => 
      m_heroProvider = heroProvider;

    private void Update()
    {
      if (m_heroProvider.IsHeroCreated)
        transform.position = m_heroProvider.Hero.transform.position;
    }
  }
}