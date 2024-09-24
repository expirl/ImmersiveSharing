using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GazeVerticesTracking : MonoBehaviour
{
    public Transform headTransform; // VR ������ Transform�� �����մϴ�.
    public float gazeDistance = 10.0f; // �ֽ� �Ÿ��� �����մϴ�.
    public LayerMask interactableLayer; // ��ȣ�ۿ� ������ ���̾ �����մϴ�.
    public float activationTime = 0.5f; // ��ȣ�ۿ��� Ȱ��ȭ�ϴ� �� �ʿ��� �ð��� �����մϴ�.
    public LineRenderer lineRenderer; // LineRenderer ������Ʈ�� �����մϴ�.

    private float gazeTimer = 0.0f; // �ֽ� Ÿ�̸Ӹ� �ʱ�ȭ�մϴ�.
    private GameObject currentGazedObject = null; // ���� �ֽ� ���� ������Ʈ�� �����մϴ�.

    public float forwardOffset = 0.1f; // Ray�� �������� �������� �̵���Ű�� ���� ������ ��

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
        // LineRenderer �ʱ�ȭ
        if (lineRenderer == null)
        {
            lineRenderer = gameObject.AddComponent<LineRenderer>();
        }
        lineRenderer.startWidth = 0.01f; // ���� ���� �κ� �ʺ� �����մϴ�.
        lineRenderer.endWidth = 0.01f; // ���� �� �κ� �ʺ� �����մϴ�.
        lineRenderer.material = new Material(Shader.Find("Sprites/Default")); // �⺻ ���̴��� ����մϴ�.
        lineRenderer.startColor = Color.blue; // ���� ���� �κ� ������ �����մϴ�.
        lineRenderer.endColor = Color.blue; // ���� �� �κ� ������ �����մϴ�.
    }


    void Update()
    {
        // ����� ����� ��ġ�� ������� Ray�� �����մϴ�.
        Vector3 offsetPosition = headTransform.position + headTransform.forward * forwardOffset;

        //Ray gazeRay = new Ray(headTransform.position, headTransform.forward);
        Ray gazeRay = new Ray(offsetPosition, headTransform.forward);
        // RaycastHit hit;

        // LineRenderer�� ����Ͽ� Ray�� �ð�ȭ�մϴ�.
        lineRenderer.SetPosition(0, gazeRay.origin);
        lineRenderer.SetPosition(1, gazeRay.origin + gazeRay.direction * gazeDistance);

        // Raycast�� ����� �ֽ� ���� ������Ʈ�� �����մϴ�.
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

                // �ֽ� �ð��� ������ �ð��� ������ ��ȣ�ۿ��� Ȱ��ȭ�մϴ�.
                if (gazeTimer >= activationTime)
                {
                    TestDrawVertices(meshCollider, gazedObject);
                   // gazeTimer = 0.0f;
                  //  Debug.Log("test");
                }
            }

            // ���ο� ������Ʈ�� �ֽ��ϱ� ������ ��� Ÿ�̸Ӹ� �ʱ�ȭ�մϴ�.
            else
            {
                currentGazedObject = gazedObject;
                gazeTimer = 0.0f;
            }

        }

        // �ֽ� ���� ������Ʈ�� ������ Ÿ�̸Ӹ� �ʱ�ȭ�մϴ�.
        else
        {
            currentGazedObject = null;
            gazeTimer = 0.0f;
        }

     }

    void TestDrawVertices(MeshCollider gazeMeshCollider, GameObject gazeObject)
    {
        // Mesh �� Vertices ��������
        Mesh mesh = gazeMeshCollider.sharedMesh;
        Vector3[] vertices = mesh.vertices;
        int[] triangles = mesh.triangles;

        // �ﰢ���� �� ���� ��������
        Vector3 p0 = vertices[triangles[hit.triangleIndex * 3 + 0]];
        Vector3 p1 = vertices[triangles[hit.triangleIndex * 3 + 1]];
        Vector3 p2 = vertices[triangles[hit.triangleIndex * 3 + 2]];
        Transform hitTransform = hit.collider.transform;

        // Transform �����Ͽ� ���� ��ǥ�� ��ȯ
        p0 = hitTransform.TransformPoint(p0);
        p1 = hitTransform.TransformPoint(p1);
        p2 = hitTransform.TransformPoint(p2);

        // �ﰢ���� �߽��� ���ϱ�
        Vector3 center = (p0 + p1 + p2) / 3;

        // �ﰢ�� ���� ��� (��Ʈ�� �ݰ濡 ����� ũ��)
        float area = Vector3.Cross(p1 - p0, p2 - p0).magnitude / 2f;
        float radius = Mathf.Sqrt(area);  // ������ ����� �ݰ� ����

        // UV ��ǥ ��������
        Vector2[] uvs = mesh.uv;
        Vector2 uv0 = uvs[triangles[hit.triangleIndex * 3 + 0]];
        Vector2 uv1 = uvs[triangles[hit.triangleIndex * 3 + 1]];
        Vector2 uv2 = uvs[triangles[hit.triangleIndex * 3 + 2]];

        // �ﰢ�� �߽����� UV ��ǥ ���
        Vector2 centerUV = (uv0 + uv1 + uv2) / 3;

        // �߽����� UV ��ǥ ������� ��Ʈ�� Ȱ��ȭ
         ActivateGazeHeatMap(centerUV.x, centerUV.y, gazeObject, 1.0f);

        // �ﰢ�� �ð�ȭ - �� ������ �߽������κ��� ���� ������ Ȯ��
        float scaleFactor = 10f;  // �ﰢ���� 10�� ũ�� �ð�ȭ
        p0 = center + (p0 - center) * scaleFactor;
        p1 = center + (p1 - center) * scaleFactor;
        p2 = center + (p2 - center) * scaleFactor;

        // �ﰢ�� �ð�ȭ
        Debug.DrawLine(p0, p1, Color.green);
        Debug.DrawLine(p1, p2, Color.green);
        Debug.DrawLine(p2, p0, Color.green);

    }

    void ActivateGazeHeatMap(float xp, float yp, GameObject gazeObject, float radius)
    {
        var meshRenderer = gazeObject.GetComponent<MeshRenderer>();
        if (meshRenderer != null)
        {
            headGazeHeatmap.addHitPoint(xp, yp, meshRenderer, radius);
        }
    }

}
