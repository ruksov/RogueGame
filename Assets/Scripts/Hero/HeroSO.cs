using UnityEngine;

namespace Rogue.Hero
{
  [CreateAssetMenu(fileName = "Hero_", menuName = "Rogue/Player/Hero")]
  public class HeroSO : ScriptableObject
  {
    public string Name;
    public GameObject Prefab;
    public RuntimeAnimatorController AnimatorController;
    public int Health;
    public float MoveSpeed;
    public Sprite MinimapIcon;
    public Sprite HandSprite;
  }
}