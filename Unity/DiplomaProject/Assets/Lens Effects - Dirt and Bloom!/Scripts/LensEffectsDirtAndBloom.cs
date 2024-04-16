using UnityEngine;
using System.Collections;

public enum LensEffect { 
    BloomOnly = 0,
	ChromaticAberrationOnly = 1,
	DirtOnly = 2,
	DirtAndBloom = 3,
	DirtAndChromaticAberration = 4,
}

public enum Lens { 
    BlurLens = 0,
	ChromaticLens = 1,
}

[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
[AddComponentMenu("Image Effects/Lens Effects/Dirt and Bloom")]

public class LensEffectsDirtAndBloom : MonoBehaviour {
	
	public LensEffect Effect;
	public Lens LensType;
	public Texture2D dirtTexture;
	public Texture2D diffractTexture;
	public float saturation     = 0.9f;
	public float lensIntensity = 2.5f;
	public float lensBloom = 1.0f;
	public float diffractIntensity = 2.5f;
	public Color diffractTint = new Color(1,1,1,0);
	public float bloomIntensity = 2.0f;
	public float chromaticIntensity = 2.0f;
	public Color bloomTint = new Color(1,1,1,0);
	public float haloIntensity = 0.6f;
	public int   blurIntensity = 10;
	public float threshold  = 0.5f;
	public int   downsample = 6;
	public int lensReflection = 2; 
	//public int reflect2 = 2;
	
	
	private Shader   blurShader;
	private Material blurMaterial;
	
	private Shader   bloomShader;
	private Material bloomMaterial;
	
	public Shader chromShader;
	private Material chromMaterial;
	
	private Shader   shader;
	private Material dirtMaterial;
	protected Material material {
		get {
			if (dirtMaterial == null) {
				dirtMaterial = new Material (shader);
				dirtMaterial.hideFlags = HideFlags.HideAndDontSave;
			}
			return dirtMaterial;
		} 
	}

	
	protected virtual void Start ()
	{
		Validation();
		
		if (!SystemInfo.supportsImageEffects) {
			enabled = false;
			return;
		}
		
		if (!shader || !shader.isSupported)
			enabled = false;
		if (!blurShader || !blurShader.isSupported)
			enabled = false;
		if (!bloomShader || !bloomShader.isSupported)
			enabled = false;
		if (!chromShader || !chromShader.isSupported)
			enabled = false;
	}
		
	
	bool Validation()
	{
		
		if(!shader)
		{
			shader = Shader.Find("Hidden/LensEffects(Advance)");
			if(!shader)
				return false;
		}
		
		if(!blurShader)
		{
			blurShader = Shader.Find("Hidden/LensEffects/Blur"	);
			if(!blurShader)
				return false;
		}
		
		if(!blurMaterial)
		{
			blurMaterial = new Material(blurShader);
			blurMaterial.hideFlags = HideFlags.HideAndDontSave;
			if(!blurMaterial)
				return false;
		}
		
		if(!bloomShader)
		{
			bloomShader = Shader.Find("Hidden/LensEffects/Bloom"	);
			if(!bloomShader)
				return false;
		}
		
		if(!bloomMaterial)
		{
			bloomMaterial = new Material(bloomShader);
			bloomMaterial.hideFlags = HideFlags.HideAndDontSave;
			if(!bloomMaterial)
				return false;
		}

		if(!chromShader)
		{
			chromShader = Shader.Find("Hidden/LensEffects/ChromaticAberration"	);
			if(!chromShader)
				return false;
		}
		
		if(!chromMaterial)
		{
			chromMaterial = new Material(chromShader);
			chromMaterial.hideFlags = HideFlags.HideAndDontSave;
			if(!chromMaterial)
				return false;
		}

		return true;
	}
	
	protected virtual void OnDisable() {
		if( dirtMaterial ) {
			DestroyImmediate( dirtMaterial );
		}
		if( blurMaterial ) {
			DestroyImmediate( blurMaterial );
		}
		if( bloomMaterial ) {
			DestroyImmediate( bloomMaterial );
		}
		if( chromMaterial ) {
			DestroyImmediate( chromMaterial );
		}
	}
	
	
	// Calculate blur iterations.
	public void FourTapCone (RenderTexture source, RenderTexture dest, int blurIntensity, Material chromMaterial)
	{
		float off = 0.5f + blurIntensity*haloIntensity;
		Graphics.BlitMultiTap (source, dest, chromMaterial,
			new Vector2(-off, -off),
			new Vector2(-off,  off),
			new Vector2( off,  off),	
			new Vector2( off, -off)
		);
	}
	
	// Apply bloom effect
	private void BloomEffect (RenderTexture source, RenderTexture dest)
	{
		float off = 1.0f;
		Graphics.BlitMultiTap (source, dest, bloomMaterial,
			new Vector2(-off, -off),
			new Vector2(-off,  off),
			new Vector2( off,  off),
			new Vector2( off, -off)
		);
	}
	
	// Apply chrom effect
	private void ChromEffect (RenderTexture source, RenderTexture dest)
	{
		float off = 1.0f;
		Graphics.BlitMultiTap (source, dest, chromMaterial,
			new Vector2(-off, -off),
			new Vector2(-off,  off),
			new Vector2( off,  off),
			new Vector2( off, -off)
		);
	}

	// Apply blur effect
	private void BlurEffect(RenderTexture source, RenderTexture dest, Material blurMaterial)
	{
		
		downsample = Mathf.Clamp(downsample, 1, 12);
		
		RenderTexture temp  = RenderTexture.GetTemporary(source.width / downsample, source.height / downsample, 0);
		RenderTexture temp2 = RenderTexture.GetTemporary(source.width / downsample, source.height / downsample, 0);
		
		// Copy source into temp buffer
		Graphics.Blit(source, temp);
		
		bool buffer = true;
		
		for(int i = 0; i < blurIntensity; i++)
		{
			if(buffer)
				FourTapCone (temp, temp2, i, blurMaterial);
			else
				FourTapCone (temp2, temp, i, blurMaterial);
			buffer = !buffer;
		}
		
		if(buffer)
			Graphics.Blit(temp, dest);
		else
			Graphics.Blit(temp2, dest);
		
		// Release temp images
		RenderTexture.ReleaseTemporary(temp);
		RenderTexture.ReleaseTemporary(temp2);
	}
	
	// Apply image effect
	void OnRenderImage (RenderTexture source, RenderTexture dest) {
		
			threshold = Mathf.Clamp01(threshold);
			lensIntensity = Mathf.Clamp(lensIntensity,0,15);
			
		
		if( Validation() )
		{
			material.SetFloat("_Threshold",  threshold);
			material.SetFloat("_LensIntensity",      lensIntensity);
			material.SetFloat("_LensBloom", lensBloom);
			material.SetFloat("_DiffractionIntensity",      diffractIntensity);
			material.SetColor("_DiffractionTint",      diffractTint);
			material.SetFloat("_Desaturate", 1.0f-saturation);
			
			
			
			RenderTexture downSampled = RenderTexture.GetTemporary (source.width /4, source.height /4, 0, RenderTextureFormat.Default);
						
			// Create chrom image
			RenderTexture chrom = RenderTexture.GetTemporary (source.width, source.height, 0, RenderTextureFormat.Default);
			
			// Apply chrom
			ChromEffect (source, chrom);
		
			chromMaterial.SetFloat("_ChromaticIntensity", chromaticIntensity);
			Graphics.Blit(source, chrom, chromMaterial);
			
			
			// Create bloom image
			RenderTexture bloom = RenderTexture.GetTemporary(source.width, source.height, 0);
			
			// Apply bloom
			BloomEffect (source, bloom);
		
			bloomMaterial.SetFloat("_BloomIntensity", bloomIntensity);
			bloomMaterial.SetColor("_Tint", bloomTint);
			Graphics.Blit (bloom, downSampled, material, 1);					
			
					
			// Create blur image
			RenderTexture blur = RenderTexture.GetTemporary (downSampled.width, downSampled.height, 0, RenderTextureFormat.Default);	
						
			// Select and apply blur
			switch(LensType)
			{
			case Lens.BlurLens:
				BlurEffect(downSampled, blur, blurMaterial);
				break;
			case Lens.ChromaticLens:
				BlurEffect(downSampled, blur, chromMaterial);
				break;
			}
			
			material.SetTexture("_Lens", blur);
			Graphics.Blit(source, downSampled, material, 1);
			material.SetTexture("_LensDirt", dirtTexture);
			material.SetTexture("_LensDiffraction", diffractTexture);
			Graphics.Blit (source, dest, material, 3);
			
			
			switch(Effect)
			{
			case LensEffect.BloomOnly:
				Graphics.Blit(source, dest, bloomMaterial); // Bloom only
				break;
			case LensEffect.ChromaticAberrationOnly:
				Graphics.Blit(source, dest, chromMaterial); // Chromatic Aberration only
				break;
			case LensEffect.DirtOnly:
				Graphics.Blit(source, dest, material, 3); // Dirt only
				break;
			case LensEffect.DirtAndBloom:
				Graphics.Blit(bloom, dest, material, 3); // Bloom + Dirt
				break;
			case LensEffect.DirtAndChromaticAberration:
				Graphics.Blit(chrom, dest, material, 3); // Chrom + Dirt
				break;
			}
		
			// Release temporary images
			RenderTexture.ReleaseTemporary (downSampled);
			RenderTexture.ReleaseTemporary (blur);
			RenderTexture.ReleaseTemporary(bloom);
			RenderTexture.ReleaseTemporary(chrom);	
		}
		
	}
	
}
