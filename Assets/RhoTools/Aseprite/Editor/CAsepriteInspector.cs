using UnityEditor;
using UnityEngine;
using RhoTools.SimpleJSON;

namespace RhoTools.Aseprite {
    /// <summary>
    /// Inspector for .ase assets
    /// </summary>
    [CustomFieltypeEditor("ase"), CanEditMultipleObjects]
    public class CAsepriteInspector : Editor {
        GUIContent m_ButtonContentImport;
        GUIContent ButtonContentImport {
            get {
                if ( m_ButtonContentImport == null ) {
                    m_ButtonContentImport = new GUIContent(
                        CAsepriteWindow.GetTexture(this, "import.png"),
                        "Creates sprite and atlas");
                }
                return m_ButtonContentImport;
            }
        }
        GUIContent m_ButtonContentPrefab;
        GUIContent ButtonContentPrefab {
            get {
                if ( m_ButtonContentPrefab == null ) {
                    m_ButtonContentPrefab = new GUIContent(
                        CAsepriteWindow.GetTexture(this, "prefab.png"),
                        "Creates a prefab");
                }
                return m_ButtonContentPrefab;
            }
        }
        GUIContent m_ButtonContentClear;
        GUIContent ButtonContentClear {
            get {
                if ( m_ButtonContentClear == null ) {
                    m_ButtonContentClear = new GUIContent(
                        CAsepriteWindow.GetTexture(this, "clear.png"),
                        "Removes all assets asociated with this file");
                }
                return m_ButtonContentClear;
            }
        }
        GUIContent m_ButtonContentSettings;
        GUIContent ButtonContentSettings {
            get {
                if ( m_ButtonContentSettings == null ) {
                    m_ButtonContentSettings = new GUIContent(
                        CAsepriteWindow.GetTexture(this, "settings.png"),
                        "Open settings window");
                }
                return m_ButtonContentSettings;
            }
        }

        CAsepriteObject[] m_AsepriteObjects;

        private void OnEnable() {
            m_AsepriteObjects = new CAsepriteObject[targets.Length];
            for ( int i = 0; i < targets.Length; i++ ) {
                AssetImporter tImporter = AssetImporter.GetAtPath(AssetDatabase.GetAssetPath(targets[i]));
                if ( tImporter == null ) {
                    continue;
                }
                JSONNode tData = JSON.Parse(tImporter.userData);
                if ( tData == null )
                    tData = new JSONObject();
                JSONNode tID = tData["assetID"];

                if ( tID != null ) {
                    m_AsepriteObjects[i] = CAsepritePostProcesor.GetAssetByID<CAsepriteObject>(tID.Value);
                    if ( m_AsepriteObjects[i] == null )
                        m_AsepriteObjects[i] = CAsepritePostProcesor.FindAsepriteObject(targets[i]);
                    //Debug.Log(tID.Value);
                }

                if ( m_AsepriteObjects[i] != null ) {
                    tID.Value = CAsepritePostProcesor.GetIDByAsset(m_AsepriteObjects[i]);
                    tImporter.userData = tData.ToString();
                }

            }
        }

        public override void OnInspectorGUI() {
            int tNotImported = 0;
            foreach ( CAsepriteObject tObj in m_AsepriteObjects ) {
                if ( tObj == null ) {
                    tNotImported++;
                }
            }

            if ( tNotImported > 0 ) {
                GUILayout.Label("No imported asset found for " + tNotImported
                    + " file" + (tNotImported > 1 ? "s" : ""));
                if ( GUILayout.Button(ButtonContentImport, GUILayout.MaxHeight(60)) ) {
                    for ( int i = 0; i < targets.Length; i++ ) {
                        if ( m_AsepriteObjects[i] == null ) {
                            Object tTarget = targets[i];
                            string tPath = AssetDatabase.GetAssetPath(tTarget);
                            m_AsepriteObjects[i] = CAsepriteObjectEditor.CreateAsset(tPath);
                            m_AsepriteObjects[i].asepriteAsset = tTarget;
                            CAsepriteObjectEditor.Import(tPath, m_AsepriteObjects[i]);

                            AssetImporter tImporter = AssetImporter.GetAtPath(tPath);
                            JSONNode tData = JSON.Parse(tImporter.userData);
                            if ( tData == null )
                                tData = new JSONObject();
                            tData["assetID"] =
                                CAsepritePostProcesor.GetIDByAsset(m_AsepriteObjects[i]);
                            tImporter.userData = tData.ToString();
                            EditorUtility.SetDirty(tTarget);
                            EditorUtility.SetDirty(m_AsepriteObjects[i]);
                            AssetDatabase.WriteImportSettingsIfDirty(tPath);
                        }
                    }
                }
            } else {
                if ( GUILayout.Button("Select imported asset", GUILayout.MaxHeight(60)) ) {
                    for ( int i = 0; i < targets.Length; i++ ) {
                        m_AsepriteObjects[i].asepriteAsset = targets[i];
                        EditorUtility.SetDirty(m_AsepriteObjects[i]);
                    }
                    Selection.objects = m_AsepriteObjects;
                }
            }
        }
    }
}
