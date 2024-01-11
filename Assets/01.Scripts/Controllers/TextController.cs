using System.Collections;
using Crogen.BishojyoGraph;
using Crogen.BishojyoGraph.RunTime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TextController : MonoBehaviour
{
    public Image chatWindow;
    private TextMeshProUGUI _storyText;

    public Image characterNameTag;
    private TextMeshProUGUI _nameText;
    
    public bool textMakeComplete;

    //Transforms
    public Transform choiceGroup;
    public GameObject choicePanelPrefab;
    
    //Controllers
    private BishojyoDataController _bishojyoDataController;
    
    //Managers
    private StoryManager _storyManager;
    
    private void Awake()
    {
        _storyManager = StoryManager.Instance;
        _bishojyoDataController = _storyManager.BishojyoDataController;
        _storyText = chatWindow.transform.Find("Text").GetComponent<TextMeshProUGUI>();
        _nameText = characterNameTag.transform.Find("Text").GetComponent<TextMeshProUGUI>();
    }
        
    public void UpdateChatWindow(string name, string text, NodeLinkData[] nodeLinkDatas, float chatDelay = 0.1f)
    {
        if (nodeLinkDatas.Length <= 0)
        {
            chatWindow.gameObject.SetActive(true);
            textMakeComplete = false;
            _nameText.text = name;
            _name = name;
            _text = text;
            StartCoroutine(UpdateChatWindowCoroutine(text, chatDelay));
        }
        else
        {
            chatWindow.gameObject.SetActive(false);
            UpdateChoicePanel(nodeLinkDatas);
        }
        
    }

    [SerializeField] private string _name;
    [SerializeField] private string _text;
    public void ChatSkip()
    {
        textMakeComplete = true;
    }

    private IEnumerator UpdateChatWindowCoroutine(string text, float chatDelay)
    {
        int childrenCount = choiceGroup.GetComponentsInChildren<Image>().Length;

        Image[] images = choiceGroup.GetComponentsInChildren<Image>();
        for (int i = 0; i < childrenCount; i++)
        {
            Destroy(images[i].gameObject);
        }
        
        _storyText.text = string.Empty;
        for (int i = 0; i < text.Length; i++)
        {
            if (textMakeComplete == false)
            {
                _storyText.text += text[i];
                yield return new WaitForSeconds(chatDelay);
            }
            else
            {
                break;
            }
        }
        
        textMakeComplete = true;
        _storyText.text = text;
    }

    private void UpdateChoicePanel(NodeLinkData[] nodeLinkDatas)
    {
        int childrenCount = choiceGroup.GetComponentsInChildren<Image>().Length;

        Image[] images = choiceGroup.GetComponentsInChildren<Image>();
        for (int i = 0; i < childrenCount; i++)
        {
            Destroy(images[i].gameObject);
        }

        //여기 수정해야 함
        for (int i = 0; i < nodeLinkDatas.Length; i++)
        {
            GameObject obj = Instantiate(choicePanelPrefab, choiceGroup);
            int index = nodeLinkDatas.Length - i;
            obj.GetComponent<Button>().onClick.AddListener(() =>
            {
                BishojyoDataController bishojyoDataController;
                bishojyoDataController = _storyManager.BishojyoDataController;
                Debug.Log(index);
                bishojyoDataController.currentSlideIndex += index;
            });
            obj.transform.GetComponentInChildren<TextMeshProUGUI>().text = nodeLinkDatas[i].PortName;
        }
    }
}
