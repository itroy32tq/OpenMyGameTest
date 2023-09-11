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
        private readonly string[] _wordMap;
        private readonly string[] _levelMap;

        private readonly string patternLevel = @"(\d{1,4}\s(?:\d{1,}[;]){1,})\d{1,}";
        private readonly string pattern = @"(\d{1,4}\s)(\S*)";

        private GridFillWords model;
        private FillWordsModelService fillService;

        public ProviderFillwordLevel()
        {
            TextAsset[] data;

            try 
            {
                data = Resources.LoadAll<TextAsset>("Fillwords");
                _levelMap = data[0].text.Split("\n");
                _wordMap = data[1].text.Split('\n');
            }
            catch 
            {
                throw new Exception("отсутствуют файлы для парсинга уровней в ProviderFillwordLevel");
            }
            
        }
        #region Parsing
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

        private List<KeyValuePair<string, int[]>> GetLevelProperty(List<string> property)
        {
            List<KeyValuePair<string, int[]>> result = new();

            foreach (string key in property) 
            {
                result.Add(ParseLevelProperty(key));
            }

            return result;
        }
        #endregion
        public GridFillWords LoadModel(int index)
        {
            //напиши реализацию не меняя сигнатуру функции
            List<KeyValuePair<string, int[]>> levelProperty = GetLevelProperty(ParseLevelData(index));

            if (ValidationPropertyData(index, levelProperty))
            {
                model = new GridFillWords(fillService.Size);
                fillService.Initialize(model);
            }
            else
            {
                Debug.Log("неправильно проиндексирован уровень " + index.ToString());
                return LoadModel(index + 1);
            } 
             
            return FillModel(levelProperty);
        }

        private GridFillWords FillModel(List<KeyValuePair<string, int[]>> levelProperty)
        {
            foreach (KeyValuePair<string, int[]> keyPair in levelProperty)
            {
                string curWord = keyPair.Key;
                int[] curProperty = keyPair.Value;
                CharGridModel tChar;
                Vector2Int coordinate;

                for (int i = 0; i < curWord.Length; i++)
                {
                    tChar = new(curWord[i]);
                    coordinate = fillService.GetСoordinate(curProperty[i]);
                    
                    model.Set(coordinate.x-1, coordinate.y-1, tChar);
                }
            }
            return model;
        }

        private bool ValidationPropertyData(int index, List<KeyValuePair<string, int[]>> levelProperty)
        {
            try
            {
                int[]  aggregateIndexArray = (from p in levelProperty select p.Value).Aggregate((first, next) => first.Concat(next).OrderBy(x => x).ToArray());
                string aggregateWordArray = (from p in levelProperty select p.Key).Aggregate((first, next) => next + first);

                //количество символов в словах несоответствует количеству индексов для их расшифровки
                if (aggregateWordArray.Length != aggregateIndexArray.Length) return false;

                fillService = new(aggregateWordArray.Length);
                
                // однобуквенные слова мы не отображаем, плюс максимальный индекс должен помещаться в матрицу на экране
                if (aggregateWordArray.Length < 2 || aggregateIndexArray.Last() >= fillService.ScalarSize) return false;

                return true;
            }
            catch
            {
                throw new Exception("некорректные данные для парсинга уровней в ProviderFillwordLevel " + index);
            }
        }
    }
}