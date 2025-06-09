using System;
using UnityEngine;

namespace Animation.Scripts.Player
{
    public class AnimationEventReceiver : MonoBehaviour
    {
        public event Action<string> OnAnimationEvent;

        public void TriggerEvent(string eventName)
        {
            OnAnimationEvent?.Invoke(eventName);
        }
    }
}