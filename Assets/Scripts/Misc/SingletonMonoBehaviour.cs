using UnityEngine;

namespace Misc
{
  public class SingletonMonoBehaviour<T> : MonoBehaviour where T : MonoBehaviour
  {
    public static T Instance { get; private set; }

    private void Awake()
    {
      if(Instance)
        Destroy(gameObject);

      Instance = this as T;
      DontDestroyOnLoad(Instance);
    }
  }
}