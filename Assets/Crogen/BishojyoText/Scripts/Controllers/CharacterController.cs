using System;
using UnityEngine;

namespace Crogen.BishojyoGraph
{
    public class CharacterController : MonoBehaviour
    {
        public Transform characterTransform;
        private SpriteRenderer _characterSpriteRenderer;
        [SerializeField] private SO_CharacterData _characterData;
        private void Awake()
        {
            characterTransform = GameObject.Find("Character").transform;
            _characterSpriteRenderer = characterTransform.GetComponent<SpriteRenderer>();
        }

        public void ChangeCharacter(string characterName, CharacterState characterState, Vector3 position)
        {
            characterTransform.position = position;
            Character currentCharacter = null;
            foreach (var character in _characterData.characters)
            {
                if (character.name == characterName)
                {
                    currentCharacter = character;
                }
            }

            foreach (var characterSprite in currentCharacter.sprites.characterSpriteGroups)
            {
                if (characterSprite.characterState == characterState)
                {
                    _characterSpriteRenderer.sprite = characterSprite.sprite;
                    break;
                }
            }
        }
    }
}