using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class AutoBoneCombine : MonoBehaviour
{
    [Header ("기준 뼈대(Amature)")]
    [SerializeField]
    private Transform parents;
    [Header("타겟 뼈대(Amature)")]
    [SerializeField]
    private Transform child;
    [Header("뼈대 구분을 위한 어미 추가 - Ex) carrot -> hip_carrot")]
    [SerializeField]
    private string childName;


    public List<Transform> parents_l = new List<Transform>();

    public List<Transform> child_l = new List<Transform>();

    public void AutoCombine()
    {
        parents_l.Clear();
        child_l.Clear();

        SearchParents_l(parents);
        SearchChild_l(child);

        for(int i = 0; i < child_l.Count - 1; i++) {
            for(int j = 0; j < parents_l.Count - 1; j++) {
                if (child_l[i].name.Equals(parents_l[j].name)) {
                    child_l[i].name = child_l[i].name + "_" + childName;
                    child_l[i].SetParent(parents_l[j]);
                }
            }
        }
    }

    public void SearchParents_l(Transform _parents)
    {
        if (_parents.childCount < 1) return;

        parents_l.Add(_parents);

        for (int i = 0; i < _parents.childCount; i++)
        {
            parents_l.Add(_parents.GetChild(i));
            SearchParents_l(_parents.GetChild(i));
        }
    }

    public void SearchChild_l(Transform _parents)
    {
        if (_parents.childCount < 1) return;

        child_l.Add(_parents);

        for (int i = 0; i < _parents.childCount; i++)
        {
            child_l.Add(_parents.GetChild(i));
            SearchChild_l(_parents.GetChild(i));
        }
    }
}