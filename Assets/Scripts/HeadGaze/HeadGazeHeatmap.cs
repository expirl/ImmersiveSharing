using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadGazeHeatmap : MonoBehaviour
{
    private Dictionary<MeshRenderer, HeatmapData> heatmaps = new Dictionary<MeshRenderer, HeatmapData>();

    private class HeatmapData
    {
        public float[] points;
        public int hitCount;

        public HeatmapData()
        {
            points = new float[32 * 3]; // 32 points
            hitCount = 0;
        }
    }


    //void Start()
    //{
    //    mPoints = new float[32 * 3]; // 32 points
    //}

    public void AddHitPoint(float xp, float yp, MeshRenderer hitRenderer)
    {
        if (hitRenderer == null) return;

        if (!heatmaps.ContainsKey(hitRenderer))
        {
            heatmaps[hitRenderer] = new HeatmapData();
        }

        HeatmapData data = heatmaps[hitRenderer];

        data.points[data.hitCount * 3] = xp;
        data.points[data.hitCount * 3 + 1] = yp;
        data.points[data.hitCount * 3 + 2] = Random.Range(1f, 3f);
        data.hitCount++;
        data.hitCount %= 32;

        UpdateShader(hitRenderer);
    }

    private void UpdateShader(MeshRenderer hitRenderer)
    {
        if (heatmaps.TryGetValue(hitRenderer, out HeatmapData data))
        {
            Material hitMaterial = hitRenderer.material;
            hitMaterial.SetFloatArray("_Hits", data.points);
            hitMaterial.SetInt("_HitCount", data.hitCount);
        }
    }

    public void ClearPoints(MeshRenderer hitRenderer = null)
    {
        if (hitRenderer != null)
        {
            if (heatmaps.ContainsKey(hitRenderer))
            {
                heatmaps[hitRenderer] = new HeatmapData();
                UpdateShader(hitRenderer);
            }
        }
        else
        {
            heatmaps.Clear();
        }
    }
}
