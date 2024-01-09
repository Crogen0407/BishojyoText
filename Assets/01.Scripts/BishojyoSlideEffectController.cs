using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Crogen.Tweening;

namespace Crogen.BishojyoGraph.SlideEffect
{
    public class BishojyoSlideEffectController : MonoBehaviour
    {
        private void Awake()
        {
            Tweening.Tweening.Instance.DOMove(transform, Vector3.one * 5, 10, EasingType.EaseInBounce);
        }

        public Image fadePanel;
        public void Fade(bool fadeType, float duration)
        {
            Image image = Instantiate(fadePanel, FindObjectOfType<Canvas>().transform);
            image.color = new Color(0, 0, 0, Convert.ToInt32(fadeType));
            StartCoroutine(FadeCoroutine(image, duration));
        }

        private IEnumerator FadeCoroutine(Image fadeImage, float duration)
        {
            float startAlpha = fadeImage.color.a;

            float currentTime = 0;
            float percentTime = 0;
            yield return new WaitForSeconds(1);
        }
    }
}

