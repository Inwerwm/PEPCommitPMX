# CommitPMX
PMX Editor用 バージョン管理プラグイン

## 機能
### 編集履歴を保存
メッセージと保存日時の情報を持った編集履歴を保存する。  
履歴モデルのファイル名は`yyyy-MM-dd-HH-mm-ss-ff_{Message}.pmx`。

### 編集履歴アーカイブの自動圧縮
履歴モデルは`archive.7z`に圧縮されて保存される。

アーカイブの再圧縮による保存容量の削減ができる。

### 現在編集状態の復元
編集履歴を選択して、現在の編集状態に上書きすることで状態の復元を行う。

## プラグイン構成
- **CommitPMX.dll**  
  プラグイン本体
- **7z.cdll**  
  7-Zipライブラリ  
  `7z.dll`をリネームしたもの
- **SevenZipSharp.dll**  
  https://github.com/squid-box/SevenZipSharp  
  7-ZipライブラリのC#ラッパー
- **Newtonsoft.Json.dll**  
  Jsonシリアライズ/デシリアライズ ライブラリ
- **PEPExtensions.dll**  
  https://github.com/Inwerwm/PEPExtensions  
  Pmx Editor プラグイン用ライブラリ

## 履歴フォルダ構成
- **archive.7z**  
  履歴モデルファイルの圧縮アーカイブ。  
  圧縮形式にzipを使用した場合、拡張子が`.zip`になる。
- **CommitLog.txt**  
  履歴データクラスのシリアライズされたJsonデータ。  
  行ごとにオブジェクトを表しているので、フォーマットされて改行が挟まってしまうと読み込めなくなる。
- **Exceptions.log**  
  発生した例外のログ。
- **archive**  
  圧縮アーカイブへのファイル追加に失敗した場合に履歴モデルファイルが保存されるフォルダ。

## アーカイブ追加失敗時
`archive.7z`へのファイル追加に失敗した場合、CommitLogフォルダ内に`archive`フォルダが生成され、その中に無圧縮で履歴モデルファイルが保存される。  
無圧縮履歴は再圧縮時に圧縮アーカイブに追加することができる。