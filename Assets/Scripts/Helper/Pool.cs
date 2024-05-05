using System.Collections.Generic;
using UnityEngine;
using Unity.Collections;
using Unity.VisualScripting;

namespace GameGuru.Helper
{
    [System.Serializable]
    public class Pool<T> where T : MonoBehaviour
    {

        [SerializeField] private T prefab;
        [SerializeField] private int startCount = 10;
        [SerializeField] private Transform parent;

        public Transform Parent => parent;

        private List<T> _pool = new List<T>();

        public void Initiliaze()
        {
            if (_pool.Count > 0)
            {
                ResetAll();
                return;
            }

            for (int i = 0; i < startCount; i++)
            {
                T obj = GameObject.Instantiate(prefab);
                obj.transform.SetParent(parent);
                Push(obj);
            }
        }

        public T Pop()
        {
            var en = _pool.GetEnumerator();
            while (en.MoveNext())
            {
                var c = en.Current;
                if (!c.gameObject.activeInHierarchy)
                {
                    c.gameObject.SetActive(true);
                    en.Dispose();
                    return c;
                }
            }

            en.Dispose();

            T newObj = GameObject.Instantiate(prefab);
            newObj.transform.SetParent(parent);
            Push(newObj);
            newObj.gameObject.SetActive(true);
            return newObj;
        }

        public void Push(T obj)
        {
            if (!_pool.Contains(obj))
            {
                _pool.Add(obj);
                obj.gameObject.SetActive(false);
            }
        }

        public void ResetAll()
        {
            var en = _pool.GetEnumerator();
            while (en.MoveNext())
            {
                var c = en.Current;
                c.gameObject.SetActive(false);

            }

            en.Dispose();

        }

        public void DestroyAll()
        {
            foreach (var obj in _pool)
            {
                if (obj == null) continue;

#if UNITY_EDITOR
                GameObject.DestroyImmediate(obj.gameObject);
#else
            GameObject.Destroy(obj.gameObject);
#endif
            }
            _pool.Clear();
        }
    }
}