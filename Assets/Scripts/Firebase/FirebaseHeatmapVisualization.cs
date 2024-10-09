/*
using System.Collections;
using System.Collections.Generic;
using TS.GazeInteraction;
using UnityEngine;
using UnityEngine.UI;
using Firebase;
using Firebase.Database;
using Firebase.Extensions;

// MeshRenderer�� x, y ��ǥ �����͸� �Բ� ������ ����ü
[System.Serializable]
public class CollectedData
{
    public Vector2 coords;
    public string meshRendererID;  // Firebase�� �����ϱ� ���� string Ÿ�� (MeshRenderer�� �̸� ���)

    public CollectedData(Vector2 coords, string meshRendererID)
    {
        this.coords = coords;
        this.meshRendererID = meshRendererID;
    }
}

public class FirebaseHeatmapVisualization : MonoBehaviour
{
    [SerializeField] private GazeInteractor gazeInteractor;
    HeadGazeHeatmap headGazeHeatmap;

    public Button collectButton;
    public Text collectText;
    private bool isCollecting = false;

    public float collectionInterval = 3.0f; // ������ ���� ���� (��)
    public int maxDataCount = 300;
    private float timeSinceLastCollection = 0.0f;

    public Button transferButton;
    public Button showHeatMapButton;

    List<CollectedData> collectedData = new List<CollectedData>();
   // private MeshRenderer currentSavedMeshRenderer;

    // Firebase ���� ����
    DatabaseReference databaseRef;

    void Start()
    {
        headGazeHeatmap = GetComponent<HeadGazeHeatmap>();
        collectButton.onClick.AddListener(ToggleDataCollection);
        transferButton.onClick.AddListener(TransferGazeData);
        showHeatMapButton.onClick.AddListener(BtnShowHeatMap);

        // Firebase �ʱ�ȭ
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task => {
            FirebaseApp app = FirebaseApp.DefaultInstance;
            databaseRef = FirebaseDatabase.DefaultInstance.RootReference;
        });
    }

    void Update()
    {
        if (isCollecting)
        {
            timeSinceLastCollection += Time.deltaTime;

            if (timeSinceLastCollection >= collectionInterval)
            {
                CollectData();
                timeSinceLastCollection = 0.0f;
            }
        }
    }

    void ToggleDataCollection()
    {
        isCollecting = !isCollecting;
        collectText.text = isCollecting ? "Stop Collecting" : "Start Collecting";

        if (isCollecting)
        {
            collectedData.Clear();
            Debug.Log("Starting new data collection. Previous data has been cleared.");
        }
        else
        {
            Debug.Log($"Data collection stopped. Total collected data points: {collectedData.Count}");
        }
    }

    void CollectData()
    {
        Vector2? textureCoord = gazeInteractor.GetTextureCoord();
        if (textureCoord.HasValue)
        {
            float dataX = textureCoord.Value.x;
            float dataY = textureCoord.Value.y;
            MeshRenderer currentSavedMeshRenderer = gazeInteractor.GetMeshRenderer();

            if (currentSavedMeshRenderer == null)
            {
                Debug.LogWarning("No object is being tracked. MeshRenderer is null.");
                return;
            }

            // MeshRenderer ������Ʈ�� �̸��� ID�� ����� Firebase�� ������ �غ�
            string objectID = currentSavedMeshRenderer.gameObject.name;
            collectedData.Add(new CollectedData(new Vector2(dataX, dataY), objectID));

            if (collectedData.Count > maxDataCount)
            {
                collectedData.RemoveAt(0);
            }

            Debug.Log($"Collected Data - X: {dataX}, Y: {dataY}, Object Name: {objectID}, MeshRenderer saved.");
        }
        else
        {
            Debug.Log("No texture coordinate data available.");
        }
    }

    // Firebase�� �����͸� �����ϴ� �Լ�
    void TransferGazeData()
    {
        if (collectedData.Count == 0)
        {
            Debug.LogWarning("No data to transfer.");
            return;
        }

        foreach (CollectedData data in collectedData)
        {
            string json = JsonUtility.ToJson(data);
            databaseRef.Child("gazeData").Push().SetRawJsonValueAsync(json).ContinueWithOnMainThread(task => {
                if (task.IsCompleted)
                {
                    Debug.Log("Data successfully transferred to Firebase.");
                }
                else
                {
                    Debug.LogWarning("Failed to transfer data to Firebase.");
                }
            });
        }
    }

    // Firebase���� �����͸� ������ Heatmap�� �����ִ� �Լ�
    void BtnShowHeatMap()
    {
        databaseRef.Child("gazeData").GetValueAsync().ContinueWithOnMainThread(task => {
            if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;

                // Firebase���� �ҷ��� �����͸� heatmap�� �߰�
                foreach (DataSnapshot data in snapshot.Children)
                {
                    string json = data.GetRawJsonValue();
                    CollectedData loadedData = JsonUtility.FromJson<CollectedData>(json);

                    // �ش� MeshRenderer ������Ʈ�� ã�� heatmap�� ����
                    MeshRenderer targetMeshRenderer = FindObjectByName(loadedData.meshRendererID);
                    if (targetMeshRenderer != null)
                    {
                        headGazeHeatmap.AddHitPoint(loadedData.coords.x, loadedData.coords.y, targetMeshRenderer);
                    }
                }

                Debug.Log("Heatmap successfully generated from Firebase data.");
            }
            else
            {
                Debug.LogWarning("Failed to load data from Firebase.");
            }
        });
    }

    // ������Ʈ �̸����� MeshRenderer ã��
    MeshRenderer FindObjectByName(string objectName)
    {
        GameObject targetObject = GameObject.Find(objectName);
        if (targetObject != null)
        {
            return targetObject.GetComponent<MeshRenderer>();
        }
        return null;
    }
}

*/
