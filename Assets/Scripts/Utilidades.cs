using System;
using UnityEngine;

public class Utilidades{
    
    public static GameObject FindObject<T>(string name) where T : UnityEngine.Object {
        T[] objects = Resources.FindObjectsOfTypeAll<T>() as T[];
        foreach(T obj in objects){
            if(obj.name.Equals(name, StringComparison.OrdinalIgnoreCase))
                return obj as GameObject;
        }
        return null;
    }

}
