Shader "RimLight"
{
    Properties {
		_Color ("Main Color", Color) = (1,1,1,1)
    	_MainTex ("Texture", 2D) = "white" {}
    	_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
    	_RimColor ("Rim Color", Color) = (0.91,0.86,0.75,0.0)
    	_RimPower ("Rim Power", Range(0.5,8.0)) = 8.0
    	[MaterialEnum(Off,0,Front,1,Back,2)] _Cull ("Cull", Int) = 2
    }
    
    SubShader {
      Tags {"Queue"="AlphaTest" "IgnoreProjector"="True" "RenderType"="TransparentCutout"}

      Cull [_Cull]

      CGPROGRAM
      #pragma surface surf Lambert alphatest:_Cutoff
      
      half4 LightingWrapLambert (SurfaceOutput s, half3 lightDir, half atten) {
        half NdotL = dot (s.Normal, lightDir);
        half diff = NdotL * 0.5 + 0.5;
        half4 c;
        c.rgb = s.Albedo * _LightColor0.rgb * (diff * atten);
        c.a = s.Alpha;
        return c;
      }
      
      struct Input {
          float2 uv_MainTex;
          float3 viewDir;
      };
      sampler2D _MainTex;
      fixed4 _Color;
      float4 _RimColor;
      float _RimPower;
      void surf (Input IN, inout SurfaceOutput o) {
	      fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
	      o.Albedo = c.rgb;
	      o.Alpha = c.a;
	      half rim = 1.0 - saturate(dot (normalize(IN.viewDir), o.Normal));
	      o.Emission = _RimColor.rgb * pow (rim, _RimPower);
      }
      ENDCG
    } 
    Fallback "Diffuse"
}
