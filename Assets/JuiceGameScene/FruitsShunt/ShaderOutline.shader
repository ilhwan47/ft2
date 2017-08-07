// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Sprite/SpriteOutline" {
    Properties
    {
        _MainTex  ("-", 2D) = "white"{}
        _Distance ("-", Float) = 1
        _Color    ("-", Color) = (1, 1, 1, 1)
    }
    Subshader
    {
		Tags {"Queue" = "Transparent" "RenderType" = "Transparent"}
		ZTest Always 
		Cull Off 
		ZWrite Off
		Blend One OneMinusSrcAlpha
		Fog { Mode off }   
		
        Pass
        {
            CGPROGRAM
            #pragma vertex vert_img
            #pragma fragment frag

			//CGINCLUDE
			#include "UnityCG.cginc"
			sampler2D _MainTex;
			
			struct v2f{
				float4 pos : SV_POSITION;
				half2 uv : TEXCOORD0;
			};
			
			v2f vert(appdata_base v){
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.uv = v.texcoord;
				return o;
			}
			
			float4 _MainTex_TexelSize;
			float _Distance;
			half4 _Color;
			
			fixed4 frag(v2f_img i) : COLOR 
			{
			    // Simple sobel filter for the alpha channel.
			
				half4 source = tex2D(_MainTex, i.uv);
				//source.a = 0;
				source.rgb *= source.a;
			
				half4 outlineC = _Color;
				outlineC.a *= ceil(source.a);
				outlineC.rgb *= outlineC.a;
			
			    float d = _MainTex_TexelSize.xy * _Distance;

				/*fixed alpha_up = tex2D(_MainTex, i.uv + d + fixed2(0, _MainTex_TexelSize.y)).a;
				fixed alpha_down = tex2D(_MainTex, i.uv - d - fixed2(0, _MainTex_TexelSize.y)).a;
				fixed alpha_right = tex2D(_MainTex, i.uv + d + fixed2(_MainTex_TexelSize.x, 0)).a;
				fixed alpha_left = tex2D(_MainTex, i.uv - d - fixed2(_MainTex_TexelSize.x, 0)).a;*/
			
			    half a1 = tex2D(_MainTex, i.uv + d * float2(-1, -1)).a;
			    half a2 = tex2D(_MainTex, i.uv + d * float2( 0, -1)).a;
			    half a3 = tex2D(_MainTex, i.uv + d * float2(+1, -1)).a;
			    half a4 = tex2D(_MainTex, i.uv + d * float2(-1,  0)).a;
			    half a6 = tex2D(_MainTex, i.uv + d * float2(+1,  0)).a;
			    half a7 = tex2D(_MainTex, i.uv + d * float2(-1, +1)).a;
			    half a8 = tex2D(_MainTex, i.uv + d * float2( 0, +1)).a;
			    half a9 = tex2D(_MainTex, i.uv + d * float2(+1, +1)).a;
			
			    float gx = - a1 - a2*2 - a3 + a7 + a8*2 + a9;
			    float gy = - a1 - a4*2 - a7 + a3 + a6*2 + a9;
			
			    float w = sqrt(gx * gx + gy * gy) / 1;
			
			    // Mix the contour color.
			
				//return half4(lerp(outlineC.rgb, source, ceil(a1 * a2 * a3 * a4 * a6 * a7 * a8 * a9)), 1);
			    //return half4(lerp(source.rgb, outlineC.rgb, w), 1);
				//return lerp(outlineC, source, ceil(alpha_up * alpha_down * alpha_right * alpha_left));
				return lerp(source, outlineC, w);
			}
			ENDCG 
        }
    }
	FallBack "Diffuse"
}