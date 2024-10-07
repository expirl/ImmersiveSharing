Shader "Unlit/TransparentHeatMap"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
        _Radius("Radius", Float) = 1.0
        _Transparency("Mesh Transparency", Range(0, 1)) = 1.0 // �޽��� ������ �����ϴ� ������Ƽ
        _HeatmapAlpha("Heatmap Alpha", Range(0, 1)) = 1.0 // ��Ʈ���� ���İ��� �����ϴ� ������Ƽ
    }
        SubShader
        {
            Tags { "Queue" = "Transparent" "RenderType" = "Transparent" } // RenderType�� Queue�� Transparent�� ����
            LOD 100

            Blend SrcAlpha OneMinusSrcAlpha // ���� ���� ����
            ZWrite Off // ZWrite�� Off�� �����Ͽ� ������ ������Ʈ�� �ùٸ��� �������ǵ��� ��

            Pass
            {
                CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag
                #pragma multi_compile_fog

                #include "UnityCG.cginc"

                struct appdata
                {
                    float4 vertex : POSITION;
                    float2 uv : TEXCOORD0;
                };

                struct v2f
                {
                    float2 uv : TEXCOORD0;
                    UNITY_FOG_COORDS(1)
                    float4 vertex : SV_POSITION;
                };

                sampler2D _MainTex;
                float4 _MainTex_ST;
                float _Radius;
                float _Transparency; // �޽��� ������ �����ϴ� ������Ƽ ����
                float _HeatmapAlpha; // ��Ʈ���� ���İ��� �����ϴ� ������Ƽ ����

                v2f vert(appdata v)
                {
                    v2f o;
                    o.vertex = UnityObjectToClipPos(v.vertex);
                    o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                    UNITY_TRANSFER_FOG(o,o.vertex);
                    return o;
                }

                float4 colors[5];
                float pointranges[5];

                float _Hits[3 * 32];
                int _HitCount = 0;

                void init()
                {
                    colors[0] = float4(0, 0, 0, 0);
                    colors[1] = float4(0, .9, .2, 1);
                    colors[2] = float4(.9, 1, .3, 1);
                    colors[3] = float4(.9, .7, .1, 1);
                    colors[4] = float4(1, 0, 0, 1);

                    pointranges[0] = 0;
                    pointranges[1] = 0.25;
                    pointranges[2] = 0.50;
                    pointranges[3] = 0.75;
                    pointranges[4] = 1.0;
                }

                float distsq(float2 a, float2 b)
                {
                    return pow(max(0.0, 1.0 - distance(a, b) / _Radius), 2.0);
                }

                float3 getHeatForPixel(float weight)
                {
                    if (weight <= pointranges[0])
                    {
                        return colors[0];
                    }
                    if (weight >= pointranges[4])
                    {
                        return colors[4];
                    }
                    for (int i = 1; i < 5; i++)
                    {
                        if (weight < pointranges[i])
                        {
                            float dist_from_lower_point = weight - pointranges[i - 1];
                            float size_of_point_range = pointranges[i] - pointranges[i - 1];
                            float ratio_over_lower_point = dist_from_lower_point / size_of_point_range;
                            float3 color_range = colors[i] - colors[i - 1];
                            float3 color_contribution = color_range * ratio_over_lower_point;
                            float3 new_color = colors[i - 1] + color_contribution;
                            return new_color;
                        }
                    }
                    return colors[0];
                }

                fixed4 frag(v2f i) : SV_Target
                {
                    init();
                    fixed4 col = tex2D(_MainTex, i.uv);
                    float2 uv = i.uv;
                    uv = uv * 4.0 - float2(2.0, 2.0);

                    float totalWeight = 0;
                    for (float j = 0; j < _HitCount; j++)
                    {
                        float2 work_pt = float2(_Hits[j * 3 + 0], _Hits[j * 3 + 1]);
                        float pt_intensity = _Hits[j * 3 + 2];
                        totalWeight += 0.5 * distsq(uv, work_pt) * pt_intensity;
                    }

                    float3 heat = getHeatForPixel(totalWeight);

                    // �޽��� ���İ��� _Transparency��, ��Ʈ���� _HeatmapAlpha�� ó��
                    float meshAlpha = _Transparency; // �޽��� ������ ������ ����
                    float heatmapAlpha = totalWeight > 0 ? _HeatmapAlpha : 0; // ��Ʈ���� ������ ��Ʈ�ʿ��� ����

                    // ���� ���: �޽��� ���İ��� ��Ʈ���� ���İ��� �и�
                    return float4(heat, heatmapAlpha) + col * float4(1, 1, 1, meshAlpha);
                }
                ENDCG
            }
        }
}