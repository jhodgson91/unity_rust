using UnityEngine;
using UnityEditor;
using System;

// Initialize On Load will mean the static ctor for this class
// is called on Start Up, and every time scripts are re-compiled
[InitializeOnLoad]
public class NativeRecompile
{
    private static Lazy<string> NativeDir = new Lazy<string>(() => System.IO.Directory.GetCurrentDirectory() + "/Native");

    [MenuItem("Assets/Recompile Native Code", false, 0)]
    private static void Run()
    {
        Debug.Log("Recompiling native..");

        foreach (var dir in System.IO.Directory.GetDirectories(NativeDir.Value))
        {
            var manifestPath = $"{dir}/Cargo.toml";

            if (System.IO.File.Exists(manifestPath))
            {
                using (var process = new System.Diagnostics.Process())
                {
                    var outDir = $"{Application.dataPath}/Plugins";
                    var startInfo = new System.Diagnostics.ProcessStartInfo();
                    process.StartInfo.RedirectStandardOutput = true;
                    process.StartInfo.RedirectStandardError = true;
                    process.StartInfo.UseShellExecute = false;
                    process.StartInfo.FileName = "cargo.exe";
                    process.StartInfo.Arguments = $"build --release --manifest-path {manifestPath} --out-dir {outDir} -Z unstable-options";

                    process.Start();
                    process.WaitForExit();

                    if (process.ExitCode > 0)
                    {
                        Debug.LogError($"Native Compile finished with return code {process.ExitCode}");
                        Debug.LogError(process.StandardError.ReadToEnd());
                    }
                }
            }
        }
    }
}
