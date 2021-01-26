using System.Diagnostics;
using System.IO;
using UnityEditor;
using UnityEngine;
using Newtonsoft.Json;
using System;

namespace RhoTools.Aseprite
{
    /// <summary>
    /// Aseprite export tool
    /// </summary>
    public class CAsepriteExporter
    {
        /// <summary>
        /// Exports a .ase file to a .json and .png file using Aseprite CLI
        /// </summary>
        /// <param name="aFile">.ase file path</param>
        /// <param name="aObj">Aseprite object</param>
        /// <returns></returns>
        public static bool Export(string aFile, CAsepriteObject aObj)
        {
            string tExePath = CAsepriteWindow.AsepriteExePath;
            if (!File.Exists(tExePath))
            {
                UnityEngine.Debug.Log("File " + tExePath + " not found");
                return false;
            }
            // Create arguments
            string tName = Path.GetFileNameWithoutExtension(aFile);

            string tOutPath;
            if (aObj.targetTexture == null)
            {
                if (!CAsepriteWindow.UseConfiguredTargets || CAsepriteWindow.TargetDir == "")
                    tOutPath = Path.GetDirectoryName(aFile);
                else
                    tOutPath = CAsepriteWindow.TargetDir;
                if (!Directory.Exists(tOutPath))
                    Directory.CreateDirectory(tOutPath);

                tOutPath += "/" + tName + ".png";
            }
            else
                tOutPath = AssetDatabase.GetAssetPath(aObj.targetTexture);

            int tBorder = aObj.border;
            string tFramename = "";
            if (aObj.useTags)
                tFramename = "{frame000}_{tag}";
            else
                tFramename = "{frame000}_{title}";
            string args = "-b \"" + aFile + "\" --filename-format "
                + tFramename + " --sheet-pack ";
            if (tBorder > 0)
                args += "--inner-padding " + tBorder.ToString() + " ";
            args += "--sheet \"" + tOutPath + "\"";

            // Create process
            Process tProcess = new Process();
            tProcess.StartInfo.FileName = tExePath;
            tProcess.StartInfo.UseShellExecute = false;
            tProcess.StartInfo.Arguments = args;
            tProcess.StartInfo.CreateNoWindow = true;
            tProcess.StartInfo.RedirectStandardOutput = true;
            tProcess.StartInfo.RedirectStandardError = true;

            tProcess.EnableRaisingEvents = true;
            tProcess.Start();

            string tOutput = tProcess.StandardOutput.ReadToEnd();

            using (StreamReader s = tProcess.StandardError)
            {
                //string error = s.ReadToEnd();
                tProcess.WaitForExit(20000);
            }

            if (string.IsNullOrEmpty(tOutput))
            {
                return false; 
            }

            // Apply border to atlas
            if (tBorder > 0)
            {
                CAtlasData tData = JsonConvert.DeserializeObject<CAtlasData>(tOutput);
                foreach (CAtlasData.Frame tFrame in tData.frames.Values)
                {
                    tFrame.frame.x += tBorder;
                    tFrame.frame.w -= tBorder * 2;
                    tFrame.frame.y += tBorder;
                    tFrame.frame.h -= tBorder * 2;
                }
                tOutput = JsonConvert.SerializeObject(tData, Formatting.Indented);
            }

            string tOutJsonFilePath;
            if (aObj.targetAtlas == null)
            {
                if (!CAsepriteWindow.UseConfiguredTargets || CAsepriteWindow.TargetDir == "")
                    tOutJsonFilePath = Path.GetDirectoryName(aFile);
                else
                    tOutJsonFilePath = CAsepriteWindow.TargetDir;
                if (!Directory.Exists(tOutJsonFilePath))
                    Directory.CreateDirectory(tOutJsonFilePath);
                tOutJsonFilePath += "/" + tName + ".json";
            }
            else
                tOutJsonFilePath = AssetDatabase.GetAssetPath(aObj.targetAtlas);
            File.WriteAllText(tOutJsonFilePath, tOutput);


            if (tOutJsonFilePath.StartsWith(Application.dataPath))
                tOutJsonFilePath = "Assets" + tOutJsonFilePath.Substring(Application.dataPath.Length);
            if (tOutPath.StartsWith(Application.dataPath))
                tOutPath = "Assets" + tOutPath.Substring(Application.dataPath.Length);

            AssetDatabase.ImportAsset(tOutPath,
                ImportAssetOptions.ForceSynchronousImport);
            AssetDatabase.ImportAsset(tOutJsonFilePath,
                ImportAssetOptions.ForceSynchronousImport);

            aObj.targetAtlas = AssetDatabase.LoadAssetAtPath<TextAsset>(tOutJsonFilePath);
            aObj.targetTexture = AssetDatabase.LoadAssetAtPath<Texture2D>(tOutPath);
            aObj.LastExport = DateTime.Now;

            return true;
        }
    }
}
