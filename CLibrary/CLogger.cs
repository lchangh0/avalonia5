
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Extensions.Logging;
using NLog.LayoutRenderers.Wrappers;
using Tmds.DBus.Protocol;

namespace CLibray;

public enum ELogLevel { Trace, Debug, Info, Warn, Error, Critical, Fatal };

public class CLogger
{

    public static void Initialize()
    {
        // NLog 설정 로드
        var config = new NLog.Config.LoggingConfiguration();
        config.AddTarget(new NLog.Targets.ConsoleTarget("console"));
        config.AddTarget(new NLog.Targets.FileTarget("file")
        {
            FileName = "${basedir}/logs/${shortdate}.log",
            Layout = "${longdate}|${level:uppercase=true}|${logger}|${message} ${exception:format=tostring}",
            ArchiveEvery = NLog.Targets.FileArchivePeriod.Day,
            //ArchiveNumbering = NLog.Targets.ArchiveNumberingMode.Sequence,
            MaxArchiveFiles = 7,
            KeepFileOpen = true,
            Encoding = System.Text.Encoding.UTF8
        });
        config.AddRuleForAllLevels("console");
        config.AddRuleForAllLevels("file");
        NLog.LogManager.Configuration = config;

        // Microsoft.Extensions.Logging과 NLog 통합 (loggerFactory가 필요 없다면 아래 줄 삭제해도 무방) 
        var loggerFactory = LoggerFactory.Create(builder =>
        {
            builder.ClearProviders();
            builder.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
            builder.AddNLog(config);
        });
    }

    NLog.Logger _logger;

    public CLogger(string name)
    {
        _logger = LogManager.GetLogger(name);
    }

    public void Log(string message, ELogLevel logLevel = ELogLevel.Info)
    {
        NLog.LogLevel nLogLevel = NLog.LogLevel.Off;
        if (logLevel == ELogLevel.Trace) nLogLevel = NLog.LogLevel.Trace;
        else if (logLevel == ELogLevel.Debug) nLogLevel = NLog.LogLevel.Debug;
        else if (logLevel == ELogLevel.Info) nLogLevel = NLog.LogLevel.Info;
        else if (logLevel == ELogLevel.Warn) nLogLevel = NLog.LogLevel.Warn;
        else if (logLevel == ELogLevel.Error) nLogLevel = NLog.LogLevel.Error;
        else if (logLevel == ELogLevel.Fatal) nLogLevel = NLog.LogLevel.Fatal;

        _logger.Log(nLogLevel, message);
    }

}