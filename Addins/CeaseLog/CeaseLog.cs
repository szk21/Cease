using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using System.ComponentModel.Composition;
using Cease.Addins.Log;

namespace CeaseLog
{
    [Export(typeof(InterfaceLog))]
    [ExportMetadata("AddinName", "CeaseLog")]
    public class CeaseLog : InterfaceLog
    {
        private int m_iDut;
        private string lastErrMsg;
        private List<KeyValuePair<LOG_TYPE, string>> ListContent;

        public event EventHandler Logged; //声明事件

        protected virtual void OnLogged(LoggedEventArgs e)
        {
            if (Logged != null)
            { // 如果有对象注册
                Logged(this, e);  // 调用所有注册对象的方法
            }
        }

        private void PushMsg(LOG_TYPE _type, string _msg)
        {
            string strType = Enum.GetName(_type.GetType(), _type);
            var _pair = new KeyValuePair<LOG_TYPE, string>(_type, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + ", " + strType + "\t" + _msg + "\n");
            ListContent.Add(_pair);

            OnLogged(new LoggedEventArgs(m_iDut, _type, _msg));
        }

        private bool InValid()
        {
            return ListContent == null;
        }

        public void SetDut(int _dut)
        {
            this.m_iDut = _dut;
        }

        public void InitialLog(string _filePath)
        {
            DateTime dt = DateTime.Now;
            string ShortDate = dt.ToString("yyyy-MM-dd");
            string LocalTime = dt.ToString("yyyy-MM-dd HH.mm.ss");

            lastErrMsg = "";
            string FilePath = _filePath + "\\" + ShortDate;

            if (!Directory.Exists(FilePath))
            {
                Directory.CreateDirectory(FilePath);
            }

            if (ListContent == null)
            {
                ListContent = new List<KeyValuePair<LOG_TYPE, string>>();
            }
            ListContent.Clear();
        }

        public void msg(string strMessage)
        {
            if (InValid())
            {
                return;
            }
            PushMsg(LOG_TYPE.DBG, strMessage);
        }

        public void warn(string strMessage)
        {
            if (InValid())
            {
                return;
            }
            PushMsg(LOG_TYPE.WARN, strMessage);
        }

        public void err(string strMessage)
        {
            lastErrMsg = strMessage;
            if (InValid())
            {
                return;
            }
            PushMsg(LOG_TYPE.ERR, strMessage);
        }

        public void err(string strMessage, Exception exp)
        {
            lastErrMsg = strMessage;
            if (InValid())
            {
                return;
            }
            PushMsg(LOG_TYPE.ERR, strMessage);
        }

        public string GetLastErrMsg()
        {
            return lastErrMsg;
        }

        public void WirteLogToFile(int DutNum, string _station, string _path, bool _bRes, string ShortDate, string LocalTime)
        {
            if (ListContent.Count == 0)
            {
                return;
            }

            string path = _path + "\\DUT" + DutNum.ToString() + "\\" + ShortDate + (_bRes ? "\\PASS" : "\\FAIL");

            //check directory
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            path += $"\\{_station}_{LocalTime}.log";

            var fs = new FileStream(path, FileMode.Append, FileAccess.Write);
            var sw = new StreamWriter(fs, Encoding.Unicode);

            //write log
            foreach (KeyValuePair<LOG_TYPE, string> s in ListContent)
            {
                sw.Write(s.Value);
            }

            //flush
            sw.Close();
        }
    }
}
