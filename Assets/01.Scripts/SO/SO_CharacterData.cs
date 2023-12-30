using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterData", menuName = "SO/CharacterData")]
public class SO_CharacterData : ScriptableObject
{
    public List<Character> characters;
}
