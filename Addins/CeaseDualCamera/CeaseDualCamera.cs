using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Management;
using System.ComponentModel.Composition;

using Cease.Addins.Log;
using Cease.Addins.DualCamera;

using System.Runtime.InteropServices;

namespace CeaseDualCamera
{
    [Export(typeof(InterfaceDualCamera))]
    [ExportMetadata("AddinName", "CeaseDualCamera")]
    public class CeaseDualCamera : InterfaceDualCamera
    {

        [DllImport("ArcsoftDualCamera.dll", EntryPoint = "arcsoft_DualCamVerify_NV21", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private static extern int arcsoft_DualCamVerify_NV21(string leftImgPath, string rightImgPath, string binFilePath, double[] Errs, int mapWidth, int mapHight, int L_RES_WIDTH, int L_RES_HEIGHT, int R_RES_WIDTH, int R_RES_HEIGHT);
        [DllImport("ArcsoftDualCamera.dll", EntryPoint = "arcsoft_DualCamVerify_RGB888", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private static extern int arcsoft_DualCamVerify_RGB888(string leftImgPath, string rightImgPath, string binFilePath, double[] Errs, int mapWidth, int mapHight, int L_RES_WIDTH, int L_RES_HEIGHT, int R_RES_WIDTH, int R_RES_HEIGHT);
        [DllImport("ArcsoftDualCamera.dll", EntryPoint = "arcsoft_DualCamVerify_JPEG", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private static extern int arcsoft_DualCamVerify_JPEG(string leftImgPath, string rightImgPath, string binFilePath, double[] Errs, int mapWidth, int mapHight);


        [DllImport("ArcsoftDualCamera.dll", EntryPoint = "SetLTEMIMOConfigure", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private static extern int SetLTEMIMOConfigure(UInt32 hResourceContext,int[] array);

        [DllImport("ArcsoftDualCamera.dll", EntryPoint = "GetLTEMIMORxValue", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private static extern int GetLTEMIMORxValue(UInt32 hResourceContext, int[] array, int[] iRxRGC);

        [DllImport("ArcsoftDualCamera.dll", EntryPoint = "LTERadioTeardown", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private static extern int LTERadioTeardown(UInt32 hResourceContext);

        [DllImport("ArcsoftDualCamera.dll", EntryPoint = "FTM_RF_Atlas_HE", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private static extern int FTM_RF_Atlas_HE(UInt32 hResourceContext, int[] array,int[] retarray,byte[] err);

        [DllImport("ArcsoftDualCamera.dll", EntryPoint = "FTM_RF_RadioTeardown", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private static extern int FTM_RF_RadioTeardown(UInt32 hResourceContext,int Tech);

        
        private InterfaceLog log;

        public void RegisterLogger(InterfaceLog _log)
        {
            log = _log;
        }

        public int DualCamVerify_NV21(string leftImgPath, string rightImgPath, string binFilePath, double[] Errs, int mapWidth, int mapHight, int L_RES_WIDTH, int L_RES_HEIGHT, int R_RES_WIDTH, int R_RES_HEIGHT)
        {
            return arcsoft_DualCamVerify_NV21(leftImgPath, rightImgPath, binFilePath, Errs,  mapWidth,  mapHight,  L_RES_WIDTH,  L_RES_HEIGHT,  R_RES_WIDTH,  R_RES_HEIGHT);
        }

        public int DualCamVerify_RGB888(string leftImgPath, string rightImgPath, string binFilePath, double[] Errs, int mapWidth, int mapHight, int L_RES_WIDTH, int L_RES_HEIGHT, int R_RES_WIDTH, int R_RES_HEIGHT)
        {

            return arcsoft_DualCamVerify_RGB888(leftImgPath, rightImgPath, binFilePath,  Errs, mapWidth,  mapHight,  L_RES_WIDTH,  L_RES_HEIGHT,  R_RES_WIDTH,  R_RES_HEIGHT);
        }

        public int DualCamVerify_JPEG(string leftImgPath, string rightImgPath, string binFilePath, double[] Errs, int mapWidth, int mapHight)
        {

            return arcsoft_DualCamVerify_JPEG( leftImgPath,  rightImgPath,  binFilePath, Errs,  mapWidth,  mapHight);
        }

        public int FTM_RF_SetLTEMIMOConfigure(UInt32 hResourceContext,int[] array)
        {

            return SetLTEMIMOConfigure( hResourceContext,array);
        }

        public int FTM_RF_GetLTEMIMORxValue(UInt32 hResourceContext, int[] array,  int[] iRxRGC)
        {

            return GetLTEMIMORxValue(hResourceContext, array, iRxRGC);
        }

        public int FTM_RF_LTERadioTeardown(UInt32 hResourceContext)
        {

            return LTERadioTeardown(hResourceContext);
        }

        //public int FTM_RF_Tx_On(UInt32 hResourceContext, int[] array,int[] retarray,byte[] err)
        //{

        //    return FTM_RF_Atlas_HE(hResourceContext, array, retarray,err);
        //}

        //public int FTM_RF_Tx_Off(UInt32 hResourceContext, int tech)
        //{

        //    return FTM_RF_RadioTeardown(hResourceContext, tech);
        //}

    }
}
