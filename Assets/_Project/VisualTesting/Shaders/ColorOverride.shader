﻿Shader "Custom/ShadowPuppet"
{
    Properties
    {
        _Color("Color", Color) = (0.0,0.0,0.0,0.0)
    }
    SubShader
    {

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
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 _Color;

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = _Color;

                return col;
            }
            ENDCG 
        }
    }
}
