using System.Collections.Generic;
using System.Linq;
using Rogue.Enums;
using Rogue.Utilities;
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
    public List<Tilemap> Tilemaps;

    public bool InitFromCode;

    public void Initialize(Room room)
    {
      Room = room;
      Collider = GetComponent<BoxCollider2D>();
      Grid = GetComponentInChildren<Grid>();
      InitTilemaps(GetComponentsInChildren<Tilemap>());

      BlockUnconnectedDoorways();
      
      DisableCollisionTilemapRenderer();
    }

    private void BlockUnconnectedDoorways()
    {
      foreach (Doorway doorway in Room.Doorways.Where(d => !d.IsConnected))
      {
        foreach (Tilemap tilemap in Tilemaps) 
          BlockDoorwayOnTilemap(tilemap, doorway);
      }
    }

    private void BlockDoorwayOnTilemap(Tilemap tilemap, Doorway doorway)
    {
      switch (doorway.orientation)
      {
        case EOrientation.North:
        case EOrientation.South:
          BlockDoorwayHorizontally(tilemap, doorway);
          break;
        
        case EOrientation.East:
        case EOrientation.West:
          BlockDoorwayVertically(tilemap, doorway);
          break;
      }
    }

    private void BlockDoorwayVertically(Tilemap tilemap, Doorway doorway)
    {
      Vector3Int startCopyPosition = doorway.doorwayStartCopyPosition.ToVector3Int();

      for (var x = 0; x < doorway.doorwayCopyTileWidth; ++x)
      {
        Vector3Int origPosition = startCopyPosition + Vector3Int.right * x;
        Matrix4x4 origTransform = tilemap.GetTransformMatrix(origPosition);

        for (var y = 0; y < doorway.doorwayCopyTileHeight; ++y)
        {
          Vector3Int positionToCopy = origPosition + Vector3Int.down * (y + 1);
          
          tilemap.SetTile(positionToCopy, tilemap.GetTile(origPosition));
          tilemap.SetTransformMatrix(positionToCopy, origTransform);
        }
      }
    }

    private void BlockDoorwayHorizontally(Tilemap tilemap, Doorway doorway)
    {
      Vector3Int startCopyPosition = doorway.doorwayStartCopyPosition.ToVector3Int();

      for (var y = 0; y < doorway.doorwayCopyTileHeight; ++y)
      {
        Vector3Int origPosition = startCopyPosition + Vector3Int.down * y;
        Matrix4x4 origTransform = tilemap.GetTransformMatrix(origPosition);

        for (var x = 0; x < doorway.doorwayCopyTileWidth; ++x)
        {
          Vector3Int positionToCopy = origPosition + Vector3Int.right * (x + 1);
          
          tilemap.SetTile(positionToCopy, tilemap.GetTile(origPosition));
          tilemap.SetTransformMatrix(positionToCopy, origTransform);
        }
      }
    }

    private void InitTilemaps(Tilemap[] tilemaps)
    {
      foreach (Tilemap tilemap in tilemaps)
      {
        Tilemaps.Add(tilemap);
        
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
        else
          Tilemaps.RemoveAt(Tilemaps.Count - 1);
      }
    }

    private void DisableCollisionTilemapRenderer() =>
      CollisionTilemap.GetComponent<TilemapRenderer>().enabled = false;
  }
}