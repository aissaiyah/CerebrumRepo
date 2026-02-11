/* 
    ------------------- Code Monkey -------------------

    Thank you for downloading this package
    I hope you find it useful in your projects
    If you have any questions let me know
    Cheers!

               unitycodemonkey.com
    --------------------------------------------------
 */
 
using UnityEngine;
using System.Reflection;
//using V_AnimationSystem;

public class GameAssets : MonoBehaviour {

    private static GameAssets _i;

    public static GameAssets i {
        get {
            if (_i == null) _i = Instantiate(Resources.Load<GameAssets>("GameAssets"));
            return _i;
        }
    }



    public Transform pfChatBubble;


    public Sprite s_ShootFlash;
    
    public Transform pfSwordSlash;
    public Transform pfEnemy;
    public Transform pfEnemyFlyingBody;
    public Transform pfImpactEffect;
    public Transform pfDamagePopup;
    public Transform pfDashEffect;
    public Transform pfProjectileArrow;
    public Transform pfBolt;
    public Transform pfSmoke;

    public Material m_WeaponTracer;
    public Material m_MarineSpriteSheet;

    public Material m_DoorRed;
    public Material m_DoorGreen;
    public Material m_DoorBlue;

    public Material m_DoorKeyHoleRed;
    public Material m_DoorKeyHoleGreen;
    public Material m_DoorKeyHoleBlue;





    
    public static class UnitAnimTypeEnum {

        static UnitAnimTypeEnum() {
//            V_Animation.Init();
            FieldInfo[] fieldInfoArr = typeof(UnitAnimTypeEnum).GetFields(BindingFlags.Static | BindingFlags.Public);
            foreach (FieldInfo fieldInfo in fieldInfoArr) {
                if (fieldInfo != null) {
//                    fieldInfo.SetValue(null, UnitAnimType.GetUnitAnimType(fieldInfo.Name));
                }
            }
        }


    }




    public static class UnitAnimEnum {

        static UnitAnimEnum() {
//            V_Animation.Init();
            FieldInfo[] fieldInfoArr = typeof(UnitAnimEnum).GetFields(BindingFlags.Static | BindingFlags.Public);
            foreach (FieldInfo fieldInfo in fieldInfoArr) {
                if (fieldInfo != null) {
//                    fieldInfo.SetValue(null, UnitAnim.GetUnitAnim(fieldInfo.Name));
                }
            }
        }
        
    }

}
