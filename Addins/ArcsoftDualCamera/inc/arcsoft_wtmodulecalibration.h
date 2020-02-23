// arcsoft_modulerectify.h

/*******************************************************************************
Copyright(c) ArcSoft, All right reserved.

This file is ArcSoft's property. It contains ArcSoft's trade secret, proprietary 
and confidential information. 

The information and code contained in this file is only for authorized ArcSoft 
employees to design, create, modify, or review.

DO NOT DISTRIBUTE, DO NOT DUPLICATE OR TRANSMIT IN ANY FORM WITHOUT PROPER 
AUTHORIZATION.

If you are not an intended recipient of this file, you must not copy, 
distribute, modify, or take any action in reliance on it. 

If you have received this file in error, please immediately notify ArcSoft and 
permanently delete the original and any copy of any file and any printout 
thereof.
*******************************************************************************/

#ifndef _ARCSOFT_WTMODULE_RECTITY_
#define _ARCSOFT_WTMODULE_RECTITY_

#include "asvloffscreen.h"
#include "merror.h"
#include "amcomdef.h"
#include "arcsoft_wtmodelcommon.h"

#ifdef ARCSOFT_WTMODULERECTIFY_EXPORTS
#define MODULEWTRECTIFY_API  __declspec(dllexport)
#else
#define MODULEWTRECTIFY_API  __declspec(dllimport)
#endif

#define WT_RET_PARAMS_BASE							0XB000
#define WT_RET_PARAMS_ROTATION_X					(WT_RET_PARAMS_BASE+1)
#define WT_RET_PARAMS_ROTATION_Y					(WT_RET_PARAMS_BASE+2)
#define WT_RET_PARAMS_ROTATION_Z					(WT_RET_PARAMS_BASE+3)
#define WT_RET_PARAMS_SHIFT_X					    (WT_RET_PARAMS_BASE+4)
#define WT_RET_PARAMS_SHIFT_Y					    (WT_RET_PARAMS_BASE+5)


#ifdef   __cplusplus
   extern "C"{
#endif 


typedef struct _tag_ArcWTModuleRectifyVersion
{
    MDWord lCodebase;		/**< platform dependent       */
    MDWord lMajor;			/**< major version            */ 
    MDWord lMinor;			/**< minor version            */
    MDWord lBuild;			/**< increasable only         */
    MPChar Version;			/**< version in string format */
    MPChar BuildDate;		/**< latest build Date        */
    MPChar CopyRight;		/**< copyright                */
} ArcWTCalibrationVersion;

typedef struct _allWTCalibrationParameters
{
	ASVLOFFSCREEN *leftImg;		//Tele camera image
	ASVLOFFSCREEN *rightImg;	//Wide camera image

	MInt32 chessboardWidth;//number of blocks in the direction of x axis
	MInt32 chessboardHeight;// number of blocks in the direction of y axis.
	MInt32 numberOfChessboards;// total number of chessboards in this situation.
	MDouble blockSize; // physical size of block(mm) 
	MInt32 numberOfImages;
	char* leftImgPath;	//Tele camera image path
	char* rightImgPath;	//Wide camera image path
}ArcWTCalibrationParameters;

typedef MHandle	MC_WTENGINE;

MODULEWTRECTIFY_API MC_WTENGINE		MCWT_CreateEngine();

MODULEWTRECTIFY_API MVoid			MCWT_DestroyEngine(MC_WTENGINE hEngine);

MODULEWTRECTIFY_API MRESULT			MCWT_ModuleCalibration(MC_WTENGINE hEngine,
											ArcWTCalibrationParameters* pParam,
											MByte* lpszOutputParam,
											MUInt32* pOutputParamLength,
											MChar* lpszDeviceID);

MODULEWTRECTIFY_API MRESULT MCWT_ModuleGetResultParameters(MC_WTENGINE hEngine,MLong type,MDouble * pRetParam);

MODULEWTRECTIFY_API const ArcWTCalibrationVersion*   MCWT_GetVersion();

//////////////////////////////////////////////////////////////////////////
//Helper API for reading parameters from calibration

MODULEWTRECTIFY_API MRESULT	ArcCalibDataReaderInit(MByte* pCalibData, MUInt32 nCalibDataLen, MHandle* phReader);

MODULEWTRECTIFY_API MRESULT   ArcCalibDataReaderGetParam(MHandle hReader,MLong lType,MDouble* pRetParam);

MODULEWTRECTIFY_API MVoid		ArcCalibDataReaderUninit(MHandle hReader);
//////////////////////////////////////////////////////////////////////////

#ifdef   __cplusplus
}                                                   
#endif

#endif