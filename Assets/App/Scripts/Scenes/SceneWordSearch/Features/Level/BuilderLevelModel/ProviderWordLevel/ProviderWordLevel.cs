using System;
using System.Collections.Generic;
using System.Linq;
using App.Scripts.Scenes.SceneWordSearch.Features.Level.Models.Level;
using UnityEngine;

namespace App.Scripts.Scenes.SceneWordSearch.Features.Level.BuilderLevelModel.ProviderWordLevel
{
    public class ProviderWordLevel : IProviderWordLevel
    {
        private readonly List<LevelInfo> _wordsList = new();

        public ProviderWordLevel()
        {
            TextAsset[] jsonFiles = Resources.LoadAll<TextAsset>("WordSearch");

            foreach (TextAsset asset in jsonFiles)
            {
                LevelInfo levelInfo = JsonUtility.FromJson<LevelInfo>(asset.text);
                if (levelInfo != null) _wordsList.Add(levelInfo);
            }

        }

        public LevelInfo LoadLevelData(int levelIndex)
        {
            //напиши реализацию не меняя сигнатуру функции

            try 
            {
                ValidationWord(_wordsList[levelIndex - 1].words, levelIndex);

                return _wordsList[levelIndex - 1];
            }
            catch
            {
                throw new Exception("Отсутствуют уровни для ProviderWordLevel в папке ресурсов");
            }
        }

        /// <summary>
        /// метод валидирует список слов. Он не должен быть пустым и в нем должны быть только буквы
        /// посколько в условиях ничего не говорилось про это конкретно, я не прерываю выполнения программы
        /// а просто вывожу сообщение в лог - даже с пустым списком сцена не падает
        /// </summary>
        /// <param name="words"></param>
        /// <param name="index"></param>
        private void ValidationWord(List<string> words, int index)
        {
            if (words.Count != 0)
            {
                string aggregateWordsString = words.Aggregate((first, next) => next + first);
                if (!aggregateWordsString.All(char.IsLetter))
                {
                    Debug.Log("В уровене " + index + " в словах есть не буквенные символы");
                }
            }
            else Debug.Log("уровень " + index + "не содержит слов") ;
        }
    }
}