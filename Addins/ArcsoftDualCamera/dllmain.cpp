// dllmain.cpp : 定义 DLL 应用程序的入口点。
#include "stdafx.h"

#include "./inc/arcsoft_verification.h"
#include "./inc/arcsoft_module_errorcommon.h"
#include "./inc/arcsoft_wtmoduleverify.h"
#include "./inc/arcsoft_wtmodelcommon.h"
// #define WIDTH 4280
// #define HEIGHT 3120 
#define BIN_SIZE 2048
#pragma comment(lib, "arcsoft_verification.lib")
#pragma warning(disable:4996)

#include <stdio.h>
#include <stdlib.h>
#include "Qlib_Defines.h"
#include "QLib.h"

#define TELE_WIDTH 4280
#define TELE_HEIGHT 3120 
#define WIDE_WIDTH 4280
#define WIDE_HEIGHT 3120 
#define BIN_SIZE 2048
#pragma comment(lib, "arcsoft_dualcamverify.lib")

HANDLE g_hResourceContext = NULL;
HANDLE deviceMutex; // For RF Cal callbox (used QSC cal and GSM cal for atlas using QSCPI callbox)


BOOL APIENTRY DllMain( HMODULE hModule,
                       DWORD  ul_reason_for_call,
                       LPVOID lpReserved
					 )
{
	switch (ul_reason_for_call)
	{
	case DLL_PROCESS_ATTACH:
	case DLL_THREAD_ATTACH:
	case DLL_THREAD_DETACH:
	case DLL_PROCESS_DETACH:
		break;
	}
	return TRUE;
}

