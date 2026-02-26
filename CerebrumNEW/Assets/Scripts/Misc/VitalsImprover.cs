using System.Collections;
using UnityEngine;

/// <summary>
/// Smoothly updates VitalsMH values in two staged improvements.
/// Step 1: called when the patient is helped to sit up.
/// Step 2: called when the nasal prong is applied.
/// Attach to any persistent GameObject and wire OnClick events to the public methods.
/// </summary>
public class VitalsImprover : MonoBehaviour
{
    [Header("Step 1 – Sit Up Deltas")]
    [Tooltip("Heart rate change (negative = slower, which is better).")]
    public int step1HRateDelta = -5;
    [Tooltip("SpO2 percentage point change.")]
    public int step1SpO2Delta = 2;
    [Tooltip("Respiratory rate change.")]
    public int step1RespRateDelta = -1;

    [Header("Step 2 – Nasal Prong Deltas")]
    public int step2HRateDelta = -8;
    public int step2SpO2Delta = 4;
    public int step2RespRateDelta = -2;

    [Header("Animation")]
    [Tooltip("How long each vital smoothly transitions to its new value.")]
    public float animationDuration = 1.5f;

    private VitalsMH _vitals;
    private bool _isAnimating;

    private void Start()
    {
        // VitalsMH is on an Addressable prefab spawned at runtime, so we defer finding it.
        StartCoroutine(FindVitalsWhenReady());
    }

    private IEnumerator FindVitalsWhenReady()
    {
        // Retry until the spawned prefab with VitalsMH exists in the scene.
        while (_vitals == null)
        {
            _vitals = FindObjectOfType<VitalsMH>();
            if (_vitals == null)
                yield return new WaitForSeconds(0.5f);
        }
    }

    /// <summary>
    /// Called after the patient is sat up. Applies a faint improvement to vitals.
    /// </summary>
    public void ImproveStep1()
    {
        ApplyImprovement(step1HRateDelta, step1SpO2Delta, step1RespRateDelta);
    }

    /// <summary>
    /// Called when the nasal prong is applied. Applies a moderate improvement to vitals.
    /// Wire this to the Nasal Prong button's OnClick event.
    /// </summary>
    public void ImproveStep2()
    {
        ApplyImprovement(step2HRateDelta, step2SpO2Delta, step2RespRateDelta);
    }

    private void ApplyImprovement(int hRateDelta, int spO2Delta, int respRateDelta)
    {
        if (_vitals == null)
            _vitals = FindObjectOfType<VitalsMH>();

        if (_vitals == null)
        {
            Debug.LogWarning("VitalsImprover: No VitalsMH found in scene.");
            return;
        }

        if (_isAnimating)
            StopAllCoroutines();

        StartCoroutine(AnimateVitals(hRateDelta, spO2Delta, respRateDelta));
    }

    private IEnumerator AnimateVitals(int hRateDelta, int spO2Delta, int respRateDelta)
    {
        _isAnimating = true;

        int startHRate = _vitals.HRate;
        int startSpO2 = _vitals.SpO2;
        int startRespRate = _vitals.RespRate;

        int targetHRate = startHRate + hRateDelta;
        int targetSpO2 = startSpO2 + spO2Delta;
        int targetRespRate = startRespRate + respRateDelta;

        float elapsed = 0f;

        while (elapsed < animationDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / animationDuration);

            int currentHRate = Mathf.RoundToInt(Mathf.Lerp(startHRate, targetHRate, t));
            int currentSpO2 = Mathf.RoundToInt(Mathf.Lerp(startSpO2, targetSpO2, t));
            int currentRespRate = Mathf.RoundToInt(Mathf.Lerp(startRespRate, targetRespRate, t));

            SetVitals(currentHRate, currentSpO2, currentRespRate);

            yield return null;
        }

        // Snap to exact target values at the end
        SetVitals(targetHRate, targetSpO2, targetRespRate);

        _isAnimating = false;
    }

    private void SetVitals(int hRate, int spO2, int respRate)
    {
        _vitals.HRate = hRate;
        _vitals.SpO2 = spO2;
        _vitals.RespRate = respRate;

        if (_vitals.HRateText != null)
            _vitals.HRateText.text = hRate.ToString();

        if (_vitals.SpO2Text != null)
            _vitals.SpO2Text.text = spO2.ToString() + "%";

        if (_vitals.RespRateText != null)
            _vitals.RespRateText.text = respRate.ToString();

        if (_vitals.heartRateMonitor != null)
            _vitals.heartRateMonitor.BeatsPerMinute = hRate;
    }
}
