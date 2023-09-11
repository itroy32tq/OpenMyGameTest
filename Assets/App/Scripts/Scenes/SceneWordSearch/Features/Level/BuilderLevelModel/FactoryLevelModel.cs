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
        
            foreach (KeyValuePair<char, int> weightedWord in weightedMap)
            {
  
                var pair = weightedMap.Where(x => x.Key == weightedWord.Key).OrderByDescending(x => x.Value).FirstOrDefault();
                int ind = pair.Value;
                char sym = pair.Key;

                if (!charsList.Contains(sym))
                {
                    List<char> ss = new();
                    for (int _ = 0; _ <= ind; _++) ss.Add(sym);

                    charsList.AddRange(ss);
                }
                
            }
            return charsList;
        }
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