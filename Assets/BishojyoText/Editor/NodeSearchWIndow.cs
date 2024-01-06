using System.Collections.Generic;
using Crogen.BishojyoGraph;
using Crogen.BishojyoGraph.Editor;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class NodeSearchWIndow : ScriptableObject, ISearchWindowProvider
{
    private BishojyoGraph _graphView;
    private EditorWindow _window;
    private Texture2D _indentaionIcon;
    
    public void Init(EditorWindow window, BishojyoGraph graphView)
    {
        _graphView = graphView;
        _window = window;
        
        //Indentation hack for search window as a transparent icon
        _indentaionIcon = new Texture2D(1, 1);
        _indentaionIcon.SetPixel(0,0,new Color(0,0,0,0)); //Dont forget to set the alpha to 0 as well
        _indentaionIcon.Apply();
    }
    
    public List<SearchTreeEntry> CreateSearchTree(SearchWindowContext context)
    {
        var tree = new List<SearchTreeEntry>
        {
            new SearchTreeGroupEntry(new GUIContent("Create Elements"), 0),
            new SearchTreeGroupEntry(new GUIContent("Bishojyo"), 1),
            new SearchTreeEntry(new GUIContent("Bishojyo Node", _indentaionIcon))
            {
                userData = new BishojyoNode(), level = 2
            },
            new SearchTreeEntry(new GUIContent("Hello World"))
        };
        return tree;
    }

    public bool OnSelectEntry(SearchTreeEntry SearchTreeEntry, SearchWindowContext context)
    {
        var worldMousePosition = _window.rootVisualElement.ChangeCoordinatesTo(_window.rootVisualElement.parent,
            context.screenMousePosition - _window.position.position);
        var localMousePosition = _graphView.contentContainer.WorldToLocal(worldMousePosition);
        
        switch (SearchTreeEntry.userData)
        {
            case BishojyoNode bishojyoNode :
                _graphView.CreateNode("Bishojyo Node", new Slide
                {
                    text = string.Empty,
                    characterState = CharacterState.Normal,
                    currentCharacter = ""
                }, localMousePosition);
                return true;
            default:
                break;
        }
        return true;
    }
}
