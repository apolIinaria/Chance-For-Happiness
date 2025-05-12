using System;
using System.IO;

namespace ChanceForHappiness.Services
{
    /// <summary>
    /// Сервіс для логування подій, попереджень та помилок додатку.
    /// Записує повідомлення у щоденні лог-файли для відстеження роботи програми.
    /// </summary>
    public class LoggingService
    {
        private readonly string _logFilePath;

        public LoggingService()
        {
            string logDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs");

            if (!Directory.Exists(logDirectory))
            {
                Directory.CreateDirectory(logDirectory);
            }

            string fileName = $"Log_{DateTime.Now:yyyy-MM-dd}.txt";
            _logFilePath = Path.Combine(logDirectory, fileName);

            Log("Logging service ініціалізовано");
        }

        public void Log(string message)
        {
            WriteToLogFile("ІНФО", message);
        }

        public void LogWarning(string message)
        {
            WriteToLogFile("УВАГА", message);
        }

        public void LogError(string message, Exception exception = null)
        {
            string errorMessage = message;

            if (exception != null)
            {
                errorMessage += $" | Виняток: {exception.GetType().Name} | Повідомленн: {exception.Message}";

                if (exception.StackTrace != null)
                {
                    errorMessage += $" | Відстежування стеку: {exception.StackTrace}";
                }
            }

            WriteToLogFile("Помилка", errorMessage);
        }

        private void WriteToLogFile(string level, string message)
        {
            try
            {
                string logEntry = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] [{level}] {message}";

                lock (this)
                {
                    using (StreamWriter writer = new StreamWriter(_logFilePath, true))
                    {
                        writer.WriteLine(logEntry);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Не вдалося записати до лог-файлу: {ex.Message}");
                Console.WriteLine($"Початкове повідомлення: [{level}] {message}");
            }
        }
    }
}