using System;
using Rogue.Input;
using UnityEngine;
using VContainer;

namespace Rogue.Hero.Components
{
  public class HeroAnimator : MonoBehaviour
  {
    public Animator Animator;
    
    private readonly int m_isIdleId = UnityEngine.Animator.StringToHash("isIdle");
    private readonly int m_isMovingId = UnityEngine.Animator.StringToHash("isMoving");
    
    private readonly int m_aimUpId = UnityEngine.Animator.StringToHash("aimUp");
    private readonly int m_aimUpRightId = UnityEngine.Animator.StringToHash("aimUpRight");
    private readonly int m_aimUpLeftId = UnityEngine.Animator.StringToHash("aimUpLeft");
    private readonly int m_aimRightId = UnityEngine.Animator.StringToHash("aimRight");
    private readonly int m_aimLeftId = UnityEngine.Animator.StringToHash("aimLeft");
    private readonly int m_aimDownId = UnityEngine.Animator.StringToHash("aimDown");
    
    private InputService m_inputService;

    [Inject]
    public void Construct(InputService inputService)
    {
      m_inputService = inputService;
    }

    private void Start()
    {
      StartIdling();
    }

    private void Update()
    {
      UpdateAimParams();
    }

    private void StartIdling()
    {
      Animator.SetBool(m_isMovingId, false);
      Animator.SetBool(m_isIdleId, true);
    }

    private void ClearAimParams()
    {
      Animator.SetBool(m_aimUpId, false);
      Animator.SetBool(m_aimUpRightId, false);
      Animator.SetBool(m_aimUpLeftId, false);
      Animator.SetBool(m_aimRightId, false);
      Animator.SetBool(m_aimLeftId, false);
      Animator.SetBool(m_aimDownId, false);
    }

    private void UpdateAimParams()
    {
      ClearAimParams();

      switch (m_inputService.AimDirection)
      {
        case EAimDirection.Up:
          Animator.SetBool(m_aimUpId, true);
          break;

        case EAimDirection.UpRight:
          Animator.SetBool(m_aimUpRightId, true);
          break;
        
        case EAimDirection.UpLeft:
          Animator.SetBool(m_aimUpLeftId, true);
          break;

        case EAimDirection.Right:
          Animator.SetBool(m_aimRightId, true);
          break;

        case EAimDirection.Left:
          Animator.SetBool(m_aimLeftId, true);
          break;

        case EAimDirection.Down:
          Animator.SetBool(m_aimDownId, true);
          break;
      }
    }
  }
}