
using UdonSharp;
using UdonVR.DisBridge;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;

public class DisBridge_AudioLinkLock : DisBridgePlugin
{
    public bool autoSetup = true;
    [SerializeField] public GraphicRaycaster AL_Raycaster;
    [SerializeField] public BoxCollider AL_BoxCollider;
    
    public override void _UVR_Init()
    {
        _RefreshAL();
    }

    public override void _UVR_Update()
    {
        _RefreshAL();
    }


    public void _RefreshAL()
    {
        if (!Utilities.IsValid(AL_Raycaster) || !Utilities.IsValid(AL_BoxCollider)) return;
        bool _unlock = _IsMemberInRoles(Networking.LocalPlayer);
        if (Utilities.IsValid(AL_Raycaster)) AL_Raycaster.enabled = _unlock;
        if (Utilities.IsValid(AL_BoxCollider)) AL_BoxCollider.enabled = _unlock;
    }
}
