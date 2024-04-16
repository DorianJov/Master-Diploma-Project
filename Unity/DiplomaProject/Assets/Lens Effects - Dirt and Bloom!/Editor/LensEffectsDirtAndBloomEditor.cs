using UnityEngine;
using UnityEditor;
using System.Collections;


[CustomEditor(typeof(LensEffectsDirtAndBloom))] 
public class LensEffectsDirtAndBloomEditor : Editor  {
	
	public Texture2D m_logo;
	SerializedObject   serObj;
	SerializedProperty Effect;
	SerializedProperty LensType;
	SerializedProperty dirtTexture;
	SerializedProperty diffractionTexture;
	SerializedProperty saturation;
	SerializedProperty lensIntensity;
	SerializedProperty lensBloom;
	SerializedProperty diffractionIntensity;
	SerializedProperty diffractionTint;
	SerializedProperty bloomIntensity;
	SerializedProperty bloomTint;
	SerializedProperty chromaticIntensity;
	SerializedProperty threshold;
	SerializedProperty downsample;
	SerializedProperty haloIntensity;
	SerializedProperty blurIntensity;
	
	
	GUIStyle style;
	
	void OnEnable()
	{
		m_logo	= (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/Lens Effects/Editor/Graphics/le_logo.png", typeof(Texture2D));
      		
		serObj         = new SerializedObject (target);
		Effect 	= serObj.FindProperty("Effect");
		LensType 	= serObj.FindProperty("LensType");
		dirtTexture    = serObj.FindProperty("dirtTexture");
		diffractionTexture    = serObj.FindProperty("diffractTexture");
		saturation     = serObj.FindProperty("saturation");
		lensIntensity = serObj.FindProperty("lensIntensity");
		lensBloom 	= serObj.FindProperty("lensBloom");
		diffractionIntensity = serObj.FindProperty("diffractIntensity");
		diffractionTint      = serObj.FindProperty("diffractTint");
		bloomIntensity      = serObj.FindProperty("bloomIntensity");
		bloomTint      = serObj.FindProperty("bloomTint");
		chromaticIntensity      = serObj.FindProperty("chromaticIntensity");
		threshold      = serObj.FindProperty("threshold");
		downsample     = serObj.FindProperty("downsample");
		haloIntensity     = serObj.FindProperty("haloIntensity");
		blurIntensity = serObj.FindProperty("blurIntensity");
	}
	
	public override void OnInspectorGUI () {
        
		serObj.Update();
		
		if (m_logo != null)
        {
            Rect rect = GUILayoutUtility.GetRect(m_logo.width, m_logo.height);
            GUI.DrawTexture(rect, m_logo, ScaleMode.ScaleToFit);
        }
		
		GUILayout.BeginVertical("Box");
		
		EditorGUILayout.PropertyField (Effect, new GUIContent("Lens Effect"));
		
		GUILayout.Space(5.0f);
        GUILayout.EndVertical();
		
		GUILayout.BeginVertical("Box");
        GUILayout.Label("Lens Effect Settings", EditorStyles.boldLabel);
				
		if( Effect.enumValueIndex == 2 || Effect.enumValueIndex == 3 || Effect.enumValueIndex == 4 )
		{
		EditorGUILayout.PropertyField (dirtTexture, new GUIContent("Dirt Texture"));
		saturation.floatValue     = EditorGUILayout.Slider ("Lens Saturation", saturation.floatValue,     -2.0f, 2.0f);
		lensIntensity.floatValue = EditorGUILayout.Slider ("Lens Intensity",  lensIntensity.floatValue,  0.0f, 10.0f);
		lensBloom.floatValue = EditorGUILayout.Slider ("Lens Bloom",  lensBloom.floatValue,  0.0f, 15.0f);
		}
		
		if ( Effect.enumValueIndex == 0 )
		{
			bloomIntensity.floatValue = EditorGUILayout.Slider ("Bloom Intensity",  bloomIntensity.floatValue,  0.0f, 3.0f);
			bloomTint.colorValue = EditorGUILayout.ColorField ("Bloom Tint", bloomTint.colorValue);
		}	
		else if ( Effect.enumValueIndex == 1 )
		{
			chromaticIntensity.floatValue = EditorGUILayout.Slider ("Chrom Intensity",  chromaticIntensity.floatValue,  0.0f, 2.0f);
		}
		else if ( Effect.enumValueIndex == 3 )
		{
			bloomIntensity.floatValue = EditorGUILayout.Slider ("Bloom Intensity",  bloomIntensity.floatValue,  0.0f, 3.0f);
			bloomTint.colorValue = EditorGUILayout.ColorField ("Bloom Tint", bloomTint.colorValue);
		}
		else if	( Effect.enumValueIndex == 4 )
		{
			chromaticIntensity.floatValue = EditorGUILayout.Slider ("Chrom Intensity",  chromaticIntensity.floatValue,  0.0f, 2.0f);
		}
		
		GUILayout.Space(5.0f);
        GUILayout.EndVertical();
		
		if (Effect.enumValueIndex == 2 || Effect.enumValueIndex == 3 || Effect.enumValueIndex == 4 )
        {
		
		GUILayout.BeginVertical("Box");
        GUILayout.Label("Diffraction Settings", EditorStyles.boldLabel);

		EditorGUILayout.PropertyField (diffractionTexture, new GUIContent("Diffract Texture"));	
		diffractionIntensity.floatValue = EditorGUILayout.Slider ("Diffract Intensity",  diffractionIntensity.floatValue,  3.0f, 0.75f);
		diffractionTint.colorValue = EditorGUILayout.ColorField ("Diffract Tint", diffractionTint.colorValue);

			
		GUILayout.Space(5.0f);
        GUILayout.EndVertical();
			
		GUILayout.BeginVertical("Box");
        GUILayout.Label("Advance Settings", EditorStyles.boldLabel);
		
		EditorGUILayout.PropertyField (LensType, new GUIContent("Lens Type"));
		threshold.floatValue     = EditorGUILayout.Slider ("Threshold", threshold.floatValue, 0.0f, 1.0f);
		downsample.intValue = EditorGUILayout.IntField("Downsample", downsample.intValue);
		downsample.intValue = Mathf.Clamp(downsample.intValue, 1, 12);
			
		haloIntensity.floatValue = EditorGUILayout.Slider ("Halo Intensity",      haloIntensity.floatValue,  0.0f, 0.75f);
		blurIntensity.intValue   = EditorGUILayout.IntSlider ("Blur Intensity",  blurIntensity.intValue,    1, 8);
		
		GUILayout.Space(5.0f);
        GUILayout.EndVertical();
		}
		serObj.ApplyModifiedProperties();
		
    }
}
