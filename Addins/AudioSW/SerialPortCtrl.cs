using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;

namespace AudioSW
{
    class SerialPortCtrl
    {
        private static SerialPort ComDevice = new SerialPort();
        private static string sLastReceive="";

        public static bool SendCommand(string _sPort, string _sCmd,int _iTimeout,ref string _sReturnMsg)
        {
            if (ComDevice.IsOpen == false)
            {
                ComDevice.PortName = _sPort;
                ComDevice.BaudRate = 9600;
                ComDevice.Parity = System.IO.Ports.Parity.None;
                ComDevice.DataBits = 8;
                ComDevice.StopBits = System.IO.Ports.StopBits.One;
                try
                {
                    ComDevice.Open();
                    return true;
                }
                catch (Exception ex)
                {
                    _sReturnMsg = "串口打开错误" + ex.Message;
                    return false;
                }
            }

            byte[] sendData = null;
            sendData=Encoding.ASCII.GetBytes(_sCmd);
            try
            {
                ComDevice.WriteTimeout = _iTimeout;
                ComDevice.Write(sendData, 0, sendData.Length);//发送数据
                return true;
            }
            catch (Exception ex)
            {
                _sReturnMsg = "Comport write error:" + ex.Message;
                return false;
            }

        }

        public static bool Receive(ref string _sReturnMsg,int nTimeout)
        {
            _sReturnMsg = "";
            if (ComDevice.IsOpen)
            {
                if (ComDevice.BytesToRead > 0)
                {
                    byte[] ReDatas = new byte[ComDevice.BytesToRead];
                    ComDevice.ReadTimeout = nTimeout;
                    try
                    {
                        ComDevice.Read(ReDatas, 0, ReDatas.Length);//读取数据
                    }
                    catch (Exception exp)
                    {
                        _sReturnMsg = "Comport read error:" + exp.ToString(); ;
                        return false;
                    }
                    _sReturnMsg = new ASCIIEncoding().GetString(ReDatas);
                    sLastReceive = _sReturnMsg;
                    return true;
                }
                else
                {
                    _sReturnMsg = sLastReceive;
                    return false;
                }
            }
            else
            {
                _sReturnMsg = "ComPort Closed";
                return false;
            }
            
        }
    }
}
