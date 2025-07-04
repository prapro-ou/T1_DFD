# Projects ディレクトリ構成ドキュメント

このドキュメントでは、`Assets/Projects/` 以下の構成とその目的・運用ルールについて説明します。  
プロジェクトの可読性・再利用性・拡張性を意識して、テンプレート・ゲームモジュール・共通ユーティリティを分類しています。

---

## ディレクトリ概要
<pre>
Assets/
└── Projects/ 
    ├── _Templates/ # 新規ミニゲーム作成のテンプレート素材
    ├── MiniGames/ # ミニゲームごとの実装モジュール(各ミニゲームはここに作る)
    └── Utils/ # 共通で使われる補助スクリプト
</pre>


---

## 各ディレクトリの説明

### `_Templates/`

- **目的**  
  新しいミニゲームを作る際のひな形（シーン、Prefab、UIなど）を提供します。
- **主な内容**  
  - `Scenes/Template.unity`: 雛形となる初期状態の Unity シーン
  - `Prefabs/`, `Scripts/`, `Textures/`, `UI/`: 最低限必要な構成を保持した空ディレクトリやテンプレート

> **使用方法：**  
> このテンプレートを複製して `MiniGames/` 以下に名前をつけて配置することで、新しいゲームの土台を素早く構築できます。

---

### `MiniGames/`

- **目的**  
  各ミニゲームの機能実装やリソースを一括管理します。
- **構成ルール**  
  各ゲームごとにディレクトリを作成し、以下の構成に従います。

<pre>
MiniGames/
└── Sample/ # ゲーム「Sample」の実装
    ├── Prefabs/ # ゲーム専用のプレハブ
    ├── Scenes/ # ゲーム専用のシーン
    ├── Scripts/ # ゲームロジック
    ├── Textures/ # 使用する画像素材
    └── UI/ # UI関連のアセット
</pre>


> **命名ルール：**  
> ゲーム名は必ずパスカルケースで統一し、他と重複しない名前にしてください。

---

### `Utils/`

- **目的**  
  全プロジェクト共通で使えるユーティリティスクリプトを格納します。
- **例**  
  - `Singleton.cs`: 各マネージャークラスなどに共通利用されるシングルトンの汎用実装

> **注意：**  
> ここにあるスクリプトは特定のゲームに依存しないように設計してください。

---

## 運用ルールまとめ

| 項目                 | 内容                                                                 |
|----------------------|----------------------------------------------------------------------|
| テンプレート作成時   | `_Templates/` を複製して `MiniGames/` に配置                         |
| 新しいゲーム追加     | `MiniGames/` に新しいディレクトリを作成し、テンプレートを基に構成     |
| ユーティリティ追加   | `Utils/` に配置し、共通処理として再利用できるように設計              |
| シーンの命名規則     | `{ゲーム名}` などに応じて命名             |
| スクリプト命名規則   | パスカルケース + ファイル内容を反映（例：`GameManager.cs`）            |

---

## 今後の拡張予定（例）

- `Document/` にマニュアルやチュートリアルを整理

---

## 補足

この構成は、**複数の小規模ゲームを統一された構造で管理する**ことを目的としています。  
大規模化・コラボレーションが増えた場合も見通しを保つため、各要素の責務を明確に分離しています。
