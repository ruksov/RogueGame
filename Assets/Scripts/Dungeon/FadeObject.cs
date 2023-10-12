using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rogue.Dungeon
{
  public class FadeObject : MonoBehaviour
  {
    public List<Renderer> Renderers;
    public float FadeInTime;

    private bool m_isFadeIn;
    private static readonly int ms_alphaSlider = Shader.PropertyToID("Alpha_Slider");

    public void FadeIn()
    {
      if (m_isFadeIn)
        return;

      foreach (Renderer objectRenderer in Renderers)
      {
        StartCoroutine(FadeInRoutine(objectRenderer));
      }

      m_isFadeIn = true;
    }

    public void Hide()
    {
      foreach (Renderer spriteRenderer in Renderers) 
        spriteRenderer.material.SetFloat(ms_alphaSlider, 0);

      m_isFadeIn = false;
    }

    private IEnumerator FadeInRoutine(Renderer objectRenderer)
    {
      float alpha = 0;
      while (alpha < 1)
      {
        objectRenderer.material.SetFloat(ms_alphaSlider, alpha);

        alpha += Time.deltaTime / FadeInTime;
        yield return null;
      }
      
      objectRenderer.material.SetFloat(ms_alphaSlider, 1);
    }
  }
}