using System;
using System.Collections.Generic;
using System.IO;

namespace EpgTimer
{
    // 【StructDef.hから自動生成→型と変数名を調整】
    // sed s/BYTE/byte/g StructDef.h |
    // sed s/DWORD/uint/g |
    // sed s/WORD/ushort/g |
    // sed s/INT/int/g |
    // sed s/BOOL/int/g |
    // sed s/FALSE/0/g |
    // sed s/TRUE/1/g |
    // sed 's/L""/""/g' |
    // sed s/__int64/long/g |
    // sed s/wstring/string/g |
    // sed s/SYSTEMTIME/DateTime/g |
    // sed s/vector/List/g |
    // tr -d '*' |
    // sed 's/}.*$/}/' |
    // sed 's/^\t\([A-Za-z].*;\)/\tpublic \1/' |
    // sed 's#^\(\t[A-Za-z].*;\)\t*//\(.*\)$#\t/// <summary>\2</summary>\n\1#' |
    // sed 's/typedef struct _/public class /' |
    // sed 's/_\(.*\)(void){$/public \1(){/' |
    // sed 's/\t/    /g'
    //public $1 $2{\nset{\n_$2=value;}\nget{\nreturn _$2;\n}\n}\nprivate $1 _$2;\n

    /// <summary>録画フォルダ情報</summary>
    public class RecFileSetInfo : ICtrlCmdReadWrite
    {
        /// <summary>録画フォルダ</summary>
        public string RecFolder
        {
            set
            {
                _RecFolder = value;
            }
            get
            {
                return _RecFolder;
            }
        }
        private string _RecFolder;

        /// <summary>出力PlugIn</summary>
        public string WritePlugIn
        {
            set
            {
                _WritePlugIn = value;
            }
            get
            {
                return _WritePlugIn;
            }
        }
        private string _WritePlugIn;

        /// <summary>ファイル名変換PlugInの使用</summary>
        public string RecNamePlugIn
        {
            set
            {
                _RecNamePlugIn = value;
            }
            get
            {
                return _RecNamePlugIn;
            }
        }
        private string _RecNamePlugIn;

        /// <summary>ファイル名個別対応 録画開始処理時に内部で使用。予約情報としては必要なし</summary>
        public string RecFileName
        {
            set
            {
                _RecFileName = value;
            }
            get
            {
                return _RecFileName;
            }
        }
        private string _RecFileName;

        public RecFileSetInfo()
        {
            RecFolder = "";
            WritePlugIn = "";
            RecNamePlugIn = "";
            RecFileName = "";
        }
        public void Write(MemoryStream s, ushort version)
        {
            var w = new CtrlCmdWriter(s, version);
            w.Begin();
            w.Write(RecFolder);
            w.Write(WritePlugIn);
            w.Write(RecNamePlugIn);
            w.Write(RecFileName);
            w.End();
        }
        public void Read(MemoryStream s, ushort version)
        {
            var r = new CtrlCmdReader(s, version);
            r.Begin();
            r.Read(ref _RecFolder);
            r.Read(ref _WritePlugIn);
            r.Read(ref _RecNamePlugIn);
            r.Read(ref _RecFileName);
            r.End();
        }
    }

    /// <summary>録画設定情報</summary>
    public class RecSettingData : ICtrlCmdReadWrite
    {
        /// <summary>録画モード</summary>
        public byte RecMode
        {
            set
            {
                _RecMode = value;
            }
            get
            {
                return _RecMode;
            }
        }
        private byte _RecMode;

        /// <summary>優先度</summary>
        public byte Priority
        {
            set
            {
                _Priority = value;
            }
            get
            {
                return _Priority;
            }
        }
        private byte _Priority;

        /// <summary>イベントリレー追従するかどうか</summary>
        public byte IsTuijyuu
        {
            set
            {
                _IsTuijyuu = value;
            }
            get
            {
                return _IsTuijyuu;
            }
        }
        private byte _IsTuijyuu;

        /// <summary>処理対象データモード</summary>
        public uint ServiceMode
        {
            set
            {
                _ServiceMode = value;
            }
            get
            {
                return _ServiceMode;
            }
        }
        private uint _ServiceMode;

        /// <summary>ぴったり？録画</summary>
        public byte IsPittari
        {
            set
            {
                _IsPittari = value;
            }
            get
            {
                return _IsPittari;
            }
        }
        private byte _IsPittari;

        /// <summary>録画後BATファイルパス</summary>
        public string BatFilePath
        {
            set
            {
                _BatFilePath = value;
            }
            get
            {
                return _BatFilePath;
            }
        }
        private string _BatFilePath;

        /// <summary>録画フォルダパス</summary>
        public List<RecFileSetInfo> RecFolderList
        {
            set
            {
                _RecFolderList = value;
            }
            get
            {
                return _RecFolderList;
            }
        }
        private List<RecFileSetInfo> _RecFolderList;

        /// <summary>休止モード</summary>
        public byte SuspendMode
        {
            set
            {
                _SuspendMode = value;
            }
            get
            {
                return _SuspendMode;
            }
        }
        private byte _SuspendMode;

        /// <summary>録画後再起動する</summary>
        public byte IsReboot
        {
            set
            {
                _IsReboot = value;
            }
            get
            {
                return _IsReboot;
            }
        }
        private byte _IsReboot;

        /// <summary>録画マージンを個別指定</summary>
        public byte UseMargin
        {
            set
            {
                _UseMargine = value;
            }
            get
            {
                return _UseMargine;
            }
        }
        private byte _UseMargine;

        /// <summary>録画開始時のマージン</summary>
        public int StartMargin
        {
            set
            {
                _StartMargine = value;
            }
            get
            {
                return _StartMargine;
            }
        }
        private int _StartMargine;

        /// <summary>録画終了時のマージン</summary>
        public int EndMargin
        {
            set
            {
                _EndMargine = value;
            }
            get
            {
                return _EndMargine;
            }
        }
        private int _EndMargine;

        /// <summary>後続同一サービス時、同一ファイルで録画</summary>
        public byte IsContinueRec
        {
            set
            {
                _IsContinueRec = value;
            }
            get
            {
                return _IsContinueRec;
            }
        }
        private byte _IsContinueRec;

        /// <summary>物理CHに部分受信サービスがある場合、同時録画するかどうか</summary>
        public byte IsPartialRec
        {
            set
            {
                _IsPartialRec = value;
            }
            get
            {
                return _IsPartialRec;
            }
        }
        private byte _IsPartialRec;

        /// <summary>強制的に使用Tunerを固定</summary>
        public uint TunerID
        {
            set
            {
                _TunerID = value;
            }
            get
            {
                return _TunerID;
            }
        }
        private uint _TunerID;

        /// <summary>部分受信サービス録画のフォルダ</summary>
        public List<RecFileSetInfo> PartialRecFolder
        {
            set
            {
                _PartialRecFolder = value;
            }
            get
            {
                return _PartialRecFolder;
            }
        }
        private List<RecFileSetInfo> _PartialRecFolder;

        public RecSettingData()
        {
            RecMode = 1;
            Priority = 1;
            IsTuijyuu = 1;
            ServiceMode = 0;
            IsPittari = 0;
            BatFilePath = "";
            RecFolderList = new List<RecFileSetInfo>();
            SuspendMode = 0;
            IsReboot = 0;
            UseMargin = 0;
            StartMargin = 10;
            EndMargin = 5;
            IsContinueRec = 0;
            IsPartialRec = 0;
            TunerID = 0;
            PartialRecFolder = new List<RecFileSetInfo>();
        }
        public void Write(MemoryStream s, ushort version)
        {
            var w = new CtrlCmdWriter(s, version);
            w.Begin();
            w.Write(RecMode);
            w.Write(Priority);
            w.Write(IsTuijyuu);
            w.Write(ServiceMode);
            w.Write(IsPittari);
            w.Write(BatFilePath);
            w.Write(RecFolderList);
            w.Write(SuspendMode);
            w.Write(IsReboot);
            w.Write(UseMargin);
            w.Write(StartMargin);
            w.Write(EndMargin);
            w.Write(IsContinueRec);
            w.Write(IsPartialRec);
            w.Write(TunerID);
            if (version >= 2)
            {
                w.Write(PartialRecFolder);
            }
            w.End();
        }
        public void Read(MemoryStream s, ushort version)
        {
            var r = new CtrlCmdReader(s, version);
            r.Begin();
            r.Read(ref _RecMode);
            r.Read(ref _Priority);
            r.Read(ref _IsTuijyuu);
            r.Read(ref _ServiceMode);
            r.Read(ref _IsPittari);
            r.Read(ref _BatFilePath);
            r.Read(ref _RecFolderList);
            r.Read(ref _SuspendMode);
            r.Read(ref _IsReboot);
            r.Read(ref _UseMargine);
            r.Read(ref _StartMargine);
            r.Read(ref _EndMargine);
            r.Read(ref _IsContinueRec);
            r.Read(ref _IsPartialRec);
            r.Read(ref _TunerID);
            if (version >= 2)
            {
                r.Read(ref _PartialRecFolder);
            }
            r.End();
        }
    }

