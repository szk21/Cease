using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

using Cease.Addins.Log;
using Cease.Addins.PowerCtrl;
using System.ComponentModel.Composition;
using NationalInstruments.VisaNS;

namespace GpibPowerCtrl
{
    public class InstrGpib
    {
        protected MessageBasedSession mbSession;
       

        /// <summary>
        /// 日志
        /// </summary>
        protected InterfaceLog log;
       

        /// <summary>
        /// using string param for resource-name
        /// </summary>
        /// <param name="ResourceName"></param>
        /// <param name="Timeout"></param>
        /// <returns></returns>
        protected bool OpenSession(string ResourceName, int Timeout)
        {
            try
            {
                log.msg("Open GPIB Session");
                log.msg("Addr: " + ResourceName + " Timeout: " + Timeout.ToString());
                
                mbSession = (MessageBasedSession)ResourceManager.GetLocalManager().Open(ResourceName, AccessModes.NoLock, 5000);
                mbSession.Timeout = Timeout;
                //mbSession.TerminationCharacterEnabled = true; //Enabled will break IQ read.
                mbSession.Clear();
                mbSession.Write("*CLS\n");
                mbSession.Query("*IDN?\n");
            }
            catch (InvalidCastException exp)
            {
                log.err("Exception in OpenSession : Resource selected must be a message-based session.", exp);
                return false;
            }
            catch (Exception exp)
            {
                log.err("Exception in OpenSession.", exp);
                return false;
            }

            return true;
        }

        /// <summary>
        /// ResourceName like as "GPIB0::14::INSTR"
        /// </summary>
        /// <param name="Board"></param>
        /// <param name="GPIBAddress"></param>
        /// <param name="Timeout"></param>
        /// <returns></returns>
        protected bool OpenSession(int Board, int GPIBAddress, int Timeout)
        {
            try
            {
                log.msg("Open GPIB Session");
                string ResourceName = "GPIB" + Board + "::" + GPIBAddress + "::INSTR";
                log.msg("Addr: " + ResourceName + " Timeout: " + Timeout.ToString());

                mbSession = (MessageBasedSession)ResourceManager.GetLocalManager().Open(ResourceName, AccessModes.NoLock, 5000);
                mbSession.Timeout = Timeout;
                mbSession.Clear();
                mbSession.Write("*CLS\n");
                mbSession.Query("*IDN?\n");
            }
            catch (InvalidCastException exp)
            {
                log.err("Exception in OpenSession : Resource selected must be a message-based session.", exp);
                return false;
            }
            catch (Exception exp)
            {
                log.err("Exception in OpenSession.", exp);
                return false;
            }

            return true;
        }

        protected void WriteGPIBCmd(string strCmd)
        {
            try
            {
                log.msg("--->" + strCmd);
                mbSession.Write(strCmd + "\n");
            }
            catch (Exception exp)
            {
                log.err("Exception in WriteGPIBCmd.", exp);
            }
        }

        protected string ReadGPIBCmd()
        {
            string ResponseContext = "";
            try
            {
                ResponseContext = mbSession.ReadString();
                log.msg("<---" + ResponseContext);
            }
            catch (Exception exp)
            {
                log.err("Exception in ReadGPIBCmd.", exp);
            }

            return ResponseContext;
        }

        protected byte[] ReadGPIBCmd(int countToRead)
        {
            byte[] ResponseContext = new byte[countToRead];
            try
            {
                ResponseContext = mbSession.ReadByteArray(countToRead);
                log.msg("<---" + ResponseContext);
            }
            catch (Exception exp)
            {
                log.err("Exception in ReadGPIBCmd.", exp);
            }

            return ResponseContext;
        }

        protected string QueryGPIBCmd(string strCmd)
        {
            string ResponseContext = "";
            try
            {
                log.msg("--->" + strCmd);
                ResponseContext = mbSession.Query(strCmd + "\n");
                log.msg("<---" + ResponseContext);
            }
            catch (Exception exp)
            {
                log.err("Exception in QueryGPIBCmd.", exp);
            }

            return ResponseContext;
        }

        protected void ReleaseSession()
        {
            log.msg("ReleaseSession");
            if (mbSession != null)
            {
                //mbSession.Clear();
                log.msg("mbSession.Dispose()");
                mbSession.Dispose();
                mbSession = null;
            }
        }

    }

    [Export(typeof(InterfacePowerCtrl))]
    [ExportMetadata("AddinName", "GpibPowerCtrl")]
    public class GpibPowerCtrl : InstrGpib, InterfacePowerCtrl
    {
        protected double LeakCur;
        protected string PowerType;

        double InterfacePowerCtrl._dLeakCur
        {
            get { return LeakCur;}
            set { LeakCur = value; }
        }
        public void RegisterLogger(InterfaceLog _log)
        {
            this.log = _log;
        }

