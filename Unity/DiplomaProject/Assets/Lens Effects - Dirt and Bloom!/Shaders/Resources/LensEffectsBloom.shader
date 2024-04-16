// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Hidden/LensEffects/Bloom" {
	Properties {
		_MainTex ("Screen Blended", 2D) = "" {}
		_Color ("Color", 2D) = "" {}
		_Tint ("Tint Color", Color) = (1,1,1,255)
	}
	
	CGINCLUDE

	#include "UnityCG.cginc"
	
	struct v2f_i {
		float4 pos : SV_POSITION;
		float2 uv[2] : TEXCOORD0;
	};
	struct v2f_o {
		float4 pos : SV_POSITION;
		float2 uv[5] : TEXCOORD0;
	};
			
	sampler2D _Color;
	sampler2D _MainTex;
	
	half _BloomIntensity;
	half4 _Tint;
	half4 _Color_TexelSize;
	half4 _MainTex_TexelSize;
		
	v2f_i vert( appdata_img v ) {
		v2f_i o;
		o.pos = UnityObjectToClipPos(v.vertex);
		o.uv[0] =  v.texcoord.xy;
		o.uv[1] =  v.texcoord.xy;
		
		#if UNITY_UV_STARTS_AT_TOP
		if (_Color_TexelSize.y < 0) 
			o.uv[1].y = 1-o.uv[1].y;
		#endif	
		
		return o;
	}

	v2f_o vertMultiTap( appdata_img v ) {
		v2f_o o;
		o.pos = UnityObjectToClipPos(v.vertex);
		o.uv[4] = v.texcoord.xy;
		o.uv[0] = v.texcoord.xy + _MainTex_TexelSize.xy * 0.5;
		o.uv[1] = v.texcoord.xy - _MainTex_TexelSize.xy * 0.5;	
		o.uv[2] = v.texcoord.xy - _MainTex_TexelSize.xy * half2(1,-1) * 0.5;	
		o.uv[3] = v.texcoord.xy + _MainTex_TexelSize.xy * half2(1,-1) * 0.5;	
		return o;
	}
	
	half4 frag (v2f_i IN) : SV_Target {
		half4 bloom = tex2D(_MainTex, IN.uv[0].xy) * _BloomIntensity;
		half4 samplecolor = tex2D(_Color, IN.uv[1]);
		return 1-(1-bloom)*(1-samplecolor);
	}

	half4 frag_Add (v2f_i IN) : SV_Target {
		half4 bloom = tex2D(_MainTex, IN.uv[0].xy);
		half4 samplecolor = tex2D(_Color, IN.uv[1]);
		return _BloomIntensity * bloom + samplecolor;
	}

	half4 frag_clr (v2f_i IN) : SV_Target {
		return 0;
	}

	half4 frag_AddOn (v2f_i IN) : SV_Target {
		half4 inputColor = tex2D(_MainTex, IN.uv[0].xy);
		return inputColor * _BloomIntensity * ( _Tint/12);
	}

	half4 frag_temp (v2f_i IN) : SV_Target {
		return tex2D(_MainTex, IN.uv[0].xy);
	}
	
	half4 frag_Max (v2f_o IN) : SV_Target {
		half4 outColor = tex2D(_MainTex, IN.uv[4].xy);
		outColor = max(outColor, tex2D(_MainTex, IN.uv[0].xy));
		outColor = max(outColor, tex2D(_MainTex, IN.uv[1].xy));
		outColor = max(outColor, tex2D(_MainTex, IN.uv[2].xy));
		outColor = max(outColor, tex2D(_MainTex, IN.uv[3].xy));
		return outColor;
	}

	half4 frag_Blur (v2f_o IN) : SV_Target {
		half4 outColor = 0;
		outColor += tex2D(_MainTex, IN.uv[0].xy);
		outColor += tex2D(_MainTex, IN.uv[1].xy);
		outColor += tex2D(_MainTex, IN.uv[2].xy);
		outColor += tex2D(_MainTex, IN.uv[3].xy);
		return outColor/4;
	}

	ENDCG 
	
Subshader {
	  ZTest Always Cull Off ZWrite Off

 Pass {    

      CGPROGRAM
      #pragma vertex vert
      #pragma fragment frag
      ENDCG
  }
 Pass {    

      CGPROGRAM
      #pragma vertex vert
      #pragma fragment frag_Add
      ENDCG
  }
 Pass {    

      CGPROGRAM
      #pragma vertex vertMultiTap
      #pragma fragment frag_Max
      ENDCG
  } 
 Pass {    

      CGPROGRAM
      #pragma vertex vertMultiTap
      #pragma fragment frag_Blur
      ENDCG
  }   
 Pass {    
 	  
      CGPROGRAM
      #pragma vertex vert
      #pragma fragment frag_clr
      ENDCG
  }   
 Pass {    

 	  Blend One One
 	  
      CGPROGRAM
      #pragma vertex vert
      #pragma fragment frag_AddOn
      ENDCG
  }  
 Pass {    

 	  Blend One One
 	  
      CGPROGRAM
      #pragma vertex vert
      #pragma fragment frag_temp
      ENDCG
  }
}

Fallback off
	
}