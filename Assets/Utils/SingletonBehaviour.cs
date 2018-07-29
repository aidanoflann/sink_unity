using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Utils
{
    public class SingletonBehaviour: MonoBehaviour
    {
        // enforce singleton behaviour
        private static List<SingletonBehaviour> singletonRegistry = new List<SingletonBehaviour>();
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
            // if this newly instantiating singleton is already in the registry, destroy its gameobject and early out
            foreach(SingletonBehaviour singletonBehaviour in singletonRegistry)
            {
                if(singletonBehaviour.GetType() == this.GetType())
                {
                    DestroyImmediate(this.gameObject);
                    return;
                }
            }

            // if we're here, it means the singleton is not in the registry. Add it, and make sure we never destroy the gameobject
            singletonRegistry.Add(this);
            DontDestroyOnLoad(gameObject);
        }
    }
}
