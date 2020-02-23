
/*----------------------------------------------------------------------------------------------
*
* This file is ArcSoft's property. It contains ArcSoft's trade secret, proprietary and 		
* confidential information. 
* 
* The information and code contained in this file is only for authorized ArcSoft employees 
* to design, create, modify, or review.
* 
* DO NOT DISTRIBUTE, DO NOT DUPLICATE OR TRANSMIT IN ANY FORM WITHOUT PROPER AUTHORIZATION.
* 
* If you are not an intended recipient of this file, you must not copy, distribute, modify, 
* or take any action in reliance on it. 
* 
* If you have received this file in error, please immediately notify ArcSoft and 
* permanently delete the original and any copy of any file and any printout thereof.
*
*-------------------------------------------------------------------------------------------------*/

#ifndef ARCSOFT_MODULE_ERRORCOMMON_H
#define ARCSOFT_MODULE_ERRORCOMMON_H

#define MERR_SSC_ERROR_BASE 0x0000A000
#define MERR_SSC_NOT_ENOUGH_CHESSBOARD (MERR_SSC_ERROR_BASE+1)
#define MERR_SSC_CHESSBOARD_OVERLAPED (MERR_SSC_ERROR_BASE+2)
#define MERR_SSC_CAL_WITH_LARGE_ERROR (MERR_SSC_ERROR_BASE+3)
#define MERR_SSC_SAME_WIDTHHEIGHT (MERR_SSC_ERROR_BASE+4)
#define MERR_SSC_INPUT_ERROR (MERR_SSC_ERROR_BASE+5)
#define MERR_SSC_INVALID_FILE_NAME (MERR_SSC_ERROR_BASE+6)
#define MERR_SSC_UNEQUAL_IMAGE_NUMBERS (MERR_SSC_ERROR_BASE+7)
#define MERR_SSC_FAIL_VERIFICATION (MERR_SSC_ERROR_BASE+8)
#define MERR_SSC_LARGE_ERROR (MERR_SSC_ERROR_BASE+9)
#define MERR_SSC_NULL_POINT (MERR_SSC_ERROR_BASE + 10)
#define MERR_SSC_INVALID_IMAGE_TYPE (MERR_SSC_ERROR_BASE + 11)
#define MERR_SSC_BLURIMAGE (MERR_SSC_ERROR_BASE + 12)
#endif