    /// <summary>登録予約情報</summary>
    public class ReserveData : ICtrlCmdReadWrite
    {
        /// <summary>番組名</summary>
        public string Title
        {
            set
            {
                _Title = value;
            }
            get
            {
                return _Title;
            }
        }
        private string _Title;

        /// <summary>録画開始時間</summary>
        public DateTime StartTime
        {
            set
            {
                _StartTime = value;
            }
            get
            {
                return _StartTime;
            }
        }
        private DateTime _StartTime;

        /// <summary>録画総時間</summary>
        public uint DurationSecond
        {
            set
            {
                _DurationSecond = value;
            }
            get
            {
                return _DurationSecond;
            }
        }
        private uint _DurationSecond;

        /// <summary>サービス名</summary>
        public string StationName
        {
            set
            {
                _StationName = value;
            }
            get
            {
                return _StationName;
            }
        }
        private string _StationName;

        /// <summary>ONID</summary>
        public ushort ONID
        {
            set
            {
                _OriginalNetworkID = value;
            }
            get
            {
                return _OriginalNetworkID;
            }
        }
        private ushort _OriginalNetworkID;

        /// <summary>TSID</summary>
        public ushort TSID
        {
            set
            {
                _TransportStreamID = value;
            }
            get
            {
                return _TransportStreamID;
            }
        }
        private ushort _TransportStreamID;

        /// <summary>SID</summary>
        public ushort SID
        {
            set
            {
                _ServiceID = value;
            }
            get
            {
                return _ServiceID;
            }
        }
        private ushort _ServiceID;

        /// <summary>EventID</summary>
        public ushort EventID
        {
            set
            {
                _EventID = value;
            }
            get
            {
                return _EventID;
            }
        }
        private ushort _EventID;

        /// <summary>コメント</summary>
        public string Comment
        {
            set
            {
                _Comment = value;
            }
            get
            {
                return _Comment;
            }
        }
        private string _Comment;

        /// <summary>予約識別ID 予約登録時は0</summary>
        public uint ReserveID
        {
            set
            {
                _ReserveID = value;
            }
            get
            {
                return _ReserveID;
            }
        }
        private uint _ReserveID;

        /// <summary>予約待機入った？ 内部で使用（廃止）</summary>
        private byte IsUnusedRecWait;
        /// <summary>かぶり状態 1:かぶってチューナー足りない予約あり 2:チューナー足りなくて予約できない</summary>
        public byte OverlapMode
        {
            set
            {
                _OverlapMode = value;
            }
            get
            {
                return _OverlapMode;
            }
        }
        private byte _OverlapMode;

        /// <summary>録画ファイルパス 旧バージョン互換用 未使用（廃止）</summary>
        private string UnusedRecFilePath;
        /// <summary>予約時の開始時間</summary>
        public DateTime StartTimeEpg
        {
            set
            {
                _StartTimeEpg = value;
            }
            get
            {
                return _StartTimeEpg;
            }
        }
        private DateTime _StartTimeEpg;

        /// <summary>録画設定</summary>
        public RecSettingData RecSetting
        {
            set
            {
                _RecSetting = value;
            }
            get
            {
                return _RecSetting;
            }
        }
        private RecSettingData _RecSetting;

        /// <summary>予約追加状態 内部で使用</summary>
        public uint ReserveStatus
        {
            set
            {
                _ReserveStatus = value;
            }
            get
            {
                return _ReserveStatus;
            }
        }
        private uint _ReserveStatus;

        /// <summary>録画予定ファイル名</summary>
        public List<string> RecFileNameList
        {
            set
            {
                _RecFileNameList = value;
            }
            get
            {
                return _RecFileNameList;
            }
        }
        private List<string> _RecFileNameList;

        /// <summary>将来用</summary>
        private uint UnusedParam1;
        public ReserveData()
        {
            Title = "";
            StartTime = new DateTime();
            DurationSecond = 0;
            StationName = "";
            ONID = 0;
            TSID = 0;
            SID = 0;
            EventID = 0;
            Comment = "";
            ReserveID = 0;
            IsUnusedRecWait = 0;
            OverlapMode = 0;
            UnusedRecFilePath = "";
            StartTimeEpg = new DateTime();
            RecSetting = new RecSettingData();
            ReserveStatus = 0;
            RecFileNameList = new List<string>();
            UnusedParam1 = 0;
        }
        public void Write(MemoryStream s, ushort version)
        {
            var w = new CtrlCmdWriter(s, version);
            w.Begin();
            w.Write(Title);
            w.Write(StartTime);
            w.Write(DurationSecond);
            w.Write(StationName);
            w.Write(ONID);
            w.Write(TSID);
            w.Write(SID);
            w.Write(EventID);
            w.Write(Comment);
            w.Write(ReserveID);
            w.Write(IsUnusedRecWait);
            w.Write(OverlapMode);
            w.Write(UnusedRecFilePath);
            w.Write(StartTimeEpg);
            w.Write(RecSetting);
            w.Write(ReserveStatus);
            if (version >= 5)
            {
                w.Write(RecFileNameList);
                w.Write(UnusedParam1);
            }
            w.End();
        }
        public void Read(MemoryStream s, ushort version)
        {
            var r = new CtrlCmdReader(s, version);
            r.Begin();
            r.Read(ref _Title);
            r.Read(ref _StartTime);
            r.Read(ref _DurationSecond);
            r.Read(ref _StationName);
            r.Read(ref _OriginalNetworkID);
            r.Read(ref _TransportStreamID);
            r.Read(ref _ServiceID);
            r.Read(ref _EventID);
            r.Read(ref _Comment);
            r.Read(ref _ReserveID);
            r.Read(ref IsUnusedRecWait);
            r.Read(ref _OverlapMode);
            r.Read(ref UnusedRecFilePath);
            r.Read(ref _StartTimeEpg);
            r.Read(ref _RecSetting);
            r.Read(ref _ReserveStatus);
            if (version >= 5)
            {
                r.Read(ref _RecFileNameList);
                r.Read(ref UnusedParam1);
            }
            r.End();
        }
    }

    public class RecFileInfo : ICtrlCmdReadWrite
    {
        /// <summary>ID</summary>
        public uint ID
        {
            set
            {
                _ID = value;
            }
            get
            {
                return _ID;
            }
        }
        private uint _ID;

        /// <summary>録画ファイルパス</summary>
        public string RecFilePath
        {
            set
            {
                _RecFilePath = value;
            }
            get
            {
                return _RecFilePath;
            }
        }
        private string _RecFilePath;

        /// <summary>番組名</summary>
        public string Title
        {
            set
            {
                _Title = value;
            }
            get
            {
                return _Title;
            }
        }
        private string _Title;

        /// <summary>開始時間</summary>
        public DateTime StartTime
        {
            set
            {
                _StartTime = value;
            }
            get
            {
                return _StartTime;
            }
        }
        private DateTime _StartTime;

        /// <summary>録画時間</summary>
        public uint DurationSecond
        {
            set
            {
                _DurationSecond = value;
            }
            get
            {
                return _DurationSecond;
            }
        }
        private uint _DurationSecond;

