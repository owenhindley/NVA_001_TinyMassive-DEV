Shader "Unlit/IdleShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _TextureOpacity("TextureOpacity", float) = 0
        _TextureYScale("TextureYScale", float) = 0
        _MasterOpacity("MasterOpacity", float) = 0
        _NoiseSpeed("NoiseSpeed", float) = 0
        _NoiseScale("NoiseScale", float) = 0
        _NoiseOpacity("NoiseOpacity", float) = 0
        _ScrollSpeed("ScrollSpeed", float) = 0
        _ScrollInterval("ScrollInterval", float) = 0

    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"
            #include "SimplexNoise3d.cginc"

            float3 snoisefloat3( float3 x ){

                float s  = snoise(float3( x ));
                float s1 = snoise(float3( x.y - 19.1 , x.z + 33.4 , x.x + 47.2 ));
                float s2 = snoise(float3( x.z + 74.2 , x.x - 124.5 , x.y + 99.4 ));
                float3 c = float3( s , s1 , s2 );
                return c;

            }


            float3 curlNoise( float3 p ){
                
                const float e = .1;
                float3 dx = float3( e   , 0.0 , 0.0 );
                float3 dy = float3( 0.0 , e   , 0.0 );
                float3 dz = float3( 0.0 , 0.0 , e   );

                float3 p_x0 = snoisefloat3( p - dx );
                float3 p_x1 = snoisefloat3( p + dx );
                float3 p_y0 = snoisefloat3( p - dy );
                float3 p_y1 = snoisefloat3( p + dy );
                float3 p_z0 = snoisefloat3( p - dz );
                float3 p_z1 = snoisefloat3( p + dz );

                float x = p_y1.z - p_y0.z - p_z1.y + p_z0.y;
                float y = p_z1.x - p_z0.x - p_x1.z + p_x0.z;
                float z = p_x1.y - p_x0.y - p_y1.x + p_y0.x;

                const float divisor = 1.0 / ( 2.0 * e );
                return normalize( float3( x , y , z ) * divisor );

            }

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _TextureOpacity;
            float _TextureYScale;
            float _MasterOpacity;
            float _NoiseSpeed;
            float _NoiseOpacity;
            float _NoiseScale;
            float _ScrollSpeed;
            float _ScrollInterval;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
               
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 texCol = tex2D(_MainTex, (i.uv * float2(_TextureYScale, 1.0))  + float2(-1.0f + ((_Time.y * _ScrollSpeed)  % _ScrollInterval), 0.0));
                // noise texture
                float3 noiseOut = curlNoise(float3(i.uv.xy * _NoiseScale, (_Time.y * _NoiseSpeed)));
                // noiseOut.yz *= noiseOut.x;
                noiseOut.y = saturate(noiseOut.y);
                noiseOut.z = saturate(noiseOut.z);
                noiseOut.x = 0.0;   // remove all the red

                if (texCol.r > 0.0f){
                    return texCol * _MasterOpacity;
                } else {
                    return float4(noiseOut * _NoiseOpacity * _MasterOpacity, 1.0f);
                }

                //  return float4(noiseOut * _NoiseOpacity * _MasterOpacity, 1.0f);
                
            }
            ENDCG
        }
    }
}
