アルファ入りの PVRTC を奇麗にするコツ
=====================================

iOS や一部の Android 端末で用いられているテクスチャ圧縮形式 PVRTC は、アルファを入れるとキワが汚くなることで悪名高いです。

ここでは、このキワが汚くなる理由のひとつを解説します。もし、あなたのプロジェクトがこれと同じ理由で画質の悪化を受けていた場合、ここで紹介する方法により改善できる可能性があります。

画質の悪化の例
--------------

例として、下のような画像があったとします。チェッカー模様の部分がアルファで抜かれた領域です。

![Original](https://github.com/keijiro/unity-pvr-cleaner/raw/gh-pages/Original.png)

これを PVRTC (Quality=Best) で圧縮すると、下のようにノイズがのりました（右の画像はアルファチャンネルの状態を表します）。

![Dirty](https://github.com/keijiro/unity-pvr-cleaner/raw/gh-pages/Dirty.png)![Dirty (alpha)](https://github.com/keijiro/unity-pvr-cleaner/raw/gh-pages/Mask Dirty.png)

特に汚い左下部分を拡大してみます。

![Dirty (enlarged)](https://github.com/keijiro/unity-pvr-cleaner/raw/gh-pages/Dirty Zoom.png)![Dirty (enlarged, alpha)](https://github.com/keijiro/unity-pvr-cleaner/raw/gh-pages/Mask Dirty Zoom.png)

本来透明であるべき領域に、薄くゴミのように不透明なドットが生じてしまっているのが分かります。画像下端には、薄くライン状のノイズ（？）も生じています。これらのゴミには妙な色（黄色）が付いているのも気になるところです。

ノイズの原因
------------

このようなノイズが生じる理由は複数ありますが、最も影響が大きいのは「アルファが 0% の領域の色成分」です。

上の元画像のアルファチャンネルを 100% 値で塗り潰すと、下のような画像が得られました。

![No Alpha](https://github.com/keijiro/unity-pvr-cleaner/raw/gh-pages/No Alpha.png)

透明の部分（アルファが 0% の領域）におかしな色が混じっています。どうしてこのような色が入ってしまうのか理由は分かりませんが、特に Photoshop から PNG を出力した場合に、このような状態になっていることが多いように感じられます。

この「透明の部分の色成分」が PVRTC 圧縮に悪影響を及ぼしていることは想像に難くありません。恐らくはこの「おかしな色」の影響を受ける形で、透明部分にゴミが生じてしまっているのでしょう。また、画像下端に生じたライン状のノイズは、ラップアラウンドした画像上端の領域と相互に影響を及ぼすことで発生しているのだと推測できます。

透明部分の塗り潰しによる改善
----------------------------

この透明部分を無難な色で塗り潰すことによって、何らかの改善が得られそうです。そこで、次のようなエディタスクリプトを作成しました。

[Assets/Editor/TextureModifier.cs](https://github.com/keijiro/unity-pvr-cleaner/blob/master/Assets/Editor/TextureModifier.cs)

スクリプトの内容はシンプルで、AssetPostprocessor を使ってテクスチャのインポート処理をオーバーライドしています。その中で「もしアルファ値が 0% だったら (50%, 50%, 50%, 0%) の色で置き換える」という処理を行っています。

下の画像は、このスクリプトを適用して得られたものです。

![Clean](https://github.com/keijiro/unity-pvr-cleaner/raw/gh-pages/Clean.png)![Clean (alpha)](https://github.com/keijiro/unity-pvr-cleaner/raw/gh-pages/Mask Clean.png)

左下部分を拡大すると、透明部分のゴミのようなノイズがかなり改善されていることが分かります。

![Clean (enlarged)](https://github.com/keijiro/unity-pvr-cleaner/raw/gh-pages/Clean Zoom.png)![Clean (enlarged, alpha)](https://github.com/keijiro/unity-pvr-cleaner/raw/gh-pages/Mask Clean Zoom.png)

今後の課題
----------

ここでは「最も無難な色」として (50%, 50%, 50%, 0%) を使用しましたが、これが不透明部分に悪影響を与えている可能性もあります。現に上の例では、透明部分のノイズは除去されたものの、不透明部分（キワの内側）については僅かに画質が劣化しているようにも感じられます。また、不透明部分が透明に近づくという、逆効果（？）のノイズも新たに発生しているようです。

この問題は、PVRTC の圧縮アルゴリズムを考慮に入れたうえで、不透明部分への影響の少ない色を適切に選択すれば、改善が可能かもしれません。

謝辞（テスト画像について）
--------------------------

上でテストに用いている画像 (Test A, Test B) は[テラシュールウェア](http://terasur.blog.fc2.com)さんよりご提供いただいたものです。 **この画像を検証以外に用いることは避けてください。**

スクリプトの使用について
------------------------

このプロジェクトに含まれるスクリプト (TextureModifier.cs) は商用・非商用を問わず自由に利用して構いません。
