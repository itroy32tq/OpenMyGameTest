using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using App.Scripts.Scenes.SceneFillwords.Features.FillwordModels;
using UnityEngine;

namespace App.Scripts.Scenes.SceneFillwords.Features.ProviderLevel
{
    public class ProviderFillwordLevel : IProviderFillwordLevel
    {
        private TextAsset _wordMapData;
        private TextAsset _levelMapData;
        private string[] wordMap;
        private string[] levelMapList;

        private string patternLevel = @"(\d{1,4}\s(?:\d{1,}[;]){1,})\d{1,}";
        private readonly string pattern = @"(\d{1,4}\s)(\S*)";

        private GridFillWords model;
        //todo пока костыль, не очень ясно какая математика для связи координат
        private readonly List<Vector2Int> modelMask = new()
        { 
            new Vector2Int(0,0), new Vector2Int(0,1), new Vector2Int(1,0), new Vector2Int(1,1),
            new Vector2Int(2,0), new Vector2Int(2,1), new Vector2Int(0,2), new Vector2Int(1,2),
            new Vector2Int(2,2)
        };

        public ProviderFillwordLevel(TextAsset wordMapData, TextAsset levelMapData)
        {
            _wordMapData = wordMapData; _levelMapData = levelMapData;

            wordMap = _wordMapData.text.Split('\n');
            levelMapList = _levelMapData.text.Split("\n");
        }

        private KeyValuePair<string, int[]> ParseLevelProperty(string level)
        {
            Regex regex = new(pattern);
            Match match = regex.Match(level);

            int wordIndex = Int32.Parse(match.Groups[1].Value);
           
            return new KeyValuePair<string, int[]>(wordMap[wordIndex], match.Groups[2].Value.Split(';').Select(x => int.Parse(x)).ToArray());
        }

        private List<string> ParseLevelData(int indexLevel)
        {

            List <string> result = new();

            string level = levelMapList[indexLevel];

            MatchCollection matches = Regex.Matches(level, patternLevel);
            foreach (Match match in matches.Cast<Match>()) result.Add(match.Value);
            
            return result;
        }

        private List<KeyValuePair<string, int[]>> GetLevelProperty(List<String> property)
        {
            List<KeyValuePair<string, int[]>> result = new();

            foreach (string key in property) 
            {
                result.Add(ParseLevelProperty(key));
            }

            return result;
        }

        public GridFillWords LoadModel(int index)
        {
            List<string> levelWordsList = ParseLevelData(index);

            List<KeyValuePair<string, int[]>> levelsProperty = GetLevelProperty(levelWordsList);

           //todo
           int maxCharCount = ValidationPropertyData(levelsProperty);
            
            model = CreateModel(maxCharCount);
            
            foreach (KeyValuePair<string, int[]> keyPair in levelsProperty)
            {
                //напиши реализацию не меняя сигнатуру функции

                //index = 8;

                string curWord = keyPair.Key;
                int[] curProperty = keyPair.Value;
                FillModel(curWord, curProperty);
            }

            return model;
        }

        private GridFillWords CreateModel(int count)
        {
            //todo пока не придумал математику для определения размеров модели
            Vector2Int size = new();
            if (count <= 4)
            {
                size.x = size.y = (count % 3) + 1;
                 
            }
            else
            {
                size.x = (count / 3);
                size.y = 3;
            }
            
            return model = new GridFillWords(size);
        }

        private int ValidationPropertyData(List<KeyValuePair<string, int[]>> tlist)
        {
            var newList = from p in tlist select p.Value;

            var aggregateArray = newList.Aggregate((first, next) => first.Concat(next).ToArray());
            var sortedArray = aggregateArray.OrderBy(x => x).ToArray();

            //проверяем что индексы образуют расширенный натуральный ряд с 0
            if (sortedArray[0] != 0 || sortedArray[sortedArray.Length - 1] != sortedArray.Length - 1)
            {
                Debug.Log("Ошибка: невозможно сгенерировать модель");
                return -1;
            }

            //to do построить можно только кратные 2 и 3 массивы, не очень понятно почему так задумано
            if (sortedArray.Length > 4 && (sortedArray.Length % 2 != 0 & sortedArray.Length % 3 != 0))
            {
                Debug.Log("Ошибка: невозможно сгенерировать модель");
                //return -1;
            } 

           return sortedArray.Length;
        }

        private void FillModel(string word, int[] prop)
        {

            for (int i = 0; i < word.Length - 1; i++)
            {
                CharGridModel tChar = new(word[i]);
                int x = modelMask[prop[i]].x;
                int y = modelMask[prop[i]].y;
                model.Set(x, y, tChar);

            }
        }
    }
}