        public bool InitialInstr(string strResourceName, int Timeout, bool DoReset)
        {
            string ResponseContext = "";
            if (!OpenSession(strResourceName, Timeout))
            { 
                return false; 
            }

            ResponseContext = QueryGPIBCmd("*IDN?");
            if (ResponseContext.Contains("KEITHLEY"))
            {
                PowerType = "KEITHLEY";
                log.msg("PowerType is KEITHLEY");
            }
            if (DoReset)
            {
                QueryGPIBCmd("*RST;*OPC?");
            }
            return true;
        }

        public bool ReleaseInstr()
        {
            ReleaseSession();
            return true;
        }

        public void PowerSetVolt(string strVolt, string VPTVolt, int InternalTime)
        {
            log.msg("PowerSetVolt");
            if (PowerType == "KEITHLEY")
            {
                WriteGPIBCmd("SENSe:CURRent:RANGe:AUTO ON");
                if (VPTVolt != "0")
                {
                    WriteGPIBCmd("VOLT:PROT " + VPTVolt);
                    WriteGPIBCmd("VOLT:PROT:STAT 1");
                }
            }
            Thread.Sleep(InternalTime);
            WriteGPIBCmd("VOLT 3.85");
            Thread.Sleep(InternalTime);
            WriteGPIBCmd("VOLT " + strVolt);
            Thread.Sleep(InternalTime);
        }

        public void PowerStepOn(string strVolt,string VPTVolt,int InternalTime)
        {
            log.msg("PowerStepOn");
            if (PowerType == "KEITHLEY")
            {
                WriteGPIBCmd("SENSe:CURRent:RANGe:AUTO ON");
                if (VPTVolt != "0")
                {
                    WriteGPIBCmd("VOLT:PROT " + VPTVolt);
                    WriteGPIBCmd("VOLT:PROT:STAT 1");
                }
            }
            WriteGPIBCmd("OUTP OFF");
            WriteGPIBCmd("CURR 3.0");
            WriteGPIBCmd("VOLT 0.00");
            WriteGPIBCmd("OUTP ON");
            WriteGPIBCmd("VOLT 0.40");
            Thread.Sleep(InternalTime);
            WriteGPIBCmd("VOLT 0.80");
            Thread.Sleep(InternalTime);
            WriteGPIBCmd("VOLT 1.20");
            Thread.Sleep(InternalTime);
            WriteGPIBCmd("VOLT 1.6");
            Thread.Sleep(InternalTime);
            WriteGPIBCmd("VOLT 2.0");
            Thread.Sleep(InternalTime);
            WriteGPIBCmd("VOLT 2.4");
            Thread.Sleep(InternalTime);
            WriteGPIBCmd("VOLT 2.8");
            Thread.Sleep(InternalTime);
            WriteGPIBCmd("VOLT 3.2");
            Thread.Sleep(InternalTime);
            WriteGPIBCmd("VOLT 3.6");
            Thread.Sleep(InternalTime);
            WriteGPIBCmd("VOLT 3.85");
            Thread.Sleep(InternalTime);
            WriteGPIBCmd("VOLT " + strVolt);
            Thread.Sleep(InternalTime);
        }

        public void PowerOff()
        {
            WriteGPIBCmd("OUTP OFF");
        }

        public void SetVoltmv(double _volt)
        {
            WriteGPIBCmd(string.Format("VOLT {0}", _volt/1000));
        }

        public bool MeasureCur(ref double _dfCur)
        {
            string strAll = QueryGPIBCmd("MEAS:CURR:DC?");

            int rtPos = -1;
		    string strTmp;
            double dfBVal, dfEVal;
            if (-1 != (rtPos = strAll.IndexOf('U')))
            {
                strAll = strAll.Replace('>', '.');
                strTmp = (strAll.Substring(0, 1) == "=") ? strAll.Substring(1, rtPos) : strAll.Substring(0, rtPos);
                dfBVal = double.Parse(strTmp);

                strTmp = strAll.Substring(rtPos + 2, strAll.Length - 2);
                if (-1 != strAll.IndexOf('='))
                {
                    dfEVal = -double.Parse(strTmp);
                }
                else
                {
                    dfEVal = double.Parse(strTmp);
                }
            }
            else if (-1 != (rtPos = strAll.IndexOf('E')))
            {
                strTmp = strAll.Substring(0, rtPos);
                dfBVal = double.Parse(strTmp);

                strTmp = strAll.Substring(rtPos + 1);
                dfEVal = double.Parse(strTmp);
            }
            else
            {
                return false;
            }

            _dfCur = dfBVal * Math.Pow(10, dfEVal + 3);
            if (_dfCur < 0 && (_dfCur) > -5)
            {
                _dfCur = _dfCur * -1;
            }
            return true;
        }
    }
}
