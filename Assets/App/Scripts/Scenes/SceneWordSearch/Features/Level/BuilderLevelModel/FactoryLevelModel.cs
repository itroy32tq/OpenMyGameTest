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

        private List<char> BuildListChars(List<string> words)
        {

            //напиши реализацию не меняя сигнатуру функции
            List<char> chars = new();
            List<KeyValuePair<char, int>> weightedMap = new();

            foreach (string word in words) 
            {
                List<KeyValuePair<char, int>> mapping = WeighCharInWord(word).ToList();
                weightedMap.AddRange(mapping);
            }
        
            foreach (var w in weightedMap)
            {
  
                var verible = weightedMap.Where(x => x.Key == w.Key).OrderByDescending(x => x.Value).FirstOrDefault();
                int ind = verible.Value;
                char sym = verible.Key;

                if (!chars.Contains(sym))
                {
                    List<char> ss = new();
                    for (int _ = 0; _ <= ind; _++) ss.Add(sym);
                    
                    chars.AddRange(ss);
                }
                
            }
            return chars;
        }
        private Dictionary<char, int> WeighCharInWord(string word)
        {
            Dictionary<char, int> res = new();

            for (int i = 0; i < word.Length; i++)
            {
                if (res.ContainsKey(word[i])) res[word[i]]++;
                else res[word[i]] = 0;
            }

            return res;
        }
    }
}