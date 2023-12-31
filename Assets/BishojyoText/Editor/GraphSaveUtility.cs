using System.Collections.Generic;
using System.Linq;
using Crogen.BishojyoGraph.RunTime;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace Crogen.BishojyoGraph.Editor
{
    public class GraphSaveUtility : MonoBehaviour
    {
        private BishojyoGraph _targetGraphView;
        private List<Edge> Edges => _targetGraphView.edges.ToList();
        private List<BishojyoNode> Nodes => _targetGraphView.edges.Cast<BishojyoNode>().ToList();
        
        public static GraphSaveUtility GetInstance(BishojyoGraph targetGraph)
        {
            return new GraphSaveUtility() { _targetGraphView = targetGraph };
        }

        public void SaveGraph(string fileName)
        {
            if(!Edges.Any()) return; //if there are no edges(no connections) then return

            var bishojyoContainer = ScriptableObject.CreateInstance<BishojyoContainer>();

            var connectedPorts = Edges.Where(x => x.input.node != null).ToArray();
            for (int i = 0; i < connectedPorts.Length; i++)
            {
                BishojyoNode outputNode = connectedPorts[i].output.node as BishojyoNode;
                BishojyoNode inputNode = connectedPorts[i].input.node as BishojyoNode;
                
                bishojyoContainer.NodeLinks.Add(new NodeLinkData
                {
                    BaseNodeGUID = outputNode.GUID
                    
                });
            }
        }

        public void LoadGraph(string fileName)
        {
            
        }
    }
}