#if UNITY_IOS

using System;
using System.IO;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.iOS.Xcode;
using UnityEngine;

public class iOSPostProccessorBuild
{
	[PostProcessBuild(2000)]
	public static void OnPostProcessBuild(BuildTarget target, string path)
	{
		Debug.Log("iOSBuildPostProcess is now postprocessing iOS Project");

		var projectPath = PBXProject.GetPBXProjectPath(path);

		var project = new PBXProject();
		project.ReadFromFile(projectPath);

		var targetGuid = project.GetUnityMainTargetGuid();

		project.SetBuildProperty(targetGuid, "SUPPORTS_MAC_DESIGNED_FOR_IPHONE_IPAD", "NO");

		try
		{
			var projectInString = File.ReadAllText(projectPath);
			projectInString = projectInString.Replace("SUPPORTS_MAC_DESIGNED_FOR_IPHONE_IPAD = YES;",
				$"SUPPORTS_MAC_DESIGNED_FOR_IPHONE_IPAD = NO;");
			File.WriteAllText(projectPath, projectInString);
		}
		catch (Exception e)
		{
			Debug.LogException(e);
		}

		project.WriteToFile(projectPath);
	}
}

#endif
