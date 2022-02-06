Shader "Custom/FullScreenWipe"
{
    Properties
    {
        _Colour ("Colour", Color) = (0,0,0,1)
        _WipePattern ("Wipe Pattern", 2D) = "white" {}
        _Ratio ("Reveal Ratio", Range(0.0, 1.0)) = 0
        _FadeSize ("Fade Size", Float) = 0.1
    }
    SubShader
    {
		Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
		LOD 100
		
		ZWrite Off
		Blend SrcAlpha OneMinusSrcAlpha 
		Cull Off
		Lighting Off

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
                float4 pos : TEXCOORD1;
            };
            
            fixed4 _Offset;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.pos = mul(unity_ObjectToWorld, v.vertex) + _Offset;
                o.uv = v.uv;
                return o;
            }

            sampler2D _WipePattern;
            fixed _Ratio;
            fixed _FadeSize;
            fixed4 _Colour;

            fixed4 frag (v2f i) : SV_Target
            {
                fixed fadeEnd = lerp(-_FadeSize-0.01,1,_Ratio); //Ratio 0 should show nothing
                fixed fadeStart = lerp(-0.01, 1+_FadeSize, _Ratio); // Ratio 1 should be fully coloured

                fixed4 wipeValue = tex2D(_WipePattern, i.uv);

                fixed4 col = _Colour;

                col.a = 1 - saturate((wipeValue.r - fadeEnd) / _FadeSize);

                return col;
            }
            ENDCG 
        }
    }
}
