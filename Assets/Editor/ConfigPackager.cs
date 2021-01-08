using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;
using UnityEditor.Callbacks;

class ConfigPackager : IPostprocessBuildWithReport
{
    public int callbackOrder { get { return 0; } }

    public void OnPostprocessBuild(BuildReport report)
    {
        string srcPath = Application.dataPath + Spawner.levelsDataPatch;
        string dstPath = System.IO.Directory.GetParent(report.summary.outputPath) + "/" + Application.productName + "_Data" + Spawner.levelsDataPatch;
        FileUtil.CopyFileOrDirectory(srcPath, dstPath);
    }
}
