using Rogue.Input;
using UnityEngine;
using VContainer;

namespace Rogue.Hero.Components
{
  public class HeroMove : MonoBehaviour
  {
    public float Speed;
    public Rigidbody2D Rigidbody;

    private InputService m_inputService;

    [Inject]
    private void Construct(InputService inputService)
    {
      m_inputService = inputService;
    }

    private void FixedUpdate()
    {
      Rigidbody.velocity = m_inputService.MoveInput * Speed;
    }
  }
}