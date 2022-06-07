using System;
using UnityEngine;

namespace Tiny
{
    public class SceneAnimation : MonoBehaviour
    {
        public virtual void Show(Action callback)
        {
            if (callback != null) callback();
        }
        public virtual void Hide(Action callback)
        {
            if (callback != null) callback();
        }
    }
}
