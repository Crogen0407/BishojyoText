using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class Slide
{
    public UnityEvent onSlideEnable;
    public string currentCharacter;
    public string text;
}

[Serializable]
public class Character
{
    public string name;
    public CharacterState characterState;
    public Color mainColor;
}