using System.Collections;
using System.Collections.Generic;
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
    HeadGazeTracking headGazeTracking;
    HeadGazeHeatmap headGazeHeatmap;

    public Button collectButton;
    public Text collectText;
    private bool isCollecting = false;

    public float collectionInterval = 1.0f; // ������ ���� ���� (��)
    public int maxDataCount = 300; // �ִ� ���� ������ ��
    private float timeSinceLastCollection = 0.0f;

    public Button showHeatMapButton;

  //  private List<Vector2> collectedData = new List<Vector2>();
    float dataX = 0;
    float dataY = 0;

    // CollectedData ����Ʈ ����
    List<CollectedData> collectedData = new List<CollectedData>();
    private MeshRenderer currentSavedMeshRenderer;

    // Start is called before the first frame update
    void Start()
    {
        headGazeTracking = GetComponent<HeadGazeTracking>();
        headGazeHeatmap = GetComponent<HeadGazeHeatmap>();
        collectButton.onClick.AddListener(BtnDataCollection);    
        showHeatMapButton.onClick.AddListener(BtnShowHeatMap);
    }

    // Update is called once per frame
    void Update()
    {
        if(isCollecting)
        {
            timeSinceLastCollection += Time.deltaTime;

            if(timeSinceLastCollection >= collectionInterval)
            {
                CollectData();
                timeSinceLastCollection = 0.0f;
            }          
        }
    }


    public void BtnDataTransfer()
    {

    }

    void BtnDataCollection()
    {
        isCollecting = !isCollecting;
        collectText.text = isCollecting ? "Stop Collecting" : "Start Collecting";

        if (isCollecting) 
        {
            collectedData.Clear();
            Debug.Log("Collected data has been cleared.");
        }

    }


    void CollectData()
    {

        // GetTextureCoord() ȣ�� �� null üũ
        Vector2? textureCoord = headGazeTracking.GetTextureCoord();
        if (textureCoord.HasValue)
        {
            dataX = textureCoord.Value.x;
            dataY = textureCoord.Value.y;
            currentSavedMeshRenderer = headGazeTracking.GetMeshRenderer();
           // MeshRenderer currentSavedMeshRenderer = headGazeTracking.GetMeshRenderer();

            // ������ ó��
            Debug.Log($"Collected Data - X: {dataX}, Y: {dataY}");
        }
        else
        {
            Debug.Log("No texture coordinate data available.");
        }
       

        // MeshRenderer�� null���� Ȯ���Ͽ� ���� ����
        if (currentSavedMeshRenderer == null)
        {
            Debug.LogWarning("No object is being tracked. MeshRenderer is null.");
            return;
        }

        // ������ ����
        if (collectedData.Count >= maxDataCount)
        {
            collectedData.RemoveAt(0); // ���� ������ ������ ����
        }
        // ���ο� CollectedData ��ü�� ����Ʈ�� �߰�
        collectedData.Add(new CollectedData(new Vector2(dataX, dataY), currentSavedMeshRenderer));

        // Debug.Log($"Collected Data - X: {dataX}, Y: {dataY}");

        // MeshRenderer�� ���� ������Ʈ�� �̸� ���
        string objectName = currentSavedMeshRenderer.gameObject.name;

        Debug.Log($"Collected Data - X: {dataX}, Y: {dataY}, Object Name: {objectName}, MeshRenderer saved.");

    }


     void BtnShowHeatMap()
    {
        if (collectedData.Count == 0)
        {
            Debug.LogWarning("Not HeatMap Data");
            return;
        }

        foreach (CollectedData data in collectedData)
        {
            // headGazeHeatmap���� ������ MeshRenderer�� ����� x, y ��ǥ �ð�ȭ
            headGazeHeatmap.addHitPoint(data.coords.x, data.coords.y, data.meshRenderer);
        }
    }
}
