using System.Collections;
using UnityEngine;

namespace Animation.Scripts.Common
{
    public interface ICoroutineRunner
    {
        Coroutine StartCoroutine(IEnumerator coroutine);
        void StopCoroutine(Coroutine coroutine);
        void StopCoroutine(IEnumerator coroutine); 
    }
}