using System.Runtime.InteropServices;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    // Created a nativefunc snippet to create the boiler-plate
    // the UNITY_EDITOR compiled code allows for re-compiling the native
    // lib without restarting the editor.

    // Example of returning a new value-type struct
    [DllImport("rust_native")]
    public static extern Vector2 test_return_vec2();

    // Example of passing a struct by-ref
    [DllImport("rust_native")]
    public static extern void test_ref_vec3(ref Vector3 v);

    // Example of passing an array of structs by-ref
    [DllImport("rust_native")]
    public static extern void test_ref_value_array(Vector3[] arr, int size);

    public void Start()
    {
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
}