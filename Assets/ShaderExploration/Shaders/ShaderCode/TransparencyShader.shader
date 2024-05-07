Shader "Introduction/TransparencyShader"
{
    Properties
    {
        _Color("Test Color", color) = (1,1,1,1)
        _MainTex("Main Texture", 2D) = "white" {}
        _AnimateXY("Animate X Y", Vector) = (0,0,0,0)
       
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"


            struct appdata // Object data or mesh data
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f // Vertex to frag
            {
                float4 vertex : SV_POSITION; // Highest precision
                float2 uv : TEXCOORD0;
            };


            fixed4 _Color;
            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _AnimateXY;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex); // MVP matrix multiplication
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.uv += _AnimateXY.xy * float2(_Time.y, 0);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 uvs = i.uv;
                fixed4 textureColor = tex2D(_MainTex, uvs);
                //textureColor = float4(textureColor.x, textureColor.y, textureColor.z, 0.5);
                textureColor = float4(textureColor.r,0, 0, 0);
                return textureColor;

            }
            ENDHLSL
        }
    }
}
