using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace Crogen.BishojyoGraph.Editor
{
    public class BishojyoGraph : GraphView
    {
        public readonly Vector2 defaultNodeSize = new Vector2(150, 200);

        private NodeSearchWIndow _searchWindow;
        
        public BishojyoGraph()
        {
            styleSheets.Add((Resources.Load<StyleSheet>("BishojyoGraph")));
            SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);
            
            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());
    
            var grid = new GridBackground();
            Insert(0, grid);
            grid.StretchToParentSize();
            AddElement(GenerateEntryPointNode());
            AddSearchWindow();
        }

        private void AddSearchWindow()
        {
            _searchWindow = ScriptableObject.CreateInstance<NodeSearchWIndow>();
            nodeCreationRequest = context =>
                SearchWindow.Open(new SearchWindowContext(context.screenMousePosition), _searchWindow);
        }

        public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
        {
            var compatiblePorts = new List<Port>();
    
            ports.ForEach(port =>
            {
                if (startPort != port && startPort.node != port.node)
                {
                    compatiblePorts.Add(port);
                }
            });
            return compatiblePorts;
        }
    
        private Port GeneratePort(BishojyoNode node, Direction portDirection, Port.Capacity capacity = Port.Capacity.Single)
        {
            return node.InstantiatePort(Orientation.Horizontal, portDirection, capacity, typeof(float)); //Arbitrary type
        }
        
        private BishojyoNode GenerateEntryPointNode()
        {
            var node = new BishojyoNode
            {
                title = "Start", 
                GUID = Guid.NewGuid().ToString(),
                Slide = new Slide
                {
                    text = "",
                    currentCharacter = "",
                    characterState = CharacterState.Normal,
                    slideEvent = null
                },
                EntryPoint = true
            };
            var generatePort = GeneratePort(node, Direction.Output);
            generatePort.portName = "Next";
            node.outputContainer.Add(generatePort);

            node.capabilities &= ~Capabilities.Movable;
            node.capabilities &= ~Capabilities.Deletable;
            
            node.RefreshExpandedState();
            node.RefreshPorts();
            
            node.SetPosition(new Rect(100, 200, 100, 150));
            return node;
        }
    
        public void AddChoicePort(BishojyoNode bishojyoNode, string overriddenPortName = "")
        {
            var generatePort = GeneratePort(bishojyoNode, Direction.Output);

            var oldLabel = generatePort.contentContainer.Q<Label>("type");
            generatePort.contentContainer.Remove(oldLabel);
            
            int outputPortCount = bishojyoNode.outputContainer.Query("connector").ToList().Count;
            generatePort.portName = $"Choice {outputPortCount}";

            var choicePortName = string.IsNullOrEmpty(overriddenPortName)
                ? $"Choice{outputPortCount + 1}"
                : overriddenPortName;

            var textField = new TextField
            {
                name = string.Empty,
                value = choicePortName
            };
            textField.RegisterValueChangedCallback(evt => generatePort.portName = evt.newValue);
            generatePort.contentContainer.Add(new Label("  "));
            generatePort.contentContainer.Add(textField);
            var deleteButton = new Button(() => RemovePort(bishojyoNode, generatePort))
            {
                text = "X"
            };
            generatePort.contentContainer.Add(deleteButton);
                    
            generatePort.portName = choicePortName;
            bishojyoNode.outputContainer.Add(generatePort);
            bishojyoNode.RefreshPorts();
            bishojyoNode.RefreshExpandedState();
        }

        private void RemovePort(BishojyoNode bishojyoNode, Port generatedPort)
        {
            var targetEdge = edges.ToList().Where(x =>
                x.output.portName == generatedPort.portName && x.output.node == generatedPort.node);

            if (targetEdge.Any())
            {
                Debug.Log(targetEdge.Count());
                var edge = targetEdge.First();
                edge.input.Disconnect(edge);
                RemoveElement(targetEdge.First());
            }
            
            bishojyoNode.outputContainer.Remove(generatedPort);
            bishojyoNode.RefreshPorts();
            bishojyoNode.RefreshExpandedState();
        }

        public void CreateNode(string nodeName, Slide slide, Vector2 position)
        {
            AddElement(CreateBishojyoNode(nodeName, slide, position));
        }
        
        public BishojyoNode CreateBishojyoNode(string nodeName, Slide slide, Vector2 position)
        {
            var bishojyoNode = new BishojyoNode
            {
                title = nodeName,
                GUID = Guid.NewGuid().ToString(),
                Slide = slide,
                EntryPoint = false
            };
            var inputPort = GeneratePort(bishojyoNode, Direction.Input, Port.Capacity.Multi);
            inputPort.portName = "Input";
            bishojyoNode.inputContainer.Add(inputPort);
            
            bishojyoNode.styleSheets.Add(Resources.Load<StyleSheet>("Node"));
            
            var button = new Button(() => { AddChoicePort(bishojyoNode);});
            button.text = "New Choice";
            bishojyoNode.titleContainer.Add(button);

            var textField = new TextField(string.Empty);
            textField.RegisterValueChangedCallback(evt =>
            {
                bishojyoNode.Slide.text = evt.newValue;
                bishojyoNode.title = evt.newValue;
            });
            textField.SetValueWithoutNotify(bishojyoNode.Slide.text);
            bishojyoNode.mainContainer.Add(textField);
            
            bishojyoNode.RefreshExpandedState();
            bishojyoNode.RefreshPorts();
            bishojyoNode.SetPosition(new Rect(position, defaultNodeSize));
    
            return bishojyoNode;
        }
    }
}
