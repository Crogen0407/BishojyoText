using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Crogen.BishojyoGraph;
using Crogen.BishojyoGraph.RunTime;
using UnityEngine;
using UnityEngine.Serialization;

public class BishojyoDataController : MonoBehaviour
{
    //Data
    [SerializeField] private List<BishojyoContainer> bishojyoContainers;
    public BishojyoContainer currentBishojyoContainer;
    
    public List<NodeLinkData> nodeLinkDatas;
    [SerializeField] private List<BishojyoNodeData> _bishojyoNodeDatas;
    
    //[SerializeField] private List<NodeLinkData> _currentNodeLinkDatas;
    [SerializeField] private List<BishojyoNodeData> _currentBishojyoNodeDatas;
    public NodeLinkData[] _currentChoices;
    
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

        
        //List Setting
        nodeLinkDatas = new List<NodeLinkData>();
        _bishojyoNodeDatas = new List<BishojyoNodeData>();

        foreach (var item in  currentBishojyoContainer.NodeLinks)
        {
            nodeLinkDatas.Add(item);
        }
        foreach (var item in   currentBishojyoContainer.BishojyoNodeDatas)
        {
            _bishojyoNodeDatas.Add(item);
        }
        
        LoadSlide();
    }


    private string[] GetTargetGUIDArray(string baseGUID)
    {
        List<string> outputGUID = new List<string>();

        foreach (var nodeLinkData in nodeLinkDatas)
        {
            if (nodeLinkData.BaseNodeGUID == baseGUID)
            {
                outputGUID.Add(nodeLinkData.TargetNodeGUID);
            }
        }

        return outputGUID.ToArray();
    }
    
    public void LoadSlide()
    {
        isChoiceMode = false;
        string[] choiceGUID = GetTargetGUIDArray(nodeLinkDatas[currentSlideIndex].BaseNodeGUID);
        
        for (int i = 0; i < choiceGUID.Length; i++)
        {
            Debug.Log(choiceGUID[i]);
        }
        
        if (choiceGUID.Length == 1)
        {
            foreach (var bishojyoNodeData in _bishojyoNodeDatas)
            {
                if (bishojyoNodeData.GUID == choiceGUID[0])
                {
                    _currentBishojyoNodeDatas.Add(bishojyoNodeData);
                    _bishojyoNodeDatas.Remove(bishojyoNodeData);
                    _textController.UpdateChatWindow(bishojyoNodeData.Slide.currentCharacter, bishojyoNodeData.Slide.text);
                    currentChoiceGUID = string.Empty;
                    break;
                }
            }
        }
        else
        {
            _textController.UpdateChoicePanel(LoadChoice(choiceGUID));
        }
    }

    public string currentChoiceGUID;
    private NodeLinkData[] LoadChoice(string[] choiceGUID)
    {
        List<NodeLinkData> nodeLinkDatas = new List<NodeLinkData>();   
        
        isChoiceMode = true;
        for (int i = 0; i < choiceGUID.Length; i++)
        {
            foreach (var nodeLinkData in this.nodeLinkDatas)
            {
                if (nodeLinkData.TargetNodeGUID == choiceGUID[i])
                {
                    nodeLinkDatas.Add(nodeLinkData);
                    Debug.Log(nodeLinkData.PortName);
                    break;
                }
            }
        }

        return nodeLinkDatas.ToArray();
    }

    public void DeleteChoice(string exceptTargetGUID)
    {
        string baseGUID = string.Empty;
        foreach (var nodeLinkData in nodeLinkDatas)
        {
            if (nodeLinkData.TargetNodeGUID == exceptTargetGUID)
            {
                baseGUID = nodeLinkData.BaseNodeGUID;
                break;
            }
        }

        nodeLinkDatas.Remove(nodeLinkDatas.Single(s=> s.BaseNodeGUID == baseGUID && s.TargetNodeGUID != exceptTargetGUID));
    }
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isChoiceMode == false)
        {
            if (_textController.textMakeComplete == true)
            {
                currentSlideIndex++;
                LoadSlide();
            }
            else if(_textController.textMakeComplete == false)
            {
                _textController.ChatSkip();
            }
        }
    }
}