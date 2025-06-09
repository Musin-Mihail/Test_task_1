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
    }
}