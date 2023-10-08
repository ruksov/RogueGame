using Rogue.Input;
using UnityEngine;
using VContainer;

namespace Rogue.Hero.Components
{
  public class HeroAnimator : MonoBehaviour
  {
    public Animator Animator;
    public HeroRoll HeroRoll;
    
    private readonly int m_isIdleId = Animator.StringToHash("isIdle");
    private readonly int m_isMovingId = Animator.StringToHash("isMoving");
    
    private readonly int m_aimUpId = Animator.StringToHash("aimUp");
    private readonly int m_aimUpRightId = Animator.StringToHash("aimUpRight");
    private readonly int m_aimUpLeftId = Animator.StringToHash("aimUpLeft");
    private readonly int m_aimRightId = Animator.StringToHash("aimRight");
    private readonly int m_aimLeftId = Animator.StringToHash("aimLeft");
    private readonly int m_aimDownId = Animator.StringToHash("aimDown");
    
    private readonly int m_rollUpId = Animator.StringToHash("rollUp");
    private readonly int m_rollDownId = Animator.StringToHash("rollDown");
    private readonly int m_rollLeftId = Animator.StringToHash("rollLeft");
    private readonly int m_rollRightId = Animator.StringToHash("rollRight");

    private InputService m_inputService;

    [Inject]
    public void Construct(InputService inputService)
    {
      m_inputService = inputService;
      
      m_inputService.AimDirectionChanged += UpdateAimParams;
      HeroRoll.IsRollingChanged += UpdateRollParams;
    }

    private void OnDestroy()
    {
      m_inputService.AimDirectionChanged -= UpdateAimParams;
    }

    private void Update()
    {
      UpdateMovementParams();
      UpdateRollParams();
    }

    private void UpdateRollParams()
    {
      ClearRollParams();

      if (!HeroRoll.IsRolling)
        return;
      
      Animator.SetBool(m_isIdleId, false);
      Animator.SetBool(m_isMovingId, false);
      
      if(m_inputService.MoveInput == Vector2.up)
        Animator.SetBool(m_rollUpId, true);
      else if(m_inputService.MoveInput == Vector2.down)
        Animator.SetBool(m_rollDownId, true);
      else if(m_inputService.MoveInput == Vector2.left)
        Animator.SetBool(m_rollLeftId, true);
      else if(m_inputService.MoveInput == Vector2.right)
        Animator.SetBool(m_rollRightId, true);
    }

    private void ClearRollParams()
    {
      Animator.SetBool(m_rollUpId, false);
      Animator.SetBool(m_rollDownId, false);
      Animator.SetBool(m_rollLeftId, false);
      Animator.SetBool(m_rollRightId, false);
    }

    private void UpdateMovementParams()
    {
      if (HeroRoll.IsRolling)
        return;
      
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