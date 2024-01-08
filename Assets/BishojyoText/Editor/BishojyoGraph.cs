using System;
using System.Collections.Generic;
using System.Linq;
using Crogen.BishojyoGraph.RunTime;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace Crogen.BishojyoGraph.Editor
{
    public class BishojyoGraph : GraphView
    {
        public readonly Vector2 defaultNodeSize = new Vector2(150, 200);
        public Blackboard Blackboard;
        public List<ExposedProperty> ExposedProperties = new List<ExposedProperty>();
        private NodeSearchWIndow _searchWindow;
        
        public BishojyoGraph(EditorWindow window)
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
            AddSearchWindow(window);
        }

        private void AddSearchWindow(EditorWindow window)
        {
            _searchWindow = ScriptableObject.CreateInstance<NodeSearchWIndow>();
            _searchWindow.Init(window, this);
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
            
            if (outputPortCount == 0)
            {
                choicePortName = string.IsNullOrEmpty(overriddenPortName)
                    ? "<next>"
                    : overriddenPortName;
            }
            

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
        
        public BishojyoNode  CreateBishojyoNode(string nodeName, Slide slide, Vector2 position)
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

            var nameLabel = new Label("Name");
            bishojyoNode.mainContainer.Add(nameLabel);

            var textFieldforName = new TextField(string.Empty);
            textFieldforName.RegisterValueChangedCallback(evt =>
            {
                bishojyoNode.Slide.currentCharacter = evt.newValue;
            });
            textFieldforName.SetValueWithoutNotify(bishojyoNode.Slide.currentCharacter);
            bishojyoNode.mainContainer.Add(textFieldforName);
            
            var textLabel = new Label("Text");
            bishojyoNode.mainContainer.Add(textLabel);
            
            var textFieldforStoryText = new TextField(string.Empty);
            textFieldforStoryText.RegisterValueChangedCallback(evt =>
            {
                bishojyoNode.Slide.text = evt.newValue;
                bishojyoNode.title = evt.newValue;
            });
            textFieldforStoryText.SetValueWithoutNotify(bishojyoNode.Slide.text);
            bishojyoNode.mainContainer.Add(textFieldforStoryText);
            
            
            
            bishojyoNode.RefreshExpandedState();
            bishojyoNode.RefreshPorts();
            bishojyoNode.SetPosition(new Rect(position, defaultNodeSize));
    
            return bishojyoNode;
        }

        public void ClearBlackBoardAndExposedProperties()
        {
            ExposedProperties.Clear();
            Blackboard.Clear(); //Yes, blackboard can clean itself!
        }
        
        public void AddPropertyToBlackBoard(ExposedProperty exposedProperty)
        {
            var localPropertyName = exposedProperty.PropertyName;
            var localPropertyValue = exposedProperty.PropertyValue;
            while (ExposedProperties.Any(x => x.PropertyName == localPropertyName))
                localPropertyName = $"{localPropertyName}(1)"; //USERNAME(1) || USERNAME(1)(1)(1) ETC...
            
            var property = new ExposedProperty();
            property.PropertyName =localPropertyName;
            property.PropertyValue = localPropertyValue;
            
            ExposedProperties.Add(property);

            var container = new VisualElement();
            var blackboardField = new BlackboardField { text = property.PropertyName, typeText = "string" };
            container.Add(blackboardField);

            var propertyValueTextField = new TextField("Value:")
            {
                value = localPropertyValue
            };
            propertyValueTextField.RegisterValueChangedCallback(evt =>
            {
                var changingPropertyIndex = ExposedProperties.FindIndex(x => x.PropertyName == property.PropertyName);
                ExposedProperties[changingPropertyIndex].PropertyValue = evt.newValue;
            });
            var blackBoardValueRow = new BlackboardRow(blackboardField, propertyValueTextField);
            container.Add(blackBoardValueRow);
                
            Blackboard.Add(container);
        }
    }
}
