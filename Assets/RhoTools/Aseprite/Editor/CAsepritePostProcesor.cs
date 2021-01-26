using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using RhoTools.SimpleJSON;

namespace RhoTools.Aseprite
{
    public class CAsepritePostProcesor : AssetPostprocessor
    {
        static List<string[]> m_ImportedAssets = new List<string[]>();

        /// <sumary>
        /// Get asset of type T by its ID
        /// </sumary>
        /// <param name="aID">Asset ID</param>
        public static T GetAssetByID<T>(string aID) where T : Object
        {
            return AssetDatabase.LoadAssetAtPath<T>(
                AssetDatabase.GUIDToAssetPath(aID));
        }

        /// <sumary>
        /// Get asset ID
        /// </sumary>
        /// <param name="aAsset">Asset</param>
        public static string GetIDByAsset(Object aAsset)
        {
            return AssetDatabase.AssetPathToGUID(
                AssetDatabase.GetAssetPath(aAsset));
        }

        static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets,
            string[] movedAssets, string[] movedFromAssetPaths)
        {
            m_ImportedAssets.Add(importedAssets);
            EditorApplication.delayCall += PostProcessDelayed;
        }

        static void PostProcessDelayed()
        {
            EditorApplication.delayCall -= PostProcessDelayed;
            if (m_ImportedAssets.Count == 0)
                return;

            string[] tImportedAssets = m_ImportedAssets[0];
            m_ImportedAssets.RemoveAt(0);

            bool tSaveAssets = false;
            for (int i = 0; i < tImportedAssets.Length; i++)
            {
                string tPath = tImportedAssets[i];
                if (tPath.EndsWith(".ase"))
                {
                    Object tAsset = AssetDatabase.LoadAssetAtPath<Object>(tPath);

                    bool tDirty = false;
                    AssetImporter tImporter = AssetImporter.GetAtPath(tPath);
                    JSONNode tData = JSON.Parse(tImporter.userData);
                    if (tData == null)
                        tData = new JSONObject();
                    JSONNode tID = tData["assetID"];
                    CAsepriteObject tObj = null;
                    if (tID != null)
                    {
                        tObj = GetAssetByID<CAsepriteObject>(tID.Value);
                        if (tObj == null)
                            tObj = FindAsepriteObject(tAsset);
                    }
                    else
                    {
                        tID = new JSONNumber(0);
                        tData["assetID"] = tID;
                        tDirty = true;
                    }

                    if (tObj == null) {
                        tObj = CAsepriteObjectEditor.CreateAsset(tPath);
                        /*
                        { "targetTexture":"c92e2bd276d32ed4984c0a349da9dcc8",
                        "targetText":"1cb757cd5d2c60f4cb1d15f7653554bc",
                        "useTags":"true",
                        "loopAnim":"false",
                        "useConfig":"true",
                        "border":"5",
                        "useChild":"true",
                        "alignment":"0",
                        "pivotX":"0.100000001490116",
                        "pivotY":"0.200000002980232",
                        "animType":"0",
                        "clips":["7ab1c6126e7e91c4d8d19e3f7e8e8f6d","416c940187f463c4f9af57db128e5cbe"],
                        "autoImport":"false",
                        "importAnimations":"true",
                        "importSpritesheet":"true",
                        "createAnimator":"true",
                        "targetAnimator":"88400be09d870114491df24486087eb5",
                        "targetPrefab":"c1df4f5e993e4b347a7ab29909bebb01"}
                        */
                        SetAssetValue(tData, "targetTexture", ref tObj.targetTexture);
                        SetAssetValue(tData, "targetText", ref tObj.targetAtlas);
                        SetBool(tData, "useTags", ref tObj.useTags);
                        SetBool(tData, "loopAnim", ref tObj.loopAnim);
                        SetBool(tData, "useConfig", ref tObj.useConfig);
                        SetInt(tData, "border", ref tObj.border);
                        SetFloat(tData, "pivotX", ref tObj.pivot.x);
                        SetFloat(tData, "pivotY", ref tObj.pivot.y);
                        int alignment = -1;
                        if ( SetInt(tData, "alignment", ref alignment) ) {
                            tObj.alignment = (SpriteAlignment)alignment;
                        }
                        var node = tData["clips"];
                        if ( node != null && node.IsArray ) {
                            var clips = node.AsArray;
                            tObj.clips = new AnimationClip[clips.Count];
                            for ( int j = 0; j < tObj.clips.Length; j++ ) {
                                tObj.clips[j] = GetAssetByID<AnimationClip>(clips[j].Value);
                            }
                        }
                        SetAssetValue(tData, "targetAnimator", ref tObj.targetAnimator);
                        SetAssetValue(tData, "targetPrefab", ref tObj.targetPrefab);

                        EditorUtility.SetDirty(tObj);
                    }
                    tID.Value = GetIDByAsset(tObj);
                    tObj.asepriteAsset = tAsset;
                    bool tExported = CAsepriteObjectEditor.Import(tPath, tObj);

                    if (tExported)
                    {
                        tImporter.userData = tData.ToString();
                        tSaveAssets = true;
                        tDirty = true;
                    }
                    if (tDirty)
                    {
                        EditorUtility.SetDirty(tAsset);
                        AssetDatabase.WriteImportSettingsIfDirty(tPath);
                    }
                    EditorUtility.SetDirty(tObj);
                }
            }

            if (tSaveAssets)
                AssetDatabase.SaveAssets();
        }

