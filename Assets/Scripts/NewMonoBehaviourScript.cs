using System;
using NUtilities.Popup;
using UnityEngine;
using VContainer;
using VContainer.Unity;

public class NewMonoBehaviourScript : MonoBehaviour
{
    private PopupService _popupService;

    [Inject]
    public void Inject(PopupService popupService)
    {
        _popupService = popupService;
    }
    
    public void Click()
    {
        _popupService.Show("base");
    }
}
