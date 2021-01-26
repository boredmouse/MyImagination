using UnityEditor;
using UnityEngine;

public class FiletypeEditor : Editor
{
    const float SAVE_INTERVAL = 3f;
    AssetImporter m_Importer;
    bool m_Save;
    float m_LastSave;

    protected virtual void OnEnable()
    {
        string tPath = AssetDatabase.GetAssetPath(target);
        m_Importer = AssetImporter.GetAtPath(tPath);
        m_LastSave = Time.realtimeSinceStartup;
    }

    public override void OnInspectorGUI()
    {
        GUI.enabled = true;
        if (Time.realtimeSinceStartup - m_LastSave > SAVE_INTERVAL && m_Save)
            ForceSave();
    }

    protected virtual string Deserialize()
    {
        return "";
    }

    protected void Save()
    {
        m_Save = true;
    }

    protected virtual void OnDestroy()
    {
        ForceSave();
    }

    protected void ForceSave()
    {
        if (m_Importer == null)
            return;

        m_Importer.userData = Deserialize();
        EditorUtility.SetDirty(target);
        AssetDatabase.WriteImportSettingsIfDirty(AssetDatabase.GetAssetPath(target));
        AssetDatabase.SaveAssets();
        m_Save = false;
    }
}
