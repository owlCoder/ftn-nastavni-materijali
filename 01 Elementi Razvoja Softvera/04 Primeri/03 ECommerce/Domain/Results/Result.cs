using System;
using System.Collections.Generic;
using System.Linq;

namespace Domain.Results
{
    public class Result
    {
        private ResultStatus resultStatus;
        private List<Tuple<LogLevel, String>> CustomerLogs { get; set; }
        public ResultStatus ResultStatus { get => resultStatus; set => resultStatus = MakeResultStatus(value); }

        public object ResultObject { get; set; }

        public bool Success { get => IsSuccess(); }

        public Result()
        {
            CustomerLogs = new List<Tuple<LogLevel, String>>();
            resultStatus = ResultStatus.Success;
            ResultObject = new object();
        }

        public void Log(LogLevel level, string log)
        {
            CustomerLogs.Add(new Tuple<LogLevel, String>(level, log));
            this.ResultStatus = LogLevelToResultStatus(level);
        }

        public void Log(string log)
        {
            Log(LogLevel.Info, log);
        }

        public void ClearLogs()
        {
            CustomerLogs.Clear();
        }

        public string GetMessage()
        {
            string s = string.Empty;
            CustomerLogs.ForEach(t => s = s + t.Item2 + System.Environment.NewLine);
            s = s.TrimEnd();
            return s;
        }

        public string GetErrorMessage()
        {
            string s = string.Empty;
            CustomerLogs.Where(t => t.Item1 == LogLevel.Error).ToList().ForEach(t => s = s + t.Item2 + System.Environment.NewLine);
            s = s.TrimEnd();
            return s;
        }

        private ResultStatus MakeResultStatus(ResultStatus status)
        {
            if (status > this.resultStatus)
            {
                return status;
            }
            return this.resultStatus;
        }

        private ResultStatus LogLevelToResultStatus(LogLevel level)
        {
            return level switch
            {
                LogLevel.Error => ResultStatus.Error,
                LogLevel.Warning => ResultStatus.Warning,
                LogLevel.Verbose => ResultStatus.Success,
                LogLevel.Info => ResultStatus.Success,
                _ => ResultStatus.Success,
            };
        }

        private bool IsSuccess()
        {
            return resultStatus < ResultStatus.Error;
        }
    }
}
