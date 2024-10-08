using System.Collections;
using System.Collections.Generic;
using TS.GazeInteraction;
using UnityEngine;
using UnityEngine.UI;

// MeshRenderer�� x, y ��ǥ �����͸� �Բ� ������ ����ü
class CollectedData
{
    public Vector2 coords;
    public MeshRenderer meshRenderer;

    public CollectedData(Vector2 coords, MeshRenderer meshRenderer)
    {
        this.coords = coords;
        this.meshRenderer = meshRenderer;
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

    public Button showHeatMapButton;

    List<CollectedData> collectedData = new List<CollectedData>();
    private MeshRenderer currentSavedMeshRenderer;

    void Start()
    {
        headGazeHeatmap = GetComponent<HeadGazeHeatmap>();
        collectButton.onClick.AddListener(ToggleDataCollection);
        showHeatMapButton.onClick.AddListener(BtnShowHeatMap);
    }

    void Update()
    {
        if (isCollecting)
        {
            timeSinceLastCollection += Time.deltaTime;

            // collectionInterval (������ ���� ����) ��ŭ �����͸� ���� �ϰڴ�. 
            // �̴� ���� �ֱ� ���� �ð��� �����ϰ� �����Ѵ�.
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
            // ���ο� ������ ���� ���� �� ���� �����͸� ���� �ϱ� ����.
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
            currentSavedMeshRenderer = gazeInteractor.GetMeshRenderer();

            if (currentSavedMeshRenderer == null)
            {
                Debug.LogWarning("No object is being tracked. MeshRenderer is null.");
                return;
            }

            collectedData.Add(new CollectedData(new Vector2(dataX, dataY), currentSavedMeshRenderer));

            // �ִ� ������ �� ����
            if (collectedData.Count > maxDataCount)
            {
                collectedData.RemoveAt(0);
            }

            string objectName = currentSavedMeshRenderer.gameObject.name;
            Debug.Log($"Collected Data - X: {dataX}, Y: {dataY}, Object Name: {objectName}, MeshRenderer saved.");
        }
        else
        {
            Debug.Log("No texture coordinate data available.");
        }
    }

    void BtnShowHeatMap()
    {
        if (collectedData.Count == 0)
        {
            Debug.LogWarning("No HeatMap Data available");
            return;
        }

        foreach (CollectedData data in collectedData)
        {
            headGazeHeatmap.AddHitPoint(data.coords.x, data.coords.y, data.meshRenderer);
        }
        Debug.Log($"Heatmap generated with {collectedData.Count} data points.");
    }
}
