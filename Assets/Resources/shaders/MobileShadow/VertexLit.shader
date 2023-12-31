Shader "MobileShadow/VertexLit" {
	Properties {
		_MainTex ("Main Texture", 2D) = "white" {}
		_AlphaR ("Alpha Texture", 2D) = "white" {}
		_EmitTex ("Emission Texture", 2D) = "white" {}
		_EmitValue ("Emission Value", Range(0, 1)) = 0
		_Shininess ("Shininess", Range(0.5, 50)) = 8
		_Dissolve ("Dissolve", Range(0, 1)) = 0
		[HideInInspector] _Mode ("__mode", Float) = 0
		[HideInInspector] _SrcBlend ("__src", Float) = 1
		[HideInInspector] _DstBlend ("__dst", Float) = 0
		[HideInInspector] _ZWrite ("__zw", Float) = 1
		[HideInInspector] _Stencil ("Stencil ID", Float) = 0
		[HideInInspector] _StencilComp ("Stencil Comparison", Float) = 0
		[HideInInspector] _StencilOp ("Stencil Operation", Float) = 0
	}
	//DummyShaderTextExporter
	SubShader{
		Tags { "RenderType"="Opaque" }
		LOD 200
		CGPROGRAM
#pragma surface surf Standard
#pragma target 3.0

		sampler2D _MainTex;
		struct Input
		{
			float2 uv_MainTex;
		};

		void surf(Input IN, inout SurfaceOutputStandard o)
		{
			fixed4 c = tex2D(_MainTex, IN.uv_MainTex);
			o.Albedo = c.rgb;
			o.Alpha = c.a;
		}
		ENDCG
	}
	//CustomEditor "VertexLitGUI"
}