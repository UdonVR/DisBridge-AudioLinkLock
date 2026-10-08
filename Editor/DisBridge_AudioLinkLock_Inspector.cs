using System.Collections;
using System.Collections.Generic;
using UdonVR.disbridge;
using UnityEditor;
using UnityEngine;
using UdonVR.DisBridge.Editors;
using UnityEngine.UI;
using VRC.SDKBase;

namespace UdonVR.DisBridgeAudioLinkLock.Editors
{

    [CustomEditor(typeof(DisBridge_AudioLinkLock))]
    public class DisBridge_AudioLinkLock_Inspector : Editor
    {
        DisBridge_AudioLinkLock myScript;

        private void OnEnable()
        {
            myScript = (DisBridge_AudioLinkLock)target;
        }

        public override void OnInspectorGUI()
        {
            Undo.RecordObject(myScript, $"Undo on {myScript.name}");

            EditorFunctions.DrawDisBridgeLogo();
            EditorFunctions.DrawPluginManagerCheck(myScript, false, true);
            EditorFunctions.DrawPermissions(myScript, false);
            DrawSettings();

            PrefabUtility.RecordPrefabInstancePropertyModifications(myScript);
            serializedObject.ApplyModifiedProperties();
        }

        private void DrawSettings()
        {
            GUI.backgroundColor = myScript.autoSetup ? Color.green : Color.red;
            if (GUILayout.Button(myScript.autoSetup ? "Auto Setup\n[Enabled]" : "Auto Setup\n[Disabled]"))
            {
                myScript.autoSetup = !myScript.autoSetup;
            }

            GUI.backgroundColor = Color.white;

            if (myScript.autoSetup)
            {
                DrawAutoSetup();
            }
            else
            {
                DrawManualSetup();
            }
        }

        private GameObject al_controller;
        private VRC_UiShape AL_UiShape;

        private void DrawAutoSetup()
        {
            if (al_controller == null)
            {
                if (DisBridge_AudioLinkLock_Helpers.FindAudioLinkController(out al_controller))
                {
                    // found AL controller, continue
                }
                else
                {
                    EditorGUILayout.HelpBox("Audio Link Controller not found!", MessageType.Error);
                    return;
                }
            }

            if (AL_UiShape == null)
            {
                AL_UiShape = al_controller.gameObject.GetComponentInChildren<VRC_UiShape>(true);
                if (AL_UiShape == null)
                    EditorGUILayout.HelpBox("Audio Link Controller Canvas not found!!", MessageType.Error);
                return;
            }

            DrawAutoSetup2();
        }

        BoxCollider AL_BoxCollider;
        GraphicRaycaster AL_Raycaster;

        private void DrawAutoSetup2()
        {
            BoxCollider AL_BoxCollider = AL_UiShape.GetComponent<BoxCollider>();
            GraphicRaycaster AL_Raycaster = AL_UiShape.GetComponent<GraphicRaycaster>();
            VRC_Pickup[] AL_Pickup = al_controller.gameObject.GetComponentsInChildren<VRC_Pickup>(true);
            
            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.ObjectField(al_controller, typeof(GameObject), true);
            EditorGUILayout.ObjectField(AL_Raycaster, typeof(GraphicRaycaster), true);
            EditorGUILayout.ObjectField(AL_BoxCollider, typeof(BoxCollider), true);
            foreach (var pickup in AL_Pickup)
            {
                EditorGUILayout.ObjectField(pickup, typeof(VRC_Pickup), true);
            }
            EditorGUI.EndDisabledGroup();
            
            if (AL_BoxCollider == null && AL_Raycaster == null)
            {
                EditorGUILayout.HelpBox("Missing both [GraphicRaycaster] and [BoxCollider]\nAuto setup will not work",
                    MessageType.Error);
                return;
            }

            if (AL_BoxCollider == null && AL_Raycaster != null)
            {
                GUILayout.BeginHorizontal();
                EditorGUILayout.HelpBox(
                    "[BoxCollider] is missing but should get automatically added by the canvas's [VRC_UiShape].\nAuto setup will most likely still succeed!\nmake sure to double check in game!",
                    MessageType.Warning);
                
                if (GUILayout.Button("Auto Fix", GUILayout.Width(90), GUILayout.ExpandHeight(true)))
                {
                    DisBridge_AudioLinkLock_Helpers.CreateUiCollider(AL_UiShape);
                }
                GUILayout.EndHorizontal();
                return;
            }

            if (AL_BoxCollider != null && AL_Raycaster == null)
            {
                EditorGUILayout.HelpBox(
                    "[GraphicRaycaster] is missing. This is unusual, something is probably wrong.\nmake sure to double check in game!",
                    MessageType.Error);
                return;
            }

            if (AL_Pickup.Length == 0)
            {
                EditorGUILayout.HelpBox(
                    "[VRC_Pickup] is missing. If you removed the pickup, this is fine.",
                    MessageType.Warning);
            }

            EditorGUILayout.HelpBox("Everything looks good!", MessageType.Info);
        }

        private void DrawManualSetup()
        {
            myScript.AL_Raycaster =
                (GraphicRaycaster)EditorGUILayout.ObjectField(myScript.AL_Raycaster, typeof(GraphicRaycaster), true);
            myScript.AL_BoxCollider =
                (BoxCollider)EditorGUILayout.ObjectField(myScript.AL_BoxCollider, typeof(BoxCollider), true);
        }
    }
}