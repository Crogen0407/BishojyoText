using System;
using System.Collections;
using System.Collections.Generic;
using Crogen.BishojyoGraph.SlideEffect;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Crogen.BishojyoGraph
{
    public class StoryManager : MonoSingleton<StoryManager>
    {
        //Controllers
        public CharacterController CharacterController { get; private set; }
        public DataController DataController { get; private set; }
        public TextController TextController { get; private set; }
        public BishojyoDataController BishojyoDataController { get; private set; }

        private void Init()
        {
            CharacterController = FindObjectOfType<CharacterController>();
            DataController = FindObjectOfType<DataController>();
            TextController = FindObjectOfType<TextController>();
            BishojyoDataController = FindObjectOfType<BishojyoDataController>();
        }

        private void Awake()
        {
            Init();
        }

        private void Start()
        {
            SceneManager.sceneLoaded += (arg0, mode) =>
            {
                Init();
            };
        }
    }
}
