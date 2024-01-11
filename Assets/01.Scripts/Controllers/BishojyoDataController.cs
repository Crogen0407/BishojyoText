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
        _nodeLinkDatas = new List<NodeLinkData>();
        _bishojyoNodeDatas = new List<BishojyoNodeData>();

        foreach (var item in  currentBishojyoContainer.NodeLinks)
        {
            _nodeLinkDatas.Add(item);
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

        foreach (var nodeLinkData in _nodeLinkDatas)
        {
            if (nodeLinkData.BaseNodeGUID == baseGUID)
            {
                outputGUID.Add(nodeLinkData.TargetNodeGUID);
            }
        }

        return outputGUID.ToArray();
    }
    
    private void LoadSlide()
    {
        string[] choiceGUID = GetTargetGUIDArray(_nodeLinkDatas[currentSlideIndex].BaseNodeGUID);

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
                    break;
                }
            }
        }
        else
        {
            LoadChoice(choiceGUID);
        }
    }

    private NodeLinkData[] LoadChoice(string[] choiceGUID)
    {
        List<NodeLinkData> nodeLinkDatas = new List<NodeLinkData>();
        
        isChoiceMode = true;
        for (int i = 0; i < choiceGUID.Length; i++)
        {
            foreach (var nodeLinkData in _nodeLinkDatas)
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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isChoiceMode == false)
        {
            currentSlideIndex++;
            LoadSlide();
        }
    }
}