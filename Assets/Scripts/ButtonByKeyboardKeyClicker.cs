using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ButtonByKeyboardKeyClicker : MonoBehaviour
{
    public KeyCode KeyCode;

    Button _button;

    // Start is called before the first frame update
    void Start()
    {
        _button = GetComponent<Button>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode))
        {
            _button.onClick.Invoke();
        }
    }
}