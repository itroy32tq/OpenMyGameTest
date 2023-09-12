using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using App.Scripts.Scenes.SceneFillwords.Features.FillwordModels;
using Unity.VisualScripting;
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
            try
            {
                TextAsset[] data = Resources.LoadAll<TextAsset>("Fillwords");
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

            List<string> result = new();

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

        #region Core
        /// <summary>
        /// Метод запускает парсинг и заполнение моделей
        /// </summary>
        /// <param name="index">входной номер уровня</param>
        /// <returns></returns>
        public GridFillWords LoadModel(int index)
        {
            //напиши реализацию не меняя сигнатуру функции
            List<KeyValuePair<string, int[]>> levelProperty = GetLevelProperty(ParseLevelData(index));

            if (ValidationPropertyData(index, levelProperty))
            {
                model = new GridFillWords(fillService.Size);
                //fillService.Initialize(model);
            }
            else
            {
                Debug.Log("неправильно проиндексирован уровень " + index.ToString());
                return LoadModel(index + 1);
            }

            return FillModel(levelProperty);
        }
        /// <summary>
        /// метод заполняет модель символами слова
        /// </summary>
        /// <param name="levelProperty">список пар, где ключ слово, а значение массив индексов</param>
        /// <returns></returns>
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
                    coordinate = fillService.GetCoordinate(curProperty[i]);

                    model.Set(coordinate.x - 1, coordinate.y - 1, tChar);
                }
            }
            return model;
        }
        /// <summary>
        /// Метод валидации для даннх уровня
        /// </summary>
        /// <param name="index">индекс уровня для отображения</param>
        /// <param name="levelProperty">ключ пара со словом и массивом индексов для расшифровки</param>
        /// <returns></returns>
        /// <exception cref="Exception">если коллекции придут пустые сработает исключения</exception>
        private bool ValidationPropertyData(int index, List<KeyValuePair<string, int[]>> levelProperty)
        {
            try
            {
                int[] aggregateIndexArray = (from p in levelProperty select p.Value).Aggregate((first, next) => first.Concat(next).ToArray());
                aggregateIndexArray = aggregateIndexArray.Distinct().OrderBy(x => x).ToArray();

                string aggregateWordArray = (from p in levelProperty select p.Key).Aggregate((first, next) => next + first);

                if (IsCharAndCellsIdentical(aggregateWordArray.Length, aggregateIndexArray.Length)) return false;

                fillService = new(aggregateWordArray.Length);

                /// <summary>
                /// техническое задание по сравнению с первоночальным менялось, и условие о том, что количество символов должно заполнять 
                /// квадратную матрицу полностью без пустот, было добавлено позже. До того как я увидел изменения мною уже был реализовал свой вариант,
                /// когда букв может быть меньше чем ячеек. В это случае пустые места я заполнял пробельными символами. Когда стало понятно, что условие 
                /// изменилось, я внес правки для соответствия: закомментировал 87 и 149 строку. На мой взгляд мой вариант даже лучше выглядит :),
                /// так как позволяет отображать больше уровней => все где количество не квадрат числа большего 1
                /// </summary>

                // if (aggregateWordArray.Length < 2 || aggregateIndexArray.Last() >= fillService.ScalarSize) return false;

                if (IsIndexLevelCorrect(aggregateIndexArray.First(), aggregateIndexArray.Last(), fillService.ScalarSize - 1)) return false;
                return true;
            }
            catch
            {
                throw new Exception("некорректные данные для парсинга уровней в ProviderFillwordLevel " + index);
            }
        }
        #endregion

        /// <summary>
        /// Метод сравнивает количество ячеек и количество букв для них
        /// </summary>
        /// <param name="cellSize">количество ячеек</param>
        /// <param name="charSize">количество клеток</param>
        /// <returns></returns>
        private bool IsCharAndCellsIdentical(int cellSize, int charSize)
        { 
            return cellSize != charSize;
        }
        /// <summary>
        /// Метод для валидации индексов, после сортировки и удаления в массиве должен быть ряд от 0 до размера матрицы - 1
        /// </summary>
        /// <param name="first">первый элемент</param>
        /// <param name="last">последний элемент</param>
        /// <param name="size">размер матрицы</param>
        /// <returns></returns>
        private bool IsIndexLevelCorrect(int first, int last, int size)
        {
            return first != 0 || last != size;
        }
    }
}