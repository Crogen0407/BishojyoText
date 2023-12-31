using System;
using System.Collections;
using System.Collections.Generic;
using Crogen.BishojyoGraph;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class BishojyoGraph : GraphView
{
    private readonly Vector2 defaultNodeSize = new Vector2(150, 200);
    public BishojyoGraph()
    {
        SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);
        
        this.AddManipulator(new ContentDragger());
        this.AddManipulator(new SelectionDragger());
        this.AddManipulator(new RectangleSelector());

        var grid = new GridBackground();
        Insert(0, grid);
        grid.StretchToParentSize();
        AddElement(GenerateEntryPointNode());
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
            slide = new Slide
            {
                text = "",
                currentCharacter = "",
                characterState = CharacterState.Normal,
                slideEvent = null
            },
            enterPoint = true
        };
        var generatePort = GeneratePort(node, Direction.Output);
        generatePort.portName = "Next";
        node.outputContainer.Add(generatePort);
        
        node.RefreshExpandedState();
        node.RefreshPorts();
        node.SetPosition(new Rect(100, 200, 100, 150));
        return node;
    }

    private void AddChoicePort(BishojyoNode node)
    {
        var generatePort = GeneratePort(node, Direction.Output);
        
        int outputPortCount = node.outputContainer.Query("connector").ToList().Count;
        generatePort.portName = $"Choice {outputPortCount}";
        
        node.outputContainer.Add(generatePort);
        node.RefreshPorts();
        node.RefreshExpandedState();
    }

    public void CreateNode(string nodeName, Vector2 position)
    {
        AddElement(CreateBishojyoNode(nodeName, position));
    }
    
    public BishojyoNode CreateBishojyoNode(string nodeName, Vector2 position)
    {
        var bishojyoNode = new BishojyoNode
        {
            title = nodeName,
            GUID = Guid.NewGuid().ToString(),
            slide = new Slide
            {
                text = nodeName,
                currentCharacter = "",
                characterState = CharacterState.Normal,
                slideEvent = null
            },
            enterPoint = true
        };
        var inputPort = GeneratePort(bishojyoNode, Direction.Input, Port.Capacity.Multi);
        inputPort.portName = "Input";
        bishojyoNode.inputContainer.Add(inputPort);
        
        var button = new Button(() => { AddChoicePort(bishojyoNode);});
        button.text = "New Choice";
        bishojyoNode.titleContainer.Add(button);

        bishojyoNode.RefreshExpandedState();
        bishojyoNode.RefreshPorts();
        bishojyoNode.SetPosition(new Rect(position, defaultNodeSize));

        return bishojyoNode;
    }
}
