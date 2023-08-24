using System;
using Rogue.Enums;
using UnityEngine;

namespace Rogue.Dungeon
{
    [Serializable]
    public class Doorway
    {
        public Vector2Int position;
        public EOrientation orientation;
        public GameObject doorPrefab;
        public Vector2Int doorwayStartCopyPosition;
        public int doorwayCopyTileWidth;
        public int doorwayCopyTileHeight;
        [NonSerialized] public bool IsConnected = false;

        public Doorway(Doorway other)
        {
            position = other.position;
            orientation = other.orientation;
            doorPrefab = other.doorPrefab;
            doorwayStartCopyPosition = other.doorwayStartCopyPosition;
            doorwayCopyTileWidth = other.doorwayCopyTileWidth;
            doorwayCopyTileHeight = other.doorwayCopyTileHeight;
        }
    }
}
