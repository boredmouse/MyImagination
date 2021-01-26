using UnityEngine;
using UnityEditor;
using System.IO;

namespace RhoTools.Aseprite
{
    /// <summary>
    /// Inspector for CAsepriteObject
    /// </summary>
    [CustomEditor(typeof(CAsepriteObject)), CanEditMultipleObjects]
    public class CAsepriteObjectEditor : Editor
    {
        class Property<T>
        {
            public T value;
            public bool mixed;
            public bool dirty;

            public Property(T aValue, bool aMixed)
            {
                value = aValue;
                dirty = false;
                mixed = aMixed;
            }
        }

        GUIContent m_ButtonContentPrefab;
        GUIContent ButtonContentPrefab
        {
            get
            {
                if (m_ButtonContentPrefab == null)
                {
                    m_ButtonContentPrefab = new GUIContent(
                        CAsepriteWindow.GetTexture(this, "prefab.png"),
                        "Creates a prefab");
                }
                return m_ButtonContentPrefab;
            }
        }
        GUIContent m_ButtonContentClear;
        GUIContent ButtonContentClear
        {
            get
            {
                if (m_ButtonContentClear == null)
                {
                    m_ButtonContentClear = new GUIContent(
                        CAsepriteWindow.GetTexture(this, "clear.png"),
                        "Removes all assets asociated with this file");
                }
                return m_ButtonContentClear;
            }
        }
        GUIContent m_ButtonContentSettings;
        GUIContent ButtonContentSettings
        {
            get
            {
                if (m_ButtonContentSettings == null)
                {
                    m_ButtonContentSettings = new GUIContent(
                        CAsepriteWindow.GetTexture(this, "settings.png"),
                        "Open settings window");
                }
                return m_ButtonContentSettings;
            }
        }

        CAsepriteObject [] m_AssetTargets;
        Property<Texture2D> m_TargetTexture;
        Property<TextAsset> m_TargetAtlas;
        Property<bool> m_UseTags;
        Property<bool> m_LoopAnim;
        Property<bool> m_UseConfig;
        Property<int> m_Border;
        Property<bool> m_UseChild;
        Property<SpriteAlignment> m_Alignment;
        Property<Vector2> m_Pivot;
        Property<AnimationType> m_AnimType;

        bool IsDirty
        {
            get
            {
                return m_TargetTexture.dirty ||
                    m_TargetAtlas.dirty ||
                    m_UseTags.dirty ||
                    m_LoopAnim.dirty ||
                    m_UseConfig.dirty ||
                    m_Border.dirty ||
                    m_UseChild.dirty ||
                    m_Alignment.dirty ||
                    m_Pivot.dirty ||
                    m_AnimType.dirty;
            }
        }

        private void OnEnable()
        {
            m_AssetTargets = new CAsepriteObject[targets.Length];
            for (int i = 0; i < targets.Length; i++)
                m_AssetTargets[i] = targets[i] as CAsepriteObject;
            RevertChanges();
        }

