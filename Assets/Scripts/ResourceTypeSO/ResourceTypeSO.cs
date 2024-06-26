using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BuilderDefender
{
    [CreateAssetMenu(menuName = "ScriptableObjects/ResourceType")]
    public class ResourceTypeSO : ScriptableObject
    {
        public string nameString;
        public string nameShort;
        public Sprite sprite;
        public string colorHex;
    }
}
