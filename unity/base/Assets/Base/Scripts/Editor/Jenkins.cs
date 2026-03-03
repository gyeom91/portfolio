using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class Jenkins
{
    public static void WindowBuild()
    {
        var projectName = Application.productName;
        var timeStamp = DateTime.Now.ToString("yyyy-MM-dd_HHmmss");
        var projectPath = Application.dataPath;
        var projectParentDir = Path.GetDirectoryName(Path.GetDirectoryName(projectPath));
        var buildDir = Path.Combine(projectParentDir, "builds", timeStamp);
        if (Directory.Exists(buildDir) == false)
        {
            Debug.Log($"빌드 경로 : {buildDir}");

            Directory.CreateDirectory(buildDir);
        }

        var executableName = projectName + ".exe";
        var buildPath = Path.Combine(buildDir, executableName);
        var scenePaths = EditorBuildSettings.scenes
            .Where(s => s.enabled)
            .Select(s => s.path)
            .ToArray();
        var buildPlayerOptions = new BuildPlayerOptions();
        buildPlayerOptions.scenes = EditorBuildSettings.scenes.Where(s => s.enabled).Select(s => s.path).ToArray();
        buildPlayerOptions.locationPathName = buildPath;
        buildPlayerOptions.target = BuildTarget.StandaloneWindows64;
        buildPlayerOptions.options = BuildOptions.None;

        BuildPipeline.BuildPlayer(buildPlayerOptions);
    }
}
