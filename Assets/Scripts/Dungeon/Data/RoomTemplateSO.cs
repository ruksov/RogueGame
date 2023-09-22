using System.Collections.Generic;
using Rogue.NodeGraph;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

namespace Rogue.Dungeon.Data
{
  [CreateAssetMenu(fileName = "Room_", menuName = "Rogue/RoomTemplates/Room")]
  public class RoomTemplateSO : ScriptableObject, ISerializationCallbackReceiver
  {
    [HideInInspector] public GUID Id;
    [HideInInspector][SerializeField] private string m_id;

    #region Header ROOM PREFAB

    [Space(10)]
    [Header("ROOM PREFAB")]

    #endregion Header ROOM PREFAB

    #region Tooltip

    [Tooltip(
      "The gameobject prefab for the room (this will contain all the tilemaps for the room and environment game objects")]

    #endregion Tooltip

    public GameObject prefab;

    [HideInInspector]
    public GameObject
      previousPrefab; // this is used to regenerate the guid if the so is copied and the prefab is changed


    #region Header ROOM CONFIGURATION

    [FormerlySerializedAs("Type")]
    [FormerlySerializedAs("roomNodeType")]
    [Space(10)]
    [Header("ROOM CONFIGURATION")]

    #endregion Header ROOM CONFIGURATION

    #region Tooltip

    [Tooltip(
      "The room node type SO. The room node types correspond to the room nodes used in the room node graph.  The exceptions being with corridors.  In the room node graph there is just one corridor type 'Corridor'.  For the room templates there are 2 corridor node types - CorridorNS and CorridorEW.")]

    #endregion Tooltip

    public ENodeType Type;

    #region Tooltip

    [Tooltip(
      "If you imagine a rectangle around the room tilemap that just completely encloses it, the room lower bounds represent the bottom left corner of that rectangle. This should be determined from the tilemap for the room (using the coordinate brush pointer to get the tilemap grid position for that bottom left corner (Note: this is the local tilemap position and NOT world position")]

    #endregion Tooltip

    public Vector2Int lowerBounds;

    #region Tooltip

    [Tooltip(
      "If you imagine a rectangle around the room tilemap that just completely encloses it, the room upper bounds represent the top right corner of that rectangle. This should be determined from the tilemap for the room (using the coordinate brush pointer to get the tilemap grid position for that top right corner (Note: this is the local tilemap position and NOT world position")]

    #endregion Tooltip

    public Vector2Int upperBounds;

    #region Tooltip

    [Tooltip(
      "There should be a maximum of four doorways for a room - one for each compass direction.  These should have a consistent 3 tile opening size, with the middle tile position being the doorway coordinate 'position'")]

    #endregion Tooltip

    [SerializeField]
    public List<Doorway> doorwayList;

    #region Tooltip

    [FormerlySerializedAs("spawnPositionArray")]
    [Tooltip(
      "Each possible spawn position (used for enemies and chests) for the room in tilemap coordinates should be added to this array")]

    #endregion Tooltip

    public Vector2Int[] SpawnGridCells;

    /// <summary>
    /// Returns the list of Entrances for the room template
    /// </summary>
    public List<Doorway> GetDoorwayList()
    {
      return doorwayList;
    }

    #region Validation

#if UNITY_EDITOR

    // Validate SO fields
    private void OnValidate()
    {
      // Set unique GUID if empty or the prefab changes
      if (Id.Empty() || previousPrefab != prefab)
      {
        Id = GUID.Generate();
        previousPrefab = prefab;
        EditorUtility.SetDirty(this);
      }

      HelperUtilities.ValidateCheckEnumerableValues(this, nameof(doorwayList), doorwayList);

      // Check spawn positions populated
      HelperUtilities.ValidateCheckEnumerableValues(this, nameof(SpawnGridCells), SpawnGridCells);
    }

#endif

    #endregion Validation

    public void OnBeforeSerialize() => 
      m_id = Id.ToString();

    public void OnAfterDeserialize() => 
      Id = new GUID(m_id);
  }
}