        public override void OnInspectorGUI()
        {
            int assetLost = 0;
            for ( int i = 0; i < m_AssetTargets.Length; i++ ) {
                if ( m_AssetTargets[i].asepriteAsset == null ) {
                    var asset = CAsepritePostProcesor.FindAsepriteAsset(
                        m_AssetTargets[i]);
                    if ( asset != null ) {
                        m_AssetTargets[i].asepriteAsset = asset;
                        EditorUtility.SetDirty(m_AssetTargets[i]);
                    } else {
                        assetLost++;
                    }
                }
            }
            if ( assetLost > 0 ) {
                GUILayout.Label((assetLost > 1 ? "These objects" : "This object")
                    + " lost connection to " 
                    + (assetLost > 1 ? "their assets" : "its asset"));
                if ( m_AssetTargets.Length > 1 ) {
                    GUILayout.Label("Select " + (assetLost > 1 ? "one" : "it")
                        + " to fix it");
                    for ( int i = 0; i < m_AssetTargets.Length; i++ ) {
                        if ( m_AssetTargets[i].asepriteAsset == null ) {
                            if ( GUILayout.Button(m_AssetTargets[i].name) ) {
                                Selection.activeObject = m_AssetTargets[i];
                            }
                        }
                    }
                } else {
                    GUILayout.Label("Select the correct asset to fix it");
                    var obj = EditorGUILayout.ObjectField(null, typeof(Object), false);
                    if ( obj != null ) {
                        string path = AssetDatabase.GetAssetPath(obj);
                        if ( path.ToLower().EndsWith(".ase") ) {
                            m_AssetTargets[0].asepriteAsset = obj;
                        }
                    }
                }
                return;
            }
            GUILayout.Label("Assets", EditorStyles.boldLabel);
            GUILayout.BeginVertical(CAsepriteGUIData.Box);

            ShowProperty(m_TargetTexture, CAsepriteGUIData.TargetTexture);
            ShowProperty(m_TargetAtlas, CAsepriteGUIData.TargetAtlas);

            GUILayout.EndVertical();

            // Configuration
            GUILayout.Label("Configuration", EditorStyles.boldLabel);
            GUILayout.BeginVertical(CAsepriteGUIData.Box);

            ShowProperty(m_UseTags, CAsepriteGUIData.UseTags);
            ShowProperty(m_LoopAnim, CAsepriteGUIData.LoopAnim);
            ShowProperty(m_UseConfig, CAsepriteGUIData.UseConfig);
            ShowProperty(m_Border, CAsepriteGUIData.Border);
            ShowProperty(m_UseChild, CAsepriteGUIData.UseChild);
            // Alignment
            EditorGUI.showMixedValue = m_Alignment.mixed;
            EditorGUI.BeginChangeCheck();
            m_Alignment.value = (SpriteAlignment)EditorGUILayout.EnumPopup(
                CAsepriteGUIData.Alignment, m_Alignment.value);
            if (EditorGUI.EndChangeCheck())
            {
                m_Alignment.dirty = true;
                m_Alignment.mixed = false;
            }
            // Pivot
            EditorGUI.BeginDisabledGroup(m_Alignment.value != SpriteAlignment.Custom
                || m_Alignment.mixed);
            EditorGUI.showMixedValue = m_Pivot.mixed;
            EditorGUI.BeginChangeCheck();
            m_Pivot.value = EditorGUILayout.Vector2Field(
                CAsepriteGUIData.Pivot, m_Pivot.value);
            if (EditorGUI.EndChangeCheck())
            {
                m_Pivot.dirty = true;
                m_Pivot.mixed = false;
            }
            EditorGUI.EndDisabledGroup();
            // Anim type
            EditorGUI.showMixedValue = m_AnimType.mixed;
            EditorGUI.BeginChangeCheck();
            m_AnimType.value = (AnimationType)EditorGUILayout.EnumPopup(
                CAsepriteGUIData.AnimType, m_AnimType.value);
            if (EditorGUI.EndChangeCheck())
            {
                m_AnimType.dirty = true;
                m_AnimType.mixed = false;
            }


            EditorGUI.showMixedValue = false;
            GUILayout.EndVertical();

            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button(ButtonContentPrefab, GUILayout.MaxHeight(64)))
            {
                foreach (CAsepriteObject tObj in m_AssetTargets)
                    CAsepriteWindow.CreatePrefab(tObj);
            }
            if (GUILayout.Button(ButtonContentSettings, GUILayout.MaxHeight(64)))
            {
                CAsepriteWindow.ShowWindow();
            }
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            EditorGUI.BeginDisabledGroup(!IsDirty);
            if (GUILayout.Button("Revert"))
            {
                RevertChanges();
            }
            if (GUILayout.Button("Apply"))
            {
                ApplyChanges();
            }
            EditorGUI.EndDisabledGroup();
            GUILayout.EndHorizontal();
        }

        void ShowProperty(Property<bool> aProperty, GUIContent aContent)
        {
            EditorGUI.showMixedValue = aProperty.mixed;
            EditorGUI.BeginChangeCheck();
            aProperty.value = EditorGUILayout.Toggle(aContent, aProperty.value);
            if (EditorGUI.EndChangeCheck())
            {
                aProperty.dirty = true;
                aProperty.mixed = false;
            }
        }

        void ShowProperty(Property<int> aProperty, GUIContent aContent)
        {
            EditorGUI.showMixedValue = aProperty.mixed;
            EditorGUI.BeginChangeCheck();
            aProperty.value = EditorGUILayout.IntField(aContent, aProperty.value);
            if (EditorGUI.EndChangeCheck())
            {
                aProperty.dirty = true;
                aProperty.mixed = false;
            }
        }

