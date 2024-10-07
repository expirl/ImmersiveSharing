using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadGazeHeatmap : MonoBehaviour
{
    private float[] mPoints;
    private int mHitCount;

    void Start()
    {
        mPoints = new float[32 * 3]; // 32 points
    }

    public void addHitPoint(float xp, float yp, MeshRenderer hitRenderer = null)
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
          ///  hitMaterial.SetFloat("_Radius", radius);  // 반경 값 반영
        }
    }
}
