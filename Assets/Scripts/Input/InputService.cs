using System;
using Rogue.Hero;
using UnityEngine;
using VContainer.Unity;

namespace Rogue.Input
{
  public class InputService : ITickable
  {
    public float AimAngle;
    public Vector3 MouseWorldPosition;
    public EAimDirection AimDirection;

    public Vector2 MoveInput;
    public bool IsMoving => MoveInput.sqrMagnitude > 0;

    public Action AimDirectionChanged;

    private readonly HeroProvider m_heroProvider;

    public InputService(HeroProvider heroProvider)
    {
      m_heroProvider = heroProvider;
    }

    private static Vector3 ComputeMouseWorldPosition()
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

      MouseWorldPosition = ComputeMouseWorldPosition();

      EAimDirection newAimDirection = (MouseWorldPosition - m_heroProvider.Hero.transform.position)
        .ToAngle2D()
        .ToAimDirection();
      
      if (newAimDirection != AimDirection)
      {
        AimDirection = newAimDirection;
        AimDirectionChanged?.Invoke();
      }
    }
  }
}