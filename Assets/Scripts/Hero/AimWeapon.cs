using Rogue.Input;
using UnityEngine;
using VContainer;

namespace Rogue.Hero
{
  public class AimWeapon : MonoBehaviour
  {
    public Transform WeaponRotationPoint;
    
    private InputService m_inputService;

    [Inject]
    private void Construct(InputService inputService)
    {
      m_inputService = inputService;
    }

    private void Update()
    {
      Aim();
    }

    private void Aim()
    {
      WeaponRotationPoint.eulerAngles = new Vector3(0, 0, m_inputService.AimAngle);

      float yScale = 1;
      if (m_inputService.AimDirection is EAimDirection.Left or EAimDirection.UpLeft)
        yScale = -1;

      WeaponRotationPoint.localScale = new Vector3(1, yScale, 0);
    }
  }
}