        /// <summary>サービス名</summary>
        public string ServiceName
        {
            set
            {
                _ServiceName = value;
            }
            get
            {
                return _ServiceName;
            }
        }
        private string _ServiceName;

        /// <summary>ONID</summary>
        public ushort OriginalNetworkID
        {
            set
            {
                _OriginalNetworkID = value;
            }
            get
            {
                return _OriginalNetworkID;
            }
        }
        private ushort _OriginalNetworkID;

        /// <summary>TSID</summary>
        public ushort TransportStreamID
        {
            set
            {
                _TransportStreamID = value;
            }
            get
            {
                return _TransportStreamID;
            }
        }
        private ushort _TransportStreamID;

        /// <summary>SID</summary>
        public ushort ServiceID
        {
            set
            {
                _ServiceID = value;
            }
            get
            {
                return _ServiceID;
            }
        }
        private ushort _ServiceID;

        /// <summary>EventID</summary>
        public ushort EventID
        {
            set
            {
                _EventID = value;
            }
            get
            {
                return _EventID;
            }
        }
        private ushort _EventID;

        /// <summary>ドロップ数</summary>
        public long Drops
        {
            set
            {
                _Drops = value;
            }
            get
            {
                return _Drops;
            }
        }
        private long _Drops;

        /// <summary>スクランブル数</summary>
        public long Scrambles
        {
            set
            {
                _Scrambles = value;
            }
            get
            {
                return _Scrambles;
            }
        }
        private long _Scrambles;

        /// <summary>録画結果のステータス</summary>
        public uint RecStatus
        {
            set
            {
                _RecStatus = value;
            }
            get
            {
                return _RecStatus;
            }
        }
        private uint _RecStatus;

        /// <summary>予約時の開始時間</summary>
        public DateTime StartTimeEpg
        {
            set
            {
                _StartTimeEpg = value;
            }
            get
            {
                return _StartTimeEpg;
            }
        }
        private DateTime _StartTimeEpg;

        /// <summary>コメント</summary>
        public string Comment
        {
            set
            {
                _Comment = value;
            }
            get
            {
                return _Comment;
            }
        }
        private string _Comment;

        /// <summary>.program.txtファイルの内容</summary>
        public string ProgramInfo
        {
            set
            {
                _ProgramInfo = value;
            }
            get
            {
                return _ProgramInfo;
            }
        }
        private string _ProgramInfo;

        /// <summary>.errファイルの内容</summary>
        public string ErrInfo
        {
            set
            {
                _ErrInfo = value;
            }
            get
            {
                return _ErrInfo;
            }
        }
        private string _ErrInfo;

        public byte IsProtect
        {
            set
            {
                _IsProtect = value;
            }
            get
            {
                return _IsProtect;
            }
        }
        private byte _IsProtect;

        public RecFileInfo()
        {
            ID = 0;
            RecFilePath = "";
            Title = "";
            StartTime = new DateTime();
            DurationSecond = 0;
            ServiceName = "";
            OriginalNetworkID = 0;
            TransportStreamID = 0;
            ServiceID = 0;
            EventID = 0;
            Drops = 0;
            Scrambles = 0;
            RecStatus = 0;
            StartTimeEpg = new DateTime();
            Comment = "";
            ProgramInfo = "";
            ErrInfo = "";
            IsProtect = 0;
        }
        public void Write(MemoryStream s, ushort version)
        {
            var w = new CtrlCmdWriter(s, version);
            w.Begin();
            w.Write(ID);
            w.Write(RecFilePath);
            w.Write(Title);
            w.Write(StartTime);
            w.Write(DurationSecond);
            w.Write(ServiceName);
            w.Write(OriginalNetworkID);
            w.Write(TransportStreamID);
            w.Write(ServiceID);
            w.Write(EventID);
            w.Write(Drops);
            w.Write(Scrambles);
            w.Write(RecStatus);
            w.Write(StartTimeEpg);
            w.Write(Comment);
            w.Write(ProgramInfo);
            w.Write(ErrInfo);
            if (version >= 4)
            {
                w.Write(IsProtect);
            }
            w.End();
        }
        public void Read(MemoryStream s, ushort version)
        {
            var r = new CtrlCmdReader(s, version);
            r.Begin();
            r.Read(ref _ID);
            r.Read(ref _RecFilePath);
            r.Read(ref _Title);
            r.Read(ref _StartTime);
            r.Read(ref _DurationSecond);
            r.Read(ref _ServiceName);
            r.Read(ref _OriginalNetworkID);
            r.Read(ref _TransportStreamID);
            r.Read(ref _ServiceID);
            r.Read(ref _EventID);
            r.Read(ref _Drops);
            r.Read(ref _Scrambles);
            r.Read(ref _RecStatus);
            r.Read(ref _StartTimeEpg);
            r.Read(ref _Comment);
            r.Read(ref _ProgramInfo);
            r.Read(ref _ErrInfo);
            if (version >= 4)
            {
                r.Read(ref _IsProtect);
            }
            r.End();
        }
    }

    public class TunerReserveInfo : ICtrlCmdReadWrite
    {
        public uint TunerID
        {
            set
            {
                _TunerID = value;
            }
            get
            {
                return _TunerID;
            }
        }
        private uint _TunerID;

        public string TunerName
        {
            set
            {
                _TunerName = value;
            }
            get
            {
                return _TunerName;
            }
        }
        private string _TunerName;

        public List<uint> ReserveList
        {
            set
            {
                _ReserveList = value;
            }
            get
            {
                return _ReserveList;
            }
        }
        private List<uint> _ReserveList;

        public TunerReserveInfo()
        {
            TunerID = 0;
            TunerName = "";
            ReserveList = new List<uint>();
        }
        public void Write(MemoryStream s, ushort version)
        {
            var w = new CtrlCmdWriter(s, version);
            w.Begin();
            w.Write(TunerID);
            w.Write(TunerName);
            w.Write(ReserveList);
            w.End();
        }
        public void Read(MemoryStream s, ushort version)
        {
            var r = new CtrlCmdReader(s, version);
            r.Begin();
            r.Read(ref _TunerID);
            r.Read(ref _TunerName);
            r.Read(ref _ReserveList);
            r.End();
        }
    }

    /// <summary>EPG基本情報</summary>
    public class EpgShortEventInfo : ICtrlCmdReadWrite
    {
        /// <summary>イベント名</summary>
        public string EventName
        {
            set
            {
                _EventName = value;
            }
            get
            {
                return _EventName;
            }
        }
        private string _EventName;

        /// <summary>情報</summary>
        public string Text
        {
            set
            {
                _Text = value;
            }
            get
            {
                return _Text;
            }
        }
        private string _Text;

        public EpgShortEventInfo()
        {
            EventName = "";
            Text = "";
        }
        public void Write(MemoryStream s, ushort version)
        {
            var w = new CtrlCmdWriter(s, version);
            w.Begin();
            w.Write(EventName);
            w.Write(Text);
            w.End();
        }
        public void Read(MemoryStream s, ushort version)
        {
            var r = new CtrlCmdReader(s, version);
            r.Begin();
            r.Read(ref _EventName);
            r.Read(ref _Text);
            r.End();
        }
    }

    /// <summary>EPG拡張情報</summary>
    public class EpgExtendedEventInfo : ICtrlCmdReadWrite
    {
        /// <summary>詳細情報</summary>
        public string Text
        {
            set
            {
                _Text = value;
            }
            get
            {
                return _Text;
            }
        }
        private string _Text;

        public EpgExtendedEventInfo()
        {
            Text = "";
        }
        public void Write(MemoryStream s, ushort version)
        {
            var w = new CtrlCmdWriter(s, version);
            w.Begin();
            w.Write(Text);
            w.End();
        }
        public void Read(MemoryStream s, ushort version)
        {
            var r = new CtrlCmdReader(s, version);
            r.Begin();
            r.Read(ref _Text);
            r.End();
        }
    }

    /// <summary>EPGジャンルデータ</summary>
    public class EpgContentData : ICtrlCmdReadWrite
    {
        public byte Content1
        {
            set
            {
                _Content1 = value;
            }
            get
            {
                return _Content1;
            }
        }
        private byte _Content1;

