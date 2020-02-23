using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
namespace CEASE.Test.Root
{

    /// <summary>
    /// BaseClass
    /// </summary>
    public class BaseClass
    {
        
        public bool ChannelToFrequency(string sMode, string sBand, int ULChannel, ref double ULfreqMHz, ref double DLfreqMHz)
        {
            if (sMode == "")
            {
                return false;
            }

            else if (sMode.ToUpper() == "WIFI")
            {
                if (sBand == "2.4G")
                {
                    ULfreqMHz =  2412 + (ULChannel - 1) * 5;
                    DLfreqMHz = ULfreqMHz;
                    return true;
                }
                else if (sBand == "5G")
                {
                    if (ULChannel >= 183 && ULChannel <= 196)
                    {
                        ULfreqMHz = 4915 + (ULChannel - 183) * 5;
                    }
                    else if ((ULChannel >= 7 && ULChannel <= 165))
                    {
                        ULfreqMHz = 5035 + (ULChannel - 7) * 5;
                    }
                    DLfreqMHz = ULfreqMHz;
                    return true;
                }
            }
            else if (sMode.ToUpper() == "BT")
            {
                ULfreqMHz = ULChannel + 2402;
                DLfreqMHz = ULfreqMHz;
                return true;
            }
            else
            {
                byte iBand;
                if (sBand == "GSM900")
                {
                    iBand = 1;
                }
                else if (sBand == "GSM850")
                {
                    iBand = 4;
                }
                else if (sBand == "DCS1800")
                {
                    iBand = 3;
                }
                else if (sBand == "PCS1900")
                {
                    iBand = 2;
                }
                else
                {
                    sBand = sBand[sBand.Length - 1] == 'A' ? sBand.Substring(0, sBand.Length - 1) : sBand;
                    sBand = sBand[sBand.Length - 1] == 'B' ? sBand.Substring(0, sBand.Length - 1) : sBand;
                    iBand = byte.Parse(sBand);
                }
                if (sMode == "TDSCDMA")
                {
                    TDSCDMAChannelToFrequency(iBand, ULChannel, ref ULfreqMHz, ref DLfreqMHz);
                }
                else if (sMode == "WCDMA")
                {
                    WcdmaChannelToFrequency(iBand, ULChannel, ref ULfreqMHz, ref DLfreqMHz);
                }
                else if (sMode == "LTE")
                {
                    LTEChannelToFrequency(iBand, ULChannel, ref ULfreqMHz, ref DLfreqMHz);
                }
                else if (sMode == "C2K")
                {
                    CdmaChannelToFrequency(iBand, ULChannel, ref ULfreqMHz, ref DLfreqMHz);
                }
                else if (sMode == "GMSK")
                {
                    GsmChannelToFrequency(iBand, ULChannel, ref ULfreqMHz, ref DLfreqMHz);
                }
            }
            
            return true;
        }

