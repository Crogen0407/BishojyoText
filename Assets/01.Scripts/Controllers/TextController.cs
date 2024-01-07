using System;
using System.Collections;
using System.Collections.Generic;
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

    //Transforms
    public Transform choiceGroup;
    public GameObject choicePanelPrefab;
    
    public bool textMakeComplete;
    
    private void Awake()
    {
        _storyText = chatWindow.transform.Find("Text").GetComponent<TextMeshProUGUI>();
        _nameText = characterNameTag.transform.Find("Text").GetComponent<TextMeshProUGUI>();
    }
        
    public void UpdateChatWindow(string name, string text, NodeLinkData[] nodeLinkDatas, float chatDelay = 0.1f)
    {
        textMakeComplete = false;
        _nameText.text = name;
        
        StartCoroutine(UpdateChatWindowCoroutine(text, nodeLinkDatas, chatDelay));
    }

    public void ChatSkip()
    {
        textMakeComplete = true;
    }

    private IEnumerator UpdateChatWindowCoroutine(string text, NodeLinkData[] nodeLinkDatas, float chatDelay)
    {
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

        for (int i = 0; i < nodeLinkDatas.Length; i++)
        {
            GameObject obj = Instantiate(choicePanelPrefab, choiceGroup);
            obj.transform.GetComponentInChildren<TextMeshProUGUI>().text = nodeLinkDatas[i].PortName;
        }
        
        
        textMakeComplete = true;
        _storyText.text = text;
    }
}
