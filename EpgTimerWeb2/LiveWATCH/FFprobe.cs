using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace EpgTimerWeb2.LiveWATCH
{
    public class FFprobe
    {
        public static string FFprobePath { set; get; }
        public class Format
        {
            public int StreamCount { set; get; }
            public int ProgramCount { set; get; }
            public string FormatName { set; get; }
            public string FormatLongName { set; get; }
            public double Duration { set; get; }
            public long Bitrate { set; get; }
            public long Size { set; get; }
            public Format()
            {
                StreamCount = -1;
                ProgramCount = -1;
                FormatName = "";
                FormatLongName = "";
                Duration = 0;
                Bitrate = 0;
                Size = 0;
            }
        }
        public static FFprobe.Format ProbeFormat(string FileName)
        {
            if (String.IsNullOrWhiteSpace(FFprobePath)) FFprobePath = "ffprobe";
            FFprobe.Format Format = new FFprobe.Format();
            ProcessStartInfo StartInfo = new ProcessStartInfo(FFprobePath,
                String.Format("-v 0 -of flat -show_format \"{0}\"", FileName))
                {
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    WorkingDirectory = Environment.CurrentDirectory
                };
            Process ProbeProcess = Process.Start(StartInfo);
            ProbeProcess.ErrorDataReceived += (s, e) =>
            {
                Debug.Print("[FFprobe] {0}\n", e.Data);
            };
            ProbeProcess.WaitForExit(5000);
            if (!ProbeProcess.HasExited)
            {
                ProbeProcess.Kill();
                throw new TimeoutException("FFprobe process timed out.");
            }
            string LineBuffer = "";
            bool HasData = false;
            while ((LineBuffer = ProbeProcess.StandardOutput.ReadLine()) != null)
            {
                if (!LineBuffer.StartsWith("format.")) continue;
                string Name = LineBuffer.Substring(0, LineBuffer.IndexOf("="));
                string Value = LineBuffer.Substring(LineBuffer.IndexOf("=") + 1);
                if (Value.StartsWith("\"") && Value.EndsWith("\""))
                {
                    Value = Value.Substring(1, Value.Length - 2);
                }
                try
                {
                    if (Name == "format.nb_streams") Format.StreamCount = int.Parse(Value);
                    else if (Name == "format.nb_programs") Format.ProgramCount = int.Parse(Value);
                    else if (Name == "format.format_name") Format.FormatName = Value;
                    else if (Name == "format.format_long_name") Format.FormatLongName = Value;
                    else if (Name == "format.duration") Format.Duration = double.Parse(Value);
                    else if (Name == "format.size") Format.Size = long.Parse(Value);
                    else if (Name == "format.bit_rate") Format.Bitrate = long.Parse(Value);
                    HasData = true;
                }
                catch (Exception ex)
                {
                    Debug.Print(ex.Message);
                }
                Debug.Print("{0}: {1}", Name, Value);
            }
            ProbeProcess.Close();
            if (!HasData) throw new FormatException("FFprobe does not response");
            return Format;
        }
        private static Regex KbitRegex = new Regex("^([0-9]+)k$", RegexOptions.IgnoreCase);
        private static Regex MbitRegex = new Regex("^([0-9]+)m$", RegexOptions.IgnoreCase);
        public static long ConvertBitrateString(string str)
        {
            if (MbitRegex.IsMatch(str))
            {
                return long.Parse(MbitRegex.Match(str).Groups[1].Value) * 1024 * 1024;
            }
            else if(KbitRegex.IsMatch(str))
            {
                return long.Parse(KbitRegex.Match(str).Groups[1].Value) * 1024;
            }
            throw new FormatException();
        }
    }
}
