Shader "Hidden/LensEffects(Advance)" {
Properties {
	_MainTex     ("Base (RGB)",  2D)           = "white" {}
	_Lens   ("Lens Bloom",       2D)           = "black" {}
	_LensBloom("Lens Scale", Range(-10,10))= 1
	_LensDirt    ("Lens Dirt",        2D)           = "black" {} 
	_LensDiffraction    ("Lens Diffraction",        2D)           = "black" {} 
	_Threshold   ("Threshold",   Range (0,1))  = 1.5
	_LensIntensity   ("Lens Intensity",       Range(-10,10))= 1
	_Desaturate  ("Desaturate",  Range(0,1))   = 0.4
	_DiffractionIntensity   ("Defraction Intensity",       Range(-10,10))= 1
	_DiffractionTint ("Diffraction Tint", Color) = (1,1,1,255)
}

	CGINCLUDE
	#include "UnityCG.cginc"
	uniform sampler2D _MainTex;
	uniform sampler2D _Lens;
	float     _LensBloom;
	uniform sampler2D _LensDirt;
	uniform sampler2D _LensDiffraction;
	float     _Threshold;
	float     _LensIntensity;
	float     _Desaturate;
	float     _DiffractionIntensity;
	half4 			  _DiffractionTint;
			
	struct v2f {
				float4 pos : POSITION;
				float2 uv : TEXCOORD0;
			};
			
	fixed4 frag_Threshold (v2f_img IN) : SV_Target
	{
		fixed4 tex = tex2D(_MainTex, IN.uv);
		
		fixed2 uv = float2(1,1) - IN.uv;
		
		fixed4 output = max( float4(0,0,0,0), tex2D(_MainTex, uv) - _Threshold ) * _LensIntensity;
		
		output = Luminance(output.rgb) * _Desaturate + output * (float4(1,1,1,1)-_Desaturate);
		
		return output;
	}
	
	fixed4 frag_ThresholdAndLensBloom (v2f_img IN) : SV_Target
	{
		fixed4 tex = tex2D(_MainTex, IN.uv);
		
		fixed4 bloom = max( float4(0,0,0,0), tex - _Threshold ) * _LensBloom;
		
		fixed2 uv = float2(1,1) - IN.uv;
		
		fixed4 output = max( float4(0,0,0,0), tex2D(_MainTex, uv) - _Threshold ) * _LensIntensity;
		
		output = Luminance(output.rgb) * _Desaturate + output * (float4(1,1,1,1)-_Desaturate);
		
		output += bloom;
		return output;
	}
	
	fixed4 frag_LensBloom (v2f_img IN) : SV_Target
	{
		fixed4 tex = tex2D(_MainTex, IN.uv);
		fixed4 bloom = max( float4(0,0,0,0), tex - _Threshold ) * _LensBloom ;
		return bloom;
	}
				
	fixed4 frag_DirtAndDiffraction (v2f_img IN) : SV_Target
	{
		fixed4 output = tex2D(_MainTex, IN.uv) + (tex2D(_LensDirt, IN.uv) + (tex2D(_LensDiffraction, IN.uv))/ _DiffractionIntensity * _DiffractionTint) * tex2D(_Lens, IN.uv);
		return output;
	}
	
	ENDCG
	
SubShader {
	
	ZTest Always Cull Off ZWrite Off
	Fog { Mode off }
	
	Pass
	{
		CGPROGRAM
		#pragma vertex vert_img
		#pragma fragment frag_Threshold
		#pragma fragmentoption ARB_precision_hint_fastest 
		ENDCG
	}
	
	Pass
	{
		CGPROGRAM
		#pragma vertex vert_img
		#pragma fragment frag_ThresholdAndLensBloom
		#pragma fragmentoption ARB_precision_hint_fastest 
		ENDCG
	}
	
	Pass
	{	
		CGPROGRAM
		#pragma vertex vert_img
		#pragma fragment frag_LensBloom
		#pragma fragmentoption ARB_precision_hint_fastest 
		ENDCG
	}
	
	Pass 
	{	
		CGPROGRAM
		#pragma vertex vert_img
		#pragma fragment frag_DirtAndDiffraction
		#pragma fragmentoption ARB_precision_hint_fastest 
		ENDCG
	}
	
}

Fallback off

}