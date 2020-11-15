﻿using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

namespace YoungPackage.Data
{
    public interface IDataBase
    {
    }

    [Serializable]
    public class SaveType<T>
    {
        public T Val;

        public SaveType()
        {
        }

        public SaveType(T defaultVal)
        {
            Val = defaultVal;
        }
    }

    /*
        * << Scenario >> 
        * void Awake()
        * {
        *     [Init]
        *     GameDataManager.PreInit(new UserInfo(), new AdInfo());
        *     GameDataManager.PreInit<UserInfo, AdInfo>(new UserInfo(), new AdInfo());
        *
        *     [Load]
        *     var userInfo = GameDataManager.GetData<UserInfo>();
        *
        *     [Save]
        *     GameDataManager.SaveData<UserInfo>();
        * }
    */
    public static class LocalDataManager
    {
        private static readonly Dictionary<Type, IDataBase> DataDictionary = new Dictionary<Type, IDataBase>();
        private static bool _isInitialized;
        private static Action<string, string> _saveCloudAction;

        public static void PreInit<T1>(T1 default1, Action<string, string> saveCloudAction = null) where T1 : IDataBase
        {
            if (_isInitialized)
                return;

            if (!DataDictionary.ContainsKey(default1.GetType()))
                DataDictionary.Add(default1.GetType(), InitData(default1));

            _saveCloudAction = saveCloudAction;
            _isInitialized = true;
        }

        public static void PreInit<T1, T2>(T1 default1, T2 default2, Action<string, string> saveCloudAction = null)
            where T1 : IDataBase where T2 : IDataBase
        {
            if (_isInitialized)
                return;

            if (!DataDictionary.ContainsKey(default1.GetType()))
                DataDictionary.Add(default1.GetType(), InitData(default1));

            if (!DataDictionary.ContainsKey(default2.GetType()))
                DataDictionary.Add(default2.GetType(), InitData(default2));

            _saveCloudAction = saveCloudAction;
            _isInitialized = true;
        }

        public static void PreInit<T1, T2, T3>(T1 default1, T2 default2, T3 default3,
            Action<string, string> saveCloudAction = null)
            where T1 : IDataBase where T2 : IDataBase where T3 : IDataBase
        {
            if (_isInitialized)
                return;

            if (!DataDictionary.ContainsKey(default1.GetType()))
                DataDictionary.Add(default1.GetType(), InitData(default1));

            if (!DataDictionary.ContainsKey(default2.GetType()))
                DataDictionary.Add(default2.GetType(), InitData(default2));

            if (!DataDictionary.ContainsKey(default3.GetType()))
                DataDictionary.Add(default3.GetType(), InitData(default3));

            _saveCloudAction = saveCloudAction;
            _isInitialized = true;
        }

        public static T GetData<T>() where T : class
        {
            if (_isInitialized == false)
                return null;

            if (DataDictionary.ContainsKey(typeof(T)) == false)
                return null;

            return DataDictionary[typeof(T)] as T;
        }

        public static void SaveData<T>(bool isSaveWithCloud = true) where T : IDataBase
        {
            if (_isInitialized == false)
                return;

            if (DataDictionary.ContainsKey(typeof(T)) == false)
                return;

            SaveDataToJson(DataDictionary[typeof(T)], isSaveWithCloud);
        }

        public static void SetData<T>(T data, bool isSaveWithCloud = false) where T : IDataBase
        {
            if (DataDictionary.ContainsKey(typeof(T)))
                DataDictionary.Remove(typeof(T));

            DataDictionary.Add(data.GetType(), data);
            SaveDataToJson(data, isSaveWithCloud);
        }

        private static T InitData<T>(T defaultT) where T : IDataBase
        {
            var curType = defaultT.GetType();
            var loadData = LoadDataFromJson(defaultT);

            var fields = curType.GetFields();

            foreach (var fieldInfo in fields)
            {
                if (fieldInfo.GetValue(loadData) == null)
                {
                    fieldInfo.SetValue(loadData, fieldInfo.GetValue(defaultT));
                }
            }

            SaveDataToJson(loadData, false);
            return loadData;
        }

        private static T LoadDataFromJson<T>(T defaultT) where T : IDataBase
        {
            var dataTypeKey = defaultT.GetType().Name;

            return defaultT;
            return PlayerPrefs.HasKey(dataTypeKey)
                ? JsonConvert.DeserializeObject<T>(PlayerPrefs.GetString(dataTypeKey, "null"))
                : defaultT;
        }

        private static void SaveDataToJson<T>(T data, bool isSaveWithCloud = false) where T : IDataBase
        {
            var saveJson = JsonConvert.SerializeObject(data);
            var typeName = data.GetType().Name;

            PlayerPrefs.SetString(typeName, saveJson);

            if (isSaveWithCloud)
                _saveCloudAction?.Invoke(typeName, saveJson);
        }
    }
}