Shader "Custom/GeometryDashColorSwap"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _PrimaryColor ("Primary Color", Color) = (1,1,1,1)
        _SecondaryColor ("Secondary Color", Color) = (1,1,1,1)
        [HideInInspector] _Color ("Tint", Color) = (1,1,1,1)
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

        Cull Off Lighting Off ZWrite Off
        Blend One OneMinusSrcAlpha

        Pass
        {
        CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            
            struct appdata_t
            {
                float4 vertex   : POSITION;
                float4 color    : COLOR;
                float2 texcoord : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex   : SV_POSITION;
                fixed4 color    : COLOR;
                float2 texcoord  : TEXCOORD0;
            };
            
            fixed4 _Color;
            fixed4 _PrimaryColor;
            fixed4 _SecondaryColor;

            v2f vert(appdata_t IN)
            {
                v2f OUT;
                OUT.vertex = UnityObjectToClipPos(IN.vertex);
                OUT.texcoord = IN.texcoord;
                OUT.color = IN.color * _Color;
                return OUT;
            }

            sampler2D _MainTex;

            fixed4 frag(v2f IN) : SV_Target
            {
                fixed4 c = tex2D(_MainTex, IN.texcoord);
                if (c.a < 0.01) return c;

                // Smart detection: checks if green or cyan dominate the pixel
                bool isGreen = (c.g > c.r + 0.1) && (c.g > c.b + 0.1);
                bool isCyan  = (c.b > c.r + 0.1) && (c.g > c.r + 0.1);

                if (isCyan)
                {
                    c.rgb = _SecondaryColor.rgb * c.a;
                }
                else if (isGreen)
                {
                    c.rgb = _PrimaryColor.rgb * c.a;
                }
                else
                {
                    c.rgb *= IN.color.rgb;
                }

                return c * IN.color.a;
            }
        ENDCG
        }
    }
}