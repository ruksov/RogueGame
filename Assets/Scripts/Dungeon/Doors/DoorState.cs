using System;
using UnityEngine;

namespace Rogue.Dungeon.Doors
{
  public class DoorState : MonoBehaviour
  {
    public bool IsOpen;
    public bool IsLock;
    
    public TriggerEnter OpenTrigger;
    public Collider2D LockCollider;

    public Action StateChanged;

    private void Awake()
    {
      OpenTrigger.Triggered += Open;
      
      UnLock();
    }

    private void Open(Collider2D other)
    {
      IsOpen = true;
      StateChanged?.Invoke();
    }

    public void Lock()
    {
      IsLock = true;
      IsOpen = false;
      OpenTrigger.Disable();
      LockCollider.enabled = true;
      StateChanged?.Invoke();
    }

    public void UnLock()
    {
      IsLock = false;
      OpenTrigger.Enable();
      LockCollider.enabled = false;
      StateChanged?.Invoke();
    }
  }
}