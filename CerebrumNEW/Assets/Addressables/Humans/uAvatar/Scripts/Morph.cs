using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UAvatar
{
    public class Morph : MonoBehaviour
    {
        public GameObject[] hairList;
        public SkinnedMeshRenderer faceSkinnedMeshRenderer;
        public SkinnedMeshRenderer bodySkinnedMeshRenderer;
        public Material[] headMaterialList;
        public Material[] bodyMaterialList;
        public int materialSelect = 0;
        public Transform rootBone;
        public float rootScale = 1.0f;
 
        public int hairSelect = 0;
        public Texture[] textureList;
        public Texture[] textureListBody;
        public int textureSelect = 0;
        public Texture[] normalMapList;
        public int normalMapSelect = 0;
        public float normalMapValue = 1f;

        public int faceBlendShapeSelect = 0;
        public float faceBlendShapeAmount = 0;
        public int faceMin = 53;
        public int faceMax = 55;
        public float cornerEyeAmount = 0;
        public float weightBlendShape = 0;

        public bool droop = false;
        public float droopLeftAmount = 0;
        public float droopRightAmount = 0;

        public Material headMaterial = null;
        public Material bodyMaterial = null;

        void Update()
        {
            //renderer.material.SetTexture("_BumpMap", normalTexture);
            SetMaterial(materialSelect);
        }

        public void SetMaterial(int materialID)
        {
            //faceSkinnedMeshRenderer.material = headMaterialList[materialID];
            //bodySkinnedMeshRenderer.material = bodyMaterialList[materialID];
            headMaterial = headMaterialList[materialID];
            bodyMaterial = bodyMaterialList[materialID];
        }

        public void ClearHair()
        {
            foreach (GameObject hair in hairList){
                hair.SetActive(false);
            }
        }

        public void SetHair(int hairID)
        {
            if (hairID == 0) {
                ClearHair();
            } else if (hairID > 0)
            {
                ClearHair();
                hairList[hairID - 1].SetActive(true);
            }
        }

        public void SetTexture(int textureID)
        {
            headMaterial.mainTexture = textureList[textureID];
            bodyMaterial.mainTexture = textureListBody[textureID];
        }

        public void SetNormalMap(int nmID, float nmVal)
        {
            headMaterial.EnableKeyword("_NORMALMAP");
            headMaterial.SetFloat("_BumpScale", nmVal);
            headMaterial.SetTexture("_BumpMap", normalMapList[nmID]);
        }

        public void SetHeight(float scale)
        {
            scale = scale / 2;
            rootBone.transform.localScale = new Vector3(scale, scale, scale);
        }

        public void SetBlendShapeFeatures(int index, float amount)
        {
            for (int faceIndex = faceMin; faceIndex <= faceMax; faceIndex=faceIndex+1)
            {
                faceSkinnedMeshRenderer.SetBlendShapeWeight(faceIndex, 0);
            }
            faceSkinnedMeshRenderer.SetBlendShapeWeight(index, amount);
        }

        public void SetEyeCorner(float amount)
        {
            faceSkinnedMeshRenderer.SetBlendShapeWeight(56, amount);
        }
        

        public void SetWeightBlendShape(float amount)
        {
            if (weightBlendShape > 0)
            {
                faceSkinnedMeshRenderer.SetBlendShapeWeight(57, 0);
                faceSkinnedMeshRenderer.SetBlendShapeWeight(58, amount);
                bodySkinnedMeshRenderer.SetBlendShapeWeight(1, 0);
                bodySkinnedMeshRenderer.SetBlendShapeWeight(2, amount);
            }
            else
            {
                faceSkinnedMeshRenderer.SetBlendShapeWeight(58, 0);
                faceSkinnedMeshRenderer.SetBlendShapeWeight(57, Mathf.Abs(amount));
                bodySkinnedMeshRenderer.SetBlendShapeWeight(2, 0);
                bodySkinnedMeshRenderer.SetBlendShapeWeight(1, Mathf.Abs(amount));
            }
        }

        public void SetDroop(bool droop = false, float left = 0f, float right = 0f)
        {
            if (droop)
            {
                faceSkinnedMeshRenderer.SetBlendShapeWeight(51, left);
                faceSkinnedMeshRenderer.SetBlendShapeWeight(52, right);
            } else
            {
                faceSkinnedMeshRenderer.SetBlendShapeWeight(51, 0);
                faceSkinnedMeshRenderer.SetBlendShapeWeight(52, 0);
            }
        }

        public void RandomTraits()
        {
            hairSelect = Random.Range(0,hairList.Length+1);
            SetHair(hairSelect);

            textureSelect = Random.Range(0, textureList.Length - 1);
            SetTexture(textureSelect);

            normalMapSelect = Random.Range(0, normalMapList.Length - 1);
            normalMapValue = Random.Range(0f, 1f);
            SetNormalMap(normalMapSelect, normalMapValue);

            rootScale = Random.Range(1.6f, 2.2f);
            SetHeight(rootScale);

            faceBlendShapeSelect = Random.Range(faceMin, faceMax); //54, 57
            faceBlendShapeAmount = Random.Range(0f, 100f);
            SetBlendShapeFeatures(faceBlendShapeSelect, faceBlendShapeAmount);
            cornerEyeAmount = Random.Range(0f, 100f);
            SetEyeCorner(cornerEyeAmount);
            weightBlendShape = Random.Range(-100f, 100f);
            SetWeightBlendShape(weightBlendShape);
        }
    }

}
