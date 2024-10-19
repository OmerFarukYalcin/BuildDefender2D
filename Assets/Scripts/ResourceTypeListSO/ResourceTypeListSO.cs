using System.Collections.Generic;
using UnityEngine;

namespace BuilderDefender
{
    // This ScriptableObject holds a list of different resource types.
    // It allows you to define and manage a collection of resource types in the Unity Inspector.
    [CreateAssetMenu(menuName = "ScriptableObjects/ResourceTypeList")] // This attribute allows you to create this ScriptableObject in the Unity editor.
    public class ResourceTypeListSO : ScriptableObject
    {
        // A list of resource types (e.g., wood, stone, etc.) represented by ResourceTypeSO.
        public List<ResourceTypeSO> list;
    }
}
