using UnityEngine;

namespace Rogue.UI
{
  public class CustomCursor : MonoBehaviour
  {
    private void Awake() =>
      Cursor.visible = false;

    private void Update() => 
      transform.position = UnityEngine.Input.mousePosition;
  }
}