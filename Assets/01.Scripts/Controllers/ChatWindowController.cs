using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Crogen.BishojyoGraph
{
    public class ChatWindowController : MonoBehaviour
    {
        private Image chatWindow;
        private TextMeshProUGUI nameText;
        private TextMeshProUGUI chetText;
        
        public bool isTextComplete;
        
        private void Awake()
        {
            chatWindow = GameObject.Find("ChatWindow").GetComponent<Image>();
            nameText = chatWindow.transform.Find("Name/Text").GetComponent<TextMeshProUGUI>();
            nameText = chatWindow.transform.Find("Text").GetComponent<TextMeshProUGUI>();
        }
    
        public void ChangeChatWindow(string characterName, string text, Color color)
        {
            chatWindow.color = color;
            nameText.text = characterName;
            StartCoroutine(DrawChatText(text, 0.2f));
        }
    
        private IEnumerator DrawChatText(string text, float delayTime)
        {
            isTextComplete = false;
            for (int i = 0; i < text.Length; i++)
            {
                if (text[i].Equals("|"))
                {
                    yield return new WaitForSeconds(text[i + 1]);
                    i += 2;
                }
                else
                {
                    yield return new WaitForSeconds(delayTime);
                }
                chetText.text += text[i];
                if (isTextComplete = true)
                {
                    for (int j = 0; j < text.Length; j++)
                    {
                        chetText.text = "";
                        if (text[i].Equals("|"))
                        {
                            j += 2;
                        }
                        chetText.text += text[j];
                    }
                    break;
                }
            }
            isTextComplete = true;
        }
    }
}

