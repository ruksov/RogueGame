using Rogue.Input;
using UnityEngine;
using VContainer;

namespace Rogue.Hero.Components
{
  public class HeroMove : MonoBehaviour
  {
    public Rigidbody2D Rigidbody;
    public float MoveSpeed;
    
    private InputService m_inputService;

    [Inject]
    private void Construct(InputService inputService)
    {
      m_inputService = inputService;
    }

    private void Update()
    {
      Rigidbody.velocity = m_inputService.MoveInput * MoveSpeed;
    }
  }
}