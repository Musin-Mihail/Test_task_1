using System.Collections;
using UnityEngine;

namespace Animation.Scripts.Common
{
    public interface ICoroutineRunner
    {
        Coroutine StartCoroutine(IEnumerator coroutine);
    }
}