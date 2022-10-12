// Unity built-in shader source. Copyright (c) 2016 Unity Technologies. MIT license (see license.txt)

// This file is included in ProPixelizer to address a bug in the existing BlitCopyDepth - that is, that it can't write to depth!

Shader "Hidden/ProPixelizer/SRP/BlitCopyDepth" {
	Properties{ _MainTex("Texture", any) = "" {} }
		SubShader{
			Pass {
				ZTest Always Cull Off ZWrite On

				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag
				#pragma target 2.0

				#include "UnityCG.cginc"

				UNITY_DECLARE_DEPTH_TEXTURE(_MainTex);
				uniform float4 _MainTex_ST;

				struct appdata_t {
					float4 vertex : POSITION;
					float2 texcoord : TEXCOORD0;
					UNITY_VERTEX_INPUT_INSTANCE_ID
				};

				struct v2f {
					float4 vertex : SV_POSITION;
					float2 texcoord : TEXCOORD0;
					UNITY_VERTEX_OUTPUT_STEREO
				};

				v2f vert(appdata_t v)
				{
					v2f o;
					UNITY_SETUP_INSTANCE_ID(v);
					UNITY_INITIALIZE_OUTPUT(v2f, o);
					UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
					o.vertex = UnityObjectToClipPos(v.vertex);
					o.texcoord = TRANSFORM_TEX(v.texcoord.xy, _MainTex);
					return o;
				}

				float4 frag(v2f i, out float outDepth : SV_Depth) : SV_Target
				{
					#if UNITY_UV_STARTS_AT_TOP
						//i.texcoord.y = 1 - i.texcoord.y;
					#endif
					outDepth = SAMPLE_RAW_DEPTH_TEXTURE(_MainTex, i.texcoord);
					UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(i);
					return SAMPLE_RAW_DEPTH_TEXTURE(_MainTex, i.texcoord);
				}
				ENDCG

			}
	}
		Fallback Off
}