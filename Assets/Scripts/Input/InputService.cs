using Rogue.Hero;
using UnityEngine;
using VContainer.Unity;

namespace Rogue.Input
{
  public class InputService : ITickable
  {
    public float AimAngle;
    public EAimDirection AimDirection;

    private readonly HeroProvider m_heroProvider;

    public InputService(HeroProvider heroProvider)
    {
      m_heroProvider = heroProvider;
    }

    private static Vector3 MouseWorldPosition()
    {
      if (!Camera.main)
        return Vector3.zero;

      Vector3 screenPosition = UnityEngine.Input.mousePosition;

      screenPosition.x = Mathf.Clamp(screenPosition.x, 0, Screen.width);
      screenPosition.y = Mathf.Clamp(screenPosition.y, 0, Screen.height);

      Vector3 worldPosition = Camera.main.ScreenToWorldPoint(screenPosition);
      worldPosition.z = 0;
      return worldPosition;
    }

    public void Tick()
    {
      UpdateHeroAimDirection();
    }

    private void UpdateHeroAimDirection()
    {
      if (!m_heroProvider.IsHeroCreated)
        return;

      AimAngle = (MouseWorldPosition() - m_heroProvider.Hero.transform.position).ToAngle2D();
      AimDirection = AimAngle.ToAimDirection();
    }
  }
}