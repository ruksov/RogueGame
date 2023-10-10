using System;
using UnityEngine;

namespace Rogue.Dungeon.Doors
{
  public class TriggerEnter : MonoBehaviour
  {
    public Collider2D Collider;
    public bool OneShot;
    
    public Action<Collider2D> Triggered;

    public void Reset() =>
      Enable();

    public void Enable() =>
      Collider.enabled = true;

    public void Disable() =>
      Collider.enabled = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
      Triggered?.Invoke(other);
      
      if(OneShot)
        Disable();
    }
  }
}