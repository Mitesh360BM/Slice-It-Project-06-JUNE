using System;
using UnityEngine;

namespace Tiny
{
    public class Controller : MonoBehaviour
    {
        public SceneAnimation SceneAnimation;

        public virtual string SceneName { get { return ""; } }
        public virtual void OnActived(object data) { }
        public virtual void OnDeactived() { }
        public virtual void OnShown(object data) { }
        public virtual void OnHidden() { }
    }
}