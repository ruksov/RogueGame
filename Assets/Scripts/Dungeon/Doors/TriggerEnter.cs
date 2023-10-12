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
      Activate();

    public void Activate() =>
      Collider.enabled = true;

    public void Deactivate() =>
      Collider.enabled = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
      Triggered?.Invoke(other);
      
      if(OneShot)
        Deactivate();
    }
  }
}