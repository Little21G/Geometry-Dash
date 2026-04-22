Shader "Custom/GroundLineReveal"
{
    Properties
    {
        _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Color", Color) = (1,1,1,1)
        _CenterX ("Center X (World)", Float) = 0
        
        // These control the fade. You can tweak them in the Material inspector!
        _SolidWidth ("Solid Line Width", Float) = 12.0
        _FadeWidth ("Fade Softness", Float) = 6.0
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
            float _CenterX;
            float _SolidWidth;
            float _FadeWidth;

            struct appdata { float4 vertex : POSITION; float2 uv : TEXCOORD0; };
            struct v2f { float4 pos : SV_POSITION; float2 uv : TEXCOORD0; float worldX : TEXCOORD1; };

            v2f vert(appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.worldX = mul(unity_ObjectToWorld, v.vertex).x;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv) * _Color;
                
                // 1. Find out how far this pixel is from the center point
                float dist = abs(i.worldX - _CenterX);
                
                // 2. Fade out smoothly once the distance exceeds our SolidWidth
                float fade = 1.0 - smoothstep(_SolidWidth, _SolidWidth + _FadeWidth, dist);
                
                // 3. Apply the fade to the alpha channel
                col.a *= fade;
                return col;
            }
            ENDCG
        }
    }
}