using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System;
using System.ComponentModel;
using System.IO;
using UnityEngine;

public static class TMConfig {	

        public enum AppMode{
            Capture,
            Receive
        }

		[System.Serializable]
		public struct Config {
			public AppMode appMode;
            public string receiverIP;
			public bool defaultEnableNetworkSender;
            public string port;
            public int defaultFrameRate;
			public string patchFilename;
			public string interfaceA_IP;
			public string interfaceB_IP;
        }

		

		/// <summary>
		/// This file should be present in the StreamingAssets folder
		/// </summary>
		public static string configFilename = "tm-config.json";


		private static bool configRead = false;

		private static Config currentConfig;

		public static Config Current {
			get {
				if (!configRead){
					LoadConfig();
				}
				return currentConfig;
			}
		}
		
		// private static Dictionary<string, string> commandLineArguments = new Dictionary<string, string>();

		private static void LoadConfig(){

			currentConfig = default(Config);

			// Loads the default config from JSON, if it exists
			var configPath = Path.Combine(Path.GetFullPath(Application.streamingAssetsPath), configFilename);
			if (File.Exists(configPath)){				
				Debug.Log("Reading JSON Config file from " + configPath);
				currentConfig = JsonUtility.FromJson<Config>(File.ReadAllText(configPath));				
			} else {
				Debug.LogError("Could not find JSON config file at " + configPath);
			}

			List<FieldInfo> fields = new List<FieldInfo>();
			fields.AddRange(typeof(Config).GetFields()); // do some reflection magic

			if (Application.isEditor && TMEditorConfigurator.DoConfigOverride){
				Debug.Log("Overriding config from editor");
				currentConfig = TMEditorConfigurator.OverrideConfig;				

			} else {

				// override these values with anything we find on the command line
				string[] rawArgs = Environment.GetCommandLineArgs();	
				for (int i = 0; i < rawArgs.Length; ++i)
				{
					fields.ForEach((FieldInfo f)=>{
						// if a string argument matches a parameter in Config struct, override it
						if (f.Name == rawArgs[i] && (i+1 < rawArgs.Length)){						
							currentConfig = UpdateWithStringValue(currentConfig, f, rawArgs[i+1]);						
						}
					});
				}

			}

			// write entire config to console
			fields.ForEach((FieldInfo f)=>{
				Debug.Log(f.Name + " : " + f.GetValue(currentConfig));
			});


			configRead = true;

		}

		private static Config UpdateWithStringValue(Config c, FieldInfo f, string value){
			try {
				var parsedValue = TypeDescriptor.GetConverter(f.GetType()).ConvertFromString(value);
				if (parsedValue != null){
					f.SetValue(c, parsedValue);
					
				}
			} catch(NotSupportedException){
				
			}

			return c;
		}

	}
