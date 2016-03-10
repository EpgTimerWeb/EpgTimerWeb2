/*
 *  EpgTimerWeb2
 *  Copyright (C) 2016 EpgTimerWeb <webmaster@epgtimerweb.net>
 *
 *  This program is free software: you can redistribute it and/or modify
 *  it under the terms of the GNU General Public License as published by
 *  the Free Software Foundation, either version 3 of the License, or
 *  (at your option) any later version.
 *
 *  This program is distributed in the hope that it will be useful,
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *  GNU General Public License for more details.
 *
 *  You should have received a copy of the GNU General Public License
 *  along with this program.  If not, see <http://www.gnu.org/licenses/>.
 */
using System;
using System.Collections.Generic;
using System.Windows.Forms;
namespace EpgTimer
{

    public class CommonManager
    {
        public CtrlCmdUtil CtrlCmd { get; set; }

        public Dictionary<ushort, ContentKindInfo> ContentKindDictionary { get; set; }
        public Dictionary<ushort, ContentKindInfo> ContentKindDictionary2 { get; set; }
        public Dictionary<ushort, ComponentKindInfo> ComponentKindDictionary { get; set; }
        public bool NWMode { get; set; }
        public NWConnect NW { get; set; }
        public DBManager DB { get; set; }
        private static CommonManager _instance;
        public static CommonManager Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new CommonManager();
                return _instance;
            }
            set { _instance = value; }
        }

        public CommonManager()
        {
            if (CtrlCmd == null)
                CtrlCmd = new CtrlCmdUtil();
            if (DB == null)
                DB = new DBManager(CtrlCmd);
            if (NW == null)
                NW = new NWConnect(CtrlCmd);
            if (ContentKindDictionary == null)
            {
                ContentKindDictionary = new Dictionary<ushort, ContentKindInfo>();
                ContentKindDictionary.Add(0x00FF, new ContentKindInfo("ニュース／報道", "", 0x00, 0xFF));
                ContentKindDictionary.Add(0x0000, new ContentKindInfo("ニュース／報道", "定時・総合", 0x00, 0x00));
                ContentKindDictionary.Add(0x0001, new ContentKindInfo("ニュース／報道", "天気", 0x00, 0x01));
                ContentKindDictionary.Add(0x0002, new ContentKindInfo("ニュース／報道", "特集・ドキュメント", 0x00, 0x02));
                ContentKindDictionary.Add(0x0003, new ContentKindInfo("ニュース／報道", "政治・国会", 0x00, 0x03));
                ContentKindDictionary.Add(0x0004, new ContentKindInfo("ニュース／報道", "経済・市況", 0x00, 0x04));
                ContentKindDictionary.Add(0x0005, new ContentKindInfo("ニュース／報道", "海外・国際", 0x00, 0x05));
                ContentKindDictionary.Add(0x0006, new ContentKindInfo("ニュース／報道", "解説", 0x00, 0x06));
                ContentKindDictionary.Add(0x0007, new ContentKindInfo("ニュース／報道", "討論・会談", 0x00, 0x07));
                ContentKindDictionary.Add(0x0008, new ContentKindInfo("ニュース／報道", "報道特番", 0x00, 0x08));
                ContentKindDictionary.Add(0x0009, new ContentKindInfo("ニュース／報道", "ローカル・地域", 0x00, 0x09));
                ContentKindDictionary.Add(0x000A, new ContentKindInfo("ニュース／報道", "交通", 0x00, 0x0A));
                ContentKindDictionary.Add(0x000F, new ContentKindInfo("ニュース／報道", "その他", 0x00, 0x0F));

                ContentKindDictionary.Add(0x01FF, new ContentKindInfo("スポーツ", "", 0x01, 0xFF));
                ContentKindDictionary.Add(0x0100, new ContentKindInfo("スポーツ", "スポーツニュース", 0x01, 0x00));
                ContentKindDictionary.Add(0x0101, new ContentKindInfo("スポーツ", "野球", 0x01, 0x01));
                ContentKindDictionary.Add(0x0102, new ContentKindInfo("スポーツ", "サッカー", 0x01, 0x02));
                ContentKindDictionary.Add(0x0103, new ContentKindInfo("スポーツ", "ゴルフ", 0x01, 0x03));
                ContentKindDictionary.Add(0x0104, new ContentKindInfo("スポーツ", "その他の球技", 0x01, 0x04));
                ContentKindDictionary.Add(0x0105, new ContentKindInfo("スポーツ", "相撲・格闘技", 0x01, 0x05));
                ContentKindDictionary.Add(0x0106, new ContentKindInfo("スポーツ", "オリンピック・国際大会", 0x01, 0x06));
                ContentKindDictionary.Add(0x0107, new ContentKindInfo("スポーツ", "マラソン・陸上・水泳", 0x01, 0x07));
                ContentKindDictionary.Add(0x0108, new ContentKindInfo("スポーツ", "モータースポーツ", 0x01, 0x08));
                ContentKindDictionary.Add(0x0109, new ContentKindInfo("スポーツ", "マリン・ウィンタースポーツ", 0x01, 0x09));
                ContentKindDictionary.Add(0x010A, new ContentKindInfo("スポーツ", "競馬・公営競技", 0x01, 0x0A));
                ContentKindDictionary.Add(0x010F, new ContentKindInfo("スポーツ", "その他", 0x01, 0x0F));

                ContentKindDictionary.Add(0x02FF, new ContentKindInfo("情報／ワイドショー", "", 0x02, 0xFF));
                ContentKindDictionary.Add(0x0200, new ContentKindInfo("情報／ワイドショー", "芸能・ワイドショー", 0x02, 0x00));
                ContentKindDictionary.Add(0x0201, new ContentKindInfo("情報／ワイドショー", "ファッション", 0x02, 0x01));
                ContentKindDictionary.Add(0x0202, new ContentKindInfo("情報／ワイドショー", "暮らし・住まい", 0x02, 0x02));
                ContentKindDictionary.Add(0x0203, new ContentKindInfo("情報／ワイドショー", "健康・医療", 0x02, 0x03));
                ContentKindDictionary.Add(0x0204, new ContentKindInfo("情報／ワイドショー", "ショッピング・通販", 0x02, 0x04));
                ContentKindDictionary.Add(0x0205, new ContentKindInfo("情報／ワイドショー", "グルメ・料理", 0x02, 0x05));
                ContentKindDictionary.Add(0x0206, new ContentKindInfo("情報／ワイドショー", "イベント", 0x02, 0x06));
                ContentKindDictionary.Add(0x0207, new ContentKindInfo("情報／ワイドショー", "番組紹介・お知らせ", 0x02, 0x07));
                ContentKindDictionary.Add(0x020F, new ContentKindInfo("情報／ワイドショー", "その他", 0x02, 0x0F));

                ContentKindDictionary.Add(0x03FF, new ContentKindInfo("ドラマ", "", 0x03, 0xFF));
                ContentKindDictionary.Add(0x0300, new ContentKindInfo("ドラマ", "国内ドラマ", 0x03, 0x00));
                ContentKindDictionary.Add(0x0301, new ContentKindInfo("ドラマ", "海外ドラマ", 0x03, 0x01));
                ContentKindDictionary.Add(0x0302, new ContentKindInfo("ドラマ", "時代劇", 0x03, 0x02));
                ContentKindDictionary.Add(0x030F, new ContentKindInfo("ドラマ", "その他", 0x03, 0x0F));

                ContentKindDictionary.Add(0x04FF, new ContentKindInfo("音楽", "", 0x04, 0xFF));
                ContentKindDictionary.Add(0x0400, new ContentKindInfo("音楽", "国内ロック・ポップス", 0x04, 0x00));
                ContentKindDictionary.Add(0x0401, new ContentKindInfo("音楽", "海外ロック・ポップス", 0x04, 0x01));
                ContentKindDictionary.Add(0x0402, new ContentKindInfo("音楽", "クラシック・オペラ", 0x04, 0x02));
                ContentKindDictionary.Add(0x0403, new ContentKindInfo("音楽", "ジャズ・フュージョン", 0x04, 0x03));
                ContentKindDictionary.Add(0x0404, new ContentKindInfo("音楽", "歌謡曲・演歌", 0x04, 0x04));
                ContentKindDictionary.Add(0x0405, new ContentKindInfo("音楽", "ライブ・コンサート", 0x04, 0x05));
                ContentKindDictionary.Add(0x0406, new ContentKindInfo("音楽", "ランキング・リクエスト", 0x04, 0x06));
                ContentKindDictionary.Add(0x0407, new ContentKindInfo("音楽", "カラオケ・のど自慢", 0x04, 0x07));
                ContentKindDictionary.Add(0x0408, new ContentKindInfo("音楽", "民謡・邦楽", 0x04, 0x08));
                ContentKindDictionary.Add(0x0409, new ContentKindInfo("音楽", "童謡・キッズ", 0x04, 0x09));
                ContentKindDictionary.Add(0x040A, new ContentKindInfo("音楽", "民族音楽・ワールドミュージック", 0x04, 0x0A));
                ContentKindDictionary.Add(0x040F, new ContentKindInfo("音楽", "その他", 0x04, 0x0F));

                ContentKindDictionary.Add(0x05FF, new ContentKindInfo("バラエティ", "", 0x05, 0xFF));
                ContentKindDictionary.Add(0x0500, new ContentKindInfo("バラエティ", "クイズ", 0x05, 0x00));
                ContentKindDictionary.Add(0x0501, new ContentKindInfo("バラエティ", "ゲーム", 0x05, 0x01));
                ContentKindDictionary.Add(0x0502, new ContentKindInfo("バラエティ", "トークバラエティ", 0x05, 0x02));
                ContentKindDictionary.Add(0x0503, new ContentKindInfo("バラエティ", "お笑い・コメディ", 0x05, 0x03));
                ContentKindDictionary.Add(0x0504, new ContentKindInfo("バラエティ", "音楽バラエティ", 0x05, 0x04));
                ContentKindDictionary.Add(0x0505, new ContentKindInfo("バラエティ", "旅バラエティ", 0x05, 0x05));
                ContentKindDictionary.Add(0x0506, new ContentKindInfo("バラエティ", "料理バラエティ", 0x05, 0x06));
                ContentKindDictionary.Add(0x050F, new ContentKindInfo("バラエティ", "その他", 0x05, 0x0F));

                ContentKindDictionary.Add(0x06FF, new ContentKindInfo("映画", "", 0x06, 0xFF));
                ContentKindDictionary.Add(0x0600, new ContentKindInfo("映画", "洋画", 0x06, 0x00));
                ContentKindDictionary.Add(0x0601, new ContentKindInfo("映画", "邦画", 0x06, 0x01));
                ContentKindDictionary.Add(0x0602, new ContentKindInfo("映画", "アニメ", 0x06, 0x02));
                ContentKindDictionary.Add(0x060F, new ContentKindInfo("映画", "その他", 0x06, 0x0F));

                ContentKindDictionary.Add(0x07FF, new ContentKindInfo("アニメ／特撮", "", 0x07, 0xFF));
                ContentKindDictionary.Add(0x0700, new ContentKindInfo("アニメ／特撮", "国内アニメ", 0x07, 0x00));
                ContentKindDictionary.Add(0x0701, new ContentKindInfo("アニメ／特撮", "海外アニメ", 0x07, 0x01));
                ContentKindDictionary.Add(0x0702, new ContentKindInfo("アニメ／特撮", "特撮", 0x07, 0x02));
                ContentKindDictionary.Add(0x070F, new ContentKindInfo("アニメ／特撮", "その他", 0x07, 0x0F));

                ContentKindDictionary.Add(0x08FF, new ContentKindInfo("ドキュメンタリー／教養", "", 0x08, 0xFF));
                ContentKindDictionary.Add(0x0800, new ContentKindInfo("ドキュメンタリー／教養", "社会・時事", 0x08, 0x00));
                ContentKindDictionary.Add(0x0801, new ContentKindInfo("ドキュメンタリー／教養", "歴史・紀行", 0x08, 0x01));
                ContentKindDictionary.Add(0x0802, new ContentKindInfo("ドキュメンタリー／教養", "自然・動物・環境", 0x08, 0x02));
                ContentKindDictionary.Add(0x0803, new ContentKindInfo("ドキュメンタリー／教養", "宇宙・科学・医学", 0x08, 0x03));
                ContentKindDictionary.Add(0x0804, new ContentKindInfo("ドキュメンタリー／教養", "カルチャー・伝統文化", 0x08, 0x04));
                ContentKindDictionary.Add(0x0805, new ContentKindInfo("ドキュメンタリー／教養", "文学・文芸", 0x08, 0x05));
                ContentKindDictionary.Add(0x0806, new ContentKindInfo("ドキュメンタリー／教養", "スポーツ", 0x08, 0x06));
                ContentKindDictionary.Add(0x0807, new ContentKindInfo("ドキュメンタリー／教養", "ドキュメンタリー全般", 0x08, 0x07));
                ContentKindDictionary.Add(0x0808, new ContentKindInfo("ドキュメンタリー／教養", "インタビュー・討論", 0x08, 0x08));
                ContentKindDictionary.Add(0x080F, new ContentKindInfo("ドキュメンタリー／教養", "その他", 0x08, 0x0F));

                ContentKindDictionary.Add(0x09FF, new ContentKindInfo("劇場／公演", "", 0x09, 0xFF));
                ContentKindDictionary.Add(0x0900, new ContentKindInfo("劇場／公演", "現代劇・新劇", 0x09, 0x00));
                ContentKindDictionary.Add(0x0901, new ContentKindInfo("劇場／公演", "ミュージカル", 0x09, 0x01));
                ContentKindDictionary.Add(0x0902, new ContentKindInfo("劇場／公演", "ダンス・バレエ", 0x09, 0x02));
                ContentKindDictionary.Add(0x0903, new ContentKindInfo("劇場／公演", "落語・演芸", 0x09, 0x03));
                ContentKindDictionary.Add(0x0904, new ContentKindInfo("劇場／公演", "歌舞伎・古典", 0x09, 0x04));
                ContentKindDictionary.Add(0x090F, new ContentKindInfo("劇場／公演", "その他", 0x09, 0x0F));

                ContentKindDictionary.Add(0x0AFF, new ContentKindInfo("趣味／教育", "", 0x0A, 0xFF));
                ContentKindDictionary.Add(0x0A00, new ContentKindInfo("趣味／教育", "旅・釣り・アウトドア", 0x0A, 0x00));
                ContentKindDictionary.Add(0x0A01, new ContentKindInfo("趣味／教育", "園芸・ペット・手芸", 0x0A, 0x01));
                ContentKindDictionary.Add(0x0A02, new ContentKindInfo("趣味／教育", "音楽・美術・工芸", 0x0A, 0x02));
                ContentKindDictionary.Add(0x0A03, new ContentKindInfo("趣味／教育", "囲碁・将棋", 0x0A, 0x03));
                ContentKindDictionary.Add(0x0A04, new ContentKindInfo("趣味／教育", "麻雀・パチンコ", 0x0A, 0x04));
                ContentKindDictionary.Add(0x0A05, new ContentKindInfo("趣味／教育", "車・オートバイ", 0x0A, 0x05));
                ContentKindDictionary.Add(0x0A06, new ContentKindInfo("趣味／教育", "コンピュータ・ＴＶゲーム", 0x0A, 0x06));
                ContentKindDictionary.Add(0x0A07, new ContentKindInfo("趣味／教育", "会話・語学", 0x0A, 0x07));
                ContentKindDictionary.Add(0x0A08, new ContentKindInfo("趣味／教育", "幼児・小学生", 0x0A, 0x08));
                ContentKindDictionary.Add(0x0A09, new ContentKindInfo("趣味／教育", "中学生・高校生", 0x0A, 0x09));
                ContentKindDictionary.Add(0x0A0A, new ContentKindInfo("趣味／教育", "大学生・受験", 0x0A, 0x0A));
                ContentKindDictionary.Add(0x0A0B, new ContentKindInfo("趣味／教育", "生涯教育・資格", 0x0A, 0x0B));
                ContentKindDictionary.Add(0x0A0C, new ContentKindInfo("趣味／教育", "教育問題", 0x0A, 0x0C));
                ContentKindDictionary.Add(0x0A0F, new ContentKindInfo("趣味／教育", "その他", 0x0A, 0x0F));

                ContentKindDictionary.Add(0x0BFF, new ContentKindInfo("福祉", "", 0x0B, 0xFF));
                ContentKindDictionary.Add(0x0B00, new ContentKindInfo("福祉", "高齢者", 0x0B, 0x00));
                ContentKindDictionary.Add(0x0B01, new ContentKindInfo("福祉", "障害者", 0x0B, 0x01));
                ContentKindDictionary.Add(0x0B02, new ContentKindInfo("福祉", "社会福祉", 0x0B, 0x02));
                ContentKindDictionary.Add(0x0B03, new ContentKindInfo("福祉", "ボランティア", 0x0B, 0x03));
                ContentKindDictionary.Add(0x0B04, new ContentKindInfo("福祉", "手話", 0x0B, 0x04));
                ContentKindDictionary.Add(0x0B05, new ContentKindInfo("福祉", "文字（字幕）", 0x0B, 0x05));
                ContentKindDictionary.Add(0x0B06, new ContentKindInfo("福祉", "音声解説", 0x0B, 0x06));
                ContentKindDictionary.Add(0x0B0F, new ContentKindInfo("福祉", "その他", 0x0B, 0x0F));

                ContentKindDictionary.Add(0x0FFF, new ContentKindInfo("その他", "", 0x0F, 0xFF));
                ContentKindDictionary.Add(0xFFFF, new ContentKindInfo("なし", "", 0xFF, 0xFF));
            }
            if (ContentKindDictionary2 == null)
            {
                ContentKindDictionary2 = new Dictionary<ushort, ContentKindInfo>();
                ContentKindDictionary2.Add(0x00FF, new ContentKindInfo("スポーツ", "", 0x00, 0xFF));
                ContentKindDictionary2.Add(0x0000, new ContentKindInfo("スポーツ", "テニス", 0x00, 0x00));
                ContentKindDictionary2.Add(0x0001, new ContentKindInfo("スポーツ", "バスケットボール", 0x00, 0x01));
                ContentKindDictionary2.Add(0x0002, new ContentKindInfo("スポーツ", "ラグビー", 0x00, 0x02));
                ContentKindDictionary2.Add(0x0003, new ContentKindInfo("スポーツ", "アメリカンフットボール", 0x00, 0x03));
                ContentKindDictionary2.Add(0x0004, new ContentKindInfo("スポーツ", "ボクシング", 0x00, 0x04));
                ContentKindDictionary2.Add(0x0005, new ContentKindInfo("スポーツ", "プロレス", 0x00, 0x05));
                ContentKindDictionary2.Add(0x000F, new ContentKindInfo("スポーツ", "その他", 0x00, 0x0F));

                ContentKindDictionary2.Add(0x01FF, new ContentKindInfo("洋画", "", 0x01, 0xFF));
                ContentKindDictionary2.Add(0x0100, new ContentKindInfo("洋画", "アクション", 0x01, 0x00));
                ContentKindDictionary2.Add(0x0101, new ContentKindInfo("洋画", "SF／ファンタジー", 0x01, 0x01));
                ContentKindDictionary2.Add(0x0102, new ContentKindInfo("洋画", "コメディー", 0x01, 0x02));
                ContentKindDictionary2.Add(0x0103, new ContentKindInfo("洋画", "サスペンス／ミステリー", 0x01, 0x03));
                ContentKindDictionary2.Add(0x0104, new ContentKindInfo("洋画", "恋愛／ロマンス", 0x01, 0x04));
                ContentKindDictionary2.Add(0x0105, new ContentKindInfo("洋画", "ホラー／スリラー", 0x01, 0x05));
                ContentKindDictionary2.Add(0x0106, new ContentKindInfo("洋画", "ウエスタン", 0x01, 0x06));
                ContentKindDictionary2.Add(0x0107, new ContentKindInfo("洋画", "ドラマ／社会派ドラマ", 0x01, 0x07));
                ContentKindDictionary2.Add(0x0108, new ContentKindInfo("洋画", "アニメーション", 0x01, 0x08));
                ContentKindDictionary2.Add(0x0109, new ContentKindInfo("洋画", "ドキュメンタリー", 0x01, 0x09));
                ContentKindDictionary2.Add(0x010A, new ContentKindInfo("洋画", "アドベンチャー／冒険", 0x01, 0x0A));
                ContentKindDictionary2.Add(0x010B, new ContentKindInfo("洋画", "ミュージカル／音楽映画", 0x01, 0x0A));
                ContentKindDictionary2.Add(0x010C, new ContentKindInfo("洋画", "ホームドラマ", 0x01, 0x0A));
                ContentKindDictionary2.Add(0x010F, new ContentKindInfo("洋画", "その他", 0x01, 0x0F));

                ContentKindDictionary2.Add(0x02FF, new ContentKindInfo("邦画", "", 0x02, 0xFF));
                ContentKindDictionary2.Add(0x0200, new ContentKindInfo("邦画", "アクション", 0x02, 0x00));
                ContentKindDictionary2.Add(0x0201, new ContentKindInfo("邦画", "SF／ファンタジー", 0x02, 0x01));
                ContentKindDictionary2.Add(0x0202, new ContentKindInfo("邦画", "コメディー", 0x02, 0x02));
                ContentKindDictionary2.Add(0x0203, new ContentKindInfo("邦画", "サスペンス／ミステリー", 0x02, 0x03));
                ContentKindDictionary2.Add(0x0204, new ContentKindInfo("邦画", "恋愛／ロマンス", 0x02, 0x04));
                ContentKindDictionary2.Add(0x0205, new ContentKindInfo("邦画", "ホラー／スリラー", 0x02, 0x05));
                ContentKindDictionary2.Add(0x0206, new ContentKindInfo("邦画", "ウエスタン", 0x02, 0x06));
                ContentKindDictionary2.Add(0x0207, new ContentKindInfo("邦画", "ドラマ／社会派ドラマ", 0x02, 0x07));
                ContentKindDictionary2.Add(0x0208, new ContentKindInfo("邦画", "アニメーション", 0x02, 0x08));
                ContentKindDictionary2.Add(0x0209, new ContentKindInfo("邦画", "ドキュメンタリー", 0x02, 0x09));
                ContentKindDictionary2.Add(0x020A, new ContentKindInfo("邦画", "アドベンチャー／冒険", 0x02, 0x0A));
                ContentKindDictionary2.Add(0x020B, new ContentKindInfo("邦画", "ミュージカル／音楽映画", 0x02, 0x0A));
                ContentKindDictionary2.Add(0x020C, new ContentKindInfo("邦画", "ホームドラマ", 0x02, 0x0A));
                ContentKindDictionary2.Add(0x020F, new ContentKindInfo("邦画", "その他", 0x02, 0x0F));
            }
            if (ComponentKindDictionary == null)
            {
                ComponentKindDictionary = new Dictionary<ushort, ComponentKindInfo>();
                ComponentKindDictionary.Add(0x0101, new ComponentKindInfo(0x01, 0x01, "480i(525i)、アスペクト比4:3"));
                ComponentKindDictionary.Add(0x0102, new ComponentKindInfo(0x01, 0x02, "480i(525i)、アスペクト比16:9 パンベクトルあり"));
                ComponentKindDictionary.Add(0x0103, new ComponentKindInfo(0x01, 0x03, "480i(525i)、アスペクト比16:9 パンベクトルなし"));
                ComponentKindDictionary.Add(0x0104, new ComponentKindInfo(0x01, 0x04, "480i(525i)、アスペクト比 > 16:9"));
                ComponentKindDictionary.Add(0x0191, new ComponentKindInfo(0x01, 0x91, "2160p、アスペクト比4:3"));
                ComponentKindDictionary.Add(0x0192, new ComponentKindInfo(0x01, 0x92, "2160p、アスペクト比16:9 パンベクトルあり"));
                ComponentKindDictionary.Add(0x0193, new ComponentKindInfo(0x01, 0x93, "2160p、アスペクト比16:9 パンベクトルなし"));
                ComponentKindDictionary.Add(0x0194, new ComponentKindInfo(0x01, 0x94, "2160p、アスペクト比 > 16:9"));
                ComponentKindDictionary.Add(0x01A1, new ComponentKindInfo(0x01, 0xA1, "480p(525p)、アスペクト比4:3"));
                ComponentKindDictionary.Add(0x01A2, new ComponentKindInfo(0x01, 0xA2, "480p(525p)、アスペクト比16:9 パンベクトルあり"));
                ComponentKindDictionary.Add(0x01A3, new ComponentKindInfo(0x01, 0xA3, "480p(525p)、アスペクト比16:9 パンベクトルなし"));
                ComponentKindDictionary.Add(0x01A4, new ComponentKindInfo(0x01, 0xA4, "480p(525p)、アスペクト比 > 16:9"));
                ComponentKindDictionary.Add(0x01B1, new ComponentKindInfo(0x01, 0xB1, "1080i(1125i)、アスペクト比4:3"));
                ComponentKindDictionary.Add(0x01B2, new ComponentKindInfo(0x01, 0xB2, "1080i(1125i)、アスペクト比16:9 パンベクトルあり"));
                ComponentKindDictionary.Add(0x01B3, new ComponentKindInfo(0x01, 0xB3, "1080i(1125i)、アスペクト比16:9 パンベクトルなし"));
                ComponentKindDictionary.Add(0x01B4, new ComponentKindInfo(0x01, 0xB4, "1080i(1125i)、アスペクト比 > 16:9"));
                ComponentKindDictionary.Add(0x01C1, new ComponentKindInfo(0x01, 0xC1, "720p(750p)、アスペクト比4:3"));
                ComponentKindDictionary.Add(0x01C2, new ComponentKindInfo(0x01, 0xC2, "720p(750p)、アスペクト比16:9 パンベクトルあり"));
                ComponentKindDictionary.Add(0x01C3, new ComponentKindInfo(0x01, 0xC3, "720p(750p)、アスペクト比16:9 パンベクトルなし"));
                ComponentKindDictionary.Add(0x01C4, new ComponentKindInfo(0x01, 0xC4, "720p(750p)、アスペクト比 > 16:9"));
                ComponentKindDictionary.Add(0x01D1, new ComponentKindInfo(0x01, 0xD1, "240p アスペクト比4:3"));
                ComponentKindDictionary.Add(0x01D2, new ComponentKindInfo(0x01, 0xD2, "240p アスペクト比16:9 パンベクトルあり"));
                ComponentKindDictionary.Add(0x01D3, new ComponentKindInfo(0x01, 0xD3, "240p アスペクト比16:9 パンベクトルなし"));
                ComponentKindDictionary.Add(0x01D4, new ComponentKindInfo(0x01, 0xD4, "240p アスペクト比 > 16:9"));
                ComponentKindDictionary.Add(0x01E1, new ComponentKindInfo(0x01, 0xE1, "1080p(1125p)、アスペクト比4:3"));
                ComponentKindDictionary.Add(0x01E2, new ComponentKindInfo(0x01, 0xE2, "1080p(1125p)、アスペクト比16:9 パンベクトルあり"));
                ComponentKindDictionary.Add(0x01E3, new ComponentKindInfo(0x01, 0xE3, "1080p(1125p)、アスペクト比16:9 パンベクトルなし"));
                ComponentKindDictionary.Add(0x01E4, new ComponentKindInfo(0x01, 0xE4, "1080p(1125p)、アスペクト比 > 16:9"));
                ComponentKindDictionary.Add(0x0201, new ComponentKindInfo(0x02, 0x01, "1/0モード（シングルモノ）"));
                ComponentKindDictionary.Add(0x0202, new ComponentKindInfo(0x02, 0x02, "1/0＋1/0モード（デュアルモノ）"));
                ComponentKindDictionary.Add(0x0203, new ComponentKindInfo(0x02, 0x03, "2/0モード（ステレオ）"));
                ComponentKindDictionary.Add(0x0204, new ComponentKindInfo(0x02, 0x04, "2/1モード"));
                ComponentKindDictionary.Add(0x0205, new ComponentKindInfo(0x02, 0x05, "3/0モード"));
                ComponentKindDictionary.Add(0x0206, new ComponentKindInfo(0x02, 0x06, "2/2モード"));
                ComponentKindDictionary.Add(0x0207, new ComponentKindInfo(0x02, 0x07, "3/1モード"));
                ComponentKindDictionary.Add(0x0208, new ComponentKindInfo(0x02, 0x08, "3/2モード"));
                ComponentKindDictionary.Add(0x0209, new ComponentKindInfo(0x02, 0x09, "3/2＋LFEモード（3/2.1モード）"));
                ComponentKindDictionary.Add(0x020A, new ComponentKindInfo(0x02, 0x0A, "3/3.1モード"));
                ComponentKindDictionary.Add(0x020B, new ComponentKindInfo(0x02, 0x0B, "2/0/0-2/0/2-0.1モード"));
                ComponentKindDictionary.Add(0x020C, new ComponentKindInfo(0x02, 0x0C, "5/2.1モード"));
                ComponentKindDictionary.Add(0x020D, new ComponentKindInfo(0x02, 0x0D, "3/2/2.1モード"));
                ComponentKindDictionary.Add(0x020E, new ComponentKindInfo(0x02, 0x0E, "2/0/0-3/0/2-0.1モード"));
                ComponentKindDictionary.Add(0x020F, new ComponentKindInfo(0x02, 0x0F, "0/2/0-3/0/2-0.1モード"));
                ComponentKindDictionary.Add(0x0210, new ComponentKindInfo(0x02, 0x10, "2/0/0-3/2/3-0.2モード"));
                ComponentKindDictionary.Add(0x0211, new ComponentKindInfo(0x02, 0x11, "3/3/3-5/2/3-3/0/0.2モード"));
                ComponentKindDictionary.Add(0x0240, new ComponentKindInfo(0x02, 0x40, "視覚障害者用音声解説"));
                ComponentKindDictionary.Add(0x0241, new ComponentKindInfo(0x02, 0x41, "聴覚障害者用音声"));
                ComponentKindDictionary.Add(0x0501, new ComponentKindInfo(0x05, 0x01, "H.264|MPEG-4 AVC、480i(525i)、アスペクト比4:3"));
                ComponentKindDictionary.Add(0x0502, new ComponentKindInfo(0x05, 0x02, "H.264|MPEG-4 AVC、480i(525i)、アスペクト比16:9 パンベクトルあり"));
                ComponentKindDictionary.Add(0x0503, new ComponentKindInfo(0x05, 0x03, "H.264|MPEG-4 AVC、480i(525i)、アスペクト比16:9 パンベクトルなし"));
                ComponentKindDictionary.Add(0x0504, new ComponentKindInfo(0x05, 0x04, "H.264|MPEG-4 AVC、480i(525i)、アスペクト比 > 16:9"));
                ComponentKindDictionary.Add(0x0591, new ComponentKindInfo(0x05, 0x91, "H.264|MPEG-4 AVC、2160p、アスペクト比4:3"));
                ComponentKindDictionary.Add(0x0592, new ComponentKindInfo(0x05, 0x92, "H.264|MPEG-4 AVC、2160p、アスペクト比16:9 パンベクトルあり"));
                ComponentKindDictionary.Add(0x0593, new ComponentKindInfo(0x05, 0x93, "H.264|MPEG-4 AVC、2160p、アスペクト比16:9 パンベクトルなし"));
                ComponentKindDictionary.Add(0x0594, new ComponentKindInfo(0x05, 0x94, "H.264|MPEG-4 AVC、2160p、アスペクト比 > 16:9"));
                ComponentKindDictionary.Add(0x05A1, new ComponentKindInfo(0x05, 0xA1, "H.264|MPEG-4 AVC、480p(525p)、アスペクト比4:3"));
                ComponentKindDictionary.Add(0x05A2, new ComponentKindInfo(0x05, 0xA2, "H.264|MPEG-4 AVC、480p(525p)、アスペクト比16:9 パンベクトルあり"));
                ComponentKindDictionary.Add(0x05A3, new ComponentKindInfo(0x05, 0xA3, "H.264|MPEG-4 AVC、480p(525p)、アスペクト比16:9 パンベクトルなし"));
                ComponentKindDictionary.Add(0x05A4, new ComponentKindInfo(0x05, 0xA4, "H.264|MPEG-4 AVC、480p(525p)、アスペクト比 > 16:9"));
                ComponentKindDictionary.Add(0x05B1, new ComponentKindInfo(0x05, 0xB1, "H.264|MPEG-4 AVC、1080i(1125i)、アスペクト比4:3"));
                ComponentKindDictionary.Add(0x05B2, new ComponentKindInfo(0x05, 0xB2, "H.264|MPEG-4 AVC、1080i(1125i)、アスペクト比16:9 パンベクトルあり"));
                ComponentKindDictionary.Add(0x05B3, new ComponentKindInfo(0x05, 0xB3, "H.264|MPEG-4 AVC、1080i(1125i)、アスペクト比16:9 パンベクトルなし"));
                ComponentKindDictionary.Add(0x05B4, new ComponentKindInfo(0x05, 0xB4, "H.264|MPEG-4 AVC、1080i(1125i)、アスペクト比 > 16:9"));
                ComponentKindDictionary.Add(0x05C1, new ComponentKindInfo(0x05, 0xC1, "H.264|MPEG-4 AVC、720p(750p)、アスペクト比4:3"));
                ComponentKindDictionary.Add(0x05C2, new ComponentKindInfo(0x05, 0xC2, "H.264|MPEG-4 AVC、720p(750p)、アスペクト比16:9 パンベクトルあり"));
                ComponentKindDictionary.Add(0x05C3, new ComponentKindInfo(0x05, 0xC3, "H.264|MPEG-4 AVC、720p(750p)、アスペクト比16:9 パンベクトルなし"));
                ComponentKindDictionary.Add(0x05C4, new ComponentKindInfo(0x05, 0xC4, "H.264|MPEG-4 AVC、720p(750p)、アスペクト比 > 16:9"));
                ComponentKindDictionary.Add(0x05D1, new ComponentKindInfo(0x05, 0xD1, "H.264|MPEG-4 AVC、240p アスペクト比4:3"));
                ComponentKindDictionary.Add(0x05D2, new ComponentKindInfo(0x05, 0xD2, "H.264|MPEG-4 AVC、240p アスペクト比16:9 パンベクトルあり"));
                ComponentKindDictionary.Add(0x05D3, new ComponentKindInfo(0x05, 0xD3, "H.264|MPEG-4 AVC、240p アスペクト比16:9 パンベクトルなし"));
                ComponentKindDictionary.Add(0x05D4, new ComponentKindInfo(0x05, 0xD4, "H.264|MPEG-4 AVC、240p アスペクト比 > 16:9"));
                ComponentKindDictionary.Add(0x05E1, new ComponentKindInfo(0x05, 0xE1, "H.264|MPEG-4 AVC、1080p(1125p)、アスペクト比4:3"));
                ComponentKindDictionary.Add(0x05E2, new ComponentKindInfo(0x05, 0xE2, "H.264|MPEG-4 AVC、1080p(1125p)、アスペクト比16:9 パンベクトルあり"));
                ComponentKindDictionary.Add(0x05E3, new ComponentKindInfo(0x05, 0xE3, "H.264|MPEG-4 AVC、1080p(1125p)、アスペクト比16:9 パンベクトルなし"));
                ComponentKindDictionary.Add(0x05E4, new ComponentKindInfo(0x05, 0xE4, "H.264|MPEG-4 AVC、1080p(1125p)、アスペクト比 > 16:9"));
            }
            NWMode = false;
        }

        public static ulong Create64Key(ushort ONID, ushort TSID, ushort SID)
        {
            return ((ulong)ONID) << 32 | ((ulong)TSID) << 16 | (ulong)SID;
        }

        public static ulong Create64PgKey(ushort ONID, ushort TSID, ushort SID, ushort EventID)
        {
            return ((ulong)ONID) << 48 | ((ulong)TSID) << 32 | ((ulong)SID) << 16 | (ulong)EventID;
        }

        public static EpgServiceInfo ConvertChSet5To(ChSet5Item item)
        {
            EpgServiceInfo info = new EpgServiceInfo();
            try
            {
                info.ONID = item.ONID;
                info.TSID = item.TSID;
                info.SID = item.SID;
                info.NetworkName = item.NetworkName;
                info.IsPartialReception = item.PartialFlag;
                info.RemoconID = item.RemoconID;
                info.ServiceName = item.ServiceName;
                info.ServiceProviderName = item.NetworkName;
                info.ServiceType = (byte)item.ServiceType;
                info.TsName = item.NetworkName;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\r\n" + ex.StackTrace);
            }
            return info;
        }

        public static string ReplaceUrl(string url)
        {
            string retText = url;

            retText = retText.Replace("ａ", "a");
            retText = retText.Replace("ｂ", "b");
            retText = retText.Replace("ｃ", "c");
            retText = retText.Replace("ｄ", "d");
            retText = retText.Replace("ｅ", "e");
            retText = retText.Replace("ｆ", "f");
            retText = retText.Replace("ｇ", "g");
            retText = retText.Replace("ｈ", "h");
            retText = retText.Replace("ｉ", "i");
            retText = retText.Replace("ｊ", "j");
            retText = retText.Replace("ｋ", "k");
            retText = retText.Replace("ｌ", "l");
            retText = retText.Replace("ｍ", "m");
            retText = retText.Replace("ｎ", "n");
            retText = retText.Replace("ｏ", "o");
            retText = retText.Replace("ｐ", "p");
            retText = retText.Replace("ｑ", "q");
            retText = retText.Replace("ｒ", "r");
            retText = retText.Replace("ｓ", "s");
            retText = retText.Replace("ｔ", "t");
            retText = retText.Replace("ｕ", "u");
            retText = retText.Replace("ｖ", "v");
            retText = retText.Replace("ｗ", "w");
            retText = retText.Replace("ｘ", "x");
            retText = retText.Replace("ｙ", "y");
            retText = retText.Replace("ｚ", "z");
            retText = retText.Replace("Ａ", "A");
            retText = retText.Replace("Ｂ", "B");
            retText = retText.Replace("Ｃ", "C");
            retText = retText.Replace("Ｄ", "D");
            retText = retText.Replace("Ｅ", "E");
            retText = retText.Replace("Ｆ", "F");
            retText = retText.Replace("Ｇ", "G");
            retText = retText.Replace("Ｈ", "H");
            retText = retText.Replace("Ｉ", "I");
            retText = retText.Replace("Ｊ", "J");
            retText = retText.Replace("Ｋ", "K");
            retText = retText.Replace("Ｌ", "L");
            retText = retText.Replace("Ｍ", "M");
            retText = retText.Replace("Ｎ", "N");
            retText = retText.Replace("Ｏ", "O");
            retText = retText.Replace("Ｐ", "P");
            retText = retText.Replace("Ｑ", "Q");
            retText = retText.Replace("Ｒ", "R");
            retText = retText.Replace("Ｓ", "S");
            retText = retText.Replace("Ｔ", "T");
            retText = retText.Replace("Ｕ", "U");
            retText = retText.Replace("Ｖ", "V");
            retText = retText.Replace("Ｗ", "W");
            retText = retText.Replace("Ｘ", "X");
            retText = retText.Replace("Ｙ", "Y");
            retText = retText.Replace("Ｚ", "Z");
            retText = retText.Replace("＃", "#");
            retText = retText.Replace("＄", "$");
            retText = retText.Replace("％", "%");
            retText = retText.Replace("＆", "&");
            retText = retText.Replace("’", "'");
            retText = retText.Replace("（", "(");
            retText = retText.Replace("）", ")");
            retText = retText.Replace("～", "~");
            retText = retText.Replace("＝", "=");
            retText = retText.Replace("｜", "|");
            retText = retText.Replace("＾", "^");
            retText = retText.Replace("￥", "\\");
            retText = retText.Replace("＠", "@");
            retText = retText.Replace("；", ";");
            retText = retText.Replace("：", ":");
            retText = retText.Replace("｀", "`");
            retText = retText.Replace("｛", "{");
            retText = retText.Replace("｝", "}");
            retText = retText.Replace("＜", "<");
            retText = retText.Replace("＞", ">");
            retText = retText.Replace("？", "?");
            retText = retText.Replace("＿", "_");
            retText = retText.Replace("＋", "+");
            retText = retText.Replace("－", "-");
            retText = retText.Replace("＊", "*");
            retText = retText.Replace("／", "/");
            retText = retText.Replace("．", ".");
            retText = retText.Replace("０", "0");
            retText = retText.Replace("１", "1");
            retText = retText.Replace("２", "2");
            retText = retText.Replace("３", "3");
            retText = retText.Replace("４", "4");
            retText = retText.Replace("５", "5");
            retText = retText.Replace("６", "6");
            retText = retText.Replace("７", "7");
            retText = retText.Replace("８", "8");
            retText = retText.Replace("９", "9");

            return retText;
        }
        public static string GetNetworkName(ushort ONID)
        {
            if (0x7880 <= ONID && ONID <= 0x7FE8)
            {
                return "地デジ";
            }
            else if (ONID == 0x0004)
            {
                return "BS";
            }
            else if (ONID == 0x0006)
            {
                return "CS1";
            }
            else if (ONID == 0x0007)
            {
                return "CS2";
            }
            return "その他";
        }
    }
}

