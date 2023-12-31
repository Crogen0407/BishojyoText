using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Crogen.BishojyoGraph
{
    public class StoryManager : MonoSingleton<StoryManager>
    {
        //Controllers
        public SlideController SlideController { get; private set; }
        public CharacterController CharacterController { get; private set; }

        private void Init()
        {
            SlideController = FindObjectOfType<SlideController>();
            CharacterController = FindObjectOfType<CharacterController>();
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

        private void OnDestroy()
        {
        
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                SlideController.CurrentSlide++;
                CharacterController.ChangeCharacter(
                    SlideController.characterData.characters[SlideController.CurrentSlide].sprites,
                    CharacterState.Angry, Vector3.zero);
            }
        }
    }
}
