Shader "Mobile/BumpedDiffuseAlpha" {
Properties {
    _MainTex ("Main Texture", 2D) = "white" {}
	_Bumped ("Normal map", 2D) = "bump" {}
	_Alpha ("Alpha", Range(0.0,1.0)) = 0.5
}
SubShader {
    Tags { "RenderType"="Transparent" "Queue" = "Transparent" }
    LOD 200

CGPROGRAM
#pragma surface surf Lambert alpha:fade noforwardadd 

sampler2D _MainTex;
sampler2D _Bumped;
float _Alpha;

struct Input {
    float2 uv_MainTex;
	float2 uv_SecondTex;
	float2 uv_Bumped;
};

void surf (Input IN, inout SurfaceOutput o) {
    fixed3 mainColor = tex2D(_MainTex, IN.uv_MainTex).rgb;
    o.Albedo = mainColor;
	o.Normal = UnpackNormal(tex2D(_Bumped, IN.uv_Bumped));
	o.Alpha = _Alpha;
}
ENDCG
}

Fallback "Mobile/Diffuse"
}