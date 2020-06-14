using System;
using System.Linq;
using System.Reflection;
using UnityEngine;

[AttributeUsage(AttributeTargets.Field)]
public class NativeDelegateAttribute : Attribute
{
}


public abstract class NativeBehaviourScript : MonoBehaviour, IDisposable
{
#if UNITY_EDITOR

    private NativeLibrary nativeLibrary;

    public static readonly string LIB_PATH =
            "D:\\Projects\\Unity\\Sandbox\\Assets\\Plugins\\rust_native.dll";

#endif

    public virtual void Awake()
    {
#if UNITY_EDITOR
        nativeLibrary = new NativeLibrary(LIB_PATH);

        var type = GetType();

        var mi = typeof(NativeLibrary).GetMethod("GetDelegate");
        foreach (var delegateField in type.GetFields().Where(field => field.GetCustomAttributes().Any(attr => attr is NativeDelegateAttribute)))
        {
            var getDelegate = mi.MakeGenericMethod(delegateField.FieldType);
            var newVal = getDelegate.Invoke(nativeLibrary, new object[] { delegateField.Name });

            if (newVal == null)
            {
                throw new Exception($"Failed to find {delegateField.Name} in library {LIB_PATH}");
            }

            delegateField.SetValue(delegateField, newVal);
        }
#endif
    }

    public virtual void Start()
    {
    }

    public virtual void Update()
    {
    }

    public virtual void OnApplicationQuit()
    {
#if UNITY_EDITOR
        nativeLibrary.Dispose();
        nativeLibrary = null;
#endif
    }

    public virtual void Dispose()
    {
    }
}
