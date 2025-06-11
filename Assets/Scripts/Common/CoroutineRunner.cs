using System.Collections;
using UnityEngine;

namespace Animation.Scripts.Common
{
    public class CoroutineRunner : MonoBehaviour, ICoroutineRunner
    {
        public Coroutine StartCoroutine(IEnumerator coroutine)
        {
            return base.StartCoroutine(coroutine);
        }

        public void StopCoroutine(Coroutine coroutine)
        {
            base.StopCoroutine(coroutine);
        }

        public void StopCoroutine(IEnumerator coroutine)
        {
            base.StopCoroutine(coroutine);
        }
    }
}