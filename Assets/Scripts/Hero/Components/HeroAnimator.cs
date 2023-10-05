using Rogue.Input;
using UnityEngine;
using VContainer;

namespace Rogue.Hero.Components
{
  public class HeroAnimator : MonoBehaviour
  {
    public Animator Animator;
    
    private readonly int m_isIdleId = Animator.StringToHash("isIdle");
    private readonly int m_isMovingId = Animator.StringToHash("isMoving");
    
    private readonly int m_aimUpId = Animator.StringToHash("aimUp");
    private readonly int m_aimUpRightId = Animator.StringToHash("aimUpRight");
    private readonly int m_aimUpLeftId = Animator.StringToHash("aimUpLeft");
    private readonly int m_aimRightId = Animator.StringToHash("aimRight");
    private readonly int m_aimLeftId = Animator.StringToHash("aimLeft");
    private readonly int m_aimDownId = Animator.StringToHash("aimDown");
    
    private InputService m_inputService;

    [Inject]
    public void Construct(InputService inputService)
    {
      m_inputService = inputService;
      m_inputService.AimDirectionChanged += UpdateAimParams;
    }

    private void OnDestroy()
    {
      m_inputService.AimDirectionChanged -= UpdateAimParams;
    }

    private void Update()
    {
      UpdateMovementParams();
    }

    private void UpdateMovementParams()
    {
      Animator.SetBool(m_isMovingId, m_inputService.IsMoving);
      Animator.SetBool(m_isIdleId, !m_inputService.IsMoving);
    }

    private void UpdateAimParams()
    {
      ClearAimParams();
      SetAimParam(m_inputService.AimDirection, true);
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

    private void SetAimParam(EAimDirection aimDirection, bool value)
    {
      switch (aimDirection)
      {
        case EAimDirection.Up:
          Animator.SetBool(m_aimUpId, value);
          break;

        case EAimDirection.UpRight:
          Animator.SetBool(m_aimUpRightId, value);
          break;

        case EAimDirection.UpLeft:
          Animator.SetBool(m_aimUpLeftId, value);
          break;

        case EAimDirection.Right:
          Animator.SetBool(m_aimRightId, value);
          break;

        case EAimDirection.Left:
          Animator.SetBool(m_aimLeftId, value);
          break;

        case EAimDirection.Down:
          Animator.SetBool(m_aimDownId, value);
          break;
      }
    }
  }
}