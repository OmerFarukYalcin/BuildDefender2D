using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BuilderDefender
{
    [CreateAssetMenu(menuName = "ScriptableObjects/ResourceTypeList")]
    public class ResourceTypeListSO : ScriptableObject
    {
        public List<ResourceTypeSO> list;
    }
}
