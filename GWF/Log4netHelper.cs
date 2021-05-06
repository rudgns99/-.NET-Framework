using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace GWF
{
    class Log4netHelper
    {
        /// <summary>
        /// log4net 처리 로거
        /// </summary>
        protected static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// log2net 초기화 처리
        /// </summary>
        public static void InitializeLog4net()
        {
            log4net.Config.XmlConfigurator.Configure();
        }

        /// <summary>
        /// 디버깅을 위한 출력 로그
        /// 예) 다양한 정보 출력
        /// </summary>
        /// <param name="ex">출력 대상 Exception</param>
        public static void Debug(ref Exception ex)
        {
            if (ex == null)
            {
                log.Debug("Trace nothing");
                return;
            }
            log.DebugFormat("Trace-----\nMSG : {0}\nSRC : {1}\nTGT : {2}\nSTK : {3}\n-----", ex.Message, ex.Source, ex.TargetSite, ex.StackTrace);
        }

        /// <summary>
        /// 일반적인 정보 출력 로그
        /// 예) 입력 값 정보 등
        /// </summary>
        /// <param name="ex">출력 대상 Exception</param>
        public static void Info(ref Exception ex)
        {
            if (ex == null)
            {
                log.Info("Trace nothing");
                return;
            }
            log.InfoFormat("Trace-----\nMSG : {0}\nSRC : {1}\nTGT : {2}\nSTK : {3}\n-----", ex.Message, ex.Source, ex.TargetSite, ex.StackTrace);
        }

        /// <summary>
        /// 처리할 수 없는 값 입력 오류인 경우 출력 로그
        /// 예) validation
        /// </summary>
        /// <param name="ex">출력 대상 Exception</param>
        public static void Warn(ref Exception ex)
        {
            if (ex == null)
            {
                log.Warn("Trace nothing");
                return;
            }
            log.WarnFormat("Trace-----\nMSG : {0}\nSRC : {1}\nTGT : {2}\nSTK : {3}\n-----", ex.Message, ex.Source, ex.TargetSite, ex.StackTrace);
        }

        /// <summary>
        /// 처리할 수 있지만 기능 오류인 경우 출력 로그
        /// 예) db 처리시 sp 실행 오류
        /// </summary>
        /// <param name="ex">출력 대상 Exception</param>
        public static void Error(ref Exception ex)
        {
            if (ex == null)
            {
                log.Error("Trace nothing");
                return;
            }
            log.ErrorFormat("Trace-----\nMSG : {0}\nSRC : {1}\nTGT : {2}\nSTK : {3}\n-----", ex.Message, ex.Source, ex.TargetSite, ex.StackTrace);
        }

        /// <summary>
        /// 처리할 수 있지만 기능 오류인 경우 출력 로그
        /// 예) db 처리시 sp 실행 오류
        /// </summary>
        /// <param name="ex">출력 대상 Exception</param>
        public static void Error(ref WebException ex)
        {
            if (ex == null)
            {
                log.Error("Trace nothing");
                return;
            }
            log.ErrorFormat("Trace-----\nMSG : {0}\nSRC : {1}\nTGT : {2}\nSTK : {3}\n-----", ex.Message, ex.Source, ex.TargetSite, ex.StackTrace);
        }

        /// <summary>
        /// 처리할 수 없는 오류의 경우 출력 로그
        /// 예) db 연결 오류
        /// </summary>
        /// <param name="ex">출력 대상 Exception</param>
        public static void Fatal(ref Exception ex)
        {
            if (ex == null)
            {
                log.Fatal("Trace nothing");
                return;
            }
            log.FatalFormat("Trace-----\nMSG : {0}\nSRC : {1}\nTGT : {2}\nSTK : {3}\n-----", ex.Message, ex.Source, ex.TargetSite, ex.StackTrace);
        }

        /// <summary>
        /// 디버깅을 위한 출력 로그
        /// 예) 다양한 정보 출력
        /// </summary>
        /// <param name="ex">출력 메시지</param>
        public static void Debug(string ex)
        {
            if (string.IsNullOrEmpty(ex))
            {
                log.Debug("Trace nothing");
                return;
            }
            log.Debug(ex);
        }

        /// <summary>
        /// 일반적인 정보 출력 로그
        /// 예) 입력 값 정보 등
        /// </summary>
        /// <param name="ex">출력 메시지</param>
        public static void Info(string ex)
        {
            if (string.IsNullOrEmpty(ex))
            {
                log.Info("Trace nothing");
                return;
            }
            log.Info(ex);
        }

        /// <summary>
        /// 처리할 수 없는 값 입력 오류인 경우 출력 로그
        /// 예) validation
        /// </summary>
        /// <param name="ex">출력 메시지</param>
        public static void Warn(string ex)
        {
            if (string.IsNullOrEmpty(ex))
            {
                log.Warn("Trace nothing");
                return;
            }
            log.Warn(ex);
        }

        /// <summary>
        /// 처리할 수 있지만 기능 오류인 경우 출력 로그
        /// 예) db 처리시 sp 실행 오류
        /// </summary>
        /// <param name="ex">출력 메시지</param>
        public static void Error(string ex)
        {
            if (string.IsNullOrEmpty(ex))
            {
                log.Error("Trace nothing");
                return;
            }
            log.Error(ex);
        }

        /// <summary>
        /// 처리할 수 없는 오류의 경우 출력 로그
        /// 예) db 연결 오류
        /// </summary>
        /// <param name="ex">출력 메시지</param>
        public static void Fatal(string ex)
        {
            if (string.IsNullOrEmpty(ex))
            {
                log.Fatal("Trace nothing");
                return;
            }
            log.Fatal(ex);
        }

        /// <summary>
        /// 디버깅을 위한 출력 로그
        /// 예) 다양한 정보 출력
        /// </summary>
        /// <param name="format">출력 형식</param>
        /// <param name="args">출력 값(목록)</param>
        public static void DebugFormat(string format, params object[] args)
        {
            if (string.IsNullOrEmpty(format))
            {
                log.Debug("Trace nothing");
                return;
            }
            log.DebugFormat(format, args);
        }

        /// <summary>
        /// 일반적인 정보 출력 로그
        /// 예) 입력 값 정보 등
        /// </summary>
        /// <param name="format">출력 형식</param>
        /// <param name="args">출력 값(목록)</param>
        public static void InfoFormat(string format, params object[] args)
        {
            if (string.IsNullOrEmpty(format))
            {
                log.Info("Trace nothing");
                return;
            }
            log.InfoFormat(format, args);
        }

        /// <summary>
        /// 처리할 수 없는 값 입력 오류인 경우 출력 로그
        /// 예) validation
        /// </summary>
        /// <param name="format">출력 형식</param>
        /// <param name="args">출력 값(목록)</param>
        public static void WarnFormat(string format, params object[] args)
        {
            if (string.IsNullOrEmpty(format))
            {
                log.Warn("Trace nothing");
                return;
            }
            log.WarnFormat(format, args);
        }

        /// <summary>
        /// 처리할 수 있지만 기능 오류인 경우 출력 로그
        /// 예) db 처리시 sp 실행 오류
        /// </summary>
        /// <param name="format">출력 형식</param>
        /// <param name="args">출력 값(목록)</param>
        public static void ErrorFormat(string format, params object[] args)
        {
            if (string.IsNullOrEmpty(format))
            {
                log.Error("Trace nothing");
                return;
            }
            log.ErrorFormat(format, args);
        }

        /// <summary>
        /// 처리할 수 없는 오류의 경우 출력 로그
        /// 예) db 연결 오류
        /// </summary>
        /// <param name="format">출력 형식</param>
        /// <param name="args">출력 값(목록)</param>
        public static void FatalFormat(string format, params object[] args)
        {
            if (string.IsNullOrEmpty(format))
            {
                log.Fatal("Trace nothing");
                return;
            }
            log.FatalFormat(format, args);
        }
    }
}
