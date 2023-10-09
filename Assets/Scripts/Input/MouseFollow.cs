using UnityEngine;
using VContainer;

namespace Rogue.Input
{
  public class MouseFollow : MonoBehaviour
  {
    private InputService m_inputService;

    [Inject]
    private void Construct(InputService inputService) => 
      m_inputService = inputService;

    private void Update() => 
      transform.position = m_inputService.MouseWorldPosition;
  }
}