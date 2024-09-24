using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadGazeHeatmap : MonoBehaviour
{
    [SerializeField] private MeshRenderer[] mMeshRenderers;

    private Material[] mMaterials;

    float[] mPoints;
    int mHitCount;

    // float mDelay;

    void Start()
    {
        //    mDelay = 3;

        // MeshRenderer�� ������ŭ Material �迭�� �ʱ�ȭ�մϴ�.
        mMaterials = new Material[mMeshRenderers.Length];

        // �� MeshRenderer���� Material�� �����ͼ� �迭�� �����մϴ�.
        for (int i = 0; i < mMeshRenderers.Length; i++)
        {
            mMaterials[i] = mMeshRenderers[i].material;
        }

        mPoints = new float[32 * 3]; //32 point 

    }



    // Update is called once per frame
    void Update()
    {
        
    }

   public void addHitPoint(float xp, float yp, MeshRenderer hitRenderer = null, float radius = 1.0f)
    {
        mPoints[mHitCount * 3] = xp;
        mPoints[mHitCount * 3 + 1] = yp;
        mPoints[mHitCount * 3 + 2] = Random.Range(1f, 3f);

        mHitCount++;
        mHitCount %= 32;

        if (hitRenderer != null)
        {
            Material hitMaterial = hitRenderer.material;
            hitMaterial.SetFloatArray("_Hits", mPoints);
            hitMaterial.SetInt("_HitCount", mHitCount);
            hitMaterial.SetFloat("_Radius", radius);  // �ݰ� �� �ݿ�
        }
    }

}
