using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace StartCease
{
    public class IniFile
    {
        [DllImport("kernel32")]
        private static extern bool WritePrivateProfileString(string section, string key, string val, string filePath);
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, byte[] retVal, int size, string filePath);

        private string _filePath;

        public IniFile(string filePath)
        {
            _filePath = filePath;
        }

        public bool WriteToFile(string section, string key, string val)
        {
            return WritePrivateProfileString(section, key, val, _filePath);
        }

        public string ReadFromFile(string section, string key, string def)
        {
            string res = "";
            Byte[] Buffer = new Byte[1024];
            int bufLen = GetPrivateProfileString(section, key, def, Buffer, 1024, _filePath);
            res = Encoding.GetEncoding(0).GetString(Buffer);
            res = res.Substring(0, bufLen).Trim();
            return res;
        }
    }
}
