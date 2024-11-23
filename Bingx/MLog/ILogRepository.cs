using System;
using System.Collections.Generic;
using System.Text;

namespace Bingx.MLog
{
    interface ILogRepository
    {
        bool Add(Log entity);
        IEnumerable<Log> GetAll();
        IEnumerable<Log> FilterLog(string status, DateTime? fDate, DateTime? eDate, string message, string title);
    }
}
