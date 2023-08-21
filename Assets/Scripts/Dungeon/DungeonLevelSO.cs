using System.Collections.Generic;
using System.Linq;
using Rogue.NodeGraph;
using UnityEngine;

namespace Rogue.Dungeon
{
    [CreateAssetMenu(fileName = "DungeonLevel_", menuName = "Rogue/Level")]
    public class DungeonLevelSO : ScriptableObject
    {
        public string Name;
        public List<RoomTemplateSO> RoomTemplates;
        public List<RoomNodeGraph> RoomNodeGraphs;

#if UNITY_EDITOR

        // Validate scriptable object details enetered
        private void OnValidate()
        {
            if(HelperUtilities.ValidateCheckEmptyString(this, nameof(Name), Name) ||
               HelperUtilities.ValidateCheckEnumerableValues(this, nameof(RoomTemplates), RoomTemplates) ||
               HelperUtilities.ValidateCheckEnumerableValues(this, nameof(RoomNodeGraphs), RoomNodeGraphs))
                return;

            // Loop through all node graphs
            foreach (RoomNodeGraph graph in RoomNodeGraphs)
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
    }
}