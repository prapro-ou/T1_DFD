using UnityEngine;
using System.Collections.Generic;

// MonoBehaviourを継承しない、ただのC#クラス
// staticにすることで、インスタンス化せずに使える便利な道具箱のようになる

namespace Projects.MiniGames.WakamonoKotoba
{

    public static class QuizDataConverter
    {
        const string quizJsonPath = "wakamono_kotoba/quiz";
        // メソッドを呼び出すと、変換済みのリストが返ってくるようにする
        public static List<QuizQuestion> LoadAndConvert()
        {
            // 1. JSONファイルを読み込む
            var jsonFile = Resources.Load<TextAsset>(quizJsonPath);
            if (jsonFile == null)
            {
                Debug.LogError("quiz.json not found in Resources folder!");
                return null; // データがなければnullを返す
            }

            // 2. JSONの構造に合わせてデシリアライズ
            var sourceList = JsonUtility.FromJson<QuizSourceData>(jsonFile.text);

            // 3. 変換後のリストを用意
            List<QuizQuestion> finalQuizList = new List<QuizQuestion>();

            // 4. 各データを変換
            foreach (var sourceData in sourceList.questions)
            {
                var newQuestion = new QuizQuestion();
                newQuestion.question = sourceData.word;

                List<string> choices = new List<string>();
                choices.Add(sourceData.correct_answer);
                choices.AddRange(sourceData.incorrect_answers);

                // 選択肢をシャッフル
                System.Random rng = new System.Random();
                int n = choices.Count;
                while (n > 1)
                {
                    n--;
                    int k = rng.Next(n + 1);
                    string value = choices[k];
                    choices[k] = choices[n];
                    choices[n] = value;
                }

                newQuestion.choices = choices.ToArray();
                newQuestion.correctIndex = choices.IndexOf(sourceData.correct_answer);

                finalQuizList.Add(newQuestion);
            }

            // 5. 完成したリストを返す
            return finalQuizList;
        }
    }
}