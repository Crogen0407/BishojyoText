using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Crogen.BishojyoGraph;
using Crogen.BishojyoGraph.RunTime;
using Crogen.BishojyoGraph.SlideEffect;
using UnityEngine;
using UnityEngine.Serialization;
using CharacterController = Crogen.BishojyoGraph.CharacterController;

public class BishojyoDataController : MonoBehaviour
{
    [Header("Data")]
    //Data
    [SerializeField] private List<BishojyoContainer> bishojyoContainers;
    
    public List<NodeLinkData> nodeLinkDatas;
    [SerializeField] private List<BishojyoNodeData> _bishojyoNodeDatas;
    [SerializeField] private List<BishojyoNodeData> _currentBishojyoNodeDatas;
    private BishojyoContainer currentBishojyoContainer;
    
    [Space]
    [Header("Setting")]
    public int currentStoryIndex;
    public int currentSlideIndex;
    public bool isChoiceMode = false;
    [SerializeField] private bool _isEndStory;
    private bool _isCanControl;

    public bool IsEndStory
    {
        get => _isEndStory;
        set
        {
            _isEndStory = value;
            BishojyoSlideEffectController.Instance.Fade(!_isEndStory, 1, () =>
            {
                _isCanControl = !_isEndStory;
            });
        }
    }
    
    //Managers
    private StoryManager _storyManager;
    
    //Controllers
    private DataController _dataController;
    private TextController _textController;
    private CharacterController _characterController;
    
    public void Start()
    {
        //Managers
        _storyManager = StoryManager.Instance;
        
        //Controllers
        _dataController = _storyManager.DataController;
        _textController = _storyManager.TextController;
        _characterController = _storyManager.CharacterController;
        
        
        _dataController.Save(new SaveData()
        {
            userName = "Crogen",
            currentStoryIndex = 0
        });
        currentBishojyoContainer = bishojyoContainers[_dataController.Load().currentStoryIndex];

        _textController.chatWindow.gameObject.SetActive(false);
        IsEndStory = false;
        _isCanControl = false;
        
        //List Setting
        nodeLinkDatas = new List<NodeLinkData>();
        _bishojyoNodeDatas = new List<BishojyoNodeData>();

        foreach (var item in currentBishojyoContainer.NodeLinks)
        {
            nodeLinkDatas.Add(item);
        }
        foreach (var item in currentBishojyoContainer.BishojyoNodeDatas)
        {
            _bishojyoNodeDatas.Add(item);
        }
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
        string[] choiceGUID = new string[1];
        try
        {
            choiceGUID = GetTargetGUIDArray(nodeLinkDatas[currentSlideIndex].BaseNodeGUID);
        }
        catch (ArgumentOutOfRangeException e)
        {
            IsEndStory = true;
            _isCanControl = false;
        }
        
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
                    bishojyoNodeData.Slide.slideEvent.onSlideEnable?.Invoke();
                    _textController.UpdateChatWindow(bishojyoNodeData.Slide.currentCharacter, bishojyoNodeData.Slide.text);
                    _characterController.ChangeCharacter(bishojyoNodeData.Slide.currentCharacter, bishojyoNodeData.Slide.characterState, Vector3.zero);
                    break;
                }
            }
        }
        else
        {
            _textController.UpdateChoicePanel(LoadChoice(choiceGUID));
        }
    }

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
            if (_isCanControl == true && IsEndStory == false)
            {
                if (_textController.textMakeComplete == true)
                {
                    currentSlideIndex++;
                    LoadSlide();
                }
                else
                {
                    _textController.ChatSkip();
                }
            }
        }
    }
}