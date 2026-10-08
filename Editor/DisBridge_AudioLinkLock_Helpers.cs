using System.Collections;
using System.Collections.Generic;
using AudioLink;
using UnityEditor;
using UnityEngine;
using VRC.SDKBase;

namespace UdonVR.DisBridgeAudioLinkLock.Editors
{
    public class DisBridge_AudioLinkLock_Helpers
    {
        public static bool FindAudioLinkController(out GameObject _object, bool _logs = false)
        {
            AudioLinkController _al = GameObject.FindObjectOfType<AudioLinkController>(true);

            if (_al == null)
            {
                if (_logs)
                    Debug.LogWarning("[AudioLinkLock][Build] AudioLink Controller not found, looking for V0...!");
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

        public static void CreateUiCollider(VRC_UiShape _UiShape)
        {
            if (_UiShape == null) return;

            RectTransform localRectTransform = _UiShape.transform as RectTransform;
            if (localRectTransform == null) return;

            GameObject targetObject = _UiShape.gameObject;
            
            if (targetObject.GetComponent<BoxCollider>() != null) return;
            
            BoxCollider boxCollider = Undo.AddComponent<BoxCollider>(targetObject);

            Vector2 size = localRectTransform.rect.size;
            Vector3 colliderSize = new Vector3(size.x, size.y, 1f);
            Vector2 pivot = localRectTransform.pivot;
            
            Vector3 colliderCenter = new Vector3((0.5f - pivot.x) * colliderSize.x, (0.5f - pivot.y) * colliderSize.y, 0.0f);

            boxCollider.center = colliderCenter;
            boxCollider.size = colliderSize;

            EditorUtility.SetDirty(boxCollider);
        }
    }
}