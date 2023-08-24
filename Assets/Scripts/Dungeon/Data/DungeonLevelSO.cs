using System.Collections.Generic;
using System.Linq;
using Rogue.Enums;
using Rogue.NodeGraph;
using Rogue.Utilities;
using UnityEditor;
using UnityEngine;

namespace Rogue.Dungeon.Data
{
    [CreateAssetMenu(fileName = "DungeonLevel_", menuName = "Rogue/Level")]
    public class DungeonLevelSO : ScriptableObject
    {
        public string Name;
        public List<RoomTemplateSO> RoomTemplates;
        public List<RoomNodeGraphSO> RoomNodeGraphs;

        public Dictionary<GUID, RoomTemplateSO> IdToRoomTemplate;

        public RoomNodeGraphSO RandomGraph() => 
            RoomNodeGraphs[Random.Range(0, RoomNodeGraphs.Count)];

#if UNITY_EDITOR

        // Validate scriptable object details entered
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
                foreach (Node roomNode in graph.Nodes)
                {
                    if(RoomTemplates.Any(template => template.Type == roomNode.Type))
                        continue;
                
                    Debug.LogWarning($"There is no type {roomNode.Type} in templates for graph {graph}");
                    return;
                }
            }
        }
    
#endif // UNITY_EDITOR

        public RoomTemplateSO RandomRoomTemplateOf(ENodeType type) => 
            RoomTemplates.Where(template => template.Type == type).ToList().RandomItem();

        public RoomTemplateSO RandomRoomTemplateFor(Node roomNode, Doorway parentDoorway)
        {
            if (roomNode.Type != ENodeType.Corridor)
                return RandomRoomTemplateOf(roomNode.Type);
      
            switch (parentDoorway.orientation)
            {
                case EOrientation.North:
                case EOrientation.South:
                    return RoomTemplates
                        .Where(template => template is CorridorTemplateSO {Direction: ECorridorDir.NorthSouth})
                        .ToList()
                        .RandomItem();
          
                case EOrientation.East:
                case EOrientation.West:
                    return RoomTemplates
                        .Where(template => template is CorridorTemplateSO {Direction: ECorridorDir.EastWest})
                        .ToList()
                        .RandomItem();
            }
            
            return null;
        }
    }
}