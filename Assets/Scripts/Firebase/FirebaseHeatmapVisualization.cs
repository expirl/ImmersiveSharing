using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// MeshRenderer와 x, y 좌표 데이터를 함께 저장할 구조체
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

    public float collectionInterval = 1.0f; // 데이터 수집 간격 (초)
    public int maxDataCount = 300; // 최대 저장 데이터 수
    private float timeSinceLastCollection = 0.0f;

    public Button showHeatMapButton;

  //  private List<Vector2> collectedData = new List<Vector2>();
    float dataX = 0;
    float dataY = 0;

    // CollectedData 리스트 선언
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

        // GetTextureCoord() 호출 및 null 체크
        Vector2? textureCoord = headGazeTracking.GetTextureCoord();
        if (textureCoord.HasValue)
        {
            dataX = textureCoord.Value.x;
            dataY = textureCoord.Value.y;
            currentSavedMeshRenderer = headGazeTracking.GetMeshRenderer();
           // MeshRenderer currentSavedMeshRenderer = headGazeTracking.GetMeshRenderer();

            // 데이터 처리
            Debug.Log($"Collected Data - X: {dataX}, Y: {dataY}");
        }
        else
        {
            Debug.Log("No texture coordinate data available.");
        }
       

        // MeshRenderer가 null인지 확인하여 오류 방지
        if (currentSavedMeshRenderer == null)
        {
            Debug.LogWarning("No object is being tracked. MeshRenderer is null.");
            return;
        }

        // 데이터 저장
        if (collectedData.Count >= maxDataCount)
        {
            collectedData.RemoveAt(0); // 가장 오래된 데이터 삭제
        }
        // 새로운 CollectedData 객체를 리스트에 추가
        collectedData.Add(new CollectedData(new Vector2(dataX, dataY), currentSavedMeshRenderer));

        // Debug.Log($"Collected Data - X: {dataX}, Y: {dataY}");

        // MeshRenderer가 속한 오브젝트의 이름 출력
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
            // headGazeHeatmap에서 적절한 MeshRenderer에 기반한 x, y 좌표 시각화
            headGazeHeatmap.addHitPoint(data.coords.x, data.coords.y, data.meshRenderer);
        }
    }
}
