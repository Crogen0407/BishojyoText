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
    public int currentSlideIndex;
    public bool isChoiceMode = false;

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
        LoadSlide();
    }

    public NodeLinkData[] LoadChoice()
    {
        List<NodeLinkData> nodeLinkDatas = new List<NodeLinkData>();

        int index = 0;
        string nodeName = currentBishojyoContainer.NodeLinks[currentSlideIndex].PortName;
        string nodeGUID = currentBishojyoContainer.NodeLinks[currentSlideIndex].BaseNodeGUID;
        while (nodeGUID == currentBishojyoContainer.NodeLinks[currentSlideIndex + index].BaseNodeGUID && !nodeName.Equals("<next>") && !nodeName.Equals("Next"))
        {
            nodeLinkDatas.Add(currentBishojyoContainer.NodeLinks[currentSlideIndex + index]);
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
    
    public void LoadSlide()
    {
        Slide slide = currentBishojyoContainer.BishojyoNodeDatas[currentSlideIndex].Slide;
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