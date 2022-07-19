using UnityEditor;

public class WebGLEditorScript
{
    [MenuItem("WebGL/Enable Embedded Resources")]
    [System.Obsolete]
    public static void EnableErrorMessageTesting()
    {
        PlayerSettings.SetPropertyBool("useEmbeddedResources", true, BuildTargetGroup.WebGL);
    }
}