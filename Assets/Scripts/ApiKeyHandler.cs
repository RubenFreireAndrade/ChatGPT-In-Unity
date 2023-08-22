using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class ApiKeyHandler : MonoBehaviour
{
    [SerializeField] private InputField inputField;
    [SerializeField] private Button button;
    [SerializeField] private GameObject canvasObj;

    private string currentValue;

    // Start is called before the first frame update
    void Start()
    {
        canvasObj.SetActive(false);
        //button.onClick.AddListener(SubmitValue);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetInputValue()
    {
        currentValue = inputField.text;
        Debug.Log(currentValue);
    }

    public string GetInputValue()
    {
        return currentValue;
    }

    public void SubmitValue()
    {
        Debug.Log(inputField.text);
    }
}
