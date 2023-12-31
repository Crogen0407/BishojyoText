using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace Crogen.BishojyoGraph.Editor
{
    public class BishojyoNode : Node
    {
        public string GUID;
        public Slide Slide;
        public bool EntryPoint = false;
    }
}