using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace UAvatar
{
    [CustomEditor(typeof(Morph))]
    public class MorphEditor : Editor
    {
        bool showAdvancedGUI;
        public override void OnInspectorGUI()
        {
            //base.OnInspectorGUI();
            serializedObject.Update();
            
            Morph morph = (Morph)target;
            EditorGUILayout.LabelField("uMaterial Target", EditorStyles.boldLabel);

            EditorGUI.BeginChangeCheck();
            morph.materialSelect = EditorGUILayout.IntSlider("Material Preset", morph.materialSelect, 0, morph.headMaterialList.Length-1);
            if (EditorGUI.EndChangeCheck())
            {
                morph.materialSelect = morph.materialSelect;
                morph.SetMaterial(morph.materialSelect);
            }
            
            //Hair Slider
            GUILayout.Space(10);
            EditorGUILayout.LabelField("Hair", EditorStyles.boldLabel);
            EditorGUI.BeginChangeCheck();
            morph.hairSelect = EditorGUILayout.IntSlider("HairType", morph.hairSelect, 0, morph.hairList.Length);
            if (EditorGUI.EndChangeCheck())
            {
                morph.SetHair(morph.hairSelect);
            }
            //Texture Slider
            //morph.headMaterial = (Material)EditorGUILayout.ObjectField("Head Material", morph.headMaterial, typeof(Material), true);
            GUILayout.Space(10);
            EditorGUILayout.LabelField("Textures", EditorStyles.boldLabel);
            EditorGUI.BeginChangeCheck();
            morph.textureSelect = EditorGUILayout.IntSlider("Skin Select", morph.textureSelect, 0, morph.textureList.Length-1);
            if (EditorGUI.EndChangeCheck())
            {
                morph.SetTexture(morph.textureSelect);
            }
            EditorGUI.BeginChangeCheck();
            morph.normalMapSelect = EditorGUILayout.IntSlider("Wrinkle Select", morph.normalMapSelect, 0, morph.normalMapList.Length-1);
            morph.normalMapValue = EditorGUILayout.Slider("Wrinkle Intensity", morph.normalMapValue, 0f, 1f);
            if (EditorGUI.EndChangeCheck())
            {
                morph.SetNormalMap(morph.normalMapSelect, morph.normalMapValue);
            }
            //HeightSlider
            GUILayout.Space(10);
            EditorGUILayout.LabelField("Height", EditorStyles.boldLabel);
            EditorGUI.BeginChangeCheck();
            morph.rootScale = EditorGUILayout.Slider("Height", morph.rootScale, 1.25f, 2.5f);
            if (EditorGUI.EndChangeCheck())
            {
                morph.SetHeight(morph.rootScale);
            }

            //face morph
            GUILayout.Space(10);
            EditorGUILayout.LabelField("Blendshapes", EditorStyles.boldLabel);
            EditorGUI.BeginChangeCheck();
            morph.faceBlendShapeSelect = EditorGUILayout.IntSlider("Face Type", morph.faceBlendShapeSelect, morph.faceMin, morph.faceMax); //54, 57
            morph.faceBlendShapeAmount = EditorGUILayout.Slider("Face BlendShape Amount", morph.faceBlendShapeAmount, 0f, 100f);
            morph.SetBlendShapeFeatures(morph.faceBlendShapeSelect, morph.faceBlendShapeAmount);
            morph.cornerEyeAmount = EditorGUILayout.Slider("Corner Eye Amount", morph.cornerEyeAmount, 0f, 100f);
            if (EditorGUI.EndChangeCheck())
            {
                morph.SetEyeCorner(morph.cornerEyeAmount);
            }
            EditorGUI.BeginChangeCheck();
            morph.weightBlendShape = EditorGUILayout.Slider("Weigth Offset", morph.weightBlendShape, -100f, 100f);
            if (EditorGUI.EndChangeCheck())
            {
                morph.SetWeightBlendShape(morph.weightBlendShape);
            }

            //droop morph
            GUILayout.Space(10);
            EditorGUILayout.LabelField("Droop", EditorStyles.boldLabel);
            EditorGUI.BeginChangeCheck();
            morph.droop = EditorGUILayout.Toggle("Droop", morph.droop);
            if (morph.droop)
            {
                morph.droopLeftAmount = EditorGUILayout.Slider("Droop Left Amount", morph.droopLeftAmount, 0f, 100f);
                morph.droopRightAmount = EditorGUILayout.Slider("Droop Right Amount", morph.droopRightAmount, 0f, 100f);
            }
            if (EditorGUI.EndChangeCheck())
            {
                morph.SetDroop(morph.droop, morph.droopLeftAmount, morph.droopRightAmount);
            }

            //randomize
            GUILayout.Space(10);
            EditorGUILayout.LabelField("Randomizer", EditorStyles.boldLabel);
            if (GUILayout.Button("Randomize Traits"))
            {
                morph.RandomTraits();
            }
            GUILayout.Space(10);
            showAdvancedGUI = EditorGUILayout.Toggle("Show Advanced Settings", showAdvancedGUI);
            if (showAdvancedGUI)
            {
                EditorGUI.BeginChangeCheck();
                base.OnInspectorGUI();
                if (EditorGUI.EndChangeCheck())
                {
                    morph.materialSelect = morph.materialSelect;
                    morph.SetMaterial(morph.materialSelect);
                    morph.SetHair(morph.hairSelect);
                    morph.SetTexture(morph.textureSelect);
                    morph.SetNormalMap(morph.normalMapSelect, morph.normalMapValue);
                    morph.SetHeight(morph.rootScale);
                    morph.SetEyeCorner(morph.cornerEyeAmount);
                    morph.SetWeightBlendShape(morph.weightBlendShape);
                    morph.SetDroop(morph.droop, morph.droopLeftAmount, morph.droopRightAmount);
                }
            }
            serializedObject.ApplyModifiedProperties();
        }

        
    }
}
