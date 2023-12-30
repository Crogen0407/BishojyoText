using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SlideController : MonoBehaviour
{
    public List<Slide> slides;

    private int _currentSlide;
    public SO_CharacterData characterData;
    
    public int CurrentSlide
    {
        get => _currentSlide;
        set
        {
            _currentSlide = value;
            for (int i = 0; i < characterData.characters.Count; i++)
            {
                if (characterData.characters[i].name.Equals(slides[_currentSlide].currentCharacter))
                {
                }
            }
            slides[_currentSlide].onSlideEnable?.Invoke();
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