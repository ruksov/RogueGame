using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "DungeonLevel_", menuName = "Scriptable Objects/Dungeon/Level")]
public class DungeonLevelSO : ScriptableObject
{
    [FormerlySerializedAs("levelName")] public string Name;
    [FormerlySerializedAs("roomTemplateList")] public List<RoomTemplateSO> RoomTemplates;
    [FormerlySerializedAs("roomNodeGraphList")] public List<RoomNodeGraphSO> RoomNodeGraphs;

#if UNITY_EDITOR

    // Validate scriptable object details enetered
    private void OnValidate()
    {
        if(HelperUtilities.ValidateCheckEmptyString(this, nameof(Name), Name) ||
           HelperUtilities.ValidateCheckEnumerableValues(this, nameof(RoomTemplates), RoomTemplates) ||
           HelperUtilities.ValidateCheckEnumerableValues(this, nameof(RoomNodeGraphs), RoomNodeGraphs))
            return;

        // Loop through all node graphs
        foreach (RoomNodeGraphSO graph in RoomNodeGraphs)
        {
            // Loop through all nodes in node graph
            foreach (RoomNodeSO roomNode in graph.Nodes)
            {
                if(roomNode.Type.isCorridor &&
                   RoomTemplates.Any(roomTemplate => roomTemplate.Type.isCorridorNS || roomTemplate.Type.isCorridorEW))
                    continue;
                
                if(RoomTemplates.Any(roomTemplate => roomTemplate.Type == roomNode.Type))
                    continue;
                
                Debug.LogWarning($"There is no type {roomNode.Type} in templates for graph {graph}");
                return;
            }
        }
    }
    
#endif // UNITY_EDITOR
}