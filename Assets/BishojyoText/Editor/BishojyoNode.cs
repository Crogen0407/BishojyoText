using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace Crogen.BishojyoGraph
{
    public class BishojyoNode : Node
    {
        public string GUID;
        public Slide slide;
        public bool enterPoint = false;
    }
}