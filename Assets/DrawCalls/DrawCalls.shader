Shader "Custom/HanaDrawCalls" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		Cull Off
		
		Pass {
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 5.0
			#include "UnityCG.cginc"

			sampler2D _MainTex;
			
			struct appdata {
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};
			struct vs2ps {
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};
			
			vs2ps vert(appdata IN) {
				vs2ps o;
				o.vertex = mul(UNITY_MATRIX_MVP, IN.vertex);
				o.uv = IN.uv;
				return o;
			}
			float4 frag(vs2ps IN) : COLOR {
				return tex2D(_MainTex, IN.uv);
			}
			ENDCG
		}
	} 
	FallBack "Diffuse"
}