        protected bool WcdmaChannelToFrequency(int Band, int ULChannel, ref double ULfreqMHz, ref double DLfreqMHz)
        {
            double freq_current_MHz;
            double freq_current_MHz_DL;
            if (Band == 1)
            {
                if (ULChannel >= 9612 && ULChannel <= 9888)
                    freq_current_MHz = ULChannel / 5.0;
                else
                    return false;
                //9612 to 9888 1922.4~1977.6
                freq_current_MHz_DL = freq_current_MHz + 190;
            } 
            else if (Band == 2)
            {
                if (ULChannel >= 9262 && ULChannel <= 9538)
                    freq_current_MHz = ULChannel / 5.0;
                else if (ULChannel == 12 || ULChannel == 37 || ULChannel == 62 || ULChannel == 87 || ULChannel == 112
                    || ULChannel == 137 || ULChannel == 162 || ULChannel == 187 || ULChannel == 212 || ULChannel == 237
                    || ULChannel == 262 || ULChannel == 287)
                    freq_current_MHz = ULChannel / 5.0 + 1850.1;
                else
                    return false;
                /*9 262 to 9 538 and 12, 37, 62, 87, 112, 137, 162, 187, 212, 237, 262, 287*/
                /*1852.4 to 1907.6 and 1852.5, 1857.5, 1862.5, 1867.5, 1872.5, 1877.5, 1882.5, 1887.5, 1892.5, 1897.5, 1902.5, 1907.5*/
                freq_current_MHz_DL = freq_current_MHz + 80;
            }
            else if (Band == 3)
            {
                if (ULChannel >= 937 && ULChannel <= 1288)
                    freq_current_MHz = ULChannel / 5.0 + 1525; //937 to 1288 , 1712.4~1782.6
                else return false;
                freq_current_MHz_DL = freq_current_MHz + 95;
            }
            else if (Band == 4)
            {
                if (ULChannel >= 1312 && ULChannel <= 1513)
                    freq_current_MHz = ULChannel / 5.0 + 1450;
                else if (ULChannel == 1662 || ULChannel == 1687 || ULChannel == 1687 || ULChannel == 1737
                    || ULChannel == 1762 || ULChannel == 1787 || ULChannel == 1812 || ULChannel == 1837 || ULChannel == 1862)
                    freq_current_MHz = ULChannel / 5.0 + 1380.1;
                else return false;
                freq_current_MHz_DL = freq_current_MHz + 400;

                //1312 to 1513 and 1662, 1687, 1712, 1737, 1762, 1787, 1812, 1837, 1862
                //1712.4~1752.6 and 1712.5, 1717.5, 1722.5, 1727.5, 1732.5, 1737.5 1742.5, 1747.5, 1752.5
            }
            else if (Band == 5)
            {
                if (ULChannel >= 4132 && ULChannel <= 4233)
                    freq_current_MHz = ULChannel / 5.0;
                else if (ULChannel == 782 || ULChannel == 787 || ULChannel == 807 || ULChannel == 812 || ULChannel == 837 || ULChannel == 862)
                    freq_current_MHz = ULChannel / 5.0 + 670.1;
                else return false;
                //4132 to 4233 and 782, 787, 807, 812, 837, 862
                //826.4~846.6 ans 670.1	826.5, 827.5, 831.5, 832.5, 837.5, 842.5
                freq_current_MHz_DL = freq_current_MHz + 45;
            }
            else if (Band == 6)
            {
                if (ULChannel >= 4162 && ULChannel <= 4188)
                    freq_current_MHz = ULChannel / 5.0;
                else if (ULChannel == 812 || ULChannel == 837)
                    freq_current_MHz = ULChannel / 5.0 + 670.1;
                else return false;
                //4162 to 4188 and 812, 837
                //832.4~837.6 and 832.5, 837.5
                freq_current_MHz_DL = freq_current_MHz + 45;
            }
            else if (Band == 7)
            {
                if (ULChannel >= 2012 && ULChannel <= 2338)
                    freq_current_MHz = ULChannel / 5.0 + 2100;
                else if (ULChannel == 2362 || ULChannel == 2387 || ULChannel == 2412 || ULChannel == 2437 || ULChannel == 2462 || ULChannel == 2487
                    || ULChannel == 2512 || ULChannel == 2537 || ULChannel == 2562 || ULChannel == 2587 || ULChannel == 2612
                    || ULChannel == 2637 || ULChannel == 2662 || ULChannel == 2687)
                    freq_current_MHz = ULChannel / 5.0 + 2030.1;
                else return false;
                //2012 to 2338 and 2362, 2387, 2412, 2437, 2462, 2487, 2512, 2537, 2562, 2587, 2612, 2637, 2662, 2687 
                //2502.4~2567.6 and 2502.5, 2507.5, 2512.5, 2517.5, 2522.5, 2527.5, 2532.5, 2537.5, 2542.5, 2547.5, 2552.5, 2557.5, 2562.5, 2567.5
                freq_current_MHz_DL = freq_current_MHz + 120;
            }
            else if (Band == 8)
            {
                if (ULChannel >= 2712 && ULChannel <= 2863)
                    freq_current_MHz = ULChannel / 5.0 + 340;
                else return false;
                //2712 to 2863, 882.4~912.6
                freq_current_MHz_DL = freq_current_MHz + 45;
            }
            else if (Band == 9)
            {
                if (ULChannel >= 8762 && ULChannel <= 8912)
                    freq_current_MHz = ULChannel / 5.0;
                else return false;
                //8762 to 8912 , 1752.4	1782.4
                freq_current_MHz_DL = freq_current_MHz + 95;
            }
            else if (Band == 10)
            {
                if (ULChannel >= 2887 && ULChannel <= 3163)
                    freq_current_MHz = ULChannel / 5.0 + 1135;
                else if (ULChannel == 3187 || ULChannel == 3212 || ULChannel == 3237 || ULChannel == 3262 || ULChannel == 3287 || ULChannel == 3312
                    || ULChannel == 3337 || ULChannel == 3362 || ULChannel == 3387 || ULChannel == 3412 || ULChannel == 3437 || ULChannel == 3462)
                    freq_current_MHz = ULChannel / 5.0 + 1075.1;
                else return false;
                //2887 to 3163 and 3187, 3212, 3237, 3262, 3287, 3312, 3337, 3362, 3387, 3412, 3437, 3462
                //1712.4~1767.6 and 1712.5, 1717.5, 1722.5, 1727.5, 1732.5, 1737.5, 1742.5, 1747.5, 1752.5, 1757.5, 1762.5, 1767.5
                freq_current_MHz_DL = freq_current_MHz + 400;

            }
            else if (Band == 11)
            {
                if (ULChannel >= 3487 && ULChannel <= 3587)
                    freq_current_MHz = ULChannel / 5.0 + 733;
                else return false;
                //3487 to 3587 , 1430.4~1450.4
                freq_current_MHz_DL = freq_current_MHz + 48;
            }
            else
                return false;

            ULfreqMHz = freq_current_MHz;
            DLfreqMHz = freq_current_MHz_DL;
            return true;
        }

