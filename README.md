アルファ付きの PVRTC を奇麗にするコツ
=====================================

iOS や一部の Android 端末で用いられているテクスチャ圧縮形式 PVRTC は、アルファを入れるとキワが汚くなることで悪名高いです。

例えば、こんな感じでノイズが入ります。

![Dirty](https://github.com/keijiro/unity-pvr-cleaner/raw/gh-pages/Dirty.png)

特に汚い左下部分を拡大してみます。

![Dirty (enlarged)](https://github.com/keijiro/unity-pvr-cleaner/raw/gh-pages/Dirty Zoom.png)

どう考えてもおかしなレベルのノイズがあります。

![No Alpha](https://github.com/keijiro/unity-pvr-cleaner/raw/gh-pages/No Alpha.png)

![Clean](https://github.com/keijiro/unity-pvr-cleaner/raw/gh-pages/Clean.png) ![Clean (enlarged)](https://github.com/keijiro/unity-pvr-cleaner/raw/gh-pages/Clean Zoom.png)
