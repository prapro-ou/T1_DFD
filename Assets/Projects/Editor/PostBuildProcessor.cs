using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks; // ビルド後の処理に必要
using System.IO;

public class PostBuildProcessor
{
    // [PostProcessBuild]属性を付けると、ビルド完了後にこのメソッドが自動で呼ばれます
    [PostProcessBuild(1)]
    public static void OnPostprocessBuild(BuildTarget target, string pathToBuiltProject)
    {
        // 1. コピー元のファイルパスを指定
        // Application.dataPath は "Assets" フォルダを指すので、".." で一つ上の階層（プロジェクトルート）に移動します
        string sourcePath = Path.Combine(Application.dataPath, "..", "CREDITS.txt");

        // コピー元のCREDIT.txtが存在するか確認
        if (!File.Exists(sourcePath))
        {
            Debug.LogWarning("プロジェクトルートに" +sourcePath + "が見つかりませんでした。コピーをスキップします。");
            return;
        }

        // 2. コピー先のパスを決定
        // pathToBuiltProject はビルドされた実行ファイル（.exeなど）のフルパスです
        string buildDirectory = Path.GetDirectoryName(pathToBuiltProject);
        string destinationPath = Path.Combine(buildDirectory, "CREDITS.txt");
        
        // 3. ファイルをコピー
        File.Copy(sourcePath, destinationPath, true); // true は上書きを許可する設定

        Debug.Log($"CREDIT.txt をビルドフォルダにコピーしました: {destinationPath}");
    }
}