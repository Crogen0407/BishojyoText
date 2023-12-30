using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SlideController : MonoBehaviour
{
    private int _currentSlide;
    public int CurrentSlide
    {
        get => _currentSlide;
        set
        {
            _currentSlide = value;
            onSlideEnable?.Invoke();
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
    
    //Events
    public UnityEvent onSlideEnable;
}