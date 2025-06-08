using System.Collections;
using UnityEngine;

namespace Animation.Scripts.Interfaces
{
    public interface ICoroutineRunner
    {
        Coroutine StartCoroutine(IEnumerator coroutine);
    }
}