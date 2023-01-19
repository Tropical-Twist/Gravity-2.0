#ifndef OUTLINE_INCLUDED
#define OUTLINE_INCLUDED

#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

struct Attributes
{
	float4 positionOS : POSITION;
	float3 normalOS : NORMAL;
#ifdef USE_PRECALCULATED_OUTLINE_NORMALS
	float3 smoothNormalOS   : TEXCOORD1; // Calculated "smooth" normals to extrude along in object space
#endif
};

struct VertexOutput
{
	float4 positionCS : SV_POSITION;
};

float _Thickness;
float4 _Color;

VertexOutput Vertex(Attributes input)
{
	VertexOutput output = (VertexOutput)0;

#ifdef USE_PRECALCULATED_OUTLINE_NORMALS
	float3 normalOS = input.smoothNormalOS;
#else
	float3 normalOS = input.normalOS;
#endif

	float3 posOS = input.positionOS.xyz + normalOS * _Thickness;

	output.positionCS = GetVertexPositionInputs(posOS).positionCS;

	return output;
}

float4 Fragment(VertexOutput input) : SV_Target
{
	return _Color;
}

#endif