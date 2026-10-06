using System.Collections;
using System.Collections.Generic;
using AudioLink;
using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.SDKBase.Editor.BuildPipeline;

namespace UdonVR.DisBridgeAudioLinkLock.Editors
{
    public class DisBridge_AudioLinkLock_BuildTools : IVRCSDKBuildRequestedCallback
    {
        public int callbackOrder => -9999;

        public bool OnBuildRequested(VRCSDKRequestedBuildType requestedBuildType)
        {
            return true;
        }

        [PostProcessScene(-9999)]
        private static void ProcessScene()
        {
            DisBridge_AudioLinkLock _myScript = GameObject.FindObjectOfType<DisBridge_AudioLinkLock>();
            if (_myScript == null) return;
            ProcessAL(_myScript);
            if (_myScript.AL_BoxCollider != null) _myScript.AL_BoxCollider.enabled = false;
            if (_myScript.AL_Raycaster   != null) _myScript.AL_Raycaster.enabled = false;
            if (_myScript.AL_Pickup      != null) _myScript.AL_Pickup.pickupable = false;
        }

        public static void ProcessAL(DisBridge_AudioLinkLock _myScript)
        {
            if (!_myScript.autoSetup)
            { Debug.LogWarning("[AudioLinkLock][Build] Autosetup is disabled, skipping."); }

            if (!DisBridge_AudioLinkLock_Helpers.FindAudioLinkController(out GameObject _al, true))
            {
                Debug.LogError("[AudioLinkLock][Build] AudioLink Controller not found, stopping build tools!");
                return;
            }

            VRC_UiShape AL_UiShape = _al.gameObject.GetComponentInChildren<VRC_UiShape>(true);

            if (AL_UiShape == null)
            { Debug.LogError("[AudioLinkLock][Build] AL_UiShape was not found!"); }
            else
            { Debug.Log($"[AudioLinkLock][Build] AL_UiShape was found on object {AL_UiShape.gameObject.name}", AL_UiShape.gameObject); }

            GameObject _audioLinkCanvas = AL_UiShape.gameObject;

            BoxCollider AL_BoxCollider = _audioLinkCanvas.GetComponent<BoxCollider>();
            GraphicRaycaster AL_Raycaster = _audioLinkCanvas.GetComponent<GraphicRaycaster>();

            if (AL_BoxCollider == null)
            { Debug.LogError("[AudioLinkLock][Build] AL_BoxCollider was not found!"); }
            else
            { Debug.Log($"[AudioLinkLock][Build] AL_BoxCollider was found on object {AL_BoxCollider.gameObject.name}", AL_BoxCollider.gameObject); }

            if (AL_Raycaster == null)
            { Debug.LogError("[AudioLinkLock][Build] AL_Raycaster was not found!"); }
            else
            { Debug.Log($"[AudioLinkLock][Build] AL_Raycaster was found on object {AL_Raycaster.gameObject.name}", AL_Raycaster.gameObject); }
            
            VRC_Pickup AL_Pickup = _al.gameObject.GetComponentInChildren<VRC_Pickup>(true);
            if (AL_Pickup == null)
            { Debug.LogWarning("[AudioLinkLock][Build] AL_Pickup was not found! If you removed the pickup, then this is fine."); }
            else
            { Debug.Log($"[AudioLinkLock][Build] AL_Pickup was found on object {AL_Pickup.gameObject.name}", AL_Pickup.gameObject); }

            _myScript.AL_BoxCollider = AL_BoxCollider;
            _myScript.AL_Raycaster = AL_Raycaster;
            _myScript.AL_Pickup = AL_Pickup;
        }
    }
}