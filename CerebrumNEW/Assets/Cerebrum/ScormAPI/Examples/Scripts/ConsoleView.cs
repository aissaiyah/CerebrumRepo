using System;
using UnityEngine;
using UnityEngine.UI;

public class ConsoleView : MonoBehaviour {

    public Button clearButton;

    public ScrollRect scrollRect;

    public Text text;

    void Awake() {
        this.text.text = string.Empty;

        clearButton.onClick.AddListener(Clear);
    }

    void Start() {
        Application.logMessageReceived += (condition, stackTrace, type) => {
            Append(condition);
        };
    }

    void Append(string text) {        
        this.text.text += text + Environment.NewLine;

        scrollRect.verticalNormalizedPosition = 0;
    }

    void Clear() {
        this.text.text = string.Empty;

        scrollRect.verticalNormalizedPosition = 1;
    }
}
