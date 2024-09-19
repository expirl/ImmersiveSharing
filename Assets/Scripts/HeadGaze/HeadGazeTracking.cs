using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.UI;
using Color = UnityEngine.Color;

public class HeadGazeTracking : MonoBehaviour
{
    public Transform headTransform; // VR 헤드셋의 Transform을 연결합니다.
    public float gazeDistance = 10.0f; // 주시 거리를 설정합니다.
    public LayerMask interactableLayer; // 상호작용 가능한 레이어를 설정합니다.
    public float activationTime = 0.5f; // 상호작용을 활성화하는 데 필요한 시간을 설정합니다.
    public LineRenderer lineRenderer; // LineRenderer 컴포넌트를 연결합니다.

    private float gazeTimer = 0.0f; // 주시 타이머를 초기화합니다.
    private GameObject currentGazedObject = null; // 현재 주시 중인 오브젝트를 추적합니다.

    public float forwardOffset = 0.1f; // Ray의 시작점을 앞쪽으로 이동시키기 위한 오프셋 값

    private Color originalColor; // 원래 색상을 저장할 변수입니다.
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
            GameObject gazedObject = hit.collider.gameObject;
           
            // 주시 오브젝트가 바뀌지 않은 경우 타이머를 증가시킵니다.
            if (gazedObject == currentGazedObject)
            {
                gazeTimer += Time.deltaTime;

                // 주시 시간이 설정된 시간을 넘으면 상호작용을 활성화합니다.
                if (gazeTimer >= activationTime)
                {
                    ActivateGazedObject(gazedObject, gazedObject.layer);
                    // gazeTimer = 0.0f; // 타이머를 초기화합니다.

                }
            }

            else
            {
                // 새로운 오브젝트를 주시하기 시작한 경우 원래 색상으로 복원하고 타이머를 초기화합니다.
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
            // 주시 중인 오브젝트가 없으면 원래 색상으로 복원하고 타이머를 초기화합니다.
            ResetGazeObjectColor();
            currentGazedObject = null;
            gazeTimer = 0.0f;
        }
    }

   
    // 주시한 오브젝트의 색상을 변경하는 메서드입니다.
    void ActivateGazedObject(GameObject gazedObject, int getLayerName)
    {
        if(getLayerName == uiLayer)
        {
            // 주시된 오브젝트에서 Button 컴포넌트를 찾습니다.
            var button = gazedObject.GetComponent<Button>();
            if (button != null)
            {
                // 버튼의 색상을 변경합니다.
                ColorBlock colors = button.colors;
                colors.normalColor = Color.red; // 원하는 색상으로 변경합니다. 여기서는 빨간색을 예로 들었습니다.
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
                headGazeHeatmap.addHitPoint(hit.textureCoord.x * 4 - 2, hit.textureCoord.y * 4 - 2, meshRenderer);
                currentMeshRenderer = meshRenderer;
            }
        }
    }

    public Vector2 GetTextureCoord()
    {
        if (hit.textureCoord.x != 0 && hit.textureCoord.y != 0)
        {
            return new Vector2(hit.textureCoord.x * 4 - 2, hit.textureCoord.y * 4 - 2);
        }
        else
        {
            return Vector2.zero; // 데이터가 없음을 나타내는 기본값 반환
        }
    }


    public MeshRenderer GetMeshRenderer()
    {
        return currentMeshRenderer;
    }


    // 이전에 주시한 오브젝트의 색상을 원래대로 복원하는 메서드입니다.
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