using System;
using System.Collections.Generic;
using Rogue.Enums;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Rogue.Utilities
{
  public static class Extensions
  {
    public static RectInt ToRectInt(this Rect rect) => 
      new((int)rect.xMin, (int)rect.xMax, (int)rect.width, (int)rect.height);

    public static T RandomItem<T>(this List<T> list) where T : class => 
      list.Count == 0 ? null : list[Random.Range(0, list.Count)];

    public static T PopRandomItem<T>(this List<T> list) where T : class
    {
      int randomIndex = Random.Range(0, list.Count);
      if (list.Count == 0)
        return null;
      
      T item = list[randomIndex];
      list.RemoveAt(randomIndex);
      return item;
    }

    public static EOrientation Opposite(this EOrientation orientation)
    {
      switch (orientation)
      {
        case EOrientation.North:
          return EOrientation.South;
        
        case EOrientation.East:
          return EOrientation.West;
        
        case EOrientation.South:
          return EOrientation.North;
        
        case EOrientation.West:
          return EOrientation.East;
      }

      return EOrientation.None;
    }

    public static Vector2Int Direction(this EOrientation orientation)
    {
      switch (orientation)
      {
        case EOrientation.North:
          return Vector2Int.up;
        
        case EOrientation.East:
          return Vector2Int.right;
        
        case EOrientation.South:
          return Vector2Int.down;
        
        case EOrientation.West:
          return Vector2Int.left;
      }

      return Vector2Int.zero;
    }
  }
}