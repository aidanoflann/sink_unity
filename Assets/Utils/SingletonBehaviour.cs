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
        // Fetch the requested SingletonBehaviour from the registry
        {
            foreach(SingletonBehaviour singletonBehaviour in singletonRegistry)
            {
                if(singletonBehaviour.GetType() == typeof(T))
                {
                    return singletonBehaviour as T;
                }
            }
            Debug.LogFormat("Cannot find object of type {0} in the singleton registry. Checking for new unregistered instances.", typeof(T).ToString());

            // Bit messy, but this may be needed if two objects awake in an unspecified order, and one requires a reference to the other.
            // The expectation is that the instance found by the following block will later be added to the registry.
            T[] allFoundSingletonBehaviours = FindObjectsOfType<T>();
            if (allFoundSingletonBehaviours.Length != 1)
            {
                Debug.LogErrorFormat("Found {0} {1}s on startup. Attaching last one in array...", allFoundSingletonBehaviours.Length, typeof(T));
            }
            return allFoundSingletonBehaviours[allFoundSingletonBehaviours.Length - 1];
        }

        // TODO: THIS DOESN'T SEEM TO ACTUALLY WORK, NEW SINGLETONBEHAVIOURS STILL GET CREATED. NEEDS INVESTIGATION
        public void Awake()
        {
            Debug.LogFormat("Trying to Awake {0} object. There are currently {1} distinct singletons registered.", this.GetType(), singletonRegistry.Count);

            // if this newly instantiating singleton is already in the registry, destroy its gameobject and early out
            foreach(SingletonBehaviour singletonBehaviour in singletonRegistry)
            {
                if(singletonBehaviour.GetType() == this.GetType())
                {
                    Debug.LogFormat("There is already a {0} object in the registry. Deleting gameobject.", this.GetType());
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
