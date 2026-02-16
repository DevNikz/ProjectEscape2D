using UnityEngine;
using System.Reflection;
using System;

public static class ComponentExtensions
{
    public static T CopyComponent<T>(this GameObject destination, T original) where T : Component
    {
        Type type = original.GetType();
        Component copy = destination.AddComponent(type);
        // Copied fields can be restricted with BindingFlags
        FieldInfo[] fields = type.GetFields();
        foreach (FieldInfo field in fields)
        {
            field.SetValue(copy, field.GetValue(original));
        }
        return copy as T;
    }
}
