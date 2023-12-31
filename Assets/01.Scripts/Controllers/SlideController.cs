using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Crogen.BishojyoGraph
{
    public class SlideController : MonoBehaviour
    {
        public List<Scene> scenes;
        public int currentSceneCount;
        
        public SO_CharacterData characterData;
        
        private int _currentSlide;
        
        public int CurrentSlide
        {
            get => _currentSlide;
            set
            {
                _currentSlide = value;
                for (int i = 0; i < characterData.characters.Count; i++)
                {
                    Slide currentSlide = scenes[currentSceneCount].slides[_currentSlide];
                    if (characterData.characters[i].name.Equals(currentSlide.currentCharacter))
                    {
                        characterData.ChangeCharacterState(currentSlide.currentCharacter, currentSlide.characterState);
                    }
                }
                scenes[currentSceneCount].slides[_currentSlide].slideEvent.onSlideEnable?.Invoke();
            }
        }
        
        private int _currentSection;
        public int CurrentSection
        {
            get => _currentSection;
            set
            {
                _currentSection = value;
            }
        }
    }
    
    [Serializable]
    public class Scene
    {
        public List<Slide> slides;
    }
    
    
}
