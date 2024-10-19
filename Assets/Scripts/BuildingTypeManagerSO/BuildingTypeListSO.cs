using System.Collections.Generic;
using UnityEngine;

namespace BuilderDefender
{
    // This ScriptableObject holds a list of BuildingTypeSO objects.
    // It is used to store and manage multiple building types in the game.
    [CreateAssetMenu(menuName = "ScriptableObjects/BuildingTypeList")]
    public class BuildingTypeListSO : ScriptableObject
    {
        // List that holds all available building types in the game
        public List<BuildingTypeSO> list;
    }
}
