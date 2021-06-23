using Caliburn.Micro;
using System;
using System.Windows;

namespace PSMDesktopUI.Utils
{
    public class NLogLogger : ILog
    {
        private readonly NLog.Logger _logger;

        public NLogLogger(Type type)
        {
            _logger = NLog.LogManager.GetLogger(type.FullName);
        }

        public void Info(string format, params object[] args)
        {
            Write(NLog.LogLevel.Info, format, args);
        }

        public void Warn(string format, params object[] args)
        {
            Write(NLog.LogLevel.Warn, format, args);
        }

        public void Error(Exception exception)
        {
            Write(NLog.LogLevel.Error, exception.ToString());

            try
            {
                MessageBox.Show($"Terjadi error: { exception }", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                Write(NLog.LogLevel.Error, ex.ToString());
            }
        }

        private void Write(NLog.LogLevel level, string format, params object[] args)
        {
            NLog.LogEventInfo eventInfo = new NLog.LogEventInfo(level, _logger.Name, null, format, args);
            _logger.Log(typeof(NLogLogger), eventInfo);
        }
    }
}
