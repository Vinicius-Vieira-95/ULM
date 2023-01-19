using System;

namespace UlmApi.Infra.CrossCutting.Logger
{
    public interface ILoggerManager
    {
        void LogError(string msg);

        void LogError(Exception exception);

        void LogError(string msg, object obj, Exception exception);

        void LogError(string msg, object obj);

        void LogInfo(string msg);

        void LogInfo(string msg, object obj);
    }
}
