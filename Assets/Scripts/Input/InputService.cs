using System;
using Rogue.Hero;
using UnityEngine;
using VContainer.Unity;

namespace Rogue.Input
{
  public class InputService : ITickable
  {
    public Vector3 MouseWorldPosition;
    public EAimDirection AimDirection;

    public Vector2 MoveInput;
    public bool IsMoving => MoveInput.sqrMagnitude > 0;

    public Action AimDirectionChanged;
    public Action RollPerformed;

    private readonly HeroProvider m_heroProvider;
    private readonly InputActions m_actions = new();

    public InputService(HeroProvider heroProvider)
    {
      m_heroProvider = heroProvider;

      m_actions.Gameplay.Roll.performed += _ => RollPerformed?.Invoke();
    }

    public void EnableGameplayInput()
    {
      m_actions.Gameplay.Enable();
    }

    public void Tick()
    {
      if (m_actions.Gameplay.enabled)
        UpdateGameplayInput();
    }

    private void UpdateGameplayInput()
    {
      UpdateMouseWorldPosition();
      UpdateMoveInput();
      UpdateHeroAimDirection();
    }

    private void UpdateMouseWorldPosition() => 
      MouseWorldPosition = ToWorldPosition(m_actions.Gameplay.MousePosition.ReadValue<Vector2>());

    private void UpdateMoveInput()
    {
      MoveInput = m_actions.Gameplay.Move.ReadValue<Vector2>();

      if (Mathf.Abs(MoveInput.x) > 0 && Mathf.Abs(MoveInput.y) > 0) 
        MoveInput *= 0.7f;
    }

    private void UpdateHeroAimDirection()
    {
      if (!m_heroProvider.IsHeroCreated)
        return;
      
      EAimDirection newAimDirection = (MouseWorldPosition - m_heroProvider.Hero.transform.position)
        .ToAngle2D()
        .ToAimDirection();
      
      if (newAimDirection != AimDirection)
      {
        AimDirection = newAimDirection;
        AimDirectionChanged?.Invoke();
      }
    }

    private static Vector3 ToWorldPosition(Vector2 screenPosition)
    {
      if (!UnityEngine.Camera.main)
        return Vector3.zero;

      Vector3 worldPosition = UnityEngine.Camera.main.ScreenToWorldPoint(screenPosition);
      worldPosition.z = 0;
      return worldPosition;
    }
  }
}