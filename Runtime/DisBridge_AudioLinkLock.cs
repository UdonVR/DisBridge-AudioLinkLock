
using System;
using UdonSharp;
using UdonVR.DisBridge;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;

namespace UdonVR.DisBridgeAudioLinkLock
{
    public class DisBridge_AudioLinkLock : DisBridgePlugin
    {
        public bool autoSetup = true;
        [SerializeField] public GraphicRaycaster AL_Raycaster;
        [SerializeField] public BoxCollider AL_BoxCollider;
        [SerializeField] public VRC_Pickup[] AL_Pickup;

        private void Start()
        {
            disBridge.AddPlugin(this);
        }

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
            if (Utilities.IsValid(  AL_Raycaster)) AL_Raycaster.enabled = _unlock;
            if (Utilities.IsValid(AL_BoxCollider)) AL_BoxCollider.enabled = _unlock;
            if (Utilities.IsValid(AL_Pickup))
            {
                for (int i = 0; i < AL_Pickup.Length; i++)
                {
                    if (Utilities.IsValid(AL_Pickup[i])) AL_Pickup[i].pickupable = _unlock;
                }
            }
        }
    }
}