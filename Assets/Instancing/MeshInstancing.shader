	Shader "Custom/MeshInstancing" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
	}
	SubShader { 
		Tags { "RenderType"="Opaque" }
		LOD 700
		Cull Off

		Pass {	
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 5.0
			#include "UnityCG.cginc"
			#include "Lighting.cginc"

			sampler2D _MainTex;

			struct vs2ps {
				float4 vertex : POSITION;
				float3 normal : NORMAL;
				float2 uv : TEXCOORD0;
			};
			
			StructuredBuffer<uint> indexBuf;
			StructuredBuffer<float3> vertexBuf;
			StructuredBuffer<float2> uvBuf;
			StructuredBuffer<float4x4> worldBuf;

			vs2ps vert(uint vertexId : SV_VertexID, uint instanceId : SV_InstanceID) {
				vs2ps o;
				
				uint index = indexBuf[vertexId];
				float4 vertex = float4(vertexBuf[index], 1);
				float2 uv = uvBuf[index];
				float4x4 worldMat = worldBuf[instanceId];

				vertex = mul(worldMat, vertex);
				o.vertex = mul(UNITY_MATRIX_VP, vertex);
				o.uv = uv;
				return o;
			}

			float4 frag(vs2ps IN) : COLOR {
				float4 c = tex2D(_MainTex, IN.uv);
				return c;
			}
			ENDCG
		}
	}
	FallBack "Diffuse"
}
