using UnityEditor;
using System;
using System.Reflection;
using UnityEngine;

[CustomEditor(typeof(DefaultAsset)), CanEditMultipleObjects]
public class CustomFiletypeInspector : Editor
{
    Editor m_CustomInspector;

    private void OnEnable()
    {

        CreateCustomEditor();
    }

    void CreateCustomEditor()
    {
        string[] paths = new string[targets.Length];
        for (int i = 0; i < targets.Length; i++)
            paths[i] = AssetDatabase.GetAssetPath(target);
        Assembly[] AS = AppDomain.CurrentDomain.GetAssemblies();
        Type tEditorType = typeof(Editor);

        foreach (var A in AS)
        {
            try
            {
                Type[] types = A.GetTypes();
                foreach (var T in types)
                {
                    if (T.IsSubclassOf(tEditorType))
                    {
                        CustomFieltypeEditorAttribute[] tExtensions = 
                            (CustomFieltypeEditorAttribute[])T.GetCustomAttributes(typeof(CustomFieltypeEditorAttribute), true);
                        CanEditMultipleObjects[] tMulti = 
                            (CanEditMultipleObjects[])T.GetCustomAttributes(typeof(CanEditMultipleObjects), true);
                        if (tExtensions.Length > 0)
                        {
                            for (int i = 0; i < tExtensions.Length; i++)
                            {
                                if (HasExtension(paths, tExtensions[i]))
                                {
                                    if (tMulti.Length > 0)
                                    {
                                        m_CustomInspector = CreateEditor(targets, T);
                                        return;
                                    }
                                    else if (tMulti.Length == 0 && targets.Length == 1)
                                    {
                                        m_CustomInspector = CreateEditor(target, T);
                                        return;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (ReflectionTypeLoadException)
            {

            }
        }
    }

    static bool HasExtension(string[] aPaths, CustomFieltypeEditorAttribute aAttr)
    {
        bool[] tHasExtension = new bool[aPaths.Length];
        for (int i = 0; i < aAttr.Extensions.Length; i++)
        {
            for (int j = 0; j < aPaths.Length; j++)
            {
                if (aPaths[j].EndsWith(aAttr.Extensions[i]))
                    tHasExtension[j] = true;
            }
            if (AllTrue(tHasExtension))
                return true;
        }
        return false;
    }

    static bool AllTrue(bool[] aList)
    {
        foreach (bool tItem in aList)
        {
            if (!tItem)
                return false;
        }
        return true;
    }

    #region Unity events
    public override void OnInspectorGUI()
    {
        if (m_CustomInspector != null)
        {
            GUI.enabled = true;
            m_CustomInspector.OnInspectorGUI();
        }
        else
            base.OnInspectorGUI();
    }
    #endregion
}
