
Shader "Sprites/Simple"
{
	Properties
	{
		_MainTex ("Sprite Texture", 2D) = "white" {}
	}

	SubShader
	{
		Tags
		{ 
			"Queue"="Transparent" 
			"IgnoreProjector"="True" 
			"RenderType"="Transparent" 
			"PreviewType"="Plane"
			"CanUseSpriteAtlas"="True"
		}

		Cull Off
		Lighting Off
		ZWrite Off
		Blend One OneMinusSrcAlpha

		Pass
		{
		CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"

			uniform sampler2D _MainTex;
			uniform float4 _MainTex_ST; 

			struct appdata_t
			{
				float4 vertex   : POSITION;
				float2 texcoord : TEXCOORD0;
			};

			struct v2f
			{
				float4 pos   : SV_POSITION;
				half2 uv  : TEXCOORD0;
			};
			
			

			v2f vert(appdata_t IN)
			{
				v2f o;
				o.pos = UnityObjectToClipPos( IN.vertex );
				o.uv = TRANSFORM_TEX( IN.texcoord, _MainTex );
				return o;
			}
			
			half4 frag(v2f IN) : COLOR
			{
				return half4( tex2D( _MainTex, IN.uv ).rgb, 1.0);
			}
			
		ENDCG
		}
	}
}
