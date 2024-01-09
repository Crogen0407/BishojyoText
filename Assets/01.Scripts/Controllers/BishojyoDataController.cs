using System;
using System.Collections;
using System.Collections.Generic;
using Crogen.BishojyoGraph;
using Crogen.BishojyoGraph.RunTime;
using UnityEngine;

public class BishojyoDataController : MonoBehaviour
{
    //Data
    [SerializeField] private List<BishojyoContainer> bishojyoContainers;
    public BishojyoContainer currentBishojyoContainer;
    
    [SerializeField] private List<NodeLinkData> _nodeLinkDatas;
    [SerializeField] private List<BishojyoNodeData> _bishojyoNodeDatas;
    
    [SerializeField] private List<NodeLinkData> _currentNodeLinkDatas;
    [SerializeField] private List<BishojyoNodeData> _currentBishojyoNodeDatas;
    
    public int currentStoryIndex;
    public int currentSlideIndex;
    public bool isChoiceMode = false;
    public int maxStoryCount;
    
    //Managers
    private StoryManager _storyManager;
    
    //Controllers
    private DataController _dataController;
    private TextController _textController;
    
    public void Awake()
    {
        //Managers
        _storyManager = StoryManager.Instance;
        
        //Controllers
        _dataController = _storyManager.DataController;
        _textController = _storyManager.TextController;
        
        _dataController.Save(new SaveData()
        {
            userName = "Crogen",
            currentStoryIndex = 0
        });
        currentBishojyoContainer = bishojyoContainers[_dataController.Load().currentStoryIndex];
        _nodeLinkDatas = currentBishojyoContainer.NodeLinks;
        _bishojyoNodeDatas = currentBishojyoContainer.BishojyoNodeDatas;
        CallbackSlide(); 
        LoadSlide();
    }

    public NodeLinkData[] LoadChoice()
    {
        List<NodeLinkData> nodeLinkDatas = new List<NodeLinkData>();
        
        int index = 0;
        string nodeName = _currentNodeLinkDatas[currentSlideIndex].PortName;
        string nodeGUID = _currentNodeLinkDatas[currentSlideIndex].BaseNodeGUID;
        while (nodeGUID == _currentNodeLinkDatas[currentSlideIndex + index].BaseNodeGUID && !nodeName.Equals("<next>") && !nodeName.Equals("Next"))
        {
            nodeLinkDatas.Add(_currentNodeLinkDatas[currentSlideIndex + index]);
            index++;
        }

        if (nodeLinkDatas.Count <= 0)
        {
            isChoiceMode = false;
        }
        else
        {
            isChoiceMode = true;
        }
        return nodeLinkDatas.ToArray();
    }

    private void CallbackSlide()
    {
        _currentNodeLinkDatas.Add(_nodeLinkDatas[0]);
        _nodeLinkDatas.Remove(_nodeLinkDatas[0]);
        int index = 0;
        for (int i = 0; i < _nodeLinkDatas.Count;)
        {
            if (_currentNodeLinkDatas[index].TargetNodeGUID == _nodeLinkDatas[i].BaseNodeGUID)
            {
                _currentNodeLinkDatas.Add(_nodeLinkDatas[i]);
                _nodeLinkDatas.RemoveAt(i);
                break;
            }
            else
            {
                i++;
            }
        }
        
        // while (_nodeLinkDatas[index].BaseNodeGUID != _nodeLinkDatas[index + 1].BaseNodeGUID)
        // {
        //     _currentNodeLinkDatas.Add(_nodeLinkDatas[index]);
        //     index++;
        // }
    }
    
    public void LoadSlide()
    {
        Slide slide = _bishojyoNodeDatas[currentSlideIndex].Slide;
        NodeLinkData[] choiceLinkDatas = LoadChoice();
        _textController.UpdateChatWindow(slide.currentCharacter, slide.text, choiceLinkDatas);
    }
    
    
    private void Update()
    {
        if (isChoiceMode == false)
        {
            if (Input.GetKeyDown(KeyCode.Space) && _textController.textMakeComplete == true)
            {
                currentSlideIndex++;
                LoadSlide();
            }
            else if(Input.GetKeyDown(KeyCode.Space) && _textController.textMakeComplete == false)
            {
                _textController.ChatSkip();
            }
        }
    }
}