        public byte Content2
        {
            set
            {
                _Content2 = value;
            }
            get
            {
                return _Content2;
            }
        }
        private byte _Content2;

        public byte User1
        {
            set
            {
                _User1 = value;
            }
            get
            {
                return _User1;
            }
        }
        private byte _User1;

        public byte User2
        {
            set
            {
                _User2 = value;
            }
            get
            {
                return _User2;
            }
        }
        private byte _User2;

        public EpgContentData()
        {
            Content1 = 0;
            User1 = 0;
            User2 = 0;
        }
        public void Write(MemoryStream s, ushort version)
        {
            var w = new CtrlCmdWriter(s, version);
            w.Begin();
            w.Write(Content1);
            w.Write(Content2);
            w.Write(User1);
            w.Write(User2);
            w.End();
        }
        public void Read(MemoryStream s, ushort version)
        {
            var r = new CtrlCmdReader(s, version);
            r.Begin();
            r.Read(ref _Content1);
            r.Read(ref _Content2);
            r.Read(ref _User1);
            r.Read(ref _User2);
            r.End();
        }
    }

    /// <summary>EPGジャンル情報</summary>
    public class EpgContentInfo : ICtrlCmdReadWrite
    {
        public List<EpgContentData> NibbleList
        {
            set
            {
                _NibbleList = value;
            }
            get
            {
                return _NibbleList;
            }
        }
        private List<EpgContentData> _NibbleList;

        public EpgContentInfo()
        {
            NibbleList = new List<EpgContentData>();
        }
        public void Write(MemoryStream s, ushort version)
        {
            var w = new CtrlCmdWriter(s, version);
            w.Begin();
            w.Write(NibbleList);
            w.End();
        }
        public void Read(MemoryStream s, ushort version)
        {
            var r = new CtrlCmdReader(s, version);
            r.Begin();
            r.Read(ref _NibbleList);
            r.End();
        }
    }

    /// <summary>EPG映像情報</summary>
    public class EpgComponentInfo : ICtrlCmdReadWrite
    {
        public byte StreamContent
        {
            set
            {
                _StreamContent = value;
            }
            get
            {
                return _StreamContent;
            }
        }
        private byte _StreamContent;

        public byte Type
        {
            set
            {
                _Type = value;
            }
            get
            {
                return _Type;
            }
        }
        private byte _Type;

        public byte Tag
        {
            set
            {
                _Tag = value;
            }
            get
            {
                return _Tag;
            }
        }
        private byte _Tag;

        /// <summary>情報</summary>
        public string Text
        {
            set
            {
                _Text = value;
            }
            get
            {
                return _Text;
            }
        }
        private string _Text;

        public EpgComponentInfo()
        {
            StreamContent = 0;
            Type = 0;
            Tag = 0;
            Text = "";
        }
        public void Write(MemoryStream s, ushort version)
        {
            var w = new CtrlCmdWriter(s, version);
            w.Begin();
            w.Write(StreamContent);
            w.Write(Type);
            w.Write(Tag);
            w.Write(Text);
            w.End();
        }
        public void Read(MemoryStream s, ushort version)
        {
            var r = new CtrlCmdReader(s, version);
            r.Begin();
            r.Read(ref _StreamContent);
            r.Read(ref _Type);
            r.Read(ref _Tag);
            r.Read(ref _Text);
            r.End();
        }
    }

    /// <summary>EPG音声情報データ</summary>
    public class EpgAudioComponentInfoData : ICtrlCmdReadWrite
    {
        public byte StreamContent
        {
            set
            {
                _StreamContent = value;
            }
            get
            {
                return _StreamContent;
            }
        }
        private byte _StreamContent;

        public byte Type
        {
            set
            {
                _Type = value;
            }
            get
            {
                return _Type;
            }
        }
        private byte _Type;

        public byte Tag
        {
            set
            {
                _Tag = value;
            }
            get
            {
                return _Tag;
            }
        }
        private byte _Tag;

        public byte StreamType
        {
            set
            {
                _StreamType = value;
            }
            get
            {
                return _StreamType;
            }
        }
        private byte _StreamType;

        public byte SimulcastGroupTag
        {
            set
            {
                _SimulcastGroupTag = value;
            }
            get
            {
                return _SimulcastGroupTag;
            }
        }
        private byte _SimulcastGroupTag;

        public byte IsESMultiLingual
        {
            set
            {
                _IsESMultiLingual = value;
            }
            get
            {
                return _IsESMultiLingual;
            }
        }
        private byte _IsESMultiLingual;

        public byte IsMain
        {
            set
            {
                _IsMain = value;
            }
            get
            {
                return _IsMain;
            }
        }
        private byte _IsMain;

        public byte QualityIndicator
        {
            set
            {
                _QualityIndicator = value;
            }
            get
            {
                return _QualityIndicator;
            }
        }
        private byte _QualityIndicator;

        public byte SamplingRate
        {
            set
            {
                _SamplingRate = value;
            }
            get
            {
                return _SamplingRate;
            }
        }
        private byte _SamplingRate;

        /// <summary>詳細情報</summary>
        public string Text
        {
            set
            {
                _Text = value;
            }
            get
            {
                return _Text;
            }
        }
        private string _Text;

        public EpgAudioComponentInfoData()
        {
            StreamContent = 0;
            Type = 0;
            Tag = 0;
            StreamType = 0;
            SimulcastGroupTag = 0;
            IsESMultiLingual = 0;
            IsMain = 0;
            QualityIndicator = 0;
            SamplingRate = 0;
            Text = "";
        }
        public void Write(MemoryStream s, ushort version)
        {
            var w = new CtrlCmdWriter(s, version);
            w.Begin();
            w.Write(StreamContent);
            w.Write(Type);
            w.Write(Tag);
            w.Write(StreamType);
            w.Write(SimulcastGroupTag);
            w.Write(IsESMultiLingual);
            w.Write(IsMain);
            w.Write(QualityIndicator);
            w.Write(SamplingRate);
            w.Write(Text);
            w.End();
        }
        public void Read(MemoryStream s, ushort version)
        {
            var r = new CtrlCmdReader(s, version);
            r.Begin();
            r.Read(ref _StreamContent);
            r.Read(ref _Type);
            r.Read(ref _Tag);
            r.Read(ref _StreamType);
            r.Read(ref _SimulcastGroupTag);
            r.Read(ref _IsESMultiLingual);
            r.Read(ref _IsMain);
            r.Read(ref _QualityIndicator);
            r.Read(ref _SamplingRate);
            r.Read(ref _Text);
            r.End();
        }
    }

    /// <summary>EPG音声情報</summary>
    public class EpgAudioComponentInfo : ICtrlCmdReadWrite
    {
        public List<EpgAudioComponentInfoData> ComponentList
        {
            set
            {
                _ComponentList = value;
            }
            get
            {
                return _ComponentList;
            }
        }
        private List<EpgAudioComponentInfoData> _ComponentList;

        public EpgAudioComponentInfo()
        {
            ComponentList = new List<EpgAudioComponentInfoData>();
        }
        public void Write(MemoryStream s, ushort version)
        {
            var w = new CtrlCmdWriter(s, version);
            w.Begin();
            w.Write(ComponentList);
            w.End();
        }
        public void Read(MemoryStream s, ushort version)
        {
            var r = new CtrlCmdReader(s, version);
            r.Begin();
            r.Read(ref _ComponentList);
            r.End();
        }
    }

    /// <summary>EPGイベントデータ</summary>
    public class EpgEventData : ICtrlCmdReadWrite
    {
        public ushort ONID
        {
            set
            {
                _ONID = value;
            }
            get
            {
                return _ONID;
            }
        }
        private ushort _ONID;

        public ushort TSID
        {
            set
            {
                _TSID = value;
            }
            get
            {
                return _TSID;
            }
        }
        private ushort _TSID;

        public ushort SID
        {
            set
            {
                _SID = value;
            }
            get
            {
                return _SID;
            }
        }
        private ushort _SID;