        void ShowProperty<T>(Property<T> aProperty, GUIContent aContent) where T : UnityEngine.Object
        {
            EditorGUI.showMixedValue = aProperty.mixed;
            EditorGUI.BeginChangeCheck();
            aProperty.value = (T)EditorGUILayout.ObjectField(aContent, aProperty.value,
                typeof(T), false);
            if (EditorGUI.EndChangeCheck())
            {
                aProperty.dirty = true;
                aProperty.mixed = false;
            }
        }

        void ApplyChanges()
        {
            for (int i = 0; i < m_AssetTargets.Length; i++)
            {
                CAsepriteObject tObj = m_AssetTargets[i];
                if (m_UseTags.dirty)
                    tObj.useTags = m_UseTags.value;
                if (m_LoopAnim.dirty)
                    tObj.loopAnim = m_LoopAnim.value;
                if (m_UseConfig.dirty)
                    tObj.useConfig = m_UseConfig.value;
                if (m_Border.dirty)
                    tObj.border = m_Border.value;
                if (m_UseChild.dirty)
                    tObj.useChild = m_UseChild.value;
                if (m_Alignment.dirty)
                    tObj.alignment = m_Alignment.value;
                if (m_Pivot.dirty)
                    tObj.pivot = m_Pivot.value;
                if (m_AnimType.dirty)
                    tObj.animType = m_AnimType.value;
                Import(AssetDatabase.GetAssetPath(tObj.asepriteAsset), tObj);
                EditorUtility.SetDirty(tObj);
            }

            m_UseTags.dirty = false;
            m_LoopAnim.dirty = false;
            m_UseConfig.dirty = false;
            m_Border.dirty = false;
            m_UseChild.dirty = false;
            m_Alignment.dirty = false;
            m_Pivot.dirty = false;
            m_AnimType.dirty = false;
        }

        void RevertChanges()
        {
            // Target texture
            Texture2D tTextureValue = m_AssetTargets[0].targetTexture;
            bool tMixed = false;
            for (int i = 1; i < m_AssetTargets.Length; i++)
            {
                if (m_AssetTargets[i].targetTexture != tTextureValue)
                {
                    tMixed = true;
                    break;
                }
            }
            m_TargetTexture = new Property<Texture2D>(tTextureValue, tMixed);
            // Text asset
            TextAsset tTextValue = m_AssetTargets[0].targetAtlas;
            tMixed = false;
            for (int i = 1; i < m_AssetTargets.Length; i++)
            {
                if (m_AssetTargets[i].targetAtlas != tTextureValue)
                {
                    tMixed = true;
                    break;
                }
            }
            m_TargetAtlas = new Property<TextAsset>(tTextValue, tMixed);
            // Use tags
            bool tValue = m_AssetTargets[0].useTags;
            tMixed = false;
            for (int i = 1; i < m_AssetTargets.Length; i++)
            {
                if (m_AssetTargets[i].useTags != tValue)
                {
                    tMixed = true;
                    break;
                }
            }
            m_UseTags = new Property<bool>(tValue, tMixed);
            // Loop anim
            tValue = m_AssetTargets[0].loopAnim;
            tMixed = false;
            for (int i = 1; i < m_AssetTargets.Length; i++)
            {
                if (m_AssetTargets[i].loopAnim != tValue)
                {
                    tMixed = true;
                    break;
                }
            }
            m_LoopAnim = new Property<bool>(tValue, tMixed);
            // Use config
            tValue = m_AssetTargets[0].useConfig;
            tMixed = false;
            for (int i = 1; i < m_AssetTargets.Length; i++)
            {
                if (m_AssetTargets[i].useConfig != tValue)
                {
                    tMixed = true;
                    break;
                }
            }
            m_UseConfig = new Property<bool>(tValue, tMixed);
            // Use config
            int tIntValue = m_AssetTargets[0].border;
            tMixed = false;
            for (int i = 1; i < m_AssetTargets.Length; i++)
            {
                if (m_AssetTargets[i].border != tIntValue)
                {
                    tMixed = true;
                    break;
                }
            }
            m_Border = new Property<int>(tIntValue, tMixed);
            // Use child
            tValue = m_AssetTargets[0].useChild;
            tMixed = false;
            for (int i = 1; i < m_AssetTargets.Length; i++)
            {
                if (m_AssetTargets[i].useChild != tValue)
                {
                    tMixed = true;
                    break;
                }
            }
            m_UseChild = new Property<bool>(tValue, tMixed);
            // Alignment
            SpriteAlignment tAlignValue = m_AssetTargets[0].alignment;
            tMixed = false;
            for (int i = 1; i < m_AssetTargets.Length; i++)
            {
                if (m_AssetTargets[i].alignment != tAlignValue)
                {
                    tMixed = true;
                    break;
                }
            }
            m_Alignment = new Property<SpriteAlignment>(tAlignValue, tMixed);
            // Pivot
            Vector2 tVectorValue = m_AssetTargets[0].pivot;
            tMixed = false;
            for (int i = 1; i < m_AssetTargets.Length; i++)
            {
                if (m_AssetTargets[i].pivot != tVectorValue)
                {
                    tMixed = true;
                    break;
                }
            }
            m_Pivot = new Property<Vector2>(tVectorValue, tMixed);
            // Pivot
            AnimationType tAnimValue = m_AssetTargets[0].animType;
            tMixed = false;
            for (int i = 1; i < m_AssetTargets.Length; i++)
            {
                if (m_AssetTargets[i].animType != tAnimValue)
                {
                    tMixed = true;
                    break;
                }
            }
            m_AnimType = new Property<AnimationType>(tAnimValue, tMixed);
        }

