using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Crogen.Tweening;

namespace Crogen.BishojyoGraph.SlideEffect
{
    public class BishojyoSlideEffectController : MonoSingleton<BishojyoSlideEffectController>
    {
        public void Fade(bool fadeType, float duration)
        { 
            Image fadePanel = new GameObject().AddComponent<Image>();
            fadePanel.color = Color.black;

            Transform rectTrm = fadePanel.transform;

            rectTrm.localScale = Vector2.one * 20;
            
            Image image = Instantiate(fadePanel, FindObjectOfType<Canvas>().transform);
            Color startColor = new Color(0, 0, 0, Convert.ToInt32(fadeType));
            image.color = startColor;

            Tweening.Tweening.Instance.DOColor(image, new Color(0, 0, 0, Convert.ToInt32(!fadeType)),  duration, FadeObjectDestroy(image),
                EasingType.EaseInSine);
        }

        private IEnumerator FadeObjectDestroy(Image image)
        {
            Destroy(image.gameObject);
            yield return null;
        }
    }
}

