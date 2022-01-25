Shader "Custom/WorldSpaceTexture-Unlit"
{
    Properties
    {
        _MainTex ("Main Tex", 2D) = "white" {}
        _GridSize ("Grid Size", float) = 1
        _Offset ("Offset", vector) = (0,0,0,0)
        _Colour ("Tint", Color) = (1,1,1,1)
    }
    SubShader
    {
        // No culling or depth
        Cull Off 
        ZWrite Off 
        ZTest LEqual

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

            sampler2D _MainTex;
            fixed _GridSize;
            fixed4 _Colour;

            fixed4 frag (v2f i) : SV_Target
            {
                fixed2 uv = (abs(i.pos) / 4 / _GridSize) % 1;

                fixed4 col = tex2D(_MainTex, uv);

                return col * _Colour;
            }
            ENDCG 
        }
    }
}
