EpgTimerWeb2
============

EpgDataCap_Bon(EpgTimer)のWebUI
* EpgTimerとの接続はTCP
* WebUIはWebSocketで**リアルタイム**を実現
* 録画一覧などの表はdataTablesを使用

## 使い方
1. EpgTimerでTCP接続を有効にします。
2. EpgTimerWeb2.exeを起動します
3. EpgTimerWeb2のコンソールに表示されるURLに接続
   - 多分 http://localhost:8080
4. 必要な設定項目と画面に表示されているPINを入力し
 「Update config」をクリック
5. 暫く待つ…
6. (ﾟдﾟ)ｳﾏｰ

----------
* 以下実装予定
  - WebRTCでクラウドもどき
  - ロケフリ

作者サイト [こちら](http://epgtimerweb.net)

