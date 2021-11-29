Shader "ImageEffect/Testing"
{
    Properties
    {
        _MainTex ("Main Tex", 2D) = "white" {}
        _OuterStrength ("Outer", float) = 0.25
        _CenterStrength("Inner", float) = 15.0
        _VignetteColor("Vignette Color", Color) = (0.0,0.0,0.0,0.0)
    }
    SubShader
    {
        // No culling or depth
        Cull Off ZWrite Off ZTest Always

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

            sampler2D _MainTex;
            float _OuterStrength;
            float _CenterStrength;
            float4 _VignetteColor;

            fixed4 vignette(v2f i, fixed4 col)
            {
                float2 vignetteUV = i.uv;
   
                vignetteUV *=  float2(1.0,1.0) - vignetteUV.xy;   //vec2(1.0)- uv.yx; -> 1.-u.yx; Thanks FabriceNeyret !
                
                float vig = vignetteUV.x*vignetteUV.y * _CenterStrength; // multiply with sth for intensity
                
                vig = saturate(pow(vig, _OuterStrength)); // change pow for modifying the extend of the  vignette

                col = lerp(col, _VignetteColor, 1-vig);

                return col;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);

                col = vignette(i, col);

                return col;
            }
            ENDCG 
        }
    }
}