        static bool SetAssetValue<T>(JSONNode data, string key, ref T variable) where T : Object {
            var node = data[key];
            if ( node != null ) {
                var asset = GetAssetByID<T>(node.Value);
                variable = asset;
                return true;
            }
            return false;
        }

        static bool SetBool(JSONNode data, string key, ref bool variable) {
            var node = data[key];
            if ( node != null ) {
                variable = node.AsBool;
                return true;
            }
            return false;
        }

        static bool SetInt(JSONNode data, string key, ref int variable) {
            var node = data[key];
            if ( node != null ) {
                variable = node.AsInt;
                return true;
            }
            return false;
        }

        static bool SetFloat(JSONNode data, string key, ref float variable) {
            var node = data[key];
            if ( node != null ) {
                variable = node.AsFloat;
                return true;
            }
            return false;
        }

        /// <sumary>
        /// Find aseprite object corresponding to Asset
        /// </sumary>
        /// <param name="aAsset">Asset</param>
        public static CAsepriteObject FindAsepriteObject(Object aAsset)
        {
            CAsepriteObject[] tObjects = Resources.FindObjectsOfTypeAll<CAsepriteObject>();
            for (int j = 0; j < tObjects.Length; j++)
            {
                if (tObjects[j].asepriteAsset == aAsset)
                {
                    return tObjects[j];
                }
            }
            return null;
        }

        /// <sumary>
        /// Find aseprite asset corresponding to AsepriteObject
        /// </sumary>
        /// <param name="aObject">Aseprite object</param>
        public static Object FindAsepriteAsset(CAsepriteObject aObject)
        {
            Object[] tObjects = Resources.FindObjectsOfTypeAll<Object>();
            for (int j = 0; j < tObjects.Length; j++)
            {
                var asset = tObjects[j];
                var path = AssetDatabase.GetAssetPath(asset);
                if (path.ToLower().EndsWith(".ase") )
                {
                    AssetImporter tImporter =
                        AssetImporter.GetAtPath(path);
                    JSONNode tData = JSON.Parse(tImporter.userData);
                    JSONNode tID = tData["assetID"];
                    if ( tID != null ) {
                        var obj = GetAssetByID<CAsepriteObject>(tID.Value);
                        if ( obj != null )
                            return asset;
                    }
                }
            }
            return null;
        }
    }
}
