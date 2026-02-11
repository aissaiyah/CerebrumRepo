using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DropdownFieldTranslator : MonoBehaviour
{
    // Start is called before the first frame update
    public ObjectMessageHandler handler;
    TMP_Dropdown myDD;
    void Start()
    {
        myDD = transform.GetComponent<TMP_Dropdown>();
    }

    // Update is called once per frame
    void Update()
    {
        handler.inputText = myDD.options[myDD.value].text;
    }
}
