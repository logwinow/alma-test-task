using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace DiractionTeam.Utils.Patterns
{
    public abstract class SingletonMono<T> : MonoBehaviour where T : SingletonMono<T>
    {
        [SerializeField] private bool _dontDestroyOnLoad;
        [SerializeField] private bool _destroyGameObjectOnDuplication = true;
        private static T s_instance;
        private bool _singletonInitialized;

        public static T Instance
        {
            get
            {
                if (s_instance != null)
                    return s_instance;

                Debug.Log($"Объект типа {typeof(T)} не проинициализирован");

                // объект не создан на сцене либо еще не проинициализирован
                var singletonInScene = FindObjectOfType<T>();

                // Попытка найти объект на сцене, если обращение к экземпляру
                // осуществляется до вызова метода Awake
                if (singletonInScene != null)
                {
                    Debug.Log($"Объект типа {typeof(T)} был найден на сцене под именем {singletonInScene.name}");

                    s_instance = singletonInScene;
                    singletonInScene.Awake();
                }
                else
                {
                    _ = new GameObject(typeof(T).ToString(), typeof(T));
                    Debug.Log(
                        $"Объект типа \"{typeof(T)}\" не проинициализирован. Новый объект был создан и проинициализирован.");
                }

                return s_instance;
            }
        }

        public static bool IsInitialized => s_instance;

        protected void Awake()
        {
            if (_singletonInitialized)
                return;

            // Попытка проиницализировать поле.
            if (s_instance == null)
                s_instance = GetComponent<T>();
            // На сцене уже есть объект этого типа
            else
            {
                // Если проинициализированный объект не является данным, то данный объект необходимо удалить
                if (s_instance.gameObject != gameObject)
                {
                    Debug.Log($"На сцене обнаружено более одного объекта типа {typeof(T)}");

                    if (_destroyGameObjectOnDuplication)
                        Destroy(gameObject);
                    else
                        Destroy(this);
                    Debug.Log($"Объект \"{name}\" был удален.");

                    return;
                }
            }

            if (_dontDestroyOnLoad)
                DontDestroyOnLoad(this);

            OnAwake();

            _singletonInitialized = true;
        }

        protected virtual void OnAwake() { }
    }
}