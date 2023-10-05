using Rogue.Input;
using UnityEngine;
using VContainer;

namespace Rogue.Hero.Components
{
  public class HeroAim : MonoBehaviour
  {
    public Transform WeaponShootPoint;
    public Transform WeaponRotationPoint;
    
    private InputService m_inputService;

    [Inject]
    private void Construct(InputService inputService) => 
      m_inputService = inputService;

    private void Update() => 
      Aim();

    private void Aim()
    {
      float aimAngle = (m_inputService.MouseWorldPosition - WeaponShootPoint.position).ToAngle2D();
      
      WeaponRotationPoint.eulerAngles = new Vector3(0, 0, aimAngle);

      float yScale = 1;
      if (m_inputService.AimDirection is EAimDirection.Left or EAimDirection.UpLeft)
        yScale = -1;

      WeaponRotationPoint.localScale = new Vector3(1, yScale, 0);
    }
  }
}