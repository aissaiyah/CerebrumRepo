using UnityEngine;

public class BreathingSpeedOverride : MonoBehaviour
{
    [Tooltip("Multiplier applied to the BreathingSpeed animator parameter on Start.")]
    public float breathingSpeed = 1.3f;

    private void Start()
    {
        Animator anim = GetComponentInChildren<Animator>();
        if (anim != null)
            anim.SetFloat("BreathingSpeed", breathingSpeed);
    }
}
