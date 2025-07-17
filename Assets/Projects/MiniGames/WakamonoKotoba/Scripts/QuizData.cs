namespace Projects.MiniGames.WakamonoKotoba
{
    [System.Serializable]
    public class QuizSourceData
    {
        public QuestionSourceData[] questions;
    }

    [System.Serializable]
    public class QuestionSourceData
    {
        public string word;
        public string correct_answer;

        public string[] incorrect_answers;
    }

    // こちらが実際にクイズの1問としてゲーム内で使うクラス
    public class QuizQuestion
    {
        public string question;
        public string[] choices; // シャッフル後の選択肢
        public int correctIndex; // シャッフル後の正解のインデックス
    }
}