Shader "Hidden/Dizziness"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _DizStr ("Dizziness Strength", Range(0, 0.1)) = 0.05
    }
    SubShader
    {
        Tags
        {
            "RenderType" = "Transparent"
            "Queue" = "Transparent+100"
        }

        GrabPass {}

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
                float4 uv : TEXCOORD0;
                float4 pos : SV_POSITION;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = ComputeGrabScreenPos(o.pos);
                return o;
            }

            sampler2D _MainTex;
            sampler2D _GrabTexture;
            float _DizStr;

            fixed4 frag(v2f i) : SV_Target
            {
                fixed4 ref = tex2D(_MainTex, float2(i.uv.x, i.uv.y + _Time.y * 0.1));

                fixed4 col = tex2D(_GrabTexture, (i.uv.xy + ref.x * _DizStr));
                return col;
            }
            ENDCG
        }
    }
}
