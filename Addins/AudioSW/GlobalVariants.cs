using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Newtonsoft.Json;
using Cease.Addins.AudioSW;

namespace AudioSW
{
    public class GV
    {
        public static string sCurrentCommandName;
        public static string sCurrentCommand;

        public static string sCurrentTestPlan;

        public static int iTotalConnections;

        public struct COMMAND_ST
        {
            public string sCommandName;
            public string sAction;
            public string sDataType;
            public string sContent;
            public string sTimeStamp;
            public string sPassConstraint;
            public string sPassString;
            public string sFullHead;
            public int iReceiveTimeOut;
        }

        public static COMMAND_ST command;

        public struct TestItem
        {
            public int iOrder;
            public string sItemName;
            public byte[] byteCommandArray;
            public string sCommandStirng;
            public int iDelayTime;
            public int iFailRetryTimes;
            public string sPassConstraint;
            public string sPassString;
            public string sCommandType;
        }

        public struct TestItemStatus
        {
            public int iOrder;
            public string sItemName;
            public bool sCurrentPassFail;
            public int iTotalTestTimes;
            public int iTotalPassTimes;
            public string sCurrentColor;
            public DateTime StartTime;
            public DateTime EndTime;
            public TimeSpan ElapsedTime;
            public TimeSpan MaxElapsedTime;
            public TimeSpan MinElapsedTime;
            public double AvrElapsedTime;
            public TimeSpan TotalElapsedTime;
        }

        public static int iCurrentTestOrder;

        public static Dictionary<int, TestItemStatus> dicTestStatus;

        public static List<TestItem> struTestPlan;

        public static TestItem tmpTestItem;

        public static bool bIsTestPlanModified;

        public static bool bTestPlanFailBreak;

        public static int iTestPlanLoopTimes;

        public static bool bIsExecuteTestPlan;

        public static int iReceivedTimeOutSeconds;

        public static string sNowClient;

        public static string sClientRetMsg;

        public static Thread thrCylceTest;

        public static Thread thrSimulationTest;

        public static bool bIsExecuteSimulationTest;

        public static string sLoopTestLogPathName;

        public static string sSimulatonTestLogPathName;

        public static bool bPause;

        public static string sLoopTestInfo;

        public static byte[] byteTestByteArray = { 80, 82, 79, 68, 85, 67, 84, 76, 73, 78, 69, 58, 66, 83, 78};

        public static byte[] byteExitApp = { 55 ,53 ,32, 50, 53, 48, 32,48,32,53,54,32,48,10};

        public static string sCurrentLogPath;

        public static string sWIFISSID;

        public static string sWIFIPassWord;

        public static string sServerIP;

        public static string sServerPort;

        public static byte[] bNewLine = { 10 };  //V1.0.4 换行符

        public static byte[] bTestConnectionByteArray = { 84, 69 ,83,84,10};

        public static string sAddCommandType;   //V1.1.1

        public static int iSecondsToStartLoopTest; //V1.1.3

        public static bool bStartRightNowFlag; //V1.1.3

        public static string sPullPushFileName; //V1.1.6

        public static string sPushLocalFileName; //V1.1.6

        public static bool bIsPullAction; //V1.1.6

        public static bool bIsPushAction; //V1.1.6

        public static bool bIsFileReceiveAction;    //1.1.8

        public static int iFileSize;

        public static int iFileSizeSum=0;

        public static byte[] bReceiveFileByte={};

        public static string sRetMessage = "";

        public static string sDataType_Send ="";
        public static string sAction_Send = "";
        public static string sTimeStamp_Send = "";
        public static string sContent_Send = "";

        public static string sDataType_Rev = "";
        public static string sAction_Rev = "";
        public static string sTimeStamp_Rev = "";
        public static string sContent_Rev = "";

        public static byte[] bDataType_Rev = null;
        public static byte[] bAction_Rev = null;
        public static byte[] bTimeStamp_Rev = null;
        public static byte[] bContent_Rev = null;

        public static byte[] bDataByte_Send = null;
        public static Byte[] bDataByte_Rev = null;

        public static SocketMsg mRevSocMsg;

        public static Dictionary<string,COMMAND_ST> dicCommand;

        //指令池公有变量
        public static Thread thrReceiving;
        public static List<byte> RecDataStore;
        public static List<byte> RecDataStoreAll;
        public static Dictionary<string, byte[]> RecDataAndTimeStoreAll;
        public static List<string> RecStringStore;
        public static Dictionary<string, string> dicRecStr;
        public static Dictionary<string, byte[]> dicRecBytes;
        public static bool IsRecevingFile;
        public static byte[] buff;
    }
}