        protected bool WcdmaUlChannelToDlChannel(int Band, int ULChannel, ref int DLChannel)
        {
            decimal freq_current_MHz;
            decimal freq_current_MHz_DL;
            if (Band == 1)
            {
                if (ULChannel >= 9612 && ULChannel <= 9888)
                    freq_current_MHz = ULChannel / 5.0M;
                else
                    return false;
                //9612 to 9888 1922.4~1977.6
                freq_current_MHz_DL = freq_current_MHz + 190M;

                DLChannel = (int)(5M * freq_current_MHz_DL);
            }
            else if (Band == 2)
            {
                if (ULChannel >= 9262 && ULChannel <= 9538)
                {
                    freq_current_MHz = ULChannel / 5.0M;
                    freq_current_MHz_DL = freq_current_MHz + 80M;
                    DLChannel = (int)(5M * freq_current_MHz_DL);
                }
                else if (ULChannel == 12 || ULChannel == 37 || ULChannel == 62 || ULChannel == 87 || ULChannel == 112
                    || ULChannel == 137 || ULChannel == 162 || ULChannel == 187 || ULChannel == 212 || ULChannel == 237
                    || ULChannel == 262 || ULChannel == 287)
                {
                    freq_current_MHz = ULChannel / 5.0M + 1850.1M;
                    freq_current_MHz_DL = freq_current_MHz + 80M;
                    DLChannel = (int)(5M * (freq_current_MHz_DL - 1850.1M));
                }
                else
                    return false;
                /*9 262 to 9 538 and 12, 37, 62, 87, 112, 137, 162, 187, 212, 237, 262, 287*/
                /*1852.4 to 1907.6 and 1852.5, 1857.5, 1862.5, 1867.5, 1872.5, 1877.5, 1882.5, 1887.5, 1892.5, 1897.5, 1902.5, 1907.5*/


            }
            else if (Band == 3)
            {
                if (ULChannel >= 937 && ULChannel <= 1288)
                    freq_current_MHz = ULChannel / 5.0M + 1525M; //937 to 1288 , 1712.4~1782.6
                else return false;
                freq_current_MHz_DL = freq_current_MHz + 95M;
                DLChannel = (int)(5M * (freq_current_MHz_DL - 1575M));

            }
            else if (Band == 4)
            {
                if (ULChannel >= 1312 && ULChannel <= 1513)
                {
                    freq_current_MHz = ULChannel / 5.0M + 1450M;
                    freq_current_MHz_DL = freq_current_MHz + 400M;
                    DLChannel = (int)(5M * (freq_current_MHz_DL - 1805M));
                }
                else if (ULChannel == 1662 || ULChannel == 1687 || ULChannel == 1687 || ULChannel == 1737
                    || ULChannel == 1762 || ULChannel == 1787 || ULChannel == 1812 || ULChannel == 1837 || ULChannel == 1862)
                {
                    freq_current_MHz = ULChannel / 5.0M + 1380.1M;
                    freq_current_MHz_DL = freq_current_MHz + 400M;
                    DLChannel = (int)(5M * (freq_current_MHz_DL - 1735.1M));
                }
                else return false;


                //1312 to 1513 and 1662, 1687, 1712, 1737, 1762, 1787, 1812, 1837, 1862
                //1712.4~1752.6 and 1712.5, 1717.5, 1722.5, 1727.5, 1732.5, 1737.5 1742.5, 1747.5, 1752.5
            }
            else if (Band == 5)
            {
                if (ULChannel >= 4132 && ULChannel <= 4233)
                {
                    freq_current_MHz = ULChannel / 5.0M;
                    freq_current_MHz_DL = freq_current_MHz + 45M;
                    DLChannel = (int)(5M * freq_current_MHz_DL);

                }
                else if (ULChannel == 782 || ULChannel == 787 || ULChannel == 807 || ULChannel == 812 || ULChannel == 837 || ULChannel == 862)
                {
                    freq_current_MHz = ULChannel / 5.0M + 670.1M;
                    freq_current_MHz_DL = freq_current_MHz + 45M;
                    DLChannel = (int)(5M * (freq_current_MHz_DL - 670.1M));
                }
                else return false;
                //4132 to 4233 and 782, 787, 807, 812, 837, 862
                //826.4~846.6 ans 670.1	826.5, 827.5, 831.5, 832.5, 837.5, 842.5

            }
            else if (Band == 6)
            {
                if (ULChannel >= 4162 && ULChannel <= 4188)
                {
                    freq_current_MHz = ULChannel / 5.0M;
                    freq_current_MHz_DL = freq_current_MHz + 45M;
                    DLChannel = (int)(5M * freq_current_MHz_DL);
                }
                else if (ULChannel == 812 || ULChannel == 837)
                {
                    freq_current_MHz = ULChannel / 5.0M + 670.1M;
                    freq_current_MHz_DL = freq_current_MHz + 45M;
                    DLChannel = (int)(5M * (freq_current_MHz_DL - 670.1M));
                }
                else return false;
                //4162 to 4188 and 812, 837
                //832.4~837.6 and 832.5, 837.5
            }
            else if (Band == 7)
            {
                if (ULChannel >= 2012 && ULChannel <= 2338)
                {
                    freq_current_MHz = ULChannel / 5.0M + 2100M;
                    freq_current_MHz_DL = freq_current_MHz + 120M;
                    DLChannel = (int)(5M * (freq_current_MHz_DL - 2175M));
                }
                else if (ULChannel == 2362 || ULChannel == 2387 || ULChannel == 2412 || ULChannel == 2437 || ULChannel == 2462 || ULChannel == 2487
                    || ULChannel == 2512 || ULChannel == 2537 || ULChannel == 2562 || ULChannel == 2587 || ULChannel == 2612
                    || ULChannel == 2637 || ULChannel == 2662 || ULChannel == 2687)
                {
                    freq_current_MHz = ULChannel / 5.0M + 2030.1M;
                    freq_current_MHz_DL = freq_current_MHz + 120M;
                    DLChannel = (int)(5M * (freq_current_MHz_DL - 2105.1M));
                }
                else return false;
                //2012 to 2338 and 2362, 2387, 2412, 2437, 2462, 2487, 2512, 2537, 2562, 2587, 2612, 2637, 2662, 2687 
                //2502.4~2567.6 and 2502.5, 2507.5, 2512.5, 2517.5, 2522.5, 2527.5, 2532.5, 2537.5, 2542.5, 2547.5, 2552.5, 2557.5, 2562.5, 2567.5

            }
            else if (Band == 8)
            {
                if (ULChannel >= 2712 && ULChannel <= 2863)
                    freq_current_MHz = ULChannel / 5.0M + 340M;
                else return false;
                //2712 to 2863, 882.4~912.6
                freq_current_MHz_DL = freq_current_MHz + 45M;
                DLChannel = (int)(5M * (freq_current_MHz_DL - 340M));
            }
            else if (Band == 9)
            {
                if (ULChannel >= 8762 && ULChannel <= 8912)
                    freq_current_MHz = ULChannel / 5.0M;
                else return false;
                //8762 to 8912 , 1752.4	1782.4
                freq_current_MHz_DL = freq_current_MHz + 95M;
                DLChannel = (int)(5M * freq_current_MHz_DL);
            }
            else if (Band == 10)
            {
                if (ULChannel >= 2887 && ULChannel <= 3163)
                {
                    freq_current_MHz = ULChannel / 5.0M + 1135M;
                    freq_current_MHz_DL = freq_current_MHz + 400M;
                    DLChannel = (int)(5M * (freq_current_MHz_DL - 1490M));
                }
                else if (ULChannel == 3187 || ULChannel == 3212 || ULChannel == 3237 || ULChannel == 3262 || ULChannel == 3287 || ULChannel == 3312
                    || ULChannel == 3337 || ULChannel == 3362 || ULChannel == 3387 || ULChannel == 3412 || ULChannel == 3437 || ULChannel == 3462)
                {
                    freq_current_MHz = ULChannel / 5.0M + 1075.1M;
                    freq_current_MHz_DL = freq_current_MHz + 400M;
                    DLChannel = (int)(5M * (freq_current_MHz_DL - 1430.1M));
                }
                else return false;
                //2887 to 3163 and 3187, 3212, 3237, 3262, 3287, 3312, 3337, 3362, 3387, 3412, 3437, 3462
                //1712.4~1767.6 and 1712.5, 1717.5, 1722.5, 1727.5, 1732.5, 1737.5, 1742.5, 1747.5, 1752.5, 1757.5, 1762.5, 1767.5             

            }
            else if (Band == 11)
            {
                if (ULChannel >= 3487 && ULChannel <= 3587)
                    freq_current_MHz = ULChannel / 5.0M + 733M;
                else return false;
                //3487 to 3587 , 1430.4~1450.4
                freq_current_MHz_DL = freq_current_MHz + 48M;
                DLChannel = (int)(5M * (freq_current_MHz_DL - 736M));
            }
            else
                return false;

            return true;
        }

