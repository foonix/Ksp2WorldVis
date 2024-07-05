Shader "KSP2/Environment/CelestialBody/CelestialBody_Local_Overlay"
{
    Properties
    {
        [MainTexture] _OverlayTexture ("Overlay Texture", 2D) = "black" {}
        _Strength ("Strength", float) = 0.9
    }

    SubShader
    {
        Tags { "RenderType"="Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                uint vertexID : SV_VertexID;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            struct QuadMeshData
            {
                float3 Position;
				float3 Normal;
				float2 Uv;
				float4 Tangent;
				float4 Height;
            };

            sampler2D _OverlayTexture;
            float _Strength;
            Buffer<uint> VisibleQuadMeshIndices;
            StructuredBuffer<QuadMeshData> QuadMeshDataBuffer;

            v2f vert (appdata v)
            {
                v2f o;
                uint meshIndex = VisibleQuadMeshIndices[v.vertexID];
                QuadMeshData data = QuadMeshDataBuffer[meshIndex];
                o.vertex = UnityObjectToClipPos(data.Position);
                o.uv = data.Uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_OverlayTexture, i.uv);
                col.a = _Strength;
                return col;
            }
            ENDCG
        }
    }
}
