#ifndef _QUADMESHDATA
#define _QUADMESHDATA

#pragma shader_feature_local _USE_PQS_BUFFER

struct QuadMeshData
{
    float3 position;
	float3 normal;
	float2 uv;
	float4 tangent;
	float4 height;
};

struct appdata
{
    uint vertexID : SV_VertexID;
    float4 vertex : POSITION;
    float2 uv : TEXCOORD0;
	float4 normal : NORMAL0;
	float4 tangent : TANGENT0;
};

#if _USE_PQS_BUFFER
Buffer<uint> VisibleQuadMeshIndices;
StructuredBuffer<QuadMeshData> QuadMeshDataBuffer;
#endif

// read from either ordinary mesh data or QuadMeshDataBuffer
QuadMeshData GetQuadMeshVert(appdata v)
{
	QuadMeshData data;
#if _USE_PQS_BUFFER
    uint meshIndex = VisibleQuadMeshIndices[v.vertexID];
	data = QuadMeshDataBuffer[meshIndex];
#else
	data.position = v.vertex;
	data.normal = v.normal;
	data.uv = v.uv;
	data.tangent = v.tangent;
	data.height = 0;
#endif
	return data;
}

#endif
