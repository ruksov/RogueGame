using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Rogue.Dungeon.Rooms
{
  [RequireComponent(typeof(FadeObject))]
  public class ShowRoom : MonoBehaviour
  {
    private FadeObject m_roomFade;
    private readonly List<FadeObject> m_doorFades = new();

    private void Awake()
    {
      m_roomFade = GetComponent<FadeObject>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
      m_roomFade.FadeIn();
      
      foreach (FadeObject doorFade in m_doorFades) 
        doorFade.FadeIn();

      enabled = false;
    }

    public void Init(IEnumerable<TilemapRenderer> tilemapRenderers, IEnumerable<FadeObject> doorFades)
    {
      m_roomFade.Renderers.Clear();
      m_doorFades.Clear();
      
      foreach (TilemapRenderer tilemapRenderer in tilemapRenderers) 
        m_roomFade.Renderers.Add(tilemapRenderer);

      foreach (FadeObject doorFade in doorFades) 
        m_doorFades.Add(doorFade);
      
      m_roomFade.Hide();
    }
  }
}