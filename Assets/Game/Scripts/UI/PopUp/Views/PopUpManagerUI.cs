using TMPro;
using UnityEngine;

public class PopUpManagerUI : MonoBehaviour
{    
    [Header("Text Setting")]
    [SerializeField] private TMP_Text popUpText;  
    [SerializeField] private CanvasGroup canvasGroup;  

    public void ShowPopUp(string message)
    {
        popUpText.text = message;
        canvasGroup.alpha = 1;
    }   

    public void HidePopUp()
    {
        popUpText.text = string.Empty;
        canvasGroup.alpha = 0;
    }
    
}
