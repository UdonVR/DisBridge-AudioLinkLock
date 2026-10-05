using System.Collections;
using System.Collections.Generic;
using AudioLink;
using UnityEngine;

public class DisBridge_AudioLinkLock_Helpers
{
    public static bool FindAudioLinkController(out GameObject _object, bool _logs = false)
    {
        AudioLinkController _al = GameObject.FindObjectOfType<AudioLinkController>(true);

        if (_al == null)
        {
            if (_logs) Debug.LogWarning("[AudioLinkLock][Build] AudioLink Controller not found, looking for V0...!");
            AudioLinkControllerV0 _alv0 = GameObject.FindObjectOfType<AudioLinkControllerV0>(true);
            if (_alv0 == null)
            {
                if (_logs) Debug.LogError("[AudioLinkLock][Build] AudioLink not found!!");
                _object = null;
                return false;
            }
            else
            {
                _object = _alv0.gameObject;
            }
        }
        else
        {
            _object = _al.gameObject;
        }
        if (_logs) Debug.Log($"[AudioLinkLock][Build] AudioLink Controller found!! {_object.name}", _object);
        return true;
    }
}
