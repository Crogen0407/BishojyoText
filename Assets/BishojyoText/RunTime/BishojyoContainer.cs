using System;
using System.Collections.Generic;
using UnityEngine;

namespace Crogen.BishojyoGraph.RunTime
{
    [Serializable]
    public class BishojyoContainer : ScriptableObject
    {
        public List<NodeLinkData> NodeLinks = new List<NodeLinkData>();
        public List<BishojyoNodeData> BishojyoNodeDatas = new List<BishojyoNodeData>();
        public List<ExposedProperty> ExposedProperties = new List<ExposedProperty>();
    }
}