        protected bool LTEChannelToFrequency(int Band, int ULChannel, ref double ULfreqMHz, ref double DLfreqMHz)
        {
            double dUlStartFreq = 0.0;
            const double dChannelSpacing = 0.1;
            double dInterval = 0.0;
            int iChannelOffset = 0;

            if (Band == 1)
            {
                dUlStartFreq = 1920.0;
                dInterval = 190.0;
                iChannelOffset = 18000;
            }
            else if (Band == 2)
            {
                dUlStartFreq = 1850.0;
                dInterval = 80.0;
                iChannelOffset = 18600;
            }
            else if (Band == 3)
            {
                dUlStartFreq = 1710.0;
                dInterval = 95.0;
                iChannelOffset = 19200;
            }
            else if (Band == 4)
            {
                dUlStartFreq = 1710.0;
                dInterval = 400.0;
                iChannelOffset = 19950;
            }
            else if (Band == 5)
            {
                dUlStartFreq = 824.0;
                dInterval = 45.0;
                iChannelOffset = 20400;
            }
            else if (Band == 7)
            {
                dUlStartFreq = 2500.0;
                dInterval = 120.0;
                iChannelOffset = 20750;
            }
            else if (Band == 8)
            {
                dUlStartFreq = 880.0;
                dInterval = 45.0;
                iChannelOffset = 21450;
            }
            else if (Band == 11)
            {
                dUlStartFreq = 1427.9;
                dInterval = 48.0;
                iChannelOffset = 22750;
            }
            else if (Band == 12)
            {
                dUlStartFreq = 698.0;
                dInterval = 30.0;
                iChannelOffset = 23000;
            }
            else if (Band == 13)
            {
                dUlStartFreq = 777.0;
                dInterval = -31.0;
                iChannelOffset = 23180;
            }
            else if (Band == 17)
            {
                dUlStartFreq = 704.0;
                dInterval = 30.0;
                iChannelOffset = 23730;
            }
            else if (Band == 18)
            {
                dUlStartFreq = 815.0;
                dInterval = 45.0;
                iChannelOffset = 23850;
            }
            else if (Band == 19)
            {
                dUlStartFreq = 830;
                dInterval = 45.0;
                iChannelOffset = 24000;
            }
            else if (Band == 20)
            {
                dUlStartFreq = 832.0;
                dInterval = -41.0;
                iChannelOffset = 24150;
            }
            else if (Band == 25)
            {
                dUlStartFreq = 1850.0;
                dInterval = 80.0;
                iChannelOffset = 26040;
            }
            else if(Band == 26)
            {
                dUlStartFreq = 814.0;
                dInterval = 45.0;
                iChannelOffset = 26690;
            }
            else if (Band == 28)  //Jerry  20181024
            {
                dUlStartFreq = 703.0;
                dInterval = 55.0;
                iChannelOffset = 27210;
            }
            else if (Band == 29)
            {
                /***** Begin Augus.hu 2016-11-23 modify *****/
                //Band 29只在CA时候使用，而且3GPP协议只定义了DLFrequency，没有ULFrequency
                //和射频Sean.yang确认使用Band 30的ULFrequency做同步
                //ULfreqMHz = 717.0;
                dUlStartFreq = 717.0;
                dInterval = 0.0;
                iChannelOffset = 9660;
                /***** End Augus.hu 2016-11-23 modify *****/
            }
            else if (Band == 30)  //16/8/2 andy.zhang
            {
                dUlStartFreq = 2305.0;
                dInterval = 45.0;
                iChannelOffset = 27660;
            }
            else if (Band == 33)
            {
                dUlStartFreq = 1900.0;
                dInterval = 0.0;
                iChannelOffset = 36000;
            }
            else if (Band == 34)
            {
                dUlStartFreq = 2010.0;
                dInterval = 0.0;
                iChannelOffset = 36200;
            }
            else if (Band == 38)
            {
                dUlStartFreq = 2570.0;
                dInterval = 0.0;
                iChannelOffset = 37750;
            }
            else if (Band == 39)
            {
                dUlStartFreq = 1880.0;
                dInterval = 0.0;
                iChannelOffset = 38250;
            }
            else if (Band == 40)
            {
                dUlStartFreq = 2300.0;
                dInterval = 0.0;
                iChannelOffset = 38650;
            }
            else if (Band == 41)
            {
                dUlStartFreq = 2496.0;
                dInterval = 0.0;
                iChannelOffset = 39650;
            }
            else
            {
                return false;
            }

            /***** Begin Augus.hu 2016-11-23 modify *****/
            //Band 29只在CA时候使用，而且3GPP协议只定义了DLFrequency，没有ULFrequency
            //和射频Sean.yang确认使用Band 30的ULFrequency做同步
            if (Band == 29)
            {
                DLfreqMHz = (ULChannel - iChannelOffset) * dChannelSpacing + dUlStartFreq;
                ULfreqMHz = 2310.0;
            }
            else
            {
                ULfreqMHz = (ULChannel - iChannelOffset) * dChannelSpacing + dUlStartFreq;
                DLfreqMHz = ULfreqMHz + dInterval;
            }
            /***** End Augus.hu 2016-11-23 modify *****/
            return true;
        }

