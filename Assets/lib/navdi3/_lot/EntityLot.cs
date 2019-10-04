using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EntityLot : MonoBehaviour
{
    public string lotName = "";
    public static EntityLot NewEntLot(string name = "", Transform parent = null)
    {
        var gob = new GameObject("entlot{"+name+"}");
        gob.transform.SetParent(parent);
        gob.transform.localPosition = Vector3.zero;
        var lot = gob.AddComponent<EntityLot>();
        lot.lotName = name;
        return lot;
    }
    public bool IsEmpty()
    {
        return transform.childCount == 0;
    }
    public void Clear()
    {
        var children = new HashSet<GameObject>();
        foreach (Transform child in transform) children.Add(child.gameObject);
        foreach (var child in children) Destroy(child);
    }

    public static implicit operator Transform(EntityLot entlot)
    {
        return entlot.transform;
    }
}
