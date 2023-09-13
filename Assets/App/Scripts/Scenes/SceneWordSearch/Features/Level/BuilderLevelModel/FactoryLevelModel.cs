using System;
using System.Collections.Generic;
using System.Linq;
using App.Scripts.Libs.Factory;
using App.Scripts.Scenes.SceneWordSearch.Features.Level.Models.Level;

namespace App.Scripts.Scenes.SceneWordSearch.Features.Level.BuilderLevelModel
{
    public class FactoryLevelModel : IFactory<LevelModel, LevelInfo, int>
    {
        public LevelModel Create(LevelInfo value, int levelNumber)
        {
            var model = new LevelModel();

            model.LevelNumber = levelNumber;

            model.Words = value.words;
            model.InputChars = BuildListChars(value.words);

            return model;
        }

        /// <summary>
        /// входной список слов разбивается на пары "символ и его количество", после чего
        /// в итоговый список записываются только уникальные ключи в максимальным значением количества
        /// </summary>
        /// <param name="words">список слов</param>
        /// <returns></returns>
        private List<char> BuildListChars(List<string> words)
        {

            //напиши реализацию не меняя сигнатуру функции

            List<char> charsList = new();
            List<KeyValuePair<char, int>> weightedMap = new();

            foreach (string word in words) 
            {
                List<KeyValuePair<char, int>> mapping = WeighCharInWord(word).ToList();
                weightedMap.AddRange(mapping);
            }
        
            foreach (KeyValuePair<char, int> pair in weightedMap)
            {

                KeyValuePair<char, int> maxValuePair = weightedMap.Where(x => x.Key == pair.Key).OrderByDescending(x => x.Value).FirstOrDefault();
                
                if (!charsList.Contains(maxValuePair.Key))
                {
                    List<char> ss = new();
                    for (int i = 0; i <= maxValuePair.Value; i++) ss.Add(maxValuePair.Key);

                    charsList.AddRange(ss);
                }   
                
            }
            return charsList;
        }

        

        /// <summary>
        /// метод "взвешивает" количество символов в слове и выдает словарь
        /// </summary>
        /// <param name="word">слово из словаря</param>
        /// <returns></returns>
        private Dictionary<char, int> WeighCharInWord(string word)
        {
            Dictionary<char, int> result = new();

            for (int i = 0; i < word.Length; i++)
            {
                if (result.ContainsKey(word[i])) result[word[i]]++;
                else result[word[i]] = 0;
            }

            return result;
        }
    }
}