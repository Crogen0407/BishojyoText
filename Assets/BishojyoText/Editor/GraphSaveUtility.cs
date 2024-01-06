using System.Collections.Generic;
using System.Linq;
using Crogen.BishojyoGraph.RunTime;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace Crogen.BishojyoGraph.Editor
{
    public class GraphSaveUtility
    {
        private BishojyoGraph _targetGraph;
        private BishojyoContainer _containerCache;
        private List<Edge> Edges => _targetGraph.edges.ToList();
        private List<BishojyoNode> Nodes => _targetGraph.nodes.ToList().Cast<BishojyoNode>().ToList();
        
        public static GraphSaveUtility GetInstance(BishojyoGraph targetGraph)
        {
            return new GraphSaveUtility
            {
                _targetGraph = targetGraph
            };
        }

        public void SaveGraph(string fileName)
        {
            if(!Edges.Any()) return; //if there are no edges(no connections) then return

            var bishojyoContainer = ScriptableObject.CreateInstance<BishojyoContainer>();

            var connectedPorts = Edges.Where(x => x.input.node != null).ToArray();
            for (int i = 0; i < connectedPorts.Length; i++)
            {
                var outputNode = connectedPorts[i].output.node as BishojyoNode;
                var inputNode = connectedPorts[i].input.node as BishojyoNode;
                
                bishojyoContainer.NodeLinks.Add(new NodeLinkData
                {
                    BaseNodeGUID = outputNode.GUID,
                    PortName = connectedPorts[i].output.portName,
                    TargetNodeGUID = inputNode.GUID
                });
            }

            foreach (var bishojyoNode in Nodes.Where(node=>!node.EntryPoint))
            {
                bishojyoContainer.BishojyoNodeDatas.Add(new BishojyoNodeData
                {
                    GUID = bishojyoNode.GUID,
                    Slide = bishojyoNode.Slide,
                    Position = bishojyoNode.GetPosition().position
                });                
            }

            //Auto creates resources folder if it doesn't exist
            if (!AssetDatabase.IsValidFolder("Assets/Resources"))
                AssetDatabase.CreateFolder("Assets", "Resources");
            AssetDatabase.CreateAsset(bishojyoContainer, $"Assets/Resources/{fileName}.asset");
            AssetDatabase.SaveAssets();
        }

        public void LoadGraph(string fileName)
        {
            _containerCache = Resources.Load<BishojyoContainer>(fileName);
            if (_containerCache == null)
            {
                EditorUtility.DisplayDialog("File Not Found", "Target dialogue graph file does not exists!", "OK");
                return;
            }

            ClearGraph();
            CreateNodes();
            ConnectNodes();
        }

        private void ConnectNodes()
        {
            for (int i = 0; i < Nodes.Count; i++)
            {
                var connections = _containerCache.NodeLinks.Where(x => x.BaseNodeGUID == Nodes[i].GUID).ToList();

                for (int j = 0; j < connections.Count; j++)
                {
                    var targetNodeGuid = connections[j].TargetNodeGUID;
                    var targetNode = Nodes.First(x => x.GUID == targetNodeGuid);
                    LinkNodes(Nodes[i].outputContainer[j].Q<Port>(), (Port)targetNode.inputContainer[0]);
                    
                    targetNode.SetPosition(new Rect(_containerCache.BishojyoNodeDatas.First(x => x.GUID == targetNodeGuid).Position,
                        _targetGraph.defaultNodeSize));
                }
            }
        }

        private void LinkNodes(Port output, Port input)
        {
            var tempEdge = new Edge
            {
                output = output,
                input = input
            };
            tempEdge?.input.Connect(tempEdge);
            tempEdge?.output.Connect(tempEdge);
            
            _targetGraph.Add(tempEdge);
        }

        private void CreateNodes()
        {
            foreach (var nodeData in _containerCache.BishojyoNodeDatas)
            {
                var tempNode = _targetGraph.CreateBishojyoNode("Bishojyo Node", Vector2.zero);
                tempNode.GUID = nodeData.GUID;
                _targetGraph.AddElement(tempNode);

                var nodePorts = _containerCache.NodeLinks.Where(x => x.BaseNodeGUID == nodeData.GUID).ToList();
                nodePorts.ForEach(x => _targetGraph.AddChoicePort(tempNode, x.PortName));
            }
        }

        private void ClearGraph()
        {
            //Set entry points GUID back from the save. Discard existing GUID
            Nodes.Find(x => x.EntryPoint).GUID = _containerCache.NodeLinks[0].BaseNodeGUID;

            foreach (var node in Nodes)
            {
                if (node.EntryPoint) continue;
                
                //Remove edges that connected this node
                Edges.Where(x => x.input.node == node).ToList().ForEach(edge=>_targetGraph.RemoveElement(edge));
                
                //Then remove the node
                _targetGraph.RemoveElement(node);
            }
        }
    }
}