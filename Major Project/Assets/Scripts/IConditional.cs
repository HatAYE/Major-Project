using System.Diagnostics;
using System;
using UnityEngine;

public interface IConditional
{
    public bool Check();
}

[AttributeUsage(AttributeTargets.Field)]
[Conditional("UNITY_EDITOR")]
public class RequireTypeAttribute : PropertyAttribute
{
    public Type requiredType { get; private set; }

    public RequireTypeAttribute(Type type)
    {
        this.requiredType = type;
    }
}