extern "C" _declspec(dllexport)  int arcsoft_DualCamVerify_NV21(char leftImgPath[], char rightImgPath[],char binFilePath[],double Errs[],int mapWidth,int mapHight,int L_RES_WIDTH,int L_RES_HEIGHT,int R_RES_WIDTH,int R_RES_HEIGHT)
{  
    char * device = "deviceID";
	MC_ENGINE m_hDPCTEngine = NULL;
	m_hDPCTEngine = MC_CreateEngine();
	MByte byteResult[BIN_SIZE] = {0};
	MUInt8* vleftnv21 = NULL; 
	MUInt8* vrightnv21 = NULL; 
    /*
	/ malloc mem for vleftnv21 and vrightnv21,then get rgb data from your image
	*/
	vleftnv21 = (MUInt8*)malloc(L_RES_WIDTH*L_RES_HEIGHT*3/2);
	vrightnv21 = (MUInt8*)malloc(R_RES_WIDTH*R_RES_HEIGHT*3/2);
	FILE * vcleftfp = fopen(leftImgPath,"rb");
	FILE * vcrightfp = fopen(rightImgPath,"rb");
	//FILE * vcleftfp = fopen("E:\\ConsoleApplication1\\Release\\vvll_4280x3120.NV21","rb");
	//FILE * vcrightfp = fopen("E:\\ConsoleApplication1\\Release\\vvrr_4280x3120.NV21","rb");
	fread(vleftnv21,L_RES_WIDTH*L_RES_HEIGHT*3/2,1,vcleftfp);
	fread(vrightnv21,R_RES_WIDTH*R_RES_HEIGHT*3/2,1,vcrightfp);
	fclose(vcleftfp);
	fclose(vcrightfp);
	/*
	/ read the bin file which size is equals 2048 byte.
	*/
	//FILE * binfile = fopen("E:\\ConsoleApplication1\\Release\\data.bin","rb+");
	FILE * binfile = fopen(binFilePath,"rb+");
	fread(byteResult,BIN_SIZE,1,binfile);
	fclose(binfile);
	
	ASVLOFFSCREEN vleftoffscreen;
	ASVLOFFSCREEN vrightoffscreen;

	memset( &vleftoffscreen, 0, sizeof(ASVLOFFSCREEN) );
	memset( &vrightoffscreen, 0, sizeof(ASVLOFFSCREEN) );

	vleftoffscreen.i32Width = L_RES_WIDTH;
	vleftoffscreen.i32Height = L_RES_HEIGHT;
	vleftoffscreen.u32PixelArrayFormat = ASVL_PAF_NV21;
	vleftoffscreen.pi32Pitch[0] = L_RES_WIDTH;
	vleftoffscreen.pi32Pitch[1] = L_RES_WIDTH;
	vleftoffscreen.ppu8Plane[0] = (MUInt8*)malloc(L_RES_WIDTH*L_RES_HEIGHT*3/2);
	vleftoffscreen.ppu8Plane[1] = vleftoffscreen.ppu8Plane[0] + L_RES_WIDTH*L_RES_HEIGHT;
	memcpy(vleftoffscreen.ppu8Plane[0],vleftnv21,L_RES_WIDTH*L_RES_HEIGHT*3/2);

	vrightoffscreen.i32Width = R_RES_WIDTH;
	vrightoffscreen.i32Height = R_RES_HEIGHT;
	vrightoffscreen.u32PixelArrayFormat = ASVL_PAF_NV21;
	vrightoffscreen.pi32Pitch[0] = R_RES_WIDTH;
	vrightoffscreen.pi32Pitch[1] = R_RES_WIDTH;
	vrightoffscreen.ppu8Plane[0] = (MUInt8*)malloc(R_RES_WIDTH*R_RES_HEIGHT*3/2);
	vrightoffscreen.ppu8Plane[1] = vrightoffscreen.ppu8Plane[0] + R_RES_WIDTH*R_RES_HEIGHT;
	memcpy(vrightoffscreen.ppu8Plane[0],vrightnv21,R_RES_WIDTH*R_RES_HEIGHT*3/2);

	ArcStereoImageData imagedata;
	memset( &imagedata, 0, sizeof(ArcStereoImageData) );
	imagedata.chessboardWidth = mapWidth;//@shawn //20
	imagedata.chessboardHeight = mapHight; //@shawn  12和20是方格数量，在产线上面要根据实际情况填写//12
	imagedata.leftImg = &vleftoffscreen;
	imagedata.rightImg = &vrightoffscreen;
	//MDouble errs[VERIFY_RES_SIZE] = {0};
    int	ret = MC_ModuleVerification(m_hDPCTEngine,&imagedata,byteResult,Errs,device);
	if( ret== MOK )
	{
		printf("the result :");//errs[VERIFY_INDEX_AVERAGE_ERR] is avg error. errs[VERIFY_INDEX_MAX_ERR] is max error. errs[VERIFY_INDEX_ERR_RANGE] is error range
	}
	if(m_hDPCTEngine)
	{
		MC_DestroyEngine(m_hDPCTEngine);
		m_hDPCTEngine= NULL;
	}
	free(vleftoffscreen.ppu8Plane[0]);
	free(vrightoffscreen.ppu8Plane[0]);
	vleftoffscreen.ppu8Plane[0] = NULL;
	vrightoffscreen.ppu8Plane[0] = NULL;
	/*
	/ free your vleftnv21,vrightnv21
	*/
	free(vleftnv21);
	free(vrightnv21);
	vleftnv21 = NULL;
	vrightnv21 = NULL;
    return ret;
}
extern "C" _declspec(dllexport)  int arcsoft_DualCamVerify_RGB888(char leftImgPath[], char rightImgPath[],char binFilePath[],double Errs[],int mapWidth,int mapHight,int L_RES_WIDTH,int L_RES_HEIGHT,int R_RES_WIDTH,int R_RES_HEIGHT)
{   
	char * device = "deviceID";
	MC_ENGINE m_hDPCTEngine = NULL;
	m_hDPCTEngine = MC_CreateEngine();
	MByte byteResult[BIN_SIZE] = {0};
	MUInt8* vleftRGB = NULL; 
	MUInt8* vrightRGB = NULL; 
    /*
	/ malloc mem for vleftRGB and vrightRGB,then get rgb data from your image
	*/
	vleftRGB = (MUInt8*)malloc(L_RES_WIDTH*L_RES_HEIGHT*3);
	vrightRGB = (MUInt8*)malloc(R_RES_WIDTH*R_RES_HEIGHT*3);
	FILE * vcleftfp = fopen(leftImgPath,"rb");
	FILE * vcrightfp = fopen(rightImgPath,"rb");
	fread(vleftRGB,L_RES_WIDTH*L_RES_HEIGHT*3,1,vcleftfp);
	fread(vrightRGB,R_RES_WIDTH*R_RES_HEIGHT*3,1,vcrightfp);
	fclose(vcleftfp);
	fclose(vcrightfp);

	/*
	/ read the bin file which size is equals 2048 byte.
	*/
	FILE * binfile = fopen(binFilePath,"rb+");
	//FILE * binfile = fopen("E:\\ConsoleApplication1\\Release\\data.bin","rb+");
	fread(byteResult,BIN_SIZE,1,binfile);
	fclose(binfile);
	
	ASVLOFFSCREEN vleftoffscreen;
	ASVLOFFSCREEN vrightoffscreen;

	memset( &vleftoffscreen, 0, sizeof(ASVLOFFSCREEN) );
	memset( &vrightoffscreen, 0, sizeof(ASVLOFFSCREEN) );

	vleftoffscreen.i32Width = L_RES_WIDTH;
	vleftoffscreen.i32Height = L_RES_HEIGHT;
	vleftoffscreen.u32PixelArrayFormat = ASVL_PAF_RGB24_R8G8B8;
	vleftoffscreen.pi32Pitch[0] = L_RES_WIDTH*3;

	vleftoffscreen.ppu8Plane[0] = (MUInt8*)malloc(L_RES_WIDTH*L_RES_HEIGHT*3);
	memcpy(vleftoffscreen.ppu8Plane[0],vleftRGB,L_RES_WIDTH*L_RES_HEIGHT*3);

	vrightoffscreen.i32Width = R_RES_WIDTH;
	vrightoffscreen.i32Height = R_RES_HEIGHT;
	vrightoffscreen.u32PixelArrayFormat = ASVL_PAF_RGB24_R8G8B8;
	vrightoffscreen.pi32Pitch[0] = R_RES_WIDTH*3;
	vrightoffscreen.ppu8Plane[0] = (MUInt8*)malloc(R_RES_WIDTH*R_RES_HEIGHT*3);
	memcpy(vrightoffscreen.ppu8Plane[0],vrightRGB,R_RES_WIDTH*R_RES_HEIGHT*3);

	ArcStereoImageData imagedata;
	memset( &imagedata, 0, sizeof(ArcStereoImageData) );
	imagedata.chessboardHeight = mapHight; //@shawn  12 //int mapWidth,int mapHight
	imagedata.chessboardWidth = mapWidth;  //@shawn  20 是方格数量，在产线上面要根据实际情况填写
	imagedata.leftImg = &vleftoffscreen;
	imagedata.rightImg = &vrightoffscreen;
	//MDouble errs[VERIFY_RES_SIZE] = {0};
	int ret = MC_ModuleVerification(m_hDPCTEngine,&imagedata,byteResult,Errs,device);
	if( ret== MOK )
	{
		printf("the result :");//errs[VERIFY_INDEX_AVERAGE_ERR] is avg error. errs[VERIFY_INDEX_MAX_ERR] is max error. errs[VERIFY_INDEX_ERR_RANGE] is error range
	}
	if(m_hDPCTEngine)
	{
		MC_DestroyEngine(m_hDPCTEngine);
		m_hDPCTEngine= NULL;
	}
	free(vleftoffscreen.ppu8Plane[0]);
	free(vrightoffscreen.ppu8Plane[0]);
	vleftoffscreen.ppu8Plane[0] = NULL;
	vrightoffscreen.ppu8Plane[0] = NULL;
	/*
	/ free your vleftRGB,vrightRGB
	*/
	free(vleftRGB);
	free(vrightRGB);
	vleftRGB = NULL;
	vrightRGB = NULL;
	return ret;
}
/*extern "C" _declspec(dllexport)  int arcsoft_DualCamVerify_JPEG(char leftImgPath[], char rightImgPath[],char binFilePath[],double Errs[],int mapWidth,int mapHight)
{   
	char * device = "deviceID";
	MC_ENGINE m_hDPCTEngine = NULL;
	m_hDPCTEngine = MC_CreateEngine();

	MByte byteResult[BIN_SIZE] = {0};

	FILE * binfile = fopen(binFilePath,"rb+");
	fread(byteResult,BIN_SIZE,1,binfile);
	fclose(binfile);

	ArcStereoImageData imagedata;
	memset( &imagedata, 0, sizeof(ArcStereoImageData) );
	imagedata.chessboardHeight = mapHight; //@shawn  //14
	imagedata.chessboardWidth = mapWidth; //19 //@shawn  14和19是图片方格数量，要根据实际情况填写
	imagedata.leftImgPath = leftImgPath;	//BMP or JPEG file path
	imagedata.rightImgPath = rightImgPath;	//BMP or JPEG file path

	//MDouble errs[VERIFY_RES_SIZE] ={0};
	int ret = MC_ModuleVerification(m_hDPCTEngine,&imagedata,byteResult,Errs,device);
	if( ret== MOK )
	{
		printf("the result :");//errs[VERIFY_INDEX_AVERAGE_ERR] is avg error. errs[VERIFY_INDEX_MAX_ERR] is max error. errs[VERIFY_INDEX_ERR_RANGE] is error range
	}
	if(m_hDPCTEngine)
	{
		MC_DestroyEngine(m_hDPCTEngine);
		m_hDPCTEngine= NULL;
	}	
	return ret;
}*/

