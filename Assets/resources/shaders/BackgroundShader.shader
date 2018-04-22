Shader "Custom/BackgroundShader"
{
	Properties
	{
		_MainTex("Main Texture", 2D) = "white" {}
		_UVoffsetX("UV offset X", float) = 0
		_UVoffsetY("UV offset Y", float) = 0
		_UVscale("UV scale", float) = 0

		_Color("color", Color) = (1.0,1.0,1.0,1.0)
	}

	SubShader 
	{
		// No culling or depth
		//Tags{ "Queue" = "2000" }
		Blend SrcAlpha OneMinusSrcAlpha
		ZWrite On
		//ZTEST Less

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag alpha

			#include "UnityCG.cginc"

			float _UVoffsetX;
			float _UVoffsetY;
			float _UVscale;

			uniform float4 _Color;

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			v2f vert(appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				return o;
			}

			sampler2D _MainTex;

			fixed4 frag(v2f i) : SV_Target
			{
				i.uv = (i.uv - 0.5) * _UVscale + 1;
				fixed4 col = tex2D(_MainTex, i.uv + float2(_UVoffsetX, _UVoffsetY));
				return col;
			}

		ENDCG
		}
	}
}