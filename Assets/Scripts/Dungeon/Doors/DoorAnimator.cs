using UnityEngine;

namespace Rogue.Dungeon.Doors
{
  public class DoorAnimator : MonoBehaviour
  {
    public DoorState DoorState;
    public Animator Animator;
    
    private readonly int m_isOpenId = Animator.StringToHash("open");

    private void Awake()
    {
      DoorState.StateChanged += UpdateOpenParam;
    }

    private void UpdateOpenParam() => 
      Animator.SetBool(m_isOpenId, DoorState.IsOpen);
  }
}