        public ushort EventID
        {
            set
            {
                _EventID = value;
            }
            get
            {
                return _EventID;
            }
        }
        private ushort _EventID;

        public EpgEventData()
        {
            ONID = 0;
            TSID = 0;
            SID = 0;
            EventID = 0;
        }
        public void Write(MemoryStream s, ushort version)
        {
            var w = new CtrlCmdWriter(s, version);
            w.Begin();
            w.Write(ONID);
            w.Write(TSID);
            w.Write(SID);
            w.Write(EventID);
            w.End();
        }
        public void Read(MemoryStream s, ushort version)
        {
            var r = new CtrlCmdReader(s, version);
            r.Begin();
            r.Read(ref _ONID);
            r.Read(ref _TSID);
            r.Read(ref _SID);
            r.Read(ref _EventID);
            r.End();
        }
    }

    /// <summary>EPGイベントグループ情報</summary>
    public class EpgEventGroupInfo : ICtrlCmdReadWrite
    {
        public byte GroupType
        {
            set
            {
                _GroupType = value;
            }
            get
            {
                return _GroupType;
            }
        }
        private byte _GroupType;

        public List<EpgEventData> EventList
        {
            set
            {
                _EventList = value;
            }
            get
            {
                return _EventList;
            }
        }
        private List<EpgEventData> _EventList;

        public EpgEventGroupInfo()
        {
            GroupType = 0;
            EventList = new List<EpgEventData>();
        }
        public void Write(MemoryStream s, ushort version)
        {
            var w = new CtrlCmdWriter(s, version);
            w.Begin();
            w.Write(GroupType);
            w.Write(EventList);
            w.End();
        }
        public void Read(MemoryStream s, ushort version)
        {
            var r = new CtrlCmdReader(s, version);
            r.Begin();
            r.Read(ref _GroupType);
            r.Read(ref _EventList);
            r.End();
        }
    }

    public class EpgEventInfo : ICtrlCmdReadWrite
    {
        public ushort ONID
        {
            set
            {
                _ONID = value;
            }
            get
            {
                return _ONID;
            }
        }
        private ushort _ONID;

        public ushort TSID
        {
            set
            {
                _TSID = value;
            }
            get
            {
                return _TSID;
            }
        }
        private ushort _TSID;

        public ushort SID
        {
            set
            {
                _SID = value;
            }
            get
            {
                return _SID;
            }
        }
        private ushort _SID;

        /// <summary>イベントID</summary>
        public ushort EventID
        {
            set
            {
                _EventID = value;
            }
            get
            {
                return _EventID;
            }
        }
        private ushort _EventID;

        /// <summary>StartTimeの値が有効かどうか</summary>
        public byte IsStartTime
        {
            set
            {
                _IsStartTime = value;
            }
            get
            {
                return _IsStartTime;
            }
        }
        private byte _IsStartTime;

        /// <summary>開始時間</summary>
        public DateTime StartTime
        {
            set
            {
                _StartTime = value;
            }
            get
            {
                return _StartTime;
            }
        }
        private DateTime _StartTime;

        /// <summary>Durationの値が有効かどうか</summary>
        public byte IsDuration
        {
            set
            {
                _IsDuration = value;
            }
            get
            {
                return _IsDuration;
            }
        }
        private byte _IsDuration;

        /// <summary>総時間（単位：秒）</summary>
        public uint DurationSec
        {
            set
            {
                _DurationSec = value;
            }
            get
            {
                return _DurationSec;
            }
        }
        private uint _DurationSec;

        /// <summary>基本情報</summary>
        public EpgShortEventInfo ShortInfo
        {
            set
            {
                _ShortInfo = value;
            }
            get
            {
                return _ShortInfo;
            }
        }
        private EpgShortEventInfo _ShortInfo;

        /// <summary>拡張情報</summary>
        public EpgExtendedEventInfo ExtInfo
        {
            set
            {
                _ExtInfo = value;
            }
            get
            {
                return _ExtInfo;
            }
        }
        private EpgExtendedEventInfo _ExtInfo;

        /// <summary>ジャンル情報</summary>
        public EpgContentInfo ContentInfo
        {
            set
            {
                _ContentInfo = value;
            }
            get
            {
                return _ContentInfo;
            }
        }
        private EpgContentInfo _ContentInfo;

        /// <summary>映像情報</summary>
        public EpgComponentInfo ComponentInfo
        {
            set
            {
                _ComponentInfo = value;
            }
            get
            {
                return _ComponentInfo;
            }
        }
        private EpgComponentInfo _ComponentInfo;

        /// <summary>音声情報</summary>
        public EpgAudioComponentInfo AudioInfo
        {
            set
            {
                _AudioInfo = value;
            }
            get
            {
                return _AudioInfo;
            }
        }
        private EpgAudioComponentInfo _AudioInfo;

        /// <summary>イベントグループ情報</summary>
        public EpgEventGroupInfo EventGroupInfo
        {
            set
            {
                _EventGroupInfo = value;
            }
            get
            {
                return _EventGroupInfo;
            }
        }
        private EpgEventGroupInfo _EventGroupInfo;

        /// <summary>イベントリレー情報</summary>
        public EpgEventGroupInfo EventRelayInfo
        {
            set
            {
                _EventRelayInfo = value;
            }
            get
            {
                return _EventRelayInfo;
            }
        }
        private EpgEventGroupInfo _EventRelayInfo;

        /// <summary>ノンスクランブルフラグ</summary>
        public byte FreeCA
        {
            set
            {
                _FreeCA = value;
            }
            get
            {
                return _FreeCA;
            }
        }
        private byte _FreeCA;

        public EpgEventInfo()
        {
            ONID = 0;
            TSID = 0;
            SID = 0;
            EventID = 0;
            IsStartTime = 0;
            StartTime = new DateTime();
            IsDuration = 0;
            DurationSec = 0;
            ShortInfo = null;
            ExtInfo = null;
            ContentInfo = null;
            ComponentInfo = null;
            AudioInfo = null;
            EventGroupInfo = null;
            EventRelayInfo = null;
            FreeCA = 0;
        }
        public void Write(MemoryStream s, ushort version)
        {
            var w = new CtrlCmdWriter(s, version);
            w.Begin();
            w.Write(ONID);
            w.Write(TSID);
            w.Write(SID);
            w.Write(EventID);
            w.Write(IsStartTime);
            w.Write(StartTime);
            w.Write(IsDuration);
            w.Write(DurationSec);
            if (ShortInfo != null) w.Write(ShortInfo); else w.Write(4);
            if (ExtInfo != null) w.Write(ExtInfo); else w.Write(4);
            if (ContentInfo != null) w.Write(ContentInfo); else w.Write(4);
            if (ComponentInfo != null) w.Write(ComponentInfo); else w.Write(4);
            if (AudioInfo != null) w.Write(AudioInfo); else w.Write(4);
            if (EventGroupInfo != null) w.Write(EventGroupInfo); else w.Write(4);
            if (EventRelayInfo != null) w.Write(EventRelayInfo); else w.Write(4);
            w.Write(FreeCA);
            w.End();
        }
        public void Read(MemoryStream s, ushort version)
        {
            var r = new CtrlCmdReader(s, version);
            r.Begin();
            r.Read(ref _ONID);
            r.Read(ref _TSID);
            r.Read(ref _SID);
            r.Read(ref _EventID);
            r.Read(ref _IsStartTime);
            try
            {
                r.Read(ref _StartTime);
            }
            catch (ArgumentOutOfRangeException)
            {
            }
            r.Read(ref _IsDuration);
            r.Read(ref _DurationSec);
            int size = 0;
            ShortInfo = null;
            r.Read(ref size);
            if (size != 4)
            {
                r.Stream.Seek(-4, SeekOrigin.Current);
                ShortInfo = new EpgShortEventInfo();
                r.Read(ref _ShortInfo);
            }
            ExtInfo = null;
            r.Read(ref size);
            if (size != 4)
            {
                r.Stream.Seek(-4, SeekOrigin.Current);
                ExtInfo = new EpgExtendedEventInfo();
                r.Read(ref _ExtInfo);
            }
            ContentInfo = null;
            r.Read(ref size);
            if (size != 4)
            {
                r.Stream.Seek(-4, SeekOrigin.Current);
                ContentInfo = new EpgContentInfo();
                r.Read(ref _ContentInfo);
            }
            ComponentInfo = null;
            r.Read(ref size);
            if (size != 4)
            {
                r.Stream.Seek(-4, SeekOrigin.Current);
                ComponentInfo = new EpgComponentInfo();
                r.Read(ref _ComponentInfo);
            }
            AudioInfo = null;
            r.Read(ref size);
            if (size != 4)
            {
                r.Stream.Seek(-4, SeekOrigin.Current);
                AudioInfo = new EpgAudioComponentInfo();
                r.Read(ref _AudioInfo);
            }
            EventGroupInfo = null;
            r.Read(ref size);
            if (size != 4)
            {
                r.Stream.Seek(-4, SeekOrigin.Current);
                EventGroupInfo = new EpgEventGroupInfo();
                r.Read(ref _EventGroupInfo);
            }
            EventRelayInfo = null;
            r.Read(ref size);
            if (size != 4)
            {
                r.Stream.Seek(-4, SeekOrigin.Current);
                EventRelayInfo = new EpgEventGroupInfo();
                r.Read(ref _EventRelayInfo);
            }
            r.Read(ref _FreeCA);
            r.End();
        }
    }

