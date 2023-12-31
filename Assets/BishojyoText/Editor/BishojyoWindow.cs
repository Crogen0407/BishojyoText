using System;
using System.Collections;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Crogen.BishojyoGraph.Editor
{
    public class BishojyoWindow : EditorWindow
    {
        private BishojyoGraph _graphview;
        private string _fileName = "New Narrative";
        
        [MenuItem("Crogen/BishojyoGraph")]
        public static void OpenBishojyoWindow()
        {
            var window = GetWindow<BishojyoWindow>();
            window.titleContent = new GUIContent("BishojyoGraph");
        }
    
        private void OnEnable()
        {
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
    
        private void GenerateToolbar()
        {
            var toolbar = new Toolbar();
    
            var fileNameTextField = new TextField("File Name");
            fileNameTextField.SetValueWithoutNotify(_fileName);
            fileNameTextField.MarkDirtyRepaint();
            fileNameTextField.RegisterValueChangedCallback(evt =>
            {
                _fileName = evt.newValue;
            });
            toolbar.Add(fileNameTextField);
            
            toolbar.Add(new Button(()=>RequestDataOperation(true))
            {
                text = "SaveData"
            });
            
            toolbar.Add(new Button(()=>RequestDataOperation(false))
            {
                text = "LoadData"
            });
            
            var nodeCreateButton = new Button(() =>
            {
                _graphview.CreateNode("Bishojyo Node", Vector2.zero);
            });
            nodeCreateButton.text = "Create Node";
            toolbar.Add(nodeCreateButton);
            
            rootVisualElement.Add(toolbar);
        }


        private void RequestDataOperation(bool save)
        {
            if (string.IsNullOrEmpty(_fileName))
            {
                EditorUtility.DisplayDialog("Invalid file name!", "Please enter a valid file name.", "OK");
                return;
            }

            var saveUtility = GraphSaveUtility.GetInstance(_graphview);
            if (save)
            {
                saveUtility.SaveGraph(_fileName);
            }
            else
            {
                saveUtility.LoadGraph(_fileName);
            }
        }
    }
}