        protected bool LTEUlChannelToDlChannel(int Band, int ULChannel, ref int DLChannel)
        {
            if (Band < 33 && Band != 30 && Band != 29)//andy.zhang /8/5
            {
                DLChannel = ULChannel - 18000;
            }
            else if (Band == 30)//andy.zhang /8/5
            {
                DLChannel = ULChannel - 17890;
            }
            else
            {
                DLChannel = ULChannel;
            }
            return true;
        }

        protected bool TDSCDMAChannelToFrequency(int Band, int ULChannel, ref double ULfreqMHz, ref double DLfreqMHz)
        {

            ULfreqMHz = ULChannel / 5.0;
            DLfreqMHz = ULfreqMHz;

            return true;
        }

        protected bool TDSCDMAUlChannelToDlChannel(int Band, int ULChannel, ref int DLChannel)
        {
            DLChannel = ULChannel;
            return true;
        }

        protected bool GsmChannelToFrequency(int Band, int chan, ref double ULfreqMHz, ref double DLfreqMHz)
        {

            switch (Band)
            {
                case 4: // GSM 850
                    if (chan < 128 || chan > 251) return false;
                    break;
                case 1:	   //EGSM
                    if (chan < 0 || (chan > 124 && chan < 975) || chan > 1023) return false;
                    break;
                case 3:		//DCS
                    if (chan < 512 || chan > 885) return false;
                    break;
                case 2:		//PCS
                    if (chan < 512 || chan > 810) return false;
                    break;

            }

            switch (Band)
            {
                case 4:  // GSM850
                    DLfreqMHz = Math.Round(824.2 + 0.2 * (chan - 128) + 45.0, 1);
                    ULfreqMHz = Math.Round(824.2 + 0.2 * (chan - 128), 1);
                    break;
                case 1:  // EGSM
                    if (chan >= 975 && chan <= 1023)
                    {
                        DLfreqMHz = Math.Round(880.2 + 0.2 * (chan - 975) + 45.0, 1);
                        ULfreqMHz = Math.Round(880.2 + 0.2 * (chan - 975), 1);
                    }
                    else
                    {
                        DLfreqMHz = Math.Round(890.0 + 0.2 * chan + 45.0, 1);
                        ULfreqMHz = Math.Round(890.000000 + 0.200000 * chan, 1);
                    }
                    break;
                case 3:  //DCS
                    DLfreqMHz = Math.Round(1710.2 + 0.2 * (chan - 512) + 95.0, 1);
                    ULfreqMHz = Math.Round(1710.2 + 0.2 * (chan - 512), 1);
                    break;
                case 2:  //PCS
                    //DLfreqMHz = Math.Round(1850.2 + 0.2 * chan  + 80.0, 1);
                    //ULfreqMHz = Math.Round(1850.2 + 0.2 * chan , 1);
                    DLfreqMHz = Math.Round(1850.2 + 0.2 * (chan - 512) + 80.0, 1);
                    ULfreqMHz = Math.Round(1850.2 + 0.2 * (chan - 512), 1);
                    break;

            }
            return true;
        }

