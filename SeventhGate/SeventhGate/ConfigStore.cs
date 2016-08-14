using System.Xml.Serialization;
using System.Collections.Generic;
using System.IO;
using System;
using System.Security;
using System.Linq;
using winapi;
using System.Collections.ObjectModel;

namespace SeventhGate
{
	#region Single Configuration Element

	/// <summary>
	/// Class represents single configuration element with value
	/// </summary>
	public class ConfigElement
	{
		/// <summary>
		/// Key
		/// </summary>
		public string Key
		{
			get;
			set;
		}

		/// <summary>
		/// Value
		/// </summary>
		public string Value
		{
			get;
			set;
		}
	}

	#endregion

	#region Main Configuration class

	public static class ConfigStore
	{
		/// <summary>
		/// Represents a list of current config elements
		/// Keeps current program configuration
		/// </summary>
		private static List<ConfigElement> config = new List<ConfigElement>();

		/// <summary>
		/// Adds new config element
		/// </summary>
		/// <param name="element"></param>
		public static void Add(ConfigElement element)
		{
			config.Add(element);
		}

		/// <summary>
		/// Removes specified config element
		/// </summary>
		/// <param name="element"></param>
		public static void Remove(ConfigElement element)
		{
			config.Remove(element);
		}

		/// <summary>
		/// Gets specified config element
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		public static ConfigElement Get(string key)
		{
			var elems = from e in config
						where string.Equals(key, e.Key)
						select e;
			if(elems.Count() > 0)
				return elems.First();
			return null;
		}

		/// <summary>
		/// Gets value of specified config elements
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		public static object GetValue(string key)
		{
			var elems = from e in config
						where string.Equals(key, e.Key)
						select e;
			if(elems.Count() > 0)
				return elems.First().Value;
			return "null";
		}

		/// <summary>
		/// Sets specified config element
		/// </summary>
		/// <param name="key"></param>
		/// <param name="value"></param>
		public static void Set(string key, string value)
		{
			var e = Get(key);
			if(e == null)
			{
				Add(new ConfigElement()
				{
					Key = key,
					Value = value
				});
				return;
			}
			e.Value = value;
		}

		/// <summary>
		/// Loads program configuration from file
		/// </summary>
		/// <param name="filePath"></param>
		/// <returns></returns>
		public static bool Load(string filePath)
		{
			if(File.Exists(filePath) == false)
				return false;

			FileStream fileStream = null;
			XmlSerializer xmlSerializer = null;
			List<ConfigElement> collection = null;

			try
			{
				fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
				xmlSerializer = new XmlSerializer(typeof(List<ConfigElement>));
				collection = (List<ConfigElement>)xmlSerializer.Deserialize(fileStream);

				fileStream.Close();
				xmlSerializer = null;

				config.Clear();
				config.AddRange(collection);

				return CheckLoadedConfigFile();
			}
			catch(ArgumentNullException ex)
			{
				Logger.LogException("ConfigStore", "Load", "讀取配置檔案失敗！", ex);
			}
			catch(ArgumentOutOfRangeException ex)
			{
				Logger.LogException("ConfigStore", "Load", "讀取配置檔案失敗！", ex);
			}
			catch(ArgumentException ex)
			{
				Logger.LogException("ConfigStore", "Load", "讀取配置檔案失敗！", ex);
			}
			catch(NotSupportedException ex)
			{
				Logger.LogException("ConfigStore", "Load", "讀取配置檔案失敗！", ex);
			}
			catch(FileNotFoundException ex)
			{
				Logger.LogException("ConfigStore", "Load", "讀取配置檔案失敗！", ex);
			}
			catch(DirectoryNotFoundException ex)
			{
				Logger.LogException("ConfigStore", "Load", "讀取配置檔案失敗！", ex);
			}
			catch(PathTooLongException ex)
			{
				Logger.LogException("ConfigStore", "Load", "讀取配置檔案失敗！", ex);
			}
			catch(IOException ex)
			{
				Logger.LogException("ConfigStore", "Load", "讀取配置檔案失敗！", ex);
			}
			catch(SecurityException ex)
			{
				Logger.LogException("ConfigStore", "Load", "讀取配置檔案失敗！", ex);
			}
			catch(UnauthorizedAccessException ex)
			{
				Logger.LogException("ConfigStore", "Load", "讀取配置檔案失敗！", ex);
			}

			return false;
		}

