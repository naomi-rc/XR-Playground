Shader "Introduction/MyShader"
{
    Properties
    {
        _Color("Test Color", color) = (1,1,1,1)
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
            };

            struct v2f // Vertex to frag
            {
                float4 vertex : SV_POSITION; // Highest precision
            };


            fixed4 _Color;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex); // MVP matrix multiplication
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                half2 someValue = half2(1,1);
                fixed4 col = _Color; // Low precision
                return col;
            }
            ENDHLSL
        }
    }
}
