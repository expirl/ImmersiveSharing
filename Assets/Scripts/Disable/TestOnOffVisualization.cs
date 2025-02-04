using GaussianSplatting.Runtime;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class TestOnOffVisualization : MonoBehaviour
{
    // public TestGaussianAssetReceiver receiver;
    public GaussianSplatRenderer gaussian_receiver;


    private void Start()
    {
        if (gaussian_receiver == null)
        {
            Debug.LogError("ReceiverScript instance not found in the scene.");
            return;
        }
    }

    // [MenuItem("Tools/Find ScriptableObjects")]
    public void FindAllScriptableObjects()
    {
        string path = "Assets/GaussianAssets";
        string[] assetGuids = AssetDatabase.FindAssets("t:ScriptableObject", new[] { path });


        foreach (string guid in assetGuids)
        {
            /*
             * assetPath : Assets/GaussianAssets/room-point_cloud-iteration_30000-point_cloud.asset
             * scriptableObject : room-point_cloud-iteration_30000-point_cloud
             */
            string assetPath = AssetDatabase.GUIDToAssetPath(guid);
            ScriptableObject scriptableObject = AssetDatabase.LoadAssetAtPath<ScriptableObject>(assetPath);

            if (scriptableObject != null)
            {
                Debug.Log($"Found ScriptableObject: {scriptableObject.name} at path: {assetPath}");
                gaussian_receiver.SetScriptableObject(scriptableObject as GaussianSplatAsset);
            }
        }
    }
}
