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
        private string[] _wordMap;
        private string[] _levelMap;

        private string patternLevel = @"(\d{1,4}\s(?:\d{1,}[;]){1,})\d{1,}";
        private readonly string pattern = @"(\d{1,4}\s)(\S*)";

        private GridFillWords model;

        public ProviderFillwordLevel()
        {
            
            var data = Resources.LoadAll<TextAsset>("Fillwords");

            _levelMap = data[0].text.Split("\n");
            _wordMap = data[1].text.Split('\n');
            
        }

        private KeyValuePair<string, int[]> ParseLevelProperty(string level)
        {
            Regex regex = new(pattern);
            Match match = regex.Match(level);

            int wordIndex = int.Parse(match.Groups[1].Value);
           
            return new KeyValuePair<string, int[]>(_wordMap[wordIndex].Replace("\r", ""), match.Groups[2].Value.Split(';').Select(x => int.Parse(x)).ToArray());
        }

        private List<string> ParseLevelData(int indexLevel)
        {
            
            List <string> result = new();


            MatchCollection matches = Regex.Matches(_levelMap[indexLevel], patternLevel);

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

            //напиши реализацию не меняя сигнатуру функции
            List<KeyValuePair<string, int[]>> levelProperty = GetLevelProperty(ParseLevelData(index));

            //todo
            int maxCharCount = ValidationPropertyData(levelProperty);
             
            return CreateModel(maxCharCount, levelProperty);
        }

        private GridFillWords CreateModel(int count, List<KeyValuePair<string, int[]>> levelProperty)
        {


            FillWordsModelService service = new(count);

            model = new GridFillWords(service.Size);

            foreach (KeyValuePair<string, int[]> keyPair in levelProperty)
            {

                string curWord = keyPair.Key;
                int[] curProperty = keyPair.Value;
                CharGridModel tChar;
                Vector2Int coordinate;

                for (int i = 0; i < curWord.Length; i++)
                {
                    tChar = new(curWord[i]);
                    coordinate = service.GetСoordinate(curProperty[i]);
                    
                    model.Set(coordinate.x-1, coordinate.y-1, tChar);
                }
                //класс GridFillWords создает только такие массивы кратные 3 и 2 (одновременно)
                //иными словами, уровни кде количество букв 5,7,8 нужно достраивать в ручную
                for (int i = count; i < service.ScalarSize; i++)
                {
                    
                    tChar = new(" ".ToCharArray().First());
                    coordinate = service.GetСoordinate(i);
                    model.Set(coordinate.x - 1, coordinate.y - 1, tChar);
                }
            }

            return model;
        }

        private int ValidationPropertyData(List<KeyValuePair<string, int[]>> levelProperty)
        {
            var newList = from p in levelProperty select p.Value;

            var aggregateArray = newList.Aggregate((first, next) => first.Concat(next).OrderBy(x => x).ToArray());
            var sortedArray = aggregateArray.OrderBy(x => x).ToArray();

            //todo try catch

            //проверяем что индексы образуют неразрывный ряд с 0
            if (sortedArray[0] != 0 || sortedArray[sortedArray.Length - 1] != sortedArray.Length - 1)
            {
                Debug.Log("Ошибка в уровне: невозможно сгенерировать модель, в описании уровня ошибка");
                return -1;
            }
           return sortedArray.Length;
        }
    }
}