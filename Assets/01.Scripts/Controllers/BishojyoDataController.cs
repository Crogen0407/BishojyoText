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
    
    public int currentStoryIndex;
    
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
    }

    public NodeLinkData[] LoadChoice()
    {
        List<NodeLinkData> nodeLinkDatas = new List<NodeLinkData>();

        int index = 0;
        string nodeName = currentBishojyoContainer.NodeLinks[currentStoryIndex].PortName;
        while (nodeName == currentBishojyoContainer.NodeLinks[currentStoryIndex + index].PortName)
        {
            nodeLinkDatas.Add(currentBishojyoContainer.NodeLinks[currentStoryIndex + index]);
            index+=2;
        }

        return nodeLinkDatas.ToArray();
    }
    
    public void LoadSlide()
    {
        Slide slide = currentBishojyoContainer.BishojyoNodeDatas[currentStoryIndex].Slide;
        _textController.UpdateChatWindow(slide.currentCharacter, slide.text, LoadChoice());
    }
    
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && _textController.textMakeComplete == true)
        {
            currentStoryIndex++;
            LoadSlide();
        }
        else if(Input.GetKeyDown(KeyCode.Space) && _textController.textMakeComplete == false)
        {
            _textController.ChatSkip();
        }
    }
}