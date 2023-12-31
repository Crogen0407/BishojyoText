using System;
using UnityEngine;
using UnityEngine.Events;

namespace Crogen.BishojyoGraph
{
    [Serializable]
    public class Slide
    {
        public string currentCharacter;
        public CharacterState characterState;
        public string text;
        public SlideEvent slideEvent;
    }

    [Serializable]
    public class SlideEvent
    {
        public UnityEvent onSlideEnable;
        public UnityEvent onSlideDisable;
    }
}