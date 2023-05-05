Shader "Custom/ToonShader"
{
	Properties
	{
		[Toggle] _ActiveToon("Active Toon", Float) = 1
		_Color("Color", Color) = (0.5, 0.65, 1, 1)
		_MainTex("Main Texture", 2D) = "white" {}
		[HDR] _AmbientColor("Ambient Color", Color) = (0.4,0.4,0.4,1)
		[HDR] _SpecularColor("Specular Color", Color) = (0.9,0.9,0.9,1)
		_Glossiness("Glossiness", Float) = 32
		[HDR] _RimColor("Rim Color", Color) = (1,1,1,1)
		_RimAmount("Rim Amount", Range(0, 1)) = 0.716
		//_RimPower("Rim Power", Range(1, 10)) = 3
		_RimThreshold("Rim Threshold", Range(0, 1)) = 0.1

		[Space(1)]

		_NoiseTex("Noise Texture", 2D) = "" {}
		[HDR] _OutColor("Out Color", Color) = (0.5,0.5,0.5,1)
		_AlphaCut("Alpha Cut", Range(0, 1)) = 0
		_Edges ("Edges", Range(0, 1)) = 0
		_NoiseScale("Noise Scale", Float) = 100

		[Space(1)]
		[Toggle] _Outline("Active Outline", Float) = 1
		_OutlineColor("Outline Color", Color) = (0,0,0,1)
		_Thickness("Outline Thickness", Float) = 0.02
	}
	SubShader
	{
		Pass
		{
			Tags { "RenderType" = "Transparent" "Queue" = "Transparnet" "LightMode" = "ForwardBase" "PassFlags" = "OnlyDirectional" }
			
			Cull Front
			
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float4 uv : TEXCOORD0;
				float3 normal : NORMAL;
			};

			struct v2f
			{
				float4 pos : SV_POSITION;
				float2 uv : TEXCOORD0;
			};

			fixed _Outline;
			float4 _OutlineColor;
			float _Thickness;


			v2f vert(appdata v)
			{
				v2f o;

				// Outline
				float3 _Object_Scale = float3(length(float3(UNITY_MATRIX_M[0].x, UNITY_MATRIX_M[1].x, UNITY_MATRIX_M[2].x)),
					length(float3(UNITY_MATRIX_M[0].y, UNITY_MATRIX_M[1].y, UNITY_MATRIX_M[2].y)),
					length(float3(UNITY_MATRIX_M[0].z, UNITY_MATRIX_M[1].z, UNITY_MATRIX_M[2].z)));
				float3 thickness = _Thickness / _Object_Scale;
				float3 normal = normalize(v.normal);
				float4 outline = float4((normal * thickness), 0) + v.vertex;

				if (_Outline == 1)
					o.pos = UnityObjectToClipPos(outline);
				else
					o.pos = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				return o;
			}


			float4 frag(v2f i) : SV_Target
			{
				if (_Outline == 0)
					discard;
				return _OutlineColor;
			}
			ENDCG
		}

		Pass
		{
			Tags { "RenderType"="Transparent" "Queue"="Transparnet" "LightMode"="ForwardBase" "PassFlags"="OnlyDirectional" }
			
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_fwdbase // Built-in Render Pipeline의 광원, 그림자 및 라이트매핑과 관련 Shader 키워드 세트 추가.
			// "그림자가 없는 방향 광원" 및 "그림자가 있는 방향 광원"의 경우를 처리.
			// 추가 키워드: DIRECTIONAL LIGHTMAP_ON DIRLIGHTMAP_COMBINED DYNAMICLIGHTMAP_ON SHADOWS_SCREEN SHADOWS_SHADOWMASK LIGHTMAP_SHADOW_MIXING LIGHTPROBE_SH

			#include "UnityCG.cginc"
			#include "Lighting.cginc"
			#include "AutoLight.cginc"


			// Generate Simple Noise by Unity Node Library //
			inline float unity_noise_randomValue(float2 uv)
			{
				return frac(sin(dot(uv, float2(12.9898, 78.233))) * 43758.5453);
			}

			inline float unity_noise_interpolate(float a, float b, float t)
			{
				return (1.0 - t) * a + (t * b);
			}

			inline float unity_valueNoise(float2 uv)
			{
				float2 i = floor(uv);
				float2 f = frac(uv);
				f = f * f * (3.0 - 2.0 * f);

				uv = abs(frac(uv) - 0.5);
				float2 c0 = i + float2(0.0, 0.0);
				float2 c1 = i + float2(1.0, 0.0);
				float2 c2 = i + float2(0.0, 1.0);
				float2 c3 = i + float2(1.0, 1.0);
				float r0 = unity_noise_randomValue(c0);
				float r1 = unity_noise_randomValue(c1);
				float r2 = unity_noise_randomValue(c2);
				float r3 = unity_noise_randomValue(c3);

				float bottomOfGrid = unity_noise_interpolate(r0, r1, f.x);
				float topOfGrid = unity_noise_interpolate(r2, r3, f.x);
				float t = unity_noise_interpolate(bottomOfGrid, topOfGrid, f.y);
				return t;
			}

			void Unity_SimpleNoise_float(float2 UV, float Scale, out float Out)
			{
				float t = 0.0;

				float freq = pow(2.0, float(0));
				float amp = pow(0.5, float(3 - 0));
				t += unity_valueNoise(float2(UV.x * Scale / freq, UV.y * Scale / freq)) * amp;

				freq = pow(2.0, float(1));
				amp = pow(0.5, float(3 - 1));
				t += unity_valueNoise(float2(UV.x * Scale / freq, UV.y * Scale / freq)) * amp;

				freq = pow(2.0, float(2));
				amp = pow(0.5, float(3 - 2));
				t += unity_valueNoise(float2(UV.x * Scale / freq, UV.y * Scale / freq)) * amp;

				Out = t;
			}
			// End Simple Noise //


			struct appdata
			{
				float4 vertex : POSITION;
				float4 uv : TEXCOORD0;
				float3 normal : NORMAL;
			};

			struct v2f
			{
				float4 pos : SV_POSITION;
				float2 uv : TEXCOORD0;
				float3 worldNormal : NORMAL;
				float3 viewDir : TEXCOORD1;
				SHADOW_COORDS(2)
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;

			fixed _ActiveToon;
			float4 _Color;
			float4 _AmbientColor;
			float4 _SpecularColor;
			float _Glossiness;
			float4 _RimColor;
			float _RimAmount;
			//float _RimPower;
			float _RimThreshold;

			float4 _OutColor;
			float _AlphaCut;
			float _Edges;
			float _NoiseScale;


			v2f vert(appdata v)
			{
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				o.worldNormal = UnityObjectToWorldNormal(v.normal); // UnityObjectToWorldNormal: UnityCG.cginc에서 선언되는 함수. normal from object to world space, use that.
				o.viewDir = WorldSpaceViewDir(v.vertex); // WorldSpaceViewDir: UnityCG.cginc에서 선언되는 함수
				TRANSFER_SHADOW(o);
				return o;
			}


			float4 frag(v2f i) : SV_Target
			{
				// Light
				float3 normal = normalize(i.worldNormal);
				float NdotL = dot(_WorldSpaceLightPos0, normal); // _WorldSpaceLightPos0: 방향 광원(월드 공간 방향, 0). 기타 라이트: (월드 공간 포지션, 1)
				float shadow = SHADOW_ATTENUATION(i); // SHADOW_ATTENUATION: 0..1의 값을 가짐. 0 = Fully Shadowed, 1 = Fully Lit
				float lightIntensity = smoothstep(0, 0.01, NdotL * shadow); // smoothstep(Edge0, Edge1, In) : Edge1 이상 -> 1, Edge0 미만 -> 0
				float4 light = lightIntensity * _LightColor0; // _LightColor0: DirectionalLight Color(광원 컬러)

				// Specular (집중 조명)
				float3 viewDir = normalize(i.viewDir);
				float3 halfVector = normalize(_WorldSpaceLightPos0 + viewDir);
				float NdotH = dot(normal, halfVector);
				float specularIntensity = pow(NdotH * lightIntensity, _Glossiness * _Glossiness);
				float specularIntensitySmooth = smoothstep(0.005, 0.01, specularIntensity);
				float4 specular = specularIntensitySmooth * _SpecularColor;

				// Rim Lighting (가장자리 조명)
				float4 rimDot = 1 - dot(viewDir, normal);
				//rimDot = pow(saturate(rimDot), _RimPower);
				float rimIntensity = rimDot * pow(NdotL, _RimThreshold); // 조명 방향이면 ↑, 그림자 영역이면 ↓. 한계값(Threshold)를 이용한 극대화.
				rimIntensity = smoothstep(_RimAmount - 0.01, _RimAmount + 0.01, rimIntensity);
				float4 rim = rimIntensity * _RimColor;

				float4 mainTex = tex2D(_MainTex, i.uv);

				// Dissolve
				float noise;
				Unity_SimpleNoise_float(i.uv, _NoiseScale, noise);
				#if _NoiseTex
					noise = tex2D(_NoiseTex, i.uv);
				#endif

				if (noise < _AlphaCut) // AlphaCut보다 값이 작다면 출력하지 않음(버림)
					discard;

				if (noise < mainTex.a && noise < _AlphaCut + _Edges) // Noise가 MainTex의 Alpha값 보다 작고 Noise가 AlphaCut + Edges보다 작다면
					mainTex = lerp(mainTex, _OutColor, (noise - _AlphaCut) / _Edges); // MainTex -> OutColor

				if (_ActiveToon == 1)
					return _Color * mainTex * (_AmbientColor + light + specular + rim);
				else
					return _Color * mainTex;
			}
			ENDCG
		}

		UsePass "Legacy Shaders/VertexLit/SHADOWCASTER" // 다른 셰이더의 pass를 가져와서 이 셰이더에 삽입함
	}
}
