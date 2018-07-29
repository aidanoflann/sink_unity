using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Utils
{
    public class SingletonBehaviour: MonoBehaviour
    {
        // enforce singleton behaviour
        private SingletonBehaviour instance;

        public static T GetSingletonBehaviour<T>() where T:MonoBehaviour
        {
            T[] allFoundSingletonBehaviours = FindObjectsOfType<T>();
            if (allFoundSingletonBehaviours.Length != 1)
            {
                Debug.LogErrorFormat("Found {0} {1}s on startup. Attaching last one in array...", allFoundSingletonBehaviours.Length, typeof(T));
            }
            return allFoundSingletonBehaviours[allFoundSingletonBehaviours.Length - 1];
        }

        public void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                DestroyImmediate(this.gameObject);
                return;
            }
            DontDestroyOnLoad(gameObject);
        }
    }
}
