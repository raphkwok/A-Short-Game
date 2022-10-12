// Copyright Elliot Bentine, 2018-
// 
// Applies a pixelization map to _MainTex.
Shader "Hidden/ProPixelizer/SRP/ApplyPixelizationMap" {
	Properties{
	}

	SubShader{
	Tags{
		"RenderType" = "Opaque"
		"PreviewType" = "Plane"
		"RenderPipeline" = "UniversalPipeline"
	}

	Pass{
		Cull Off
		ZWrite On
		ZTest Off
		Blend Off

		HLSLPROGRAM 
		#pragma vertex vert
		#pragma fragment frag
		#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
		#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
		#include "PixelUtils.hlsl"
		#include "PackingUtils.hlsl"
		//#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DeclareDepthTexture.hlsl"

		TEXTURE2D(_PixelizationMap);
		TEXTURE2D(_MainTex);
		SAMPLER(sampler_point_clamp);
		float4 _MainTex_TexelSize;
		TEXTURE2D_X_FLOAT(_SourceDepthTexture);
		TEXTURE2D_X_FLOAT(_SceneDepthTexture);

		struct v2f {
			float4 pos : SV_POSITION;
			float4 scrPos:TEXCOORD1;
		};

		struct appdata_base
		{
			float4 vertex   : POSITION;  // The vertex position in model space.
			float4 texcoord : TEXCOORD0; // The first UV coordinate.
		};

		v2f vert(appdata_base v) {
			v2f o;
			o.pos = TransformObjectToHClip(v.vertex.xyz);
			o.scrPos = ComputeScreenPos(o.pos);
			return o;
		}

		void frag(v2f i, out float4 color: COLOR, out float depth : SV_DEPTH) {
			float4 packed = SAMPLE_TEXTURE2D(_PixelizationMap, sampler_point_clamp, i.scrPos.xy);
			float2 uvs = UnpackPixelMapUV(packed, _MainTex_TexelSize); 
			color = SAMPLE_TEXTURE2D(_MainTex, sampler_point_clamp, uvs.xy); // scene color at pixelised coordinate
			depth = SAMPLE_TEXTURE2D_X(_SceneDepthTexture, sampler_point_clamp, uvs.xy); // scene depth at pixelised coordinate
			float4 original_color = SAMPLE_TEXTURE2D(_MainTex, sampler_point_clamp, i.scrPos.xy); // scene color at unpixelised coordinate
			float original_depth = SAMPLE_TEXTURE2D_X(_SceneDepthTexture, sampler_point_clamp, i.scrPos.xy); // scene depth at unpixelised coordinate
			float pixelated_depth = SAMPLE_TEXTURE2D_X(_SourceDepthTexture, sampler_point_clamp, uvs.xy).r; // depth at pixelised coordinate, of the pixelised object.

			#if UNITY_REVERSED_Z
				float delta = original_depth - pixelated_depth;
			#else
				float delta = pixelated_depth - original_depth;
			#endif

			if (delta > 0.0)
			{
				color = original_color;
				depth = original_depth;
			}
		}
		ENDHLSL
		}
	}
	FallBack "Diffuse"
}