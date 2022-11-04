// Unity built-in shader source. Copyright (c) 2016 Unity Technologies. MIT license (see license.txt)
// Edits by Glynn Taylor. MIT license
// Includes code for splitmap by https://twitter.com/adamgryu and triplanar mapping by https://github.com/keijiro. MIT License

#ifndef TERRAIN_SPLATMAP_COMMON_CGINC_INCLUDED
#define TERRAIN_SPLATMAP_COMMON_CGINC_INCLUDED

struct Input
{
	float3 localNormal : TEXCOORD0;
	float3 localCoord : TEXCOORD1;
	float2 tc_Control : TEXCOORD4;  // Not prefixing '_Contorl' with 'uv' allows a tighter packing of interpolators, which is necessary to support directional lightmap.
	UNITY_FOG_COORDS(5)
};

sampler2D _VerticalTex;
float4 _VerticalTex_ST;

sampler2D _Control;
float4 _Control_ST;
sampler2D _Splat0,_Splat1,_Splat2,_Splat3;
float4 _Splat0_ST, _Splat1_ST, _Splat2_ST, _Splat3_ST;

#ifdef _TERRAIN_NORMAL_MAP
	sampler2D _Normal0, _Normal1, _Normal2, _Normal3;
#endif

void SplatmapVert(inout appdata_full v, out Input data)
{
	UNITY_INITIALIZE_OUTPUT(Input, data);
	data.tc_Control = TRANSFORM_TEX(v.texcoord, _Control);  // Need to manually transform uv here, as we choose not to use 'uv' prefix for this texcoord.
	float4 pos = UnityObjectToClipPos(v.vertex);
	UNITY_TRANSFER_FOG(data, pos);

	data.localNormal = v.normal.xyz;
	data.localCoord = v.vertex.xyz;

#ifdef _TERRAIN_NORMAL_MAP
	v.tangent.xyz = cross(v.normal, float3(0,0,1));
	v.tangent.w = -1;
#endif
}

half4 SplitMap(half4 map)
{
	map.r = step(0.1, map.r - map.g - map.b - map.a);
	map.g = step(0.1, map.g - map.r - map.b - map.a);
	map.b = step(0.1, map.b - map.g - map.r - map.a);
	map.a = step(0.1, map.a - map.g - map.b - map.r);
	return map;
}

#ifdef TERRAIN_STANDARD_SHADER
void SplatmapMix(Input IN, half4 defaultAlpha, out half4 splat_control, out half weight, out fixed4 mixedDiffuse, inout float3 mixedNormal)
#else
void SplatmapMix(Input IN, out half4 splat_control, out half weight, out fixed4 mixedDiffuse, inout fixed3 mixedNormal)
#endif
{
	splat_control = tex2D(_Control, IN.tc_Control);
	weight = dot(splat_control, half4(1,1,1,1));

	#if defined(TERRAIN_SPLAT_ADDPASS)
		weight = step(0.1, weight);
		clip(weight == 0.0f ? -1 : 1);
	#endif

	// Normalize weights before lighting and restore weights in final modifier functions so that the overal
	// lighting result can be correctly weighted.
	splat_control /= (weight + 1e-3f);

	splat_control = SplitMap(splat_control);

	float2 uvSplat0 = TRANSFORM_TEX(IN.tc_Control.xy * _Splat0_ST.xy + _Splat0_ST.zw, _Splat0 );
	float2 uvSplat1 = TRANSFORM_TEX(IN.tc_Control.xy * _Splat1_ST.xy + _Splat1_ST.zw, _Splat1);
	float2 uvSplat2 = TRANSFORM_TEX(IN.tc_Control.xy * _Splat2_ST.xy + _Splat2_ST.zw, _Splat2);
	float2 uvSplat3 = TRANSFORM_TEX(IN.tc_Control.xy * _Splat3_ST.xy + _Splat3_ST.zw, _Splat3);

	mixedDiffuse = 0.0f;

	mixedDiffuse += splat_control.r * tex2D(_Splat0, uvSplat0);
	mixedDiffuse += splat_control.g * tex2D(_Splat1, uvSplat1);
	mixedDiffuse += splat_control.b * tex2D(_Splat2, uvSplat2);
	mixedDiffuse += splat_control.a * tex2D(_Splat3, uvSplat3);

}

#ifndef TERRAIN_SURFACE_OUTPUT
	#define TERRAIN_SURFACE_OUTPUT SurfaceOutput
#endif

void SplatmapFinalColor(Input IN, TERRAIN_SURFACE_OUTPUT o, inout fixed4 color)
{
	color *= o.Alpha;
	#ifdef TERRAIN_SPLAT_ADDPASS
		UNITY_APPLY_FOG_COLOR(IN.fogCoord, color, fixed4(0,0,0,0));
	#else
		UNITY_APPLY_FOG(IN.fogCoord, color);
	#endif
}

void SplatmapFinalPrepass(Input IN, TERRAIN_SURFACE_OUTPUT o, inout fixed4 normalSpec)
{
	normalSpec *= o.Alpha;
}

void SplatmapFinalGBuffer(Input IN, TERRAIN_SURFACE_OUTPUT o, inout half4 outGBuffer0, inout half4 outGBuffer1, inout half4 outGBuffer2, inout half4 emission)
{
	UnityStandardDataApplyWeightToGbuffer(outGBuffer0, outGBuffer1, outGBuffer2, o.Alpha);
	emission *= o.Alpha;
}

#endif // TERRAIN_SPLATMAP_COMMON_CGINC_INCLUDED
