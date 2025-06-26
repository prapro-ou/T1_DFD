# Utilsの説明

## Singleton.cs
### 目的

- グローバルに1つだけ存在するインスタンスを提供する。
- 同じ型のコンポーネントが複数生成された場合、冗長なインスタンスを自動で破棄する。
- 必要に応じて `DontDestroyOnLoad` によりシーンをまたいで永続させることも可能。

---

### 使用方法

1. 任意の `MonoBehaviour` クラスに継承させます。

```csharp
public class GameManager : Singleton<GameManager>
{
    protected override void Awake()
    {
        base.Awake();
        // 初期化処理など
    }
}
```
2. どこからでも以下のようにアクセス可能です。

```csharp
GameManager.Instance.DoSomething();
```

