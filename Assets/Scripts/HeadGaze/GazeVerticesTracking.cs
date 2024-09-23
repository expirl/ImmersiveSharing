using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GazeVerticesTracking : MonoBehaviour
{
    public Transform headTransform; // VR 헤드셋의 Transform을 연결합니다.
    public float gazeDistance = 10.0f; // 주시 거리를 설정합니다.
    public LayerMask interactableLayer; // 상호작용 가능한 레이어를 설정합니다.
    public float activationTime = 0.5f; // 상호작용을 활성화하는 데 필요한 시간을 설정합니다.
    public LineRenderer lineRenderer; // LineRenderer 컴포넌트를 연결합니다.

    private float gazeTimer = 0.0f; // 주시 타이머를 초기화합니다.
    private GameObject currentGazedObject = null; // 현재 주시 중인 오브젝트를 추적합니다.

    public float forwardOffset = 0.1f; // Ray의 시작점을 앞쪽으로 이동시키기 위한 오프셋 값

    private int headGazeHeatmapObject;

    HeadGazeHeatmap headGazeHeatmap;
    RaycastHit hit;
    MeshRenderer currentMeshRenderer = null;


 
    void Start()
    {
        headGazeHeatmapObject = LayerMask.NameToLayer("HeadGazeHeatmapObject");
        headGazeHeatmap = GetComponent<HeadGazeHeatmap>();
        GazeRayVisualization();
    }

    void GazeRayVisualization()
    {
        // LineRenderer 초기화
        if (lineRenderer == null)
        {
            lineRenderer = gameObject.AddComponent<LineRenderer>();
        }
        lineRenderer.startWidth = 0.01f; // 선의 시작 부분 너비를 설정합니다.
        lineRenderer.endWidth = 0.01f; // 선의 끝 부분 너비를 설정합니다.
        lineRenderer.material = new Material(Shader.Find("Sprites/Default")); // 기본 셰이더를 사용합니다.
        lineRenderer.startColor = Color.blue; // 선의 시작 부분 색상을 설정합니다.
        lineRenderer.endColor = Color.blue; // 선의 끝 부분 색상을 설정합니다.
    }


    void Update()
    {
        // 헤드의 방향과 위치를 기반으로 Ray를 생성합니다.
        Vector3 offsetPosition = headTransform.position + headTransform.forward * forwardOffset;

        //Ray gazeRay = new Ray(headTransform.position, headTransform.forward);
        Ray gazeRay = new Ray(offsetPosition, headTransform.forward);
        // RaycastHit hit;

        // LineRenderer를 사용하여 Ray를 시각화합니다.
        lineRenderer.SetPosition(0, gazeRay.origin);
        lineRenderer.SetPosition(1, gazeRay.origin + gazeRay.direction * gazeDistance);

        // Raycast를 사용해 주시 중인 오브젝트를 감지합니다.
        if (Physics.Raycast(gazeRay, out hit, gazeDistance, interactableLayer))
        {
            MeshCollider meshCollider = hit.collider as MeshCollider;
            if (meshCollider == null || meshCollider.sharedMesh == null)
            {
                Debug.LogWarning("Null Mesh Collider");
                return;
            }

            GameObject gazedObject = hit.collider.gameObject;

            if (gazedObject == currentGazedObject)
            {
                gazeTimer += Time.deltaTime;

                // 주시 시간이 설정된 시간을 넘으면 상호작용을 활성화합니다.
                if (gazeTimer >= activationTime)
                {
                    TestDrawVertices(meshCollider);
                  //  Debug.Log("test");
                }
            }

            // 새로운 오브젝트를 주시하기 시작한 경우 타이머를 초기화합니다.
            else
            {
                currentGazedObject = gazedObject;
                gazeTimer = 0.0f;
            }

        }

        // 주시 중인 오브젝트가 없으면 타이머를 초기화합니다.
        else
        {
            currentGazedObject = null;
            gazeTimer = 0.0f;
        }

     }

    void TestDrawVertices(MeshCollider gazeMeshCollider)
    {

        Mesh mesh = gazeMeshCollider.sharedMesh;
        Vector3[] vertices = mesh.vertices;
        int[] triangles = mesh.triangles;
        Vector3 p0 = vertices[triangles[hit.triangleIndex * 3 + 0]];
        Vector3 p1 = vertices[triangles[hit.triangleIndex * 3 + 1]];
        Vector3 p2 = vertices[triangles[hit.triangleIndex * 3 + 2]];
        Transform hitTransform = hit.collider.transform;

        // Transform을 적용하여 월드 좌표로 변환
        p0 = hitTransform.TransformPoint(p0);
        p1 = hitTransform.TransformPoint(p1);
        p2 = hitTransform.TransformPoint(p2);

        // 삼각형의 중심점 구하기
        Vector3 center = (p0 + p1 + p2) / 3;

        // 각 정점을 중심점으로부터 일정 비율로 확장
        float scaleFactor = 10f; // 삼각형을 1.5배 크게 시각화
        p0 = center + (p0 - center) * scaleFactor;
        p1 = center + (p1 - center) * scaleFactor;
        p2 = center + (p2 - center) * scaleFactor;

        // 삼각형 시각화
        Debug.DrawLine(p0, p1, Color.red);
        Debug.DrawLine(p1, p2, Color.red);
        Debug.DrawLine(p2, p0, Color.red);

        Debug.Log("DrawVertices");


    }

}
