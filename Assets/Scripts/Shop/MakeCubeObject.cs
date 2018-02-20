using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class MakeCubeObject  {
    #if UNITY_EDITOR
    [MenuItem("Assets/Create/CubeObject")]
    public static void Create()
    {
        CubeModel asset = ScriptableObject.CreateInstance<CubeModel>();
        AssetDatabase.CreateAsset(asset, "Assets/NewCubeObject.asset");
        AssetDatabase.SaveAssets();
        EditorUtility.FocusProjectWindow();
        Selection.activeObject = asset;
    }
    #endif
}
