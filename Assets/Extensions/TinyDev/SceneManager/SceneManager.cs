using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Tiny
{
    public class SceneManager : Singleton<SceneManager>
    {
        private SceneManager() { }

        [SerializeField]
        private List<PopupInfo> _popupPrefabs = new List<PopupInfo>();
        [SerializeField]
        private Transform _root;
        [SerializeField]
        private Camera _uiCamera;

        public Camera UICamera { get { return _uiCamera; } }
        private Stack<string> _currentPopups = new Stack<string>();
        private Dictionary<string, Controller> _popups = new Dictionary<string, Controller>();
        private Controller _currentScene;

        public string Current
        {
            get
            {
                if (_currentPopups.Count > 0)
                {
                    return _currentPopups.Peek();
                }
                else if (_currentScene != null)
                {
                    return _currentScene.SceneName;
                }
                else
                {
                    return "Empty";
                }
            }
        }

        private void Awake()
        {
            UnityEngine.SceneManagement.SceneManager.sceneLoaded += sceneLoaded;
            DontDestroyOnLoad(gameObject);
        }

        private void OnApplicationQuit()
        {
        }

        private void sceneLoaded(Scene scene, LoadSceneMode mode)
        {
            var roots = scene.GetRootGameObjects();
            foreach (var item in roots)
            {
                var controller = item.GetComponent<Controller>();
                if (controller != null)
                {
                    _currentScene = controller;
                    controller.OnActived(null);
                    // TODO: send data through scenes
                    controller.OnShown(null);
                    return;
                }
            }
        }

        public void Scene(string name)
        {
            forceCloseAllPopups();

            if (_currentScene != null)
            {
                _currentScene.OnHidden();
                _currentScene.OnDeactived();
                _currentScene = null;
            }

            UnityEngine.SceneManagement.SceneManager.LoadScene(name);
        }

        public void Popup(string name, bool doAnimation = false, object data = null)
        {
            var popup = loadScene(name);
            if (popup == null)
            {
                Debug.Log("cannot load new popup!");
                return;
            }

            _currentPopups.Push(name);

            checkEnableBlurCameraIfAny(true);

            popup.gameObject.SetActive(true);
            popup.OnActived(data);

            if (doAnimation && popup.SceneAnimation != null)
            {
                popup.SceneAnimation.Show(() =>
                {
                    popup.OnShown(data);
                });
            }
            else
            {
                popup.OnShown(data);
            }
        }

        private void checkEnableBlurCameraIfAny(bool v)
        {
        }

        public void Close(bool doAnimation = false, Action callback = null)
        {
            if (_currentPopups.Count == 0) return;

            var name = _currentPopups.Pop();
            var popup = _popups[name];

            if (doAnimation && popup.SceneAnimation != null)
            {
                popup.SceneAnimation.Hide(() =>
                {
                    popup.OnHidden();
                    popup.gameObject.SetActive(false);
                    popup.OnDeactived();
                    checkEnableBlurCameraIfAny(false);

                    if (callback != null) callback();
                });
            }
            else
            {
                popup.OnHidden();
                popup.gameObject.SetActive(false);
                popup.OnDeactived();
                checkEnableBlurCameraIfAny(false);

                if (callback != null) callback();
            }
        }

        private void forceCloseAllPopups()
        {
            while (_currentPopups.Count > 0)
            {
                Close(false);
            }
        }

        private Controller loadScene(string name)
        {
            if (_popups.ContainsKey(name))
            {
                Debug.Log("load popup " + name);
                
                return _popups[name];
            }

            Debug.Log("new popup " + name);

            var prefabName = _popupPrefabs.Find(x => x.Name == name);
            if (prefabName == null)
            {
                return null;
            }

            var popupObj = Instantiate(prefabName.prefab);
            popupObj.transform.SetParent(_root, false);

            var popup = popupObj.GetComponent<Controller>();
            _popups.Add(name, popup);

            return popup;
        }

        [Serializable]
        public class PopupInfo
        {
            public string Name;
            public GameObject prefab;
        }
    }
}

