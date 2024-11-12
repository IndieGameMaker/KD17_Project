using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;

namespace Sym4D
{
    public unsafe class Sym4DEmulator : MonoBehaviour
    {
        const string Sym4D_Dll = "Sym4D.dll";

        [DllImport(Sym4D_Dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern int Sym4D_X_Find();                                                // Sym4D-X100 장비가 연결되어 있는 Serial Port를 찾는 함수		리턴값의 정의  예) 25 -> 장비가 연결된 Serial Port = COM25 

        [DllImport(Sym4D_Dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern int Sym4D_W_Find();                                                // Sym4D-W100 장비가 연결되어 있는 Serial Port를 찾는 함수		리턴값의 정의  예) 25 -> 장비가 연결된 Serial Port = COM25 

        [DllImport(Sym4D_Dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern int Sym4D_X_Multi_Find(Byte* aPortData);                           // Sym4D-X100 장비가 연결되어 있는 Serial Port를 찾는 함수		리턴값의 정의  예) 2 -> 연결된 장비의 개수 2개 
                                                                                                // *aPortData 리턴값의 정의  예) 장비가 연결된 Serial Port의 배열 1Byte당 포트 번호 1개

        [DllImport(Sym4D_Dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern int Sym4D_W_Multi_Find(Byte* aPortData);                           // Sym4D-W100 장비가 연결되어 있는 Serial Port를 찾는 함수		리턴값의 정의  예) 2 -> 연결된 장비의 개수 2개 
                                                                                                // *aPortData 리턴값의 정의  예) 장비가 연결된 Serial Port의 배열 1Byte당 포트 번호 1개

        [DllImport(Sym4D_Dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool Sym4D_X_Init();                                               // Sym4D-X100 장치의 포지션을 중심으로 이동

        [DllImport(Sym4D_Dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool Sym4D_W_Init();                                               // Sym4D-W100 장치의 풍량을 0로 초기화

        [DllImport(Sym4D_Dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool Sym4D_X_SetConfig(int mRoll, int mPitch);                     // Sym4D-X100 장치의 최대 허용 각도를 설정  : mRoll, mPitch의 범위 = 0 ~ 100 (0도 ~ 10도)

        [DllImport(Sym4D_Dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool Sym4D_W_SetConfig(int mWind);                                 // Sym4D-W100 장치의 최대 풍량 설정  : mWind 범위 = 0 ~ 100 (0% ~ 100%)

        [DllImport(Sym4D_Dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool Sym4D_X_SendMosionData(int mRoll, int mPitch);                // Sym4D-X100 장치에 모션 Data를 전달  : mRoll, mPitch의 범위 = -100 ~ 100 (-10도 ~ 10도)

        [DllImport(Sym4D_Dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool Sym4D_W_SendMosionData(int mWind);                            // Sym4D-W100 장치에 바람 Data를 전달  : mWind의 범위 = -100 ~ 100 (-100% ~ 100%)

        [DllImport(Sym4D_Dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool Sym4D_X_StartContents(int sComPortName);                      // Sym4D-X100 COM Port Open  및 컨텐츠 시작을 장치에 전달		sComPortName 인자의 예) COM5 -> 5, COM17 -> 17

        [DllImport(Sym4D_Dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool Sym4D_W_StartContents(int sComPortName);                      // Sym4D-W100 COM Port Open  및 컨텐츠 시작을 장치에 전달		sComPortName 인자의 예) COM5 -> 5, COM17 -> 17

        [DllImport(Sym4D_Dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool Sym4D_X_EndContents();                                        // Sym4D-X100 COM Port Close 및 컨텐츠 종료를 장치에 전달

        [DllImport(Sym4D_Dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool Sym4D_W_EndContents();                                        // Sym4D-W100 COM Port Close 및 컨텐츠 종료를 장치에 전달

        [DllImport(Sym4D_Dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern int Sym4D_API_Version();                                           // Dll API 모듈의 버전 정보			리턴값의 정의  예) 1000 = Ver 1.00.0      10000 = Ver 10.00.0

        [DllImport(Sym4D_Dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern int Sym4D_TotalCounter();                                          // Sym4D-X100 누적 동작 카운트 정보

        [DllImport(Sym4D_Dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool Sym4D_X_Reset();                                              // Sym4D-X Board Reset

        [DllImport(Sym4D_Dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool Sym4D_X_SendMosionData2(int mRoll, int mPitch, int mSpeed);   // Sym4D-X 장치에 모션 Data를 전달  : mRoll, mPitch의 범위 = -100 ~ 100 (-10도 ~ 10도), mSpeed의 범위  = 0 ~ 100

        /****************************************************************************************/
        /*                                 New Type Board Only                                  */
        /****************************************************************************************/

        [DllImport(Sym4D_Dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool Sym4D_X_New_SendYaw(int mYaw, int mSpeed, int mDirection);     // Sym4D-X 장치에 Yaw 모션 Data를 전달  : mYaw의 범위 = 0 ~ 360 (0도 ~ 360도), mSpeed의 범위  = 0 ~ 100, mDirection의 범위  = 0(좌회전) ~ 1(우회전)

        [DllImport(Sym4D_Dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool Sym4D_X_New_SendWind(int mWind1, int mWind2);                  // Sym4D-X 장치에 바람 Data를 전달  : mWind의 범위 = -100 ~ 100 (-100% ~ 100%)

        [DllImport(Sym4D_Dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool Sym4D_X_New_SendVibration(int mVib);                           // Sym4D-X 장치에 진동 Data를 전달  : mVib의 범위 = -100 ~ 100 (-100% ~ 100%)

        [DllImport(Sym4D_Dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool Sym4D_X_New_SendHeave(int mHeave, int mSpeed);                 // Sym4D-X 장치에 Heave Data를 전달  : mHeave의 범위 = -100 ~ 100 (-100% ~ 100%), mSpeed의 범위  = 0 ~ 100

        [DllImport(Sym4D_Dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool Sym4D_X_New_TotalCtrl(int mPitch, int mRoll, int mSpeed1,      // Sym4D-X 장치에 모든 옵션의 모션 Data를 전달
                                            int mYaw, int mSpeed2, int mDirection,
                                            int mWind1, int mWind2, int mVib);

        [DllImport(Sym4D_Dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern uint Sym4D_X_New_RunCounterRead();                                 // Sym4D-X 누적 동작 카운트 정보

        [DllImport(Sym4D_Dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool Sym4D_X_New_RunCounterInit();                                 // Sym4D-X 누적 동작 카운트 정보 초기화

        [DllImport(Sym4D_Dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool Sym4D_X_New_RS232Reset();                                     // Sym4D-X 누적 통신 포트 초기화

        [DllImport(Sym4D_Dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool Sym4D_X_New_Multi_SendMosionData(int mGroupId,                // Sym4D-X 병렬 제어 모션 Data를 전달  : mGroupId의 범위 = 0 ~ 999 (0 = 전체 그룹 제어), mDeviceId의 범위  = 0 ~ 999, (0 = 그에 속한 전체 장치 제어)
                                                        int mDeviceId,
                                                        int mRoll,
                                                        int mPitch,
                                                        int mSpeed);

        [DllImport(Sym4D_Dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool Sym4D_X_New_Multi_SendYaw(int mGroupId, int mDeviceId,        // Sym4D-X 병렬 제어 Yaw 모션 Data를 전달
                                                int mYaw, int mSpeed,
                                                int mDirection);

        [DllImport(Sym4D_Dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool Sym4D_X_New_Multi_SendHeave(int mGroupId, int mDeviceId,      // Sym4D-X 병렬 제어 Heave 모션 Data를 전달
                                                    int mHeave, int mSpeed);

        [DllImport(Sym4D_Dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool Sym4D_X_New_Multi_SendWind(int mGroupId, int mDeviceId,       // Sym4D-X 병렬 제어 바람 모션 Data를 전달
                                                int mWind1, int mWind2);

        [DllImport(Sym4D_Dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool Sym4D_X_New_Multi_SendVibration(int mGroupId,                 // Sym4D-X 병렬 제어 진동 모션 Data를 전달
                                                        int mDeviceId, int mVib);

        [DllImport(Sym4D_Dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool Sym4D_X_New_Multi_TotalCtrl(int mGroupId, int mDeviceId,      // Sym4D-X 병렬 제어 모든 옵션의 모션 Data를 전달
                                                    int mPitch, int mRoll,
                                                    int mSpeed1, int mYaw,
                                                    int mSpeed2, int mDirection,
                                                    int mWind1, int mWind2,
                                                    int mVib);

        [DllImport(Sym4D_Dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool Sym4D_X_New_Multi_Reset(int mGroupId, int mDeviceId);         // Sym4D-X 병렬 제어 Board Reset

        /****************************************************************************************/

    }
}