using System.Collections;
using Animation.Scripts.Interfaces;
using UnityEngine;

namespace Animation.Scripts
{
    public class CoroutineRunner : MonoBehaviour, ICoroutineRunner
    {
        public Coroutine StartCoroutine(IEnumerator coroutine)
        {
            return base.StartCoroutine(coroutine);
        }
    }
}