using System;
using System.Collections.Generic;
using System.IO;
using App.Scripts.Scenes.SceneWordSearch.Features.Level.Models.Level;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using static UnityEditor.Progress;

namespace App.Scripts.Scenes.SceneWordSearch.Features.Level.BuilderLevelModel.ProviderWordLevel
{
    public class ProviderWordLevel : IProviderWordLevel
    {
        private List<LevelInfo> _wordsList = new();

        //todo насколько критично загружать все файлы? может лучше загружать поодиночке
        public ProviderWordLevel()
        {
            var jsonFiles =  Resources.LoadAll<TextAsset>("WordSearch");


            foreach (var asset in jsonFiles)
            {
                var dSClass = JsonUtility.FromJson<LevelInfo>(asset.text);
                _wordsList.Add(dSClass);
            }

        }

        public LevelInfo LoadLevelData(int levelIndex)
        {
            //напиши реализацию не меняя сигнатуру функции
            
            return _wordsList[levelIndex - 1];
        }
    }
}