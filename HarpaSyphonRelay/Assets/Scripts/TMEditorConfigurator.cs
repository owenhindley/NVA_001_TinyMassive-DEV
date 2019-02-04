using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class TMEditorConfigurator : MonoBehaviour {


		static TMConfig.Config overrideConfig; // this static config accessed by OKGOConfig at runtime
        public static TMConfig.Config OverrideConfig
        {
            get
            {
                return overrideConfig;
            }
            set
            {
                overrideConfig = value;
                var editors = FindObjectsOfType<TMEditorConfigurator>();
                foreach (var editor in editors)
                {
                    editor.EditorOverrideConfig = overrideConfig;
                }
            }
        }

		[Header("Hover for tooltip info")]
        public TMConfig.Config EditorOverrideConfig; // what's visible in the editor - gets copied over to the static one at validate-time		

		
		public static bool DoConfigOverride = false;		
		[Header("Important - enable editor override here")]
		public bool EditorDoConfigOverride;

		[Header("Generates and overwrites the config file based on these settings")]
		[InspectorButton("LocalCreateConfigFile")] public bool doCreateConfigFile;

		public void OnValidate(){
			OverrideConfig = EditorOverrideConfig;
			DoConfigOverride = EditorDoConfigOverride;
			Debug.Log("Updating override config");
		}

		public void LocalCreateConfigFile(){
			CreateConfigFile(OverrideConfig);
		}

		public static void CreateConfigFile(TMConfig.Config configToWrite){

			var jsonString = JsonUtility.ToJson(configToWrite, true);
			var configPath = Path.Combine(Path.GetFullPath(Application.streamingAssetsPath), TMConfig.configFilename);
			Debug.Log("Saving JSON config file to " + configPath);
			if (File.Exists(configPath)){ File.Delete(configPath); }
			File.WriteAllText(configPath, jsonString);

		}

    }

