namespace navdi3
{

    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class Bank : MonoBehaviour
    {

        public GameObject _prefab;

        public GameObject Prefab
        {
            get
            {
                if (_prefab == null) _prefab = transform.GetChild(0).gameObject;
                return _prefab;
            }
        }

        private void Start()
        {
            Prefab.SetActive(false); // put my prefab 2 sleep
        }

        public GameObject Spawn(Transform parent = null, Vector3? position = null)
        {
            var spawned = InstancePrefab(Prefab);
            spawned.name = this.name;

            if (parent) spawned.transform.SetParent(parent);
            if (position.HasValue) spawned.transform.position = position.Value;

            spawned.SetActive(true);
            return spawned;
        }

        public T Spawn<T>(Transform parent = null, Vector3? position = null) where T : Component
        {
            var spawned = InstancePrefab<T>(Prefab);
            spawned.gameObject.name = this.name;

            if (parent) spawned.transform.SetParent(parent);
            if (position.HasValue) spawned.transform.position = position.Value;

            spawned.gameObject.SetActive(true);
            return spawned;
        }

        GameObject InstancePrefab(GameObject prefab)
        {
            return (GameObject)Object.Instantiate(prefab);
        }
        T InstancePrefab<T>(GameObject prefab) where T : Component
        {
            if (prefab.GetComponent<T>())
            {
                return InstancePrefab(prefab).GetComponent<T>();
            }
            else
            {
                throw new BadPrefabException("Not of type " + typeof(T));
            }
        }
    }

    public class BadPrefabException : System.Exception
    {
        public BadPrefabException(string message) : base(message) { }
    }

}