extern "C" _declspec(dllexport)  int arcsoft_DualCamVerify_JPEG(char leftImgPath[], char rightImgPath[],char binFilePath[],double Errs[],int mapWidth,int mapHight)
{   
	char * device = "deviceID";

	MV_WTENGINE m_hDPCTEngine = NULL;

	m_hDPCTEngine = MVWT_CreateEngine();

	MByte byteResult[BIN_SIZE] = {0};

	/*
	/ read the bin file which size is equals 2048 byte.
	*/
	//FILE * binfile = fopen("E:\\ConsoleApplication1\\Release\\data.bin","rb+");
	FILE * binfile = fopen(binFilePath,"rb+");
	fread(byteResult,BIN_SIZE,1,binfile);
	fclose(binfile);
	
	ArcWTVerifystereoImageData imagedata;
	memset( &imagedata, 0, sizeof(ArcWTVerifystereoImageData) );
	imagedata.chessboardHeight = mapHight; 
	imagedata.chessboardWidth = mapWidth;
	imagedata.leftImgPath = leftImgPath;//"E:\\ConsoleApplication1\\Release\\vvll_4280x3120.bmp";	    //or .jpg, tele-image
	imagedata.rightImgPath = rightImgPath;//"E:\\ConsoleApplication1\\Release\\vvrr_4280x3120.bmp";	//or .jpg, wide-image
	MDouble avgerr = 0;
	MDouble maxerr= 0;
	MDouble errRange = 0;
	int ret = MVWT_ModuleVerification (m_hDPCTEngine,&imagedata,byteResult,&avgerr,&maxerr,&errRange, device);
	//int ret = MC_ModuleVerification(m_hDPCTEngine,&imagedata,byteResult,Errs,device);
	Errs[0] = avgerr;
	Errs[1] = maxerr;
	Errs[2] = errRange;
	if( ret== MOK ){
		printf("the result :");
	}

	if(m_hDPCTEngine)
	{
		MVWT_DestroyEngine (m_hDPCTEngine);
		m_hDPCTEngine= NULL;
	}

	return ret;
}
void Add(ftm_rf_test_property_data &req, int type,int value)
{
	if ( req.num_properties < 256)
	{
		req.properties[req.num_properties].type = type;
		req.properties[req.num_properties].value = static_cast <long long>(value);
		req.num_properties++;
	}
}
void  PopulateTxControlProperties(ftm_rf_test_property_data &requestData,int array[])
{
	requestData.num_properties = 0;
	Add( requestData, FTM_RF_TEST_RADIO_CFG_PROP_RX_CARRIER, array[0]); //0
	Add( requestData, FTM_RF_TEST_RADIO_CFG_PROP_RFM_DEVICE_PRI, array[1]);//2
	Add( requestData, FTM_RF_TEST_RADIO_CFG_PROP_SIG_PATH, array[2]);//6
	Add( requestData, FTM_RF_TEST_RADIO_CFG_PROP_ANT_PATH, array[3]);//8
	Add( requestData, FTM_RF_TEST_RADIO_CFG_PROP_PLL_ID, array[4]);//0


	Add( requestData, FTM_RF_TEST_RADIO_CFG_PROP_RFM_DEVICE_PRI, array[5]);//3
	Add( requestData, FTM_RF_TEST_RADIO_CFG_PROP_SIG_PATH, array[6]);//7
	Add( requestData, FTM_RF_TEST_RADIO_CFG_PROP_ANT_PATH, array[7]);//12
	Add( requestData, FTM_RF_TEST_RADIO_CFG_PROP_PLL_ID, array[8]);//0

	Add( requestData, FTM_RF_TEST_RADIO_CFG_PROP_BAND, array[9]);//1
	Add( requestData, FTM_RF_TEST_RADIO_CFG_PROP_CHANNEL, array[10]);//299  DL
	Add( requestData, FTM_RF_TEST_RADIO_CFG_PROP_BANDWIDTH, array[11]);//3  
	Add( requestData, FTM_RF_TEST_RADIO_CFG_PROP_BAND, array[12]);//1
}

