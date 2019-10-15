using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "MyScriptableObject/Create Prefectures")]
public class Prefectures : ScriptableObject
{
    [Serializable]
    public struct Prefecture
    {
        public string name;
        public Sprite sprite;
    }

    public int Count => prefectures.Count;

    public Prefecture Get(int index) => prefectures[index];

    public List<string> Names => prefectures.ConvertAll(_pref => _pref.name);

    [SerializeField] private List<Prefecture> prefectures;
}
