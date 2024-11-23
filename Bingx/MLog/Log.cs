using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Bingx.MLog
{
    public class Log : ILogRepository
    {
        public DateTime Date { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public string Status { get; set; }


        private string FileName { get; set; }
        public Log()
        {
            FileName = Directory.GetCurrentDirectory() + "/LogFile.txt";
        }
        public  bool Add(Log entity)
        {
            try
            {
                var data = new StringBuilder();
                using (StreamWriter writer = new StreamWriter(FileName, true))
                {
                    data.Append(entity.Date.ToString() + "|*|" + entity.Title + "|*|" + entity.Message + "|*|" + entity.Status);
                    writer.WriteLine(data.ToString());
                }

                if (DataFullStatus())
                {
                    Remove();
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        private bool DataFullStatus()
        {
            if (File.ReadAllLines(FileName).Count() > 3000) return true;
            return false;
        }
        public IEnumerable<Log> GetAll()
        {
            var data = File.ReadAllLines(FileName);
            var lstLog = new List<Log>();
            foreach (var item in data)
            {
                Log log = new Log();
                item.Split("|*|");
                log.Date = DateTime.Parse(item.Split("|*|")[0].ToString());
                log.Title = item.Split("|*|")[1].ToString();
                log.Message = item.Split("|*|")[2].ToString();
                log.Status = item.Split("|*|")[3].ToString();
                lstLog.Add(log);
            }

            return lstLog;
        }
        public IEnumerable<Log> FilterLog(string status, DateTime? fDate, DateTime? eDate, string message, string title)
        {
            var data = File.ReadAllLines(FileName);
            var lstLog = new List<Log>();
            foreach (var item in data)
            {
                Log log = new Log();
                item.Split("|!|");
                log.Date = DateTime.Parse(item.Split("|*|")[0].ToString());
                log.Title = item.Split("|*|")[1].ToString();
                log.Message = item.Split("|*|")[2].ToString();
                log.Status = item.Split("|*|")[3].ToString();
                lstLog.Add(log);
            }

            if (fDate != null)
                lstLog = lstLog.Where(c => c.Date >= fDate).ToList();
            if (eDate != null)
                lstLog = lstLog.Where(c => c.Date <= fDate).ToList();
            if (status != "")
                lstLog = lstLog.Where(c => c.Status == status).ToList();
            if (title != "")
                lstLog = lstLog.Where(c => c.Title.Contains(title)).ToList();
            if (message != "")
                lstLog = lstLog.Where(c => c.Message.Contains(title)).ToList();

            return lstLog;
        }
        private void Remove()
        {
            var data = File.ReadAllLines(FileName).ToList();
            data.RemoveAt(0);

            using (StreamWriter writer = new StreamWriter(FileName))
                writer.WriteLine("");

            using (StreamWriter writer = new StreamWriter(FileName, true))
            {
                for (int i = 0; i < data.Count(); i++)
                {
                    writer.WriteLine(data[i].ToString());
                }
            }
        }
    }
}
