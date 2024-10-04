using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TS.GazeInteraction
{
    public class TestGazeInteraction : MonoBehaviour
    {

        [SerializeField] HeadGazeHeatmap headGazeHeatmap;
        MeshRenderer currentMeshRenderer = null;

        RaycastHit currentHit;

        // �ֽ��� ������Ʈ�� ������ �����ϴ� �޼����Դϴ�.
       public void ActivateGazedObject(MeshRenderer gazedMeshRenderer, RaycastHit gazeHit)
        {
           
            if (gazedMeshRenderer != null)
            {
                currentMeshRenderer = gazedMeshRenderer;
                currentHit = gazeHit;
            }
        }



        public void GazeEneter()
        {
            Debug.Log("Gaze Enter");
        }

        public void GazeActive()
        {
           

            if (currentMeshRenderer != null)
            headGazeHeatmap.addHitPoint(currentHit.textureCoord.x * 4 - 2, currentHit.textureCoord.y * 4 - 2, currentMeshRenderer);
           // currentMeshRenderer = gazedMeshRenderer;

        }

        public void GazeExit()
        {
            Debug.Log("Gaze Exit");

        }
    }
}
