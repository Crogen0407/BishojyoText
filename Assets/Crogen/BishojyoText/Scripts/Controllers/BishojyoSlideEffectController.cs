using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Crogen.Tweening;
using UnityEngine.Events;

namespace Crogen.BishojyoGraph.SlideEffect
{
    public class BishojyoSlideEffectController : MonoSingleton<BishojyoSlideEffectController>
    {
        private Image _image;
        
        public void Fade(bool fadeType, float duration)
        { 
            if (_image == null)
            {
                Image fadePanel = new GameObject().AddComponent<Image>();
                fadePanel.color = Color.black;

                Transform rectTrm = fadePanel.transform;

                rectTrm.localScale = Vector2.one * 20;
                _image = Instantiate(fadePanel, FindObjectOfType<Canvas>().transform);
            }
            _image.gameObject.SetActive(true);
            Color startColor = new Color(0, 0, 0, Convert.ToInt32(fadeType));
            _image.color = startColor;

            Tweening.Tweening.Instance.DOColor(_image, new Color(0, 0, 0, Convert.ToInt32(!fadeType)),  duration, FadeObjectDestroy(_image),
                EasingType.EaseInSine);
        }
        
        public void Fade(bool fadeType, float duration, Action endEvent)
        { 
            if (_image == null)
            {
                Image fadePanel = new GameObject().AddComponent<Image>();
                fadePanel.color = Color.black;

                Transform rectTrm = fadePanel.transform;

                rectTrm.localScale = Vector2.one * 20;
                _image = Instantiate(fadePanel, FindObjectOfType<Canvas>().transform);
            }
            _image.gameObject.SetActive(true);
            Color startColor = new Color(0, 0, 0, Convert.ToInt32(fadeType));
            _image.color = startColor;

            Tweening.Tweening.Instance.DOColor(_image, new Color(0, 0, 0, Convert.ToInt32(!fadeType)),  duration, FadeObjectDestroy(_image, endEvent),
                EasingType.EaseInSine);
        }

        private IEnumerator FadeObjectDestroy(Image image, Action endEvent = null)
        {
            if (_image.color.a == 0)
            {
                _image.gameObject.SetActive(false);
            }
            endEvent?.Invoke();
            yield return null;
        }
    }
}

