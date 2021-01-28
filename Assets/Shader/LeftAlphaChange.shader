Shader "Unlit/LeftAlphaChange"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
		_Color("Color", Color) = (1, 1, 1, 1) 
		_Distance("Distance", float) = 1
		_SetX("SetX", float) = 1

    }
    SubShader
    {
        Tags { "RenderType" = "Transparent"  "IgnoreProjector" = "True"  "Queue" = "Transparent"}
			LOD 200
            ZWrite Off
			Blend SrcAlpha OneMinusSrcAlpha
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float4 worldPos: TEXCOORD1;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            fixed4 _Color;
            float _Distance;
            float _SetX;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.worldPos = mul(unity_ObjectToWorld, v.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                clip(_SetX-i.worldPos.x);
                fixed4 col = tex2D(_MainTex, i.uv);
				col.rgb *= _Color.rgb;

                col.a =col.a*saturate((_SetX - i.worldPos.x)/_Distance);
                return col;
            }
            ENDCG
        }
    }
}
