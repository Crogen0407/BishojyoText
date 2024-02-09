using System;
using System.Collections;
using System.Linq;
using Crogen.BishojyoGraph.RunTime;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.Rendering;
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

        private VisualElement.Hierarchy _hierarchy;
        private void OnEnable()
        {
            ConstructGraphView();
            GenerateToolbar();
            GenerateMiniMap();
            GenerateBlackBoard();
        }

        private void GenerateBlackBoard()
        {
            var blackboard = new Blackboard(_graphview);
            blackboard.Add(new BlackboardSection
            {
                title = "Exposed Properties"
            });
            blackboard.addItemRequested = blackboard1 =>
            {
                _graphview.AddPropertyToBlackBoard(new ExposedProperty());
            };
            blackboard.editTextRequested = (blackboard1, element, newValue) =>
            {
                var oldPropertyName = ((BlackboardField)element).text;
                if (_graphview.ExposedProperties.Any(x => x.PropertyName == newValue))
                {
                    EditorUtility.DisplayDialog("Error", "This property name already exists, please chose another one!",
                        "OK");
                    return;
                }

                var propertyIndex = _graphview.ExposedProperties.FindIndex(x => x.PropertyName == oldPropertyName);
                _graphview.ExposedProperties[propertyIndex].PropertyName = newValue;
                ((BlackboardField)element).text = newValue;
            };
            blackboard.SetPosition(new Rect(10, 30, 200, 300));
            _graphview.Add(blackboard);
            _graphview.Blackboard = blackboard;
        }

        private void GenerateMiniMap()
        {
            var miniMap = new MiniMap{anchored = true};
            //This will give 10px offset from left side
            var cords = _graphview.contentViewContainer.WorldToLocal(new Vector2(this.maxSize.x - 10, 30));
            miniMap.SetPosition(new Rect(cords.x, cords.y, 200, 140));
            _graphview.Add(miniMap);
        }

        private void OnDisable()
        {
            rootVisualElement.Remove(_graphview);
        }
    
        private void ConstructGraphView()
        {
            _graphview = new BishojyoGraph(this)
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
                _graphview.CreateNode("Bishojyo Node", new Slide
                {
                    text = string.Empty,
                    characterState = CharacterState.Normal,
                    currentCharacter = ""
                }, Vector2.zero);
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

