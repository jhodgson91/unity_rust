using UnityEngine;

public class TestScript : NativeBehaviourScript
{
    // Created a nativefunc snippet to create the boiler-plate
    // the UNITY_EDITOR compiled code allows for re-compiling the native
    // lib without restarting the editor.

    // Example of returning a new value-type struct
#if UNITY_EDITOR
    public delegate Vector2 test_return_vec2_Delegate();

    [NativeDelegate]
    public static test_return_vec2_Delegate test_return_vec2;
#else
    [DllImport("rust_native")]
    public static extern Vector2 test_return_vec2();
#endif

    // Example of passing a struct by-ref
#if UNITY_EDITOR
    public delegate void test_ref_vec3_Delegate(ref Vector3 v);

    [NativeDelegate]
    public static test_ref_vec3_Delegate test_ref_vec3;
#else
    [DllImport("rust_native")]
    public static extern void test_ref_vec3(ref Vector3 v);
#endif

    // Example of passing an array of structs by-ref
#if UNITY_EDITOR
    public delegate void test_ref_value_array_Delegate(Vector3[] arr, int size);

    [NativeDelegate]
    public static test_ref_value_array_Delegate test_ref_value_array;
#else
    [DllImport("rust_native")]
    public static extern void test_ref_value_array(Vector3[] arr, int size);
#endif

    public override void Awake()
    {
        base.Awake();
    }

    public override void Start()
    {
        base.Start();

        var v2 = test_return_vec2();
        Debug.Log($"Vec2 Test - {v2}");

        var v3 = Vector3.zero;
        Debug.Log($"Vec3 ref Test: before {v3}");
        test_ref_vec3(ref v3);
        Debug.Log($"Vec3 ref Test: after {v3}");

        var array = new Vector3[10];
        Debug.Log($"Array ref Test: before {array[4]}");
        test_ref_value_array(array, array.Length);
        Debug.Log($"Array ref Test: after {array[4]}");
    }

    public override void Update()
    {
        base.Update();
    }

    public void OnCallback(int val)
    {
        Debug.Log(val);
    }
}