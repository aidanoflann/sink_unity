using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Utils
{
    public class SingletonBehaviour: MonoBehaviour
    {
        // enforce singleton behaviour
        public static SingletonBehaviour instance;

        public static T GetSingletonBehaviour<T>() where T:MonoBehaviour
        {
            T[] allFoundSingletonBehaviours = FindObjectsOfType<T>();
            if (allFoundSingletonBehaviours.Length != 1)
            {
                Debug.LogErrorFormat("Found more than one {0} on startup. Attaching last one in array...", typeof(T));
            }
            return allFoundSingletonBehaviours[allFoundSingletonBehaviours.Length - 1];
        }

        void Awake()
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