        protected bool CdmaChannelToFrequency(int Band, int chan, ref double ULfreqMHz, ref double DLfreqMHz)
        {

            switch (Band)
            {
                case 0:	  //BC0 800M
                    if (chan < 1 || (chan > 799 && chan < 991) || chan > 1023)
                        return false;
                    break;
                case 1:	   //BC1 1900M
                    if (chan < 0 || chan > 1199)
                        return false;
                    break;
                case 6:       //BC6 IMT
                    if (chan < 0 || chan > 1199)
                        return false;
                    break;
                case 15:      //BC15
                    if (chan < 0 || chan > 899)
                        return false;
                    break;

                default:
                    return false;
            }


            switch (Band)
            {
                case 0:  //BC0 800M
                    if (chan >= 991 && chan <= 1023)
                    {
                        DLfreqMHz = (double)(825000 + 30 * (chan - 1023) + 45000) / 1000;
                        ULfreqMHz = (double)(825000 + 30 * (chan - 1023)) / 1000;
                    }
                    else
                    {
                        DLfreqMHz = (double)(825000 + 30 * chan + 45000) / 1000;
                        ULfreqMHz = (double)(825000 + 30 * chan) / 1000;
                    }
                    break;
                case 1:  //BC1 1900M
                    if (chan >= 0 && chan <= 1199)
                    {
                        DLfreqMHz = (double)(1850000 + 50 * chan + 80000) / 1000;
                        ULfreqMHz = (double)(1850000 + 50 * chan) / 1000;
                    }
                    break;
                case 6:  //BC6 IMT
                    if (chan >= 0 && chan <= 1199)
                    {
                        DLfreqMHz = (double)(2110000 + 50 * chan) / 1000;
                        ULfreqMHz = (double)(1920000 + 50 * chan) / 1000;
                    }
                    break;
                case 15:  //BC15
                    if (chan >= 0 && chan <= 899)
                    {
                        DLfreqMHz = (double)(2110000 + 50 * chan) / 1000;
                        ULfreqMHz = (double)(1710000 + 50 * chan) / 1000;
                    }
                    break;

                default:
                    return false;
            }
            return true;
        }

        protected bool CdmaUlChannelToDlChannel(int Band, int ULChannel, ref int DLChannel)
        {
            DLChannel = ULChannel;
            return true;
        }

    }
}

