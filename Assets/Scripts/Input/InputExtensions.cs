using Rogue.Hero;
using UnityEngine;

namespace Rogue.Input
{
  public static class InputExtensions
  {
    public static float ToAngle2DRad(this Vector3 vector)
      => Mathf.Atan2(vector.y, vector.x);
    
    public static float ToAngle2D(this Vector3 vector) => 
      vector.ToAngle2DRad() * Mathf.Rad2Deg;

    public static EAimDirection ToAimDirection(this Vector3 dir)
    {
      float angle = dir.ToAngle2D();
      angle = Mathf.Clamp(angle, -180, 180);

      return angle switch
      {
        >= 22 and <= 67 => EAimDirection.UpRight,
        > 67 and <= 112 => EAimDirection.Up,
        > 112 and <= 158 => EAimDirection.UpLeft,
        > 158 or <= -135 => EAimDirection.Left,
        > -135 and <= -45 => EAimDirection.Down,
        _ => EAimDirection.Right
      };
    }
  }
}