    public class EpgServiceInfo : ICtrlCmdReadWrite
    {
        public ushort ONID
        {
            set
            {
                _ONID = value;
            }
            get
            {
                return _ONID;
            }
        }
        private ushort _ONID;

        public ushort TSID
        {
            set
            {
                _TSID = value;
            }
            get
            {
                return _TSID;
            }
        }
        private ushort _TSID;

        public ushort SID
        {
            set
            {
                _SID = value;
            }
            get
            {
                return _SID;
            }
        }
        private ushort _SID;

        public byte ServiceType
        {
            set
            {
                _ServiceType = value;
            }
            get
            {
                return _ServiceType;
            }
        }
        private byte _ServiceType;

        public byte IsPartialReception
        {
            set
            {
                _IsPartialReception = value;
            }
            get
            {
                return _IsPartialReception;
            }
        }
        private byte _IsPartialReception;

        public string ServiceProviderName
        {
            set
            {
                _ServiceProviderName = value;
            }
            get
            {
                return _ServiceProviderName;
            }
        }
        private string _ServiceProviderName;

        public string ServiceName
        {
            set
            {
                _ServiceName = value;
            }
            get
            {
                return _ServiceName;
            }
        }
        private string _ServiceName;

        public string NetworkName
        {
            set
            {
                _NetworkName = value;
            }
            get
            {
                return _NetworkName;
            }
        }
        private string _NetworkName;

        public string TsName
        {
            set
            {
                _TsName = value;
            }
            get
            {
                return _TsName;
            }
        }
        private string _TsName;

        public byte RemoconID
        {
            set
            {
                _RemoconID = value;
            }
            get
            {
                return _RemoconID;
            }
        }
        private byte _RemoconID;

        public EpgServiceInfo()
        {
            ONID = 0;
            TSID = 0;
            SID = 0;
            ServiceType = 0;
            IsPartialReception = 0;
            ServiceProviderName = "";
            ServiceName = "";
            NetworkName = "";
            TsName = "";
            RemoconID = 0;
        }
        public void Write(MemoryStream s, ushort version)
        {
            var w = new CtrlCmdWriter(s, version);
            w.Begin();
            w.Write(ONID);
            w.Write(TSID);
            w.Write(SID);
            w.Write(ServiceType);
            w.Write(IsPartialReception);
            w.Write(ServiceProviderName);
            w.Write(ServiceName);
            w.Write(NetworkName);
            w.Write(TsName);
            w.Write(RemoconID);
            w.End();
        }
        public void Read(MemoryStream s, ushort version)
        {
            var r = new CtrlCmdReader(s, version);
            r.Begin();
            r.Read(ref _ONID);
            r.Read(ref _TSID);
            r.Read(ref _SID);
            r.Read(ref _ServiceType);
            r.Read(ref _IsPartialReception);
            r.Read(ref _ServiceProviderName);
            r.Read(ref _ServiceName);
            r.Read(ref _NetworkName);
            r.Read(ref _TsName);
            r.Read(ref _RemoconID);
            r.End();
        }
    }

    public class EpgServiceEventInfo : ICtrlCmdReadWrite
    {
        public EpgServiceInfo ServiceInfo
        {
            set
            {
                _ServiceInfo = value;
            }
            get
            {
                return _ServiceInfo;
            }
        }
        private EpgServiceInfo _ServiceInfo;

        public List<EpgEventInfo> EventList
        {
            set
            {
                _EventList = value;
            }
            get
            {
                return _EventList;
            }
        }
        private List<EpgEventInfo> _EventList;

        public EpgServiceEventInfo()
        {
            ServiceInfo = new EpgServiceInfo();
            EventList = new List<EpgEventInfo>();
        }
        public void Write(MemoryStream s, ushort version)
        {
            var w = new CtrlCmdWriter(s, version);
            w.Begin();
            w.Write(ServiceInfo);
            w.Write(EventList);
            w.End();
        }
        public void Read(MemoryStream s, ushort version)
        {
            var r = new CtrlCmdReader(s, version);
            r.Begin();
            r.Read(ref _ServiceInfo);
            r.Read(ref _EventList);
            r.End();
        }
    }

    public class EpgSearchDateInfo : ICtrlCmdReadWrite
    {
        public byte StartDayOfWeek
        {
            set
            {
                _StartDayOfWeek = value;
            }
            get
            {
                return _StartDayOfWeek;
            }
        }
        private byte _StartDayOfWeek;

        public ushort StartHour
        {
            set
            {
                _StartHour = value;
            }
            get
            {
                return _StartHour;
            }
        }
        private ushort _StartHour;

        public ushort StartMin
        {
            set
            {
                _StartMin = value;
            }
            get
            {
                return _StartMin;
            }
        }
        private ushort _StartMin;

        public byte EndDayOfWeek
        {
            set
            {
                _EndDayOfWeek = value;
            }
            get
            {
                return _EndDayOfWeek;
            }
        }
        private byte _EndDayOfWeek;

        public ushort EndHour
        {
            set
            {
                _EndHour = value;
            }
            get
            {
                return _EndHour;
            }
        }
        private ushort _EndHour;

        public ushort EndMin
        {
            set
            {
                _EndMin = value;
            }
            get
            {
                return _EndMin;
            }
        }
        private ushort _EndMin;

        public EpgSearchDateInfo()
        {
            StartDayOfWeek = 0;
            StartHour = 0;
            StartMin = 0;
            EndDayOfWeek = 0;
            EndHour = 0;
            EndMin = 0;
        }
        public void Write(MemoryStream s, ushort version)
        {
            var w = new CtrlCmdWriter(s, version);
            w.Begin();
            w.Write(StartDayOfWeek);
            w.Write(StartHour);
            w.Write(StartMin);
            w.Write(EndDayOfWeek);
            w.Write(EndHour);
            w.Write(EndMin);
            w.End();
        }
        public void Read(MemoryStream s, ushort version)
        {
            var r = new CtrlCmdReader(s, version);
            r.Begin();
            r.Read(ref _StartDayOfWeek);
            r.Read(ref _StartHour);
            r.Read(ref _StartMin);
            r.Read(ref _EndDayOfWeek);
            r.Read(ref _EndHour);
            r.Read(ref _EndMin);
            r.End();
        }
    }

    /// <summary>検索条件</summary>
    public class EpgSearchKeyInfo : ICtrlCmdReadWrite
    {
        public string AndKey
        {
            set
            {
                _AndKey = value;
            }
            get
            {
                return _AndKey;
            }
        }
        private string _AndKey;

        public string NotKey
        {
            set
            {
                _NotKey = value;
            }
            get
            {
                return _NotKey;
            }
        }
        private string _NotKey;

        public int IsRegExp
        {
            set
            {
                _IsRegExp = value;
            }
            get
            {
                return _IsRegExp;
            }
        }
        private int _IsRegExp;

