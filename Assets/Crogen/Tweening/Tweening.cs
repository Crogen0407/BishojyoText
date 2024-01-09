using System;
using System.Collections;
using System.Reflection;
using UnityEngine;
using Crogen.Tweening;

namespace Crogen.Tweening
{
    public class  Tweening : MonoSingleton<Tweening>
    {
        #region Move
            public void DOMove(Transform transform, Vector3 endPoint, float duration, EasingType easing)
            {
                StartCoroutine(Move(transform, endPoint, duration, easing));
            }
            public void DOMove(Transform transform, Vector3 endPoint, float duration, IEnumerator lateCoroutine, EasingType easing)
            {
                StartCoroutine(Move(transform, endPoint, duration, lateCoroutine,easing));
            }
            public void DOMove(Rigidbody rigidbody, Vector3 endPoint, float duration, EasingType easing)
            {
                StartCoroutine(Move(rigidbody, endPoint, duration, easing));
            }
            public void DOMove(Rigidbody rigidbody, Vector3 endPoint, float duration, IEnumerator lateCoroutine, EasingType easing)
            {
                StartCoroutine(Move(rigidbody, endPoint, duration, lateCoroutine, easing));
            }
            
            private IEnumerator Move(Transform transform, Vector3 endPoint, float duration, EasingType easing)
            {
                float currentTime = 0;
                float percentTime = 0;
                
                Vector3 startPoint = transform.position;
                
                while (currentTime < duration)
                {
                    currentTime += Time.deltaTime;
                    percentTime = currentTime / duration;
                    transform.localPosition = Vector3.Lerp(startPoint, endPoint, new EaseTweeningCollection().SetEase(easing, percentTime));
                    yield return null;
                }
                transform.localPosition = endPoint;
            }
            private IEnumerator Move(Transform transform, Vector3 endPoint, float duration, IEnumerator lateCoroutine, EasingType easing)
            {
                float currentTime = 0;
                float percentTime = 0;
                
                Vector3 startPoint = transform.position;
                
                while (currentTime < duration)
                {
                    currentTime += Time.deltaTime;
                    percentTime = currentTime / duration;
                    transform.localPosition = Vector3.Lerp(startPoint, endPoint, new EaseTweeningCollection().SetEase(easing, percentTime));
                    yield return null;
                }
                transform.localPosition = endPoint;
            
                yield return StartCoroutine(lateCoroutine);
            }
            private IEnumerator Move(Rigidbody rigidbody, Vector3 endPoint, float duration, EasingType easing)
            {
                float currentTime = 0;
                float percentTime = 0;
                
                Vector3 startPoint = rigidbody.position;
                
                while (currentTime < duration)
                {
                    currentTime += Time.deltaTime;
                    percentTime = currentTime / duration;
                    transform.localPosition = Vector3.Lerp(startPoint, endPoint, new EaseTweeningCollection().SetEase(easing, percentTime));
                    rigidbody.position = endPoint;
                    yield return null;
                }
            }
            private IEnumerator Move(Rigidbody rigidbody, Vector3 endPoint, float duration, IEnumerator lateCoroutine, EasingType easing)
            {
                float currentTime = 0;
                float percentTime = 0;
                
                Vector3 startPoint = rigidbody.position;
                Type thisClassType = typeof(EaseTweeningCollection);
                string funcName = easing.ToString();
                MethodInfo easingFunc = thisClassType.GetMethod(funcName, BindingFlags.NonPublic | BindingFlags.Instance);
                
                
                while (currentTime < duration)
                {
                    currentTime += Time.deltaTime;
                    percentTime = currentTime / duration;
                    if(easingFunc != null)
                    {
                        rigidbody.position = Vector3.Lerp(startPoint, endPoint, (float)(easingFunc.Invoke(this, new object[]{percentTime})));
                    }
                    yield return null;
                }
                rigidbody.position = endPoint;
            
                yield return StartCoroutine(lateCoroutine);
            }
        #endregion

        // public void Fade<T>(T target, float endPoint, float duration, EasingType easing) where T : 
        // {
        //     
        // }
    }
}