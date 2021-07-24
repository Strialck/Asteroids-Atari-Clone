using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pool<T> where T: MonoBehaviour
{
    Queue<T> _instances;
    Transform _container;
    int _poolSize;
    T _storedObj;

    public Pool(Transform container, T storedObj, int initCount)
    {
        _poolSize = initCount;
        _container = container;
        _storedObj = storedObj;
    }

    public void InitPool()
    {
        _instances = new Queue<T>(_poolSize);
        T obj;

        for (int i = 0; i < _poolSize; i++)
        {
            obj = Object.Instantiate(_storedObj, _container);
            obj.gameObject.SetActive(false);
            _instances.Enqueue(obj);
        }
    }

    public T GetObject()
    {
        if(_instances.Count != 0)
        {
            T inst = _instances.Dequeue();
            inst.transform.parent = null;
            inst.gameObject.SetActive(true);
            return inst;
        }

        return Object.Instantiate(_storedObj);
    }

    public void PoolObject(T objToStore)
    {
        if (_instances.Count < _poolSize)
        {
            objToStore.transform.parent = _container;
            objToStore.gameObject.SetActive(false);
            _instances.Enqueue(objToStore);

            
        }
        else
            Object.Destroy(objToStore.gameObject);

    }
}
