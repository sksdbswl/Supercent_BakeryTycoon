using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class AssetTable<T> : ScriptableObject
{
    [Serializable]
    public class Entry
    {
        public int key;
        public T asset;
    }

    public Entry[] entries;
    protected Dictionary<int, T> map;

    protected virtual void OnEnable()
    {
        map = new Dictionary<int, T>();
        foreach (var entry in entries)
        {
            if (!map.ContainsKey(entry.key))
                map.Add(entry.key, entry.asset);
        }
    }

    public T GetAsset(int key) => map.GetValueOrDefault(key);
}