void PopulateRxMeasureProperties(ftm_rf_test_property_data &requestData,int array[])
{
	requestData.num_properties = 0;
	Add( requestData, FTM_RX_MEASURE_PROP_RX_CARRIER, array[0]);  //0
	Add( requestData, FTM_RX_MEASURE_PROP_RFM_DEVICE, array[1]);//2
	Add( requestData, FTM_RX_MEASURE_PROP_EXPECTED_AGC, array[2]*10);//-850
	Add( requestData, FTM_RX_MEASURE_PROP_RX_AGC, 0);//0
	Add( requestData, FTM_RX_MEASURE_PROP_NUM_AVERAGES, array[3]);//10
}
extern "C" _declspec(dllexport) int  LTERadioTeardown(HANDLE hResourceContext)
{

	const int WIDTH = 15;
	unsigned long error_code;
	unsigned long specificErrCode = 0;
	ftm_rf_test_property_data requestData;
	ftm_rf_test_property_data responseData;
	unsigned char status = 0;
	requestData.num_properties = 0;

	status = QLIB_FTM_RF_TEST_CMD_RADIO_CONFIGURE(hResourceContext,
		0,
		3,
		static_cast<void*>(&requestData),
		static_cast<void*>(&responseData),
		&error_code,
		&specificErrCode );

	return 0;
}
extern "C" _declspec(dllexport) int GetLTEMIMORxValue(HANDLE m_Phone, int array[] ,int RxAGC[])
{
	unsigned long error_code = 1;
	unsigned long specificErrCode = 1;
	ftm_rf_test_property_data requestData;
	ftm_rf_test_property_data responseData;
	unsigned char numBlobs;
	ftm_rf_test_blob_data responseBlobData[1]; 
	PopulateRxMeasureProperties( requestData,array);
	unsigned int status = QLIB_FTM_RF_TEST_CMD_RX_MEASURE_AND_FETCH_IQ_DATA(m_Phone,
		0,
		3,
		(void *)&requestData,
		(void *)&responseData,
		&numBlobs,
		(void *)responseBlobData,
		&error_code,
		&specificErrCode );

		for( unsigned int propIdx = 0; propIdx < responseData.num_properties; propIdx++ )
		{
			if( responseData.properties[propIdx].type == FTM_RX_MEASURE_PROP_RX_AGC )
			{
				RxAGC[0] = static_cast<short>( responseData.properties[propIdx].value );
				break;
			}
		}
	  return 0;
}
extern "C" _declspec(dllexport) int SetLTEMIMOConfigure(HANDLE m_Phone,int array[])
{

	ftm_rf_test_property_data requestData;
	ftm_rf_test_property_data responseData;
	unsigned long error_code= 1;
	unsigned long specificErrCode = 1;
	PopulateTxControlProperties(requestData,array);
	unsigned int status = QLIB_FTM_RF_TEST_CMD_RADIO_CONFIGURE(m_Phone,
		0,
		3,
		static_cast<void*>(&requestData),
		static_cast<void*>(&responseData),
		&error_code,
		&specificErrCode );

	 // RxAGC = 
	return 0;
}

//extern "C" _declspec(dllexport) int FTM_RF_Atlas_HE(HANDLE m_Phone,int array[],int retarray[],char* ErrMsg)
//{
//	strcpy(ErrMsg,"hello");
//	g_hResourceContext = m_Phone;
//	return Test_FTM_RF_Atlas_HE(m_Phone,array,retarray,ErrMsg);
//}
//
//extern "C" _declspec(dllexport) int  FTM_RF_RadioTeardown(HANDLE hResourceContext,int Tech)
//{
//
//	const int WIDTH = 15;
//	unsigned long error_code;
//	unsigned long specificErrCode = 0;
//	ftm_rf_test_property_data requestData;
//	ftm_rf_test_property_data responseData;
//	unsigned char status = 0;
//	requestData.num_properties = 0;
//
//	status = QLIB_FTM_RF_TEST_CMD_RADIO_CONFIGURE(hResourceContext,
//		0,
//		Tech,
//		static_cast<void*>(&requestData),
//		static_cast<void*>(&responseData),
//		&error_code,
//		&specificErrCode );
//
//	return status;
//}
