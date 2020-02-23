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

#ifndef _ARCSOFT_WTMODULE_VERIFY_
#define _ARCSOFT_WTMODULE_VERIFY_

#include "./inc/platform/asvloffscreen.h"
#include "./inc/platform/merror.h"
#include "./inc/platform/amcomdef.h"
#include "./inc/arcsoft_wtmodelcommon.h"

#ifdef ARCSOFT_WTMODULEVERIFY_EXPORTS
#define MODULEWTVERIFY_API  __declspec(dllexport)
#else
#define MODULEWTVERIFY_API  __declspec(dllimport)
#endif

#ifdef   __cplusplus
extern "C"{
#endif

typedef struct _tag_ArcModuleVerifyVersion
{
    MDWord lCodebase;		/**< platform dependent       */
    MDWord lMajor;			/**< major version            */ 
    MDWord lMinor;			/**< minor version            */
    MDWord lBuild;			/**< increasable only         */
    MPChar Version;			/**< version in string format */
    MPChar BuildDate;		/**< latest build Date        */
    MPChar CopyRight;		/**< copyright                */
} ArcWTVerifyVersion;

typedef struct _VerifystereoImageData
{
	ASVLOFFSCREEN *leftImg;		//Tele camera image
	ASVLOFFSCREEN *rightImg;	//Wide camera image
	int chessboardWidth;//number of blocks in the direction of x axis
	int chessboardHeight;// number of blocks in the direction of y axis.
	char* leftImgPath;	//Tele camera image path
	char* rightImgPath;	//Wide camera image path
}ArcWTVerifystereoImageData;

typedef MHandle	MV_WTENGINE;

MODULEWTVERIFY_API MV_WTENGINE		MVWT_CreateEngine();

MODULEWTVERIFY_API MVoid			MVWT_DestroyEngine(MV_WTENGINE hEngine);

MODULEWTVERIFY_API MRESULT			MVWT_ModuleVerification(MV_WTENGINE hEngine,
											ArcWTVerifystereoImageData *pImages, MByte* allPara,
											MDouble* pAverageErr, MDouble* pMaxErr,MDouble* pErrRange, MChar* lpszDeviceID);

MODULEWTVERIFY_API const ArcWTVerifyVersion*   MVWT_GetVersion();

#ifdef  __cplusplus
}
#endif 

                                                    
#endif