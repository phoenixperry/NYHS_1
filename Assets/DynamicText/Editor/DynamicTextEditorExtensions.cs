/// Copyright 2013 Jetro Lauha (Strobotnik Ltd)

using UnityEditor;
using UnityEngine;


[InitializeOnLoad]
public class DynamicTextEditorExtensions
{
    [MenuItem("GameObject/Create Other/Dynamic Text")]
    static void createDynamicTextGameObject()
    {
        //Debug.Log("DynamicText: (GameObject/Create Other/Dynamic Text)");
        GameObject go = new GameObject("Dynamic Text");

        // enable these rows if you want new one to be child of selection
        //if (Selection.activeTransform != null)
        //    go.transform.parent = Selection.activeTransform;

        DynamicText dt = go.AddComponent<DynamicText>();

        if (dt.cam)
        {
            float camDistance = 10.0f;
            go.transform.position = dt.cam.transform.position + dt.cam.transform.forward * camDistance;
        }
    }

    [MenuItem("CONTEXT/TextMesh/Convert to Dynamic Text")]
    static void convertTextMeshToDynamicText(MenuCommand command)
    {
        //Debug.Log("DynamicText: (CONTEXT/TextMesh/Convert to Dynamic Text)");
        TextMesh tm = command.context as TextMesh;
        if (tm != null && tm.gameObject)
        {
            GameObject go = tm.gameObject;

            // Note: Dynamic Text doesn't support Unity 4.1 or older versions
#if UNITY_4_2
            // Unity 4.2
            Undo.RegisterSceneUndo("Convert TextMesh to Dynamic Text");
            go.AddComponent<DynamicText>();
#else
            // Unity 4.3 or newer:
            Undo.AddComponent<DynamicText>(go);
#endif
        }
    }


    static bool prev_PlayerSettings_useDirect3D11 = false;

    static void check_if_useDirect3D11_changed()
    {
        if (prev_PlayerSettings_useDirect3D11 != PlayerSettings.useDirect3D11)
        {
            prev_PlayerSettings_useDirect3D11 = PlayerSettings.useDirect3D11;
#if UNITY_EDITOR
            PlayerPrefs.SetInt("DynamicText_force_alternate_sampling", PlayerSettings.useDirect3D11 ? 1 : 0);
#endif
        }
    }

    static DynamicTextEditorExtensions()
    {
        check_if_useDirect3D11_changed();
#if UNITY_EDITOR
        // remove & add editor update callback (make sure we don't get dups)
        EditorApplication.update -= check_if_useDirect3D11_changed;
        EditorApplication.update += check_if_useDirect3D11_changed;
#endif
    }
}