        private void OnDestroy()
        {
            if (IsDirty)
            {
                string tText = "Unapplied import settings for "
                    + (targets.Length > 1 ? "'" + targets.Length.ToString()
                    + "' fies" : "'" + AssetDatabase.GetAssetPath(target) + "'");
                if (EditorUtility.DisplayDialog("Unapplied import settings", tText, "Apply", "Revert"))
                    ApplyChanges();
            }
        }

        /// <sumary>
        /// Creates a new generic CAsepriteObject
        /// </sumary>
        public static CAsepriteObject NewInstance()
        {
            CAsepriteObject tObj = CreateInstance<CAsepriteObject>();
            tObj.useTags = CAsepriteWindow.UseTags;
            tObj.loopAnim = CAsepriteWindow.LoopAnim;
            tObj.useConfig = CAsepriteWindow.UseConfig;
            tObj.border = CAsepriteWindow.Border;
            tObj.useChild = CAsepriteWindow.UseChild;
            tObj.alignment = CAsepriteWindow.Alignment;
            tObj.pivot = CAsepriteWindow.Pivot;
            tObj.animType = CAsepriteWindow.AnimType;
            /*tObj.autoImport = CAsepriteWindow.AutoImport;
            tObj.importSpritesheet = CAsepriteWindow.ImportSpritesheet;
            tObj.importAnimations = CAsepriteWindow.ImportAnimations;
            tObj.createAnimator = CAsepriteWindow.CreateAnimatorDefault;*/
            tObj.clips = new AnimationClip[0];
            return tObj;
        }

        /// <sumary>
        /// Creates a new CAsepriteObject asset on aPath
        /// </sumary>
        /// <param name="aPath">Path</param>
        public static CAsepriteObject CreateAsset(string aPath)
        {
            CAsepriteObject tObj = NewInstance();
            AssetDatabase.CreateAsset(tObj, Path.GetDirectoryName(aPath)
                + "/" + Path.GetFileNameWithoutExtension(aPath) + ".asset");
            return tObj;
        }

        /// <sumary>
        /// Import ase file at aPath to aObject
        /// </sumary>
        /// <param name="aPath">Path</param>
        /// <param name="aObject">Aseprite object</param>
        public static bool Import(string aPath, CAsepriteObject aObject)
        {
            bool tExported = CAsepriteExporter.Export(aPath, aObject);

            if (tExported)
            {
                TextureImporter tTexture = AssetImporter.GetAtPath(
                        AssetDatabase.GetAssetPath(aObject.targetTexture)) as TextureImporter;

                // TODO: See if I can put the texture as an embeded object
                /*
                Texture2D tNewTex = new Texture2D(1, 1);
                tNewTex.SetPixel(0, 0, Color.cyan);
                tNewTex.Apply();
                AssetDatabase.AddObjectToAsset(tNewTex, aObject);
                */

                CAsepriteImporter.ImportSheet(tTexture, aObject);
                CAsepriteImporter.ImportAnimation(tTexture, aObject);
                CAsepriteWindow.CreateAnimator(aObject);
            }

            return tExported;
        }
    }
}
