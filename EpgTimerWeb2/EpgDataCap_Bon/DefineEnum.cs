﻿/*
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
namespace EpgTimer
{
    public delegate void ViewSettingClickHandler(object sender, object param);

    public enum ErrCode : uint
    {
        CMD_SUCCESS = 1, //成功
        CMD_ERR = 0, //汎用エラー
        CMD_NEXT = 202, //Enumコマンド用、続きあり
        CMD_NON_SUPPORT = 203, //未サポートのコマンド
        CMD_ERR_INVALID_ARG = 204, //引数エラー
        CMD_ERR_CONNECT = 205, //サーバーにコネクトできなかった
        CMD_ERR_DISCONNECT = 206, //サーバーから切断された
        CMD_ERR_TIMEOUT = 207, //タイムアウト発生
        CMD_ERR_BUSY = 208, //ビジー状態で現在処理できない（EPGデータ読み込み中、録画中など）
        CMD_NO_RES = 250, //Post用でレスポンスの必要なし
        CMD_NO_ARG = 255 //何か不足している
    };

    public enum UpdateNotifyItem : uint
    {
        No = 0, //なし
        EpgData = 1, //EPGデータ更新
        ReserveInfo = 2, //予約情報更新
        RecInfo = 3, //録画結果更新
        AutoAddEpgInfo = 4, //EPG自動予約登録更新
        AutoAddManualInfo = 5, //プログラム自動予約登録更新
        PlugInFile = 6, //PlugIn系のファイル一覧更新
        SrvStatus = 100,
        PreRecStart = 101,
        RecStart = 102,
        RecEnd = 103,
        RecTuijyu = 104,
        ChgTuijyu = 105,
        PreEpgCapStart = 106,
        EpgCapStart = 107,
        EpgCapEnd = 108,
    };

    public enum EpgViewMode : uint
    {
        Unknown = 0, //無効
        BS = 1, //BS
        CS = 2, //CS
        Tere = 3, //地デジ
        Other = 4, //その他
        Custom = 5, //カスタム
    };
    
    public enum EventInfoTextMode : uint
    {
        All = 0, //基本＋詳細
        BasicOnly = 1, //基本のみ
        ExtOnly = 2, //詳細のみ
    };
    /*
    public enum CtrlCmd : uint
    {
        //タイマーGUI（EpgTimer_Bon.exe）用
        CMD_TIMER_GUI_SHOW_DLG = 101, //ダイアログを前面に表示
        CMD_TIMER_GUI_UPDATE_RESERVE = 102, //予約一覧の情報が更新された
        CMD_TIMER_GUI_UPDATE_EPGDATA = 103, //EPGデータの再読み込みが完了した
        CMD_TIMER_GUI_VIEW_EXECUTE = 110, //Viewアプリ（EpgDataCap_Bon.exe）を起動
        CMD_TIMER_GUI_QUERY_SUSPEND = 120, //スタンバイ、休止、シャットダウンに入っていいかの確認をユーザーに行う（入っていいならCMD_EPG_SRV_SUSPENDを送る）
        CMD_TIMER_GUI_QUERY_REBOOT = 121, //PC再起動に入っていいかの確認をユーザーに行う（入っていいならCMD_EPG_SRV_REBOOTを送る）
        CMD_TIMER_GUI_SRV_STATUS_CHG = 130, //サーバーのステータス変更通知（1:通常、2:EPGデータ取得開始、3:予約録画開始）
        CMD_TIMER_GUI_SRV_STATUS_NOTIFY2 = 1130 //サーバーの情報変更通知
    }*/

    public class CMD_STREAM
    {
        public uint uiParam;
        public uint uiSize;
        public byte[] bData;

        public CMD_STREAM()
        {
            uiParam = 0;
            uiSize = 0;
            bData = null;
        }
    }

    public delegate int CMD_CALLBACK_PROC(object pParam, CMD_STREAM pCmdParam, ref CMD_STREAM pResParam);

}
