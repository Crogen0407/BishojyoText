using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.TerrainTools;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterData", menuName = "SO/CharacterData")]
public class SO_CharacterData : ScriptableObject
{
    public List<Character> characters;
    
    public void ChangeCharacterState(string characterName, CharacterState characterState)
    {
        for (int i = 0; i < characters.Count; i++)
        {
            if (characters[i].name.Equals(characterName))
            {
                characters[i].characterState = characterState;
                break;
            }
        }
    }
}

[Serializable]
public class Character
{
    public string name;
    public CharacterState characterState;
    public Color mainColor;
    public CharacterSprite sprites;
}

[Serializable]
public class CharacterSprite
{
    public Sprite normal;
    public Sprite angry;
    public Sprite sad;
    public Sprite ashamed;
    public Sprite happy;
    public Sprite jealousy;
    public Sprite mischievous;
    public Sprite thoughtful;
    public Sprite refreshed;
    public Sprite scared;
}
