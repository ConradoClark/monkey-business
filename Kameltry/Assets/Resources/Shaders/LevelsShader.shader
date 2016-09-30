Shader "Unlit/LevelsShader"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}

		[Header(Color Levels)]
		_LevelsMinInput("RGB - Min Input",Range(0,255)) = 0
		_LevelsGamma("RGB - Gamma",Range(0.01,10)) = 1
		_LevelsMaxInput("RGB - Max Input",Range(0,255)) = 255

	}
	SubShader
	{
		Tags { "Queue" = "Transparent" "RenderType" = "Transparent" }
		LOD 100

		Cull Off
		Blend SrcAlpha OneMinusSrcAlpha

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			// make fog work
			#pragma multi_compile_fog
			
			#include "UnityCG.cginc"

			#define GammaCorrection(color, gamma)  pow(color, 1.0 / gamma)
			#define LevelsControlInputRange(color, minInput, maxInput) min(max(color - half4(minInput,minInput,minInput,0), 0.0) / ( half4(maxInput,maxInput,maxInput,1) - half4(minInput,minInput,minInput,0)), 1.0)
			#define LevelsControlInput(color, minInput, gamma, maxInput) GammaCorrection(LevelsControlInputRange(color, minInput/255, maxInput/255), gamma)

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
				float4 color : COLOR;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				UNITY_FOG_COORDS(1)
				float4 vertex : SV_POSITION;
				float4 color : COLOR;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;

			float _LevelsMinInput;
			float _LevelsGamma;
			float _LevelsMaxInput;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				o.vertex = UnityPixelSnap(o.vertex);
				o.color = v.color;
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				float2 uv = i.uv;
				// sample the texture
				fixed4 col = tex2D(_MainTex, uv) * i.color;

				// Color Levels
				col = LevelsControlInput(col, _LevelsMinInput, _LevelsGamma, _LevelsMaxInput);

				// apply fog
				UNITY_APPLY_FOG(i.fogCoord, col);
				return col;
			}
			ENDCG
		}
	}
}
