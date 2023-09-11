using System;
using System.Collections.Generic;
using App.Scripts.Scenes.SceneWordSearch.Features.Level.Models.Level;
using UnityEngine;

namespace App.Scripts.Scenes.SceneWordSearch.Features.Level.BuilderLevelModel.ProviderWordLevel
{
    public class ProviderWordLevel : IProviderWordLevel
    {
        private readonly List<LevelInfo> _wordsList = new();

        //todo насколько критично загружать все файлы? может лучше загружать поодиночке
        public ProviderWordLevel()
        {
            try
            {
                TextAsset[] jsonFiles = Resources.LoadAll<TextAsset>("WordSearch");

                foreach (var asset in jsonFiles)
                {
                    var dSClass = JsonUtility.FromJson<LevelInfo>(asset.text);
                    _wordsList.Add(dSClass);
                }
            }
            catch
            {
                throw new Exception("проблемы с данными для уровней в ProviderWordLevel");
            }

        }

        public LevelInfo LoadLevelData(int levelIndex)
        {
            //напиши реализацию не меняя сигнатуру функции
            
            return _wordsList[levelIndex - 1];
        }
    }
}