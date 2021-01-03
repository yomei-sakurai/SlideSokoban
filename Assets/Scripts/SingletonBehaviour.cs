using System;
using UnityEngine;

namespace Assets.Scripts
{
    public abstract class SingletonBehaviour<T> : MonoBehaviour where T : SingletonBehaviour<T>
    {
        protected static T instance = null;
        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    Type t = typeof (T);

                    instance = (T) FindObjectOfType (t);
                    if (instance == null)
                    {
                        instance = CreateInstance ();
                    }
                }

                return instance;
            }
        }

        private bool isInitialized = false;
        protected virtual bool DontDestroy => true; // DontDestroy設定

        private static T CreateInstance ()
        {
            var obj = new GameObject (typeof (T).Name);
            instance = obj.AddComponent<T> ();
            instance.SetDontDestroy ();
            instance.Initialize ();
            return instance;
        }

        protected virtual void Awake ()
        {
            CheckInstance ();
        }

        protected bool CheckInstance ()
        {
            if (instance == null)
            {
                instance = this as T;
                SetDontDestroy ();
                return true;
            }
            else if (Instance == this)
            {
                return true;
            }
            Destroy (this);
            return false;
        }

        private void SetDontDestroy ()
        {
            if (DontDestroy)
            {
                DontDestroyOnLoad (instance);
            }
        }

        public void Initialize ()
        {
            OnInitialize ();
            isInitialized = true;
        }

        protected virtual void OnInitialize () { }

        private void Update ()
        {
            if (!isInitialized) return;
            OnUpdate ();
        }

        protected virtual void OnUpdate () { }

        protected virtual void OnDestroy ()
        {
            instance = null;
        }
    }
}