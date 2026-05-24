using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct SerializableDictionary<TKey, TValue>
{
    [SerializeField] List<TKey> _keys;
    [SerializeField] List<TValue> _values;
    [SerializeField] int size;

    public SerializableDictionary(Dictionary<TKey, TValue> dict)
    {
        size = dict.Count;

        _keys = new List<TKey>(size);
        _values = new List<TValue>(size);
        foreach ((TKey key, TValue value) in dict)
        {
            _keys.Add(key);
            _values.Add(value);
        }
    }

    public Dictionary<TKey, TValue> ToDict()
    {
        Dictionary<TKey, TValue> dict = new Dictionary<TKey, TValue>(size);

        for (int i = 0; i < size; i++)
        {
            dict.Add(_keys[i], _values[i]);
        }

        return dict;
    }
}
