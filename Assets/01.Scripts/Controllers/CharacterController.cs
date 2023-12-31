using System;
using UnityEngine;

namespace Crogen.BishojyoGraph
{
    public class CharacterController : MonoBehaviour
    {
        private Transform _characterTransform;
        private SpriteRenderer _characterSpriteRenderer;

        private void Awake()
        {
            _characterTransform = GameObject.Find("Character").transform;
            _characterSpriteRenderer = _characterTransform.GetComponent<SpriteRenderer>();
        }

        public void ChangeCharacter(CharacterSprite characterSprite, CharacterState characterState, Vector3 position)
        {
            _characterTransform.position = position;
            Debug.Log(typeof(CharacterSprite).GetProperties().Length);
            for (int i = 0; i < typeof(CharacterSprite).GetProperties().Length; i++)
            {
                
            }
        }
    }
}