Shader "KSP2/Environment/CelestialBody/CelestialBody_Local_Overlay"
{
    Properties
    {
        [MainTexture] _OverlayTexture ("Overlay Texture", 2D) = "black" {}
        _Strength ("Strength", float) = 0.9
        [Toggle(_USE_PQS_BUFFER)] _NoComputeBuffer ("Use PQS QuadMeshDataBuffer", float) = 0.9
    }

    SubShader
    {
        Tags { "RenderType"="Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"
            #include "QuadMeshDataBuffer.hlsl"

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _OverlayTexture;
            float _Strength;

            v2f vert (appdata v)
            {
                v2f o;
                QuadMeshData data = GetQuadMeshVert(v);
                o.vertex = UnityObjectToClipPos(data.position);
                o.uv = data.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_OverlayTexture, i.uv);
                col.a = _Strength;
                return col;
            }
            ENDHLSL
        }
    }
}
