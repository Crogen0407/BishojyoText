using System;
using System.Collections;
using Crogen.BishojyoGraph;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

public class BishojyoWindow : EditorWindow
{
    private BishojyoGraph _graphview;
    
    [MenuItem("Crogen/BishojyoGraph")]
    public static void OpenBishojyoWindow()
    {
        var window = GetWindow<BishojyoWindow>();
        window.titleContent = new GUIContent("BishojyoGraph");
    }

    private void OnEnable()
    {
        GenerateSettingWindow();
        ConstructGraphView();
        GenerateToolbar();
    }

    private void OnDisable()
    {
        rootVisualElement.Remove(_graphview);
    }

    private void ConstructGraphView()
    {
        _graphview = new BishojyoGraph
        {
            name = "BishojyoGraph"
        };
        
        _graphview.StretchToParentSize();
        rootVisualElement.Add(_graphview);
    }

    public Rect windowRect = new Rect(10, 30, 200, 200);

    private void OnGUI()
    {
    }

    private void GenerateSettingWindow()
    {

    }
    
    // The window function. This works just like ingame GUI.Window
    void DoWindow(int unusedWindowID)
    {
        GUILayout.Button("Hi");
        GUI.DragWindow();
    }
    
    private void GenerateToolbar()
    {
        var toolbar = new Toolbar();

        var nodeCreateButton = new Button(() =>
        {
            _graphview.CreateNode("Bishojyo Node", Vector2.zero);
        });
        nodeCreateButton.text = "Create Node";
        toolbar.Add(nodeCreateButton);
        
        rootVisualElement.Add(toolbar);
    }
}
