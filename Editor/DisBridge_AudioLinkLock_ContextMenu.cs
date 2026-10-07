using UnityEditor;
using UnityEngine;

namespace UdonVR.DisBridgeAudioLinkLock.Editors
{
    public class DisBridge_AudioLinkLock_ContextMenu
    {
        private const int Menu = 49;

        [MenuItem("GameObject/AudioLink/DisBridge - AudioLink Lock", false, Menu)]
        [MenuItem("GameObject/UdonVR/DisBridge/Plugins/AudioLink Lock", false, Menu)]
        private static void CreateDisBridgeAudioLinkLock(MenuCommand menuCommand)
        {
            GameObject AL_Lock = new GameObject("DisBridge AudioLink Lock");
            AL_Lock.AddComponent<DisBridge_AudioLinkLock>();
            GameObjectUtility.SetParentAndAlign(AL_Lock, menuCommand.context as GameObject);
            GameObjectUtility.EnsureUniqueNameForSibling(AL_Lock);
            Undo.RegisterCreatedObjectUndo(AL_Lock, "Create " + AL_Lock.name);
            Selection.activeObject = AL_Lock;
        }
    }
}