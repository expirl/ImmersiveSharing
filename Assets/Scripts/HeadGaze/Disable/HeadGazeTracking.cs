using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.UI;
using Color = UnityEngine.Color;

public class HeadGazeTracking : MonoBehaviour
{
    public Transform headTransform; // VR ������ Transform�� �����մϴ�.
    public float gazeDistance = 10.0f; // �ֽ� �Ÿ��� �����մϴ�.
    public LayerMask interactableLayer; // ��ȣ�ۿ� ������ ���̾ �����մϴ�.
    public float activationTime = 0.5f; // ��ȣ�ۿ��� Ȱ��ȭ�ϴ� �� �ʿ��� �ð��� �����մϴ�.
    public LineRenderer lineRenderer; // LineRenderer ������Ʈ�� �����մϴ�.

    private float gazeTimer = 0.0f; // �ֽ� Ÿ�̸Ӹ� �ʱ�ȭ�մϴ�.
    private GameObject currentGazedObject = null; // ���� �ֽ� ���� ������Ʈ�� �����մϴ�.

    public float forwardOffset = 0.1f; // Ray�� �������� �������� �̵���Ű�� ���� ������ ��

    private Color originalColor; // ���� ������ ������ �����Դϴ�.
    private int uiLayer;
    private int boxObjectLayer;
    private int headGazeHeatmapObject;


    HeadGazeHeatmap headGazeHeatmap;
    RaycastHit hit;
    MeshRenderer currentMeshRenderer = null;

    void Start()
    {

        uiLayer = LayerMask.NameToLayer("UI");
        boxObjectLayer = LayerMask.NameToLayer("Box");
        headGazeHeatmapObject = LayerMask.NameToLayer("HeadGazeHeatmapObject");

        headGazeHeatmap = GetComponent<HeadGazeHeatmap>();

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
           
            // �ֽ� ������Ʈ�� �ٲ��� ���� ��� Ÿ�̸Ӹ� ������ŵ�ϴ�.
            if (gazedObject == currentGazedObject)
            {
                gazeTimer += Time.deltaTime;

                // �ֽ� �ð��� ������ �ð��� ������ ��ȣ�ۿ��� Ȱ��ȭ�մϴ�.
                if (gazeTimer >= activationTime)
                {
                    //gazedObject.GetComponent<Renderer>().material.color = Color.red;
                    ActivateGazedObject(gazedObject, gazedObject.layer);
                    //TestDrawVertices(meshCollider);
                    gazeTimer = 0.0f; // Ÿ�̸Ӹ� �ʱ�ȭ�մϴ�.
                    //Debug.Log("test");

                }
            }

            else
            {
                // ���ο� ������Ʈ�� �ֽ��ϱ� ������ ��� ���� �������� �����ϰ� Ÿ�̸Ӹ� �ʱ�ȭ�մϴ�.
                ResetGazeObjectColor();
                currentGazedObject = gazedObject;
                gazeTimer = 0.0f;

                if(gazedObject.layer != headGazeHeatmapObject)
                {
                    originalColor = gazedObject.GetComponent<Renderer>().material.color;
                }
                
            }
        }

        else
        {
            // �ֽ� ���� ������Ʈ�� ������ ���� �������� �����ϰ� Ÿ�̸Ӹ� �ʱ�ȭ�մϴ�.
            ResetGazeObjectColor();
            currentGazedObject = null;
            gazeTimer = 0.0f;
        }
    }

   
    // �ֽ��� ������Ʈ�� ������ �����ϴ� �޼����Դϴ�.
    void ActivateGazedObject(GameObject gazedObject, int getLayerName)
    {
        if(getLayerName == uiLayer)
        {
            // �ֽõ� ������Ʈ���� Button ������Ʈ�� ã���ϴ�.
            var button = gazedObject.GetComponent<Button>();
            if (button != null)
            {
                // ��ư�� ������ �����մϴ�.
                ColorBlock colors = button.colors;
                colors.normalColor = Color.red; // ���ϴ� �������� �����մϴ�. ���⼭�� �������� ���� ������ϴ�.
                button.colors = colors;
            }
        }

        else if(getLayerName == boxObjectLayer)
        {
            var renderer = gazedObject.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.material.color = Color.green;
            }
        }

        else if(getLayerName == headGazeHeatmapObject)
        {
            // GetTextureCoord();
            var meshRenderer = gazedObject.GetComponent<MeshRenderer>();
            if (meshRenderer != null)
            {
                headGazeHeatmap.AddHitPoint(hit.textureCoord.x * 4 - 2, hit.textureCoord.y * 4 - 2, meshRenderer);
                currentMeshRenderer = meshRenderer;
            }
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
        p0 = hitTransform.TransformPoint(p0);
        p1 = hitTransform.TransformPoint(p1);
        p2 = hitTransform.TransformPoint(p2);
        Debug.DrawLine(p0, p1, Color.red);
        Debug.DrawLine(p1, p2, Color.red);
        Debug.DrawLine(p2, p0, Color.red);
        Debug.Log("DrawVertices");
    }


    public Vector2? GetTextureCoord()
    {
        if (hit.textureCoord.x != 0 && hit.textureCoord.y != 0)
        {
            return new Vector2(hit.textureCoord.x * 4 - 2, hit.textureCoord.y * 4 - 2);
        }
        else
        {
            return null; // �����Ͱ� ���� ��� null ��ȯ
        }
    }


    public MeshRenderer GetMeshRenderer()
    {
        return currentMeshRenderer;
    }


    // ������ �ֽ��� ������Ʈ�� ������ ������� �����ϴ� �޼����Դϴ�.
    private void ResetGazeObjectColor()
    {
        if (currentGazedObject != null)
        {
            var render = currentGazedObject.GetComponent<Renderer>();
            if (render != null)
            {
                render.material.color = originalColor;
            }
        }
    }

}