using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.TerrainTools;
using UnityEngine;

namespace Crogen.BishojyoGraph
{
    [CreateAssetMenu(fileName = "CharacterData", menuName = "SO/CharacterData")]
    public class SO_CharacterData : ScriptableObject
    {
        public List<Character> characters;
    }

    [Serializable]
    public class Character
    {
        public string name;
        public Color mainColor;
        public Vector3 position;
        public CharacterSprite sprites;
    }

    [Serializable]
    public struct CharacterSprite
    {
        public List<CharacterSpriteGroup> characterSpriteGroups;
    }
    
    [Serializable]
    public struct CharacterSpriteGroup
    {
        public CharacterState characterState;
        public Sprite sprite;
    }
    
}

