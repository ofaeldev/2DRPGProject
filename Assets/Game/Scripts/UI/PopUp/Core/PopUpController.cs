using System;
using UnityEngine;

public class PopUpController : MonoBehaviour
{
    [Header("References")]
    public PlayerInteraction playerInteraction;
    private IInteraction currentInteraction;

    [Header("Pulse Setting")]
    [SerializeField] private Transform popUpRoot;
    [SerializeField] private float pulseSpeed;
    [SerializeField] private float minPulseScale;
    [SerializeField] private float maxPulseScale;
    [SerializeField] private CanvasGroup canvasGroup;    
    private Vector3 baseScale;
    private PopUpManagerUI popUpManagerUI;

    #region Unity Methods
    private void Awake()
    {
        popUpManagerUI = GetComponent<PopUpManagerUI>();
        baseScale = popUpRoot.transform.localScale;
    }

    private void OnEnable()
    {
        PopUpService.PopUpStartedEvent += OnStartedPopUp;
        PopUpService.PopUpEnded += OnEndedPopUp;
    }   

    private void OnDisable()
    {
        PopUpService.PopUpStartedEvent -= OnStartedPopUp;
        PopUpService.PopUpEnded -= OnEndedPopUp;
    }

    private void LateUpdate()
    {
        UpdatePopUpPosition();
        UpdatePulse();
    }
    #endregion

    #region Pop Up Region   

    private void OnEndedPopUp()
    {
        popUpManagerUI.HidePopUp();
        currentInteraction = null;
    }

    private void OnStartedPopUp(IInteraction interaction)
    {
        currentInteraction = interaction;

        if(currentInteraction != null)
        {
            popUpManagerUI.ShowPopUp(interaction.GetPopUpText());
            return;
        }
    }

    private void UpdatePopUpPosition()
    {
        if(currentInteraction != null)
        {
            Vector2 screenPosition = Camera.main.WorldToScreenPoint(currentInteraction.InteractionPoint().position);
            popUpRoot.transform.position = screenPosition;
            return;
        }
    }

    private void UpdatePulse()
    {
        if(currentInteraction == null)
        {
            popUpRoot.transform.localScale = baseScale;
            return;
        }

        float sinValue = Mathf.Sin(Time.time * pulseSpeed);
        float normalizedValue = (sinValue + 1) / 2;
        float scaleValue =  Mathf.Lerp(minPulseScale,maxPulseScale,normalizedValue);
        popUpRoot.transform.localScale = new Vector3(scaleValue, scaleValue, 1f);
    }
    #endregion
}