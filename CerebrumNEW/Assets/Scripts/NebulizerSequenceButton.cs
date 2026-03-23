using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Cerebrum;

/// <summary>
/// Drives a two-step button sequence: Sit Up -> Administer Nebulizer -> End Screen (perfect score).
/// Attach this to any persistent GameObject (not the buttons themselves).
/// </summary>
public class NebuliserSequenceButton : MonoBehaviour
{
    [Header("Buttons")]
    public GameObject sitUpButton;
    public GameObject nebuliserButton;

    [Header("Prompt")]
    public GameObject promptPanel;
    public TMP_Text promptText;

    [Header("Prompt Messages")]
    public string sitUpMessage = "You helped the patient sit up.";
    public string nebuliserMessage = "The treatment worked.";

    [Header("End Screen")]
    [Tooltip("Assign the root EndCanvas GameObject. The script will activate its Background child, matching how the scenario runner shows it.")]
    public GameObject endCanvas;
    public UITextShower uiCompletion;

    [Header("Timing")]
    [Tooltip("Delay before showing the prompt after a button is pressed.")]
    public float prePromptDelay = 1f;
    [Tooltip("How long the prompt is displayed before the next action fires.")]
    public float postPromptDelay = 2f;

    private void Start()
    {
        if (nebuliserButton != null)
            nebuliserButton.SetActive(false);

        if (promptPanel != null)
            promptPanel.SetActive(false);
    }

    /// <summary>
    /// Called by the Sit Up button's OnClick event.
    /// </summary>
    public void OnSitUpPressed()
    {
        StartCoroutine(SitUpSequence());
    }

    /// <summary>
    /// Called by the Administer Nebulizer button's OnClick event.
    /// </summary>
    public void OnNebuliserPressed()
    {
        StartCoroutine(NebuliserSequence());
    }

    private IEnumerator SitUpSequence()
    {
        SetButtonInteractable(sitUpButton, false);

        yield return new WaitForSeconds(prePromptDelay);

        ShowPrompt(sitUpMessage);

        yield return new WaitForSeconds(postPromptDelay);

        HidePrompt();

        if (sitUpButton != null)
            sitUpButton.SetActive(false);

        if (nebuliserButton != null)
            nebuliserButton.SetActive(true);
    }

    private IEnumerator NebuliserSequence()
    {
        SetButtonInteractable(nebuliserButton, false);

        yield return new WaitForSeconds(prePromptDelay);

        ShowPrompt(nebuliserMessage);

        yield return new WaitForSeconds(postPromptDelay);

        HidePrompt();

        TriggerPerfectEnd();

        if (nebuliserButton != null)
            nebuliserButton.SetActive(false);
    }

    private void ShowPrompt(string message)
    {
        if (promptPanel != null)
            promptPanel.SetActive(true);

        if (promptText != null)
            promptText.text = message;
    }

    private void HidePrompt()
    {
        if (promptPanel != null)
            promptPanel.SetActive(false);
    }

    private void TriggerPerfectEnd()
    {
        if (uiCompletion != null)
            uiCompletion.ShowPerfectDirect();

        if (endCanvas == null)
            return;

        // The scenario runner hides EndCanvas by deactivating its first child (Background),
        // not the root canvas itself. Mirror that same pattern to show it.
        if (endCanvas.transform.childCount > 0)
            endCanvas.transform.GetChild(0).gameObject.SetActive(true);

        // Ensure it renders on top of all other sortingOrder-0 canvases
        var canvas = endCanvas.GetComponent<Canvas>();
        if (canvas != null)
            canvas.sortingOrder = 999;
    }

    private void SetButtonInteractable(GameObject buttonGO, bool interactable)
    {
        if (buttonGO == null)
            return;

        var btn = buttonGO.GetComponent<Button>();
        if (btn != null)
            btn.interactable = interactable;
    }
}
