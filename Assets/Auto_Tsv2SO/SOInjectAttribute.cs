using System;
using UnityEngine;

[AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
public class SOInjectAttribute : Attribute
{
    public int Index { get; private set; }

    public SOInjectAttribute(int index)
    {
        Index = index;
    }
}
