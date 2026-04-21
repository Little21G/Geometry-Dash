Shader "Custom/GroundLineReveal"
{
    Properties
    {
        _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Color", Color) = (1,1,1,1)
        _RevealX ("Reveal X (World)", Float) = 0
        _EdgeSoftness ("Edge Softness", Float) = 0.5
    }

    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha
        Cull Off
        ZWrite Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float4 _Color;
            float _RevealX;
            float _EdgeSoftness;

            struct appdata { float4 vertex : POSITION; float2 uv : TEXCOORD0; };
            struct v2f { float4 pos : SV_POSITION; float2 uv : TEXCOORD0; float worldX : TEXCOORD1; };

            v2f vert(appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                // Pass the world X position of this pixel to the fragment shader
                o.worldX = mul(unity_ObjectToWorld, v.vertex).x;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv) * _Color;
                // Pixels to the RIGHT of _RevealX fade out
                float reveal = 1.0 - smoothstep(_RevealX - _EdgeSoftness, _RevealX + _EdgeSoftness, i.worldX);
                col.a *= reveal;
                return col;
            }
            ENDCG
        }
    }
}