        public int IsTitleOnly
        {
            set
            {
                _IsTitleOnly = value;
            }
            get
            {
                return _IsTitleOnly;
            }
        }
        private int _IsTitleOnly;

        public List<EpgContentData> ContentList
        {
            set
            {
                _ContentList = value;
            }
            get
            {
                return _ContentList;
            }
        }
        private List<EpgContentData> _ContentList;

        public List<EpgSearchDateInfo> DateList
        {
            set
            {
                _DateList = value;
            }
            get
            {
                return _DateList;
            }
        }
        private List<EpgSearchDateInfo> _DateList;

        public List<long> ServiceList
        {
            set
            {
                _ServiceList = value;
            }
            get
            {
                return _ServiceList;
            }
        }
        private List<long> _ServiceList;

        public List<ushort> VideoList
        {
            set
            {
                _VideoList = value;
            }
            get
            {
                return _VideoList;
            }
        }
        private List<ushort> _VideoList;

        public List<ushort> AudioList
        {
            set
            {
                _AudioList = value;
            }
            get
            {
                return _AudioList;
            }
        }
        private List<ushort> _AudioList;

        public byte IsAimai
        {
            set
            {
                _IsAimai = value;
            }
            get
            {
                return _IsAimai;
            }
        }
        private byte _IsAimai;

        public byte IsNotContent
        {
            set
            {
                _IsNotContent = value;
            }
            get
            {
                return _IsNotContent;
            }
        }
        private byte _IsNotContent;

        public byte IsNotDate
        {
            set
            {
                _IsNotDate = value;
            }
            get
            {
                return _IsNotDate;
            }
        }
        private byte _IsNotDate;

        public byte FreeCA
        {
            set
            {
                _FreeCA = value;
            }
            get
            {
                return _FreeCA;
            }
        }
        private byte _FreeCA;

        /// <summary>(自動予約登録の条件専用)録画済かのチェックあり</summary>
        public byte ChkRecEnd
        {
            set
            {
                _ChkRecEnd = value;
            }
            get
            {
                return _ChkRecEnd;
            }
        }
        private byte _ChkRecEnd;

        /// <summary>(自動予約登録の条件専用)録画済かのチェック対象期間</summary>
        public ushort ChkRecDay
        {
            set
            {
                _ChkRecDay = value;
            }
            get
            {
                return _ChkRecDay;
            }
        }
        private ushort _ChkRecDay;

        public EpgSearchKeyInfo()
        {
            AndKey = "";
            NotKey = "";
            IsRegExp = 0;
            IsTitleOnly = 0;
            ContentList = new List<EpgContentData>();
            DateList = new List<EpgSearchDateInfo>();
            ServiceList = new List<long>();
            VideoList = new List<ushort>();
            AudioList = new List<ushort>();
            IsAimai = 0;
            IsNotContent = 0;
            IsNotDate = 0;
            FreeCA = 0;
            ChkRecEnd = 0;
            ChkRecDay = 6;
        }
        public void Write(MemoryStream s, ushort version)
        {
            var w = new CtrlCmdWriter(s, version);
            w.Begin();
            w.Write(AndKey);
            w.Write(NotKey);
            w.Write(IsRegExp);
            w.Write(IsTitleOnly);
            w.Write(ContentList);
            w.Write(DateList);
            w.Write(ServiceList);
            w.Write(VideoList);
            w.Write(AudioList);
            w.Write(IsAimai);
            w.Write(IsNotContent);
            w.Write(IsNotDate);
            w.Write(FreeCA);
            if (version >= 3)
            {
                w.Write(ChkRecEnd);
                w.Write(ChkRecDay);
            }
            w.End();
        }
        public void Read(MemoryStream s, ushort version)
        {
            var r = new CtrlCmdReader(s, version);
            r.Begin();
            r.Read(ref _AndKey);
            r.Read(ref _NotKey);
            r.Read(ref _IsRegExp);
            r.Read(ref _IsTitleOnly);
            r.Read(ref _ContentList);
            r.Read(ref _DateList);
            r.Read(ref _ServiceList);
            r.Read(ref _VideoList);
            r.Read(ref _AudioList);
            r.Read(ref _IsAimai);
            r.Read(ref _IsNotContent);
            r.Read(ref _IsNotDate);
            r.Read(ref _FreeCA);
            if (version >= 3)
            {
                r.Read(ref _ChkRecEnd);
                r.Read(ref _ChkRecDay);
            }
            r.End();
        }
    }

    /// <summary>自動予約登録情報</summary>
    public class EpgAutoAddData : ICtrlCmdReadWrite
    {
        public uint ID
        {
            set
            {
                _ID = value;
            }
            get
            {
                return _ID;
            }
        }
        private uint _ID;

        /// <summary>検索キー</summary>
        public EpgSearchKeyInfo Search
        {
            set
            {
                _Search = value;
            }
            get
            {
                return _Search;
            }
        }
        private EpgSearchKeyInfo _Search;

        /// <summary>録画設定</summary>
        public RecSettingData Setting
        {
            set
            {
                _Setting = value;
            }
            get
            {
                return _Setting;
            }
        }
        private RecSettingData _Setting;

        /// <summary>予約登録数</summary>
        public uint Count
        {
            set
            {
                _Count = value;
            }
            get
            {
                return _Count;
            }
        }
        private uint _Count;

        public EpgAutoAddData()
        {
            ID = 0;
            Search = new EpgSearchKeyInfo();
            Setting = new RecSettingData();
            Count = 0;
        }
        public void Write(MemoryStream s, ushort version)
        {
            var w = new CtrlCmdWriter(s, version);
            w.Begin();
            w.Write(ID);
            w.Write(Search);
            w.Write(Setting);
            if (version >= 5)
            {
                w.Write(Count);
            }
            w.End();
        }
        public void Read(MemoryStream s, ushort version)
        {
            var r = new CtrlCmdReader(s, version);
            r.Begin();
            r.Read(ref _ID);
            r.Read(ref _Search);
            r.Read(ref _Setting);
            if (version >= 5)
            {
                r.Read(ref _Count);
            }
            r.End();
        }
    }

    public class ManualAutoAddData : ICtrlCmdReadWrite
    {
        public uint ID
        {
            set
            {
                _ID = value;
            }
            get
            {
                return _ID;
            }
        }
        private uint _ID;

        /// <summary>対象曜日</summary>
        public byte DayOfWeek
        {
            set
            {
                _DayOfWeek = value;
            }
            get
            {
                return _DayOfWeek;
            }
        }
        private byte _DayOfWeek;

        /// <summary>録画開始時間（00:00を0として秒単位）</summary>
        public uint StartTime
        {
            set
            {
                _StartTime = value;
            }
            get
            {
                return _StartTime;
            }
        }
        private uint _StartTime;

        /// <summary>録画総時間</summary>
        public uint DurationSecond
        {
            set
            {
                _DurationSecond = value;
            }
            get
            {
                return _DurationSecond;
            }
        }
        private uint _DurationSecond;

        /// <summary>番組名</summary>
        public string Title
        {
            set
            {
                _Title = value;
            }
            get
            {
                return _Title;
            }
        }
        private string _Title;

        /// <summary>サービス名</summary>
        public string StationName
        {
            set
            {
                _StationName = value;
            }
            get
            {
                return _StationName;
            }
        }
        private string _StationName;

        /// <summary>ONID</summary>
        public ushort ONID
        {
            set
            {
                _ONID = value;
            }
            get
            {
                return _ONID;
            }
        }
        private ushort _ONID;

        /// <summary>TSID</summary>
        public ushort TSID
        {
            set
            {
                _TSID = value;
            }
            get
            {
                return _TSID;
            }
        }
        private ushort _TSID;

        /// <summary>SID</summary>
        public ushort SID
        {
            set
            {
                _SID = value;
            }
            get
            {
                return _SID;
            }
        }
        private ushort _SID;

        /// <summary>録画設定</summary>
        public RecSettingData Setting
        {
            set
            {
                _Setting = value;
            }
            get
            {
                return _Setting;
            }
        }
        private RecSettingData _Setting;

