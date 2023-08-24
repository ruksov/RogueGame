using UnityEngine;
using UnityEngine.Tilemaps;

namespace Rogue.Dungeon
{
  public class RoomInstance : MonoBehaviour
  {
    public Room Room;
    public Grid Grid;
    public Tilemap GroundTilemap;
    public Tilemap Decoration1Tilemap;
    public Tilemap Decoration2Tilemap;
    public Tilemap FrontTilemap;
    public Tilemap CollisionTilemap;
    public Tilemap MinimapTilemap;
    public BoxCollider2D Collider;

    public bool InitFromCode;

    public void Initialize(Room room)
    {
      Room = room;
      Collider = GetComponent<BoxCollider2D>();
      Grid = GetComponentInChildren<Grid>();
      InitTilemaps(GetComponentsInChildren<Tilemap>());

      DisableCollisionTilemapRenderer();
    }

    private void InitTilemaps(Tilemap[] tilemaps)
    {
      foreach (Tilemap tilemap in tilemaps)
      {
        if (tilemap.CompareTag("groundTilemap"))
          GroundTilemap = tilemap;
        else if (tilemap.CompareTag("decoration1Tilemap"))
          Decoration1Tilemap = tilemap;
        else if (tilemap.CompareTag("decoration2Tilemap"))
          Decoration2Tilemap = tilemap;
        else if (tilemap.CompareTag("frontTilemap"))
          FrontTilemap = tilemap;
        else if (tilemap.CompareTag("collisionTilemap"))
          CollisionTilemap = tilemap;
        else if (tilemap.CompareTag("minimapTilemap"))
          MinimapTilemap = tilemap;
      }
    }

    private void DisableCollisionTilemapRenderer() =>
      CollisionTilemap.GetComponent<TilemapRenderer>().enabled = false;
  }
}