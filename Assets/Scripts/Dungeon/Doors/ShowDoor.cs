using UnityEngine;

namespace Rogue.Dungeon.Doors
{
  public class ShowDoor : MonoBehaviour
  {
    public FadeObject FadeObject;

    private void Awake() => 
      FadeObject.Hide();

    private void OnTriggerEnter2D(Collider2D other)
    {
      FadeObject.FadeIn();
      gameObject.SetActive(false);
    }
  }
}