        public ManualAutoAddData()
        {
            ID = 0;
            DayOfWeek = 0;
            StartTime = 0;
            DurationSecond = 0;
            Title = "";
            StationName = "";
            ONID = 0;
            TSID = 0;
            SID = 0;
            Setting = new RecSettingData();
        }
        public void Write(MemoryStream s, ushort version)
        {
            var w = new CtrlCmdWriter(s, version);
            w.Begin();
            w.Write(ID);
            w.Write(DayOfWeek);
            w.Write(StartTime);
            w.Write(DurationSecond);
            w.Write(Title);
            w.Write(StationName);
            w.Write(ONID);
            w.Write(TSID);
            w.Write(SID);
            w.Write(Setting);
            w.End();
        }
        public void Read(MemoryStream s, ushort version)
        {
            var r = new CtrlCmdReader(s, version);
            r.Begin();
            r.Read(ref _ID);
            r.Read(ref _DayOfWeek);
            r.Read(ref _StartTime);
            r.Read(ref _DurationSecond);
            r.Read(ref _Title);
            r.Read(ref _StationName);
            r.Read(ref _ONID);
            r.Read(ref _TSID);
            r.Read(ref _SID);
            r.Read(ref _Setting);
            r.End();
        }
    }

    /// <summary>チャンネル変更情報</summary>
    public class SetChInfo : ICtrlCmdReadWrite
    {
        /// <summary>wONIDとwTSIDとwSIDの値が使用できるかどうか</summary>
        public int useSID
        {
            set
            {
                _useSID = value;
            }
            get
            {
                return _useSID;
            }
        }
        private int _useSID;

        public ushort ONID
        {
            set
            {
                _ONID = value;
            }
            get
            {
                return _ONID;
            }
        }
        private ushort _ONID;

        public ushort TSID
        {
            set
            {
                _TSID = value;
            }
            get
            {
                return _TSID;
            }
        }
        private ushort _TSID;

        public ushort SID
        {
            set
            {
                _SID = value;
            }
            get
            {
                return _SID;
            }
        }
        private ushort _SID;

        /// <summary>dwSpaceとdwChの値が使用できるかどうか</summary>
        public int useBonCh
        {
            set
            {
                _useBonCh = value;
            }
            get
            {
                return _useBonCh;
            }
        }
        private int _useBonCh;

        public uint space
        {
            set
            {
                _space = value;
            }
            get
            {
                return _space;
            }
        }
        private uint _space;

        public uint ch
        {
            set
            {
                _ch = value;
            }
            get
            {
                return _ch;
            }
        }
        private uint _ch;

        public SetChInfo()
        {
            useSID = 0;
            ONID = 0;
            TSID = 0;
            SID = 0;
            useBonCh = 0;
            space = 0;
            ch = 0;
        }
        public void Write(MemoryStream s, ushort version)
        {
            var w = new CtrlCmdWriter(s, version);
            w.Begin();
            w.Write(useSID);
            w.Write(ONID);
            w.Write(TSID);
            w.Write(SID);
            w.Write(useBonCh);
            w.Write(space);
            w.Write(ch);
            w.End();
        }
        public void Read(MemoryStream s, ushort version)
        {
            var r = new CtrlCmdReader(s, version);
            r.Begin();
            r.Read(ref _useSID);
            r.Read(ref _ONID);
            r.Read(ref _TSID);
            r.Read(ref _SID);
            r.Read(ref _useBonCh);
            r.Read(ref _space);
            r.Read(ref _ch);
            r.End();
        }
    }

    public class TvTestChChgInfo : ICtrlCmdReadWrite
    {
        public string bonDriver
        {
            set
            {
                _bonDriver = value;
            }
            get
            {
                return _bonDriver;
            }
        }
        private string _bonDriver;

        public SetChInfo chInfo
        {
            set
            {
                _chInfo = value;
            }
            get
            {
                return _chInfo;
            }
        }
        private SetChInfo _chInfo;

        public TvTestChChgInfo()
        {
            bonDriver = "";
            chInfo = new SetChInfo();
        }
        public void Write(MemoryStream s, ushort version)
        {
            var w = new CtrlCmdWriter(s, version);
            w.Begin();
            w.Write(bonDriver);
            w.Write(chInfo);
            w.End();
        }
        public void Read(MemoryStream s, ushort version)
        {
            var r = new CtrlCmdReader(s, version);
            r.Begin();
            r.Read(ref _bonDriver);
            r.Read(ref _chInfo);
            r.End();
        }
    }

    public class NWPlayTimeShiftInfo : ICtrlCmdReadWrite
    {
        public uint ctrlID
        {
            set
            {
                _ctrlID = value;
            }
            get
            {
                return _ctrlID;
            }
        }
        private uint _ctrlID;

        public string filePath
        {
            set
            {
                _filePath = value;
            }
            get
            {
                return _filePath;
            }
        }
        private string _filePath;

        public NWPlayTimeShiftInfo()
        {
            ctrlID = 0;
            filePath = "";
        }
        public void Write(MemoryStream s, ushort version)
        {
            var w = new CtrlCmdWriter(s, version);
            w.Begin();
            w.Write(ctrlID);
            w.Write(filePath);
            w.End();
        }
        public void Read(MemoryStream s, ushort version)
        {
            var r = new CtrlCmdReader(s, version);
            r.Begin();
            r.Read(ref _ctrlID);
            r.Read(ref _filePath);
            r.End();
        }
    }

    /// <summary>情報通知用パラメーター</summary>
    public class NotifySrvInfo : ICtrlCmdReadWrite
    {
        /// <summary>通知情報の種類</summary>
        public uint notifyID
        {
            set
            {
                _notifyID = value;
            }
            get
            {
                return _notifyID;
            }
        }
        private uint _notifyID;

        /// <summary>通知状態の発生した時間</summary>
        public DateTime time
        {
            set
            {
                _time = value;
            }
            get
            {
                return _time;
            }
        }
        private DateTime _time;

        /// <summary>パラメーター１（種類によって内容変更）</summary>
        public uint param1
        {
            set
            {
                _param1 = value;
            }
            get
            {
                return _param1;
            }
        }
        private uint _param1;

        /// <summary>パラメーター２（種類によって内容変更）</summary>
        public uint param2
        {
            set
            {
                _param2 = value;
            }
            get
            {
                return _param2;
            }
        }
        private uint _param2;

        /// <summary>パラメーター３（種類によって内容変更）</summary>
        public uint param3
        {
            set
            {
                _param3 = value;
            }
            get
            {
                return _param3;
            }
        }
        private uint _param3;

        /// <summary>パラメーター４（種類によって内容変更）</summary>
        public string param4
        {
            set
            {
                _param4 = value;
            }
            get
            {
                return _param4;
            }
        }
        private string _param4;

        /// <summary>パラメーター５（種類によって内容変更）</summary>
        public string param5
        {
            set
            {
                _param5 = value;
            }
            get
            {
                return _param5;
            }
        }
        private string _param5;

        /// <summary>パラメーター６（種類によって内容変更）</summary>
        public string param6
        {
            set
            {
                _param6 = value;
            }
            get
            {
                return _param6;
            }
        }
        private string _param6;

        public NotifySrvInfo()
        {
            notifyID = 0;
            time = new DateTime();
            param1 = 0;
            param2 = 0;
            param3 = 0;
            param4 = "";
            param5 = "";
            param6 = "";
        }
        public void Write(MemoryStream s, ushort version)
        {
            var w = new CtrlCmdWriter(s, version);
            w.Begin();
            w.Write(notifyID);
            w.Write(time);
            w.Write(param1);
            w.Write(param2);
            w.Write(param3);
            w.Write(param4);
            w.Write(param5);
            w.Write(param6);
            w.End();
        }
        public void Read(MemoryStream s, ushort version)
        {
            var r = new CtrlCmdReader(s, version);
            r.Begin();
            r.Read(ref _notifyID);
            r.Read(ref _time);
            r.Read(ref _param1);
            r.Read(ref _param2);
            r.Read(ref _param3);
            r.Read(ref _param4);
            r.Read(ref _param5);
            r.Read(ref _param6);
            r.End();
        }
    }
}