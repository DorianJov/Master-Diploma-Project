// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Hidden/LensEffects/ChromaticAberration" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_ChromaticIntensity   ("Chromatic Intensity",       Range(0,10))= 0
	}
	
	CGINCLUDE
	#include "UnityCG.cginc"

		uniform sampler2D _MainTex;
		float _ChromaticIntensity;
		
		struct v2f_c {
				float4 pos : POSITION;
				float2 uv : TEXCOORD0;
			};
			
				v2f_c vert_i ( appdata_img v ) {
				v2f_c o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.uv = v.texcoord.xy;
				return o;
				}
			
				half4 frag_chrom ( v2f_c IN ) : SV_Target {
				half4 col = tex2D(_MainTex, IN.uv);
				
				half2 realCoordOffs;
				half2 coords = IN.uv;
				coords = (coords - 0.5) * 2.0;
				
				realCoordOffs.x = coords.x * 0.1 * 0.1;
				realCoordOffs.y = coords.y * 0.1 * 0.1;
				
				half red = tex2D(_MainTex, IN.uv - realCoordOffs).r;
				half green = tex2D(_MainTex, IN.uv - realCoordOffs * 1.5).g;
				half blue = tex2D(_MainTex, IN.uv - realCoordOffs * 2).b;
				half red2 = tex2D(_MainTex, IN.uv - realCoordOffs * 2.4).r;
				half green2 = tex2D(_MainTex, IN.uv - realCoordOffs * 2.8).g;
				half blue2 = tex2D(_MainTex, IN.uv - realCoordOffs * 3.2).b;
				half4 chroma = half4(red,0,0,1) + half4(0,green,0,1) + half4(0,0,blue,1);
				half4 chroma2 = half4(red2,0,0,1) + half4(0,green2,0,1) + half4(0,0,blue2,1);
				
				half4 chromaFinal = chroma + chroma2 * _ChromaticIntensity;
				
				half4 finalCol = lerp(chromaFinal/2.5 ,col,Luminance(col.rgb));
				return finalCol;
			}

	
		ENDCG
	
SubShader {
	
	ZTest Always Cull Off ZWrite Off
	Fog { Mode off }
	
	Pass
	{	
		CGPROGRAM
		#pragma vertex vert_i
		#pragma fragment frag_chrom
		#pragma target 3.0
			
		
		ENDCG
	}
	
}

Fallback off

}