		/// <summary>
		/// Method checks all config elements whether they can be found
		/// </summary>
		/// <returns></returns>
		private static bool CheckLoadedConfigFile()
		{
			ConfigElement c = ConfigStore.Get(Constants.ConfigStringNetworkName);
			if(c == null)
			{
				Logger.LogError("ConfigStore", "CheckLoadedConfigFile", "配置檔案驗證失敗，檔案中找不到項目 " + Constants.ConfigStringNetworkName + " ");
				return false;
			}

			c = ConfigStore.Get(Constants.ConfigStringNetworkPass);
			if(c == null)
			{
				Logger.LogError("ConfigStore", "CheckLoadedConfigFile", "配置檔案驗證失敗，檔案中找不到項目 " + Constants.ConfigStringNetworkPass + " ");
				return false;
			}

			c = ConfigStore.Get(Constants.ConfigStringNetworkPassShown);
			if(c == null)
			{
				Logger.LogError("ConfigStore", "CheckLoadedConfigFile", "配置檔案驗證失敗，檔案中找不到項目 " + Constants.ConfigStringNetworkPassShown + " ");
				return false;
			}

			c = ConfigStore.Get(Constants.ConfigStringMaxPeersNum);
			if(c == null)
			{
				Logger.LogError("ConfigStore", "CheckLoadedConfigFile", "配置檔案驗證失敗，檔案中找不到項目 " + Constants.ConfigStringMaxPeersNum + " ");
				return false;
			}

			c = ConfigStore.Get(Constants.ConfigStringDebugWindow);
			if(c == null)
			{
				Logger.LogError("ConfigStore", "CheckLoadedConfigFile", "配置檔案驗證失敗，檔案中找不到項目 " + Constants.ConfigStringDebugWindow + " ");
				return false;
			}

			c = ConfigStore.Get(Constants.ConfigStringCheckUpdatesAtStart);
			if(c == null)
			{
				Logger.LogError("ConfigStore", "CheckLoadedConfigFile", "配置檔案驗證失敗，檔案中找不到項目 " + Constants.ConfigStringCheckUpdatesAtStart + " ");
				return false;
			}

			c = ConfigStore.Get(Constants.ConfigStringAnimateTrayIcon);
			if(c == null)
			{
				Logger.LogError("ConfigStore", "CheckLoadedConfigFile", "配置檔案驗證失敗，檔案中找不到項目 " + Constants.ConfigStringAnimateTrayIcon + " ");
				return false;
			}

			c = ConfigStore.Get(Constants.ConfigStringLastUsedConnection);
			if(c == null)
			{
				Logger.LogError("ConfigStore", "CheckLoadedConfigFile", "配置檔案驗證失敗，檔案中找不到項目 " + Constants.ConfigStringLastUsedConnection + " ");
				return false;
			}

			c = ConfigStore.Get(Constants.ConfigStringStartMinimized);
			if(c == null)
			{
				Logger.LogError("ConfigStore", "CheckLoadedConfigFile", "配置檔案驗證失敗，檔案中找不到項目 " + Constants.ConfigStringStartMinimized + " ");
				return false;
			}

			return true;
		}

		/// <summary>
		/// Saves program configuration to file
		/// </summary>
		/// <param name="filePath"></param>
		public static bool Save(string filePath)
		{
			FileStream fileStream = null;
			XmlSerializer xmlSerializer = null;

			try
			{
				fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None);
				xmlSerializer = new XmlSerializer(typeof(List<ConfigElement>));

				xmlSerializer.Serialize(fileStream, config);
				fileStream.Close();

				return true;
			}
			catch(ArgumentNullException ex)
			{
				Logger.LogException("ConfigStore", "Save", "儲存配置檔案時發生錯誤！", ex);
			}
			catch(ArgumentOutOfRangeException ex)
			{
				Logger.LogException("ConfigStore", "Save", "儲存配置檔案時發生錯誤！", ex);
			}
			catch(ArgumentException ex)
			{
				Logger.LogException("ConfigStore", "Save", "儲存配置檔案時發生錯誤！", ex);
			}
			catch(NotSupportedException ex)
			{
				Logger.LogException("ConfigStore", "Save", "儲存配置檔案時發生錯誤！", ex);
			}
			catch(FileNotFoundException ex)
			{
				Logger.LogException("ConfigStore", "Save", "儲存配置檔案時發生錯誤！", ex);
			}
			catch(DirectoryNotFoundException ex)
			{
				Logger.LogException("ConfigStore", "Save", "儲存配置檔案時發生錯誤！", ex);
			}
			catch(PathTooLongException ex)
			{
				Logger.LogException("ConfigStore", "Save", "儲存配置檔案時發生錯誤！", ex);
			}
			catch(IOException ex)
			{
				Logger.LogException("ConfigStore", "Save", "儲存配置檔案時發生錯誤！", ex);
			}
			catch(SecurityException ex)
			{
				Logger.LogException("ConfigStore", "Save", "儲存配置檔案時發生錯誤！", ex);
			}
			catch(UnauthorizedAccessException ex)
			{
				Logger.LogException("ConfigStore", "Save", "儲存配置檔案時發生錯誤！", ex);
			}
			catch(InvalidOperationException ex)
			{
				Logger.LogException("ConfigStore", "Save", "儲存配置檔案時發生錯誤！", ex);
			}

			return false;
		}

		/// <summary>
		/// Creates empty standard program config if file not found
		/// with default configuration
		/// </summary>
		public static void NewConfig()
		{
			Set(Constants.ConfigStringNetworkName, "MyNetworkName");
			Set(Constants.ConfigStringNetworkPass, "myp1a2s3s4w5o6r6d7");
			Set(Constants.ConfigStringNetworkPassShown, bool.TrueString);
			Set(Constants.ConfigStringMaxPeersNum, "5");
			Set(Constants.ConfigStringDebugWindow, bool.TrueString);
			Set(Constants.ConfigStringCheckUpdatesAtStart, bool.TrueString);
			Set(Constants.ConfigStringAnimateTrayIcon, bool.TrueString);
			Set(Constants.ConfigStringLastUsedConnection, "");
			Set(Constants.ConfigStringStartMinimized, bool.FalseString);

			Save(Constants.ConfigStringFileName);
		}
	}

	#endregion
}
