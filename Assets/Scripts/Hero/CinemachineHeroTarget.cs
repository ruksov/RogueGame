using Cinemachine;
using UnityEngine;
using VContainer;

namespace Rogue.Hero
{
  public class CinemachineHeroTarget : MonoBehaviour
  {
    public CinemachineTargetGroup CinemachineTargetGroup;
    
    private HeroProvider m_heroProvider;

    [Inject]
    private void Construct(HeroProvider heroProvider)
    {
      m_heroProvider = heroProvider;
      m_heroProvider.HeroCreated += OnHeroCreated;
      m_heroProvider.HeroDestroyed += OnHerDestroyed;

      AddHero();
    }

    private void AddHero()
    {
      if(m_heroProvider.Hero == null)
        return;
      
      CinemachineTargetGroup.AddMember(m_heroProvider.Hero.transform, 1, 1);
    }

    private void RemoveHero()
    {
      if (m_heroProvider.Hero == null)
        return;
      
      CinemachineTargetGroup.RemoveMember(m_heroProvider.Hero.transform);
    }

    private void OnHeroCreated() => 
      AddHero();

    private void OnHerDestroyed() => 
      RemoveHero();
  }
}