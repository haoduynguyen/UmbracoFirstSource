using System;
using System.IO;

namespace Customs.Helper
{
    /// <summary>
    /// A general purpose logging facility.
    /// </summary>
    public static class Log
    {
        #region Initialization
        static Log ()
        {
            Prefix = "[Update] ";
            BaseDirectory = System.Web.HttpContext.Current.Server.MapPath("~/App_Data/");
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Log"/> logs to the console.
        /// </summary>
        /// <value><c>true</c> if console; otherwise, <c>false</c>.</value>
        public static bool Console { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Log"/> logs using the debug facilty.
        /// </summary>
        /// <value><c>true</c> if debug; otherwise, <c>false</c>.</value>
        public static bool Debug { get; set; }

        /// <summary>
        /// Gets or sets the prefix.
        /// </summary>
        /// <value>The prefix.</value>
        public static string Prefix { get; set; }

        #endregion

        #region Events
        /// <summary>
        /// Occurs when an event occurs.
        /// </summary>
        public static event EventHandler<LogEventArgs> Event;

        /// <summary>
        /// Called when an event occurs.
        /// </summary>
        /// <param name="message">The message.</param>
        private static void OnEvent (string message)
        {
            if(Event != null)
                Event(null, new LogEventArgs(message));
        }

        public static string BaseDirectory { get; set; }

        #endregion

        #region Methods
        /// <summary>
        /// Writes to the log.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="args">The arguments.</param>
        public static void Write (string format, params object[] args)
        {
            try
            {
                string message = string.Format(format, args);
                OnEvent(message);
                using (StreamWriter sw = new StreamWriter(BaseDirectory + "\\Logs\\TraceLog_" + DateTime.Now.ToString("yyyy-MM-dd") + ".txt", true))
                {
                    sw.WriteLine(DateTime.Now.ToString() + ": " + message);
                    sw.Flush();
                    sw.Close();
                }
            }
            catch (Exception ex)
            {
            }

            /*
            OnEvent(message);
            
            if (Console)
                System.Console.WriteLine(message);

            if (Debug)
                System.Diagnostics.Debug.WriteLine(message);
            */
        }
        public static void Write(Exception ex)
        {
            
            bool exists = System.IO.Directory.Exists(BaseDirectory + "\\Logs\\");
            if (!exists)
                System.IO.Directory.CreateDirectory(BaseDirectory + "\\Logs\\");

            using (StreamWriter sw = new StreamWriter(BaseDirectory + "\\Logs\\" + DateTime.Now.ToString("yyyy-MM-dd") + ".txt", true))
            {

                sw.WriteLine(DateTime.Now.ToString() + ": " + ex.Source.ToString().Trim() + "; " + ex.Message.ToString().Trim());
                sw.Flush();
                sw.Close();
            }
        }
        public static void Write(string Message)
        {
            
            bool exists = System.IO.Directory.Exists(BaseDirectory + "\\Logs\\");
            if (!exists)
                System.IO.Directory.CreateDirectory(BaseDirectory + "\\Logs\\");

            using (StreamWriter sw = new StreamWriter(BaseDirectory + "\\Logs\\TraceLog_" + DateTime.Now.ToString("yyyy-MM-dd") + ".txt", true))
            {
                sw.WriteLine(DateTime.Now.ToString() + ": " + Message);
                sw.Flush();
                sw.Close();
            }
        }
        #endregion
    }
    public class LogEventArgs : EventArgs
    {
        #region Initialization
        /// <summary>
        /// Initializes a new instance of the <see cref="LogEventArgs"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public LogEventArgs(string message)
        {
            Message = message;
            TimeStamp = DateTime.Now;
        }
        #endregion

        #region Proprties
        /// <summary>
        /// Gets the time stamp.
        /// </summary>
        /// <value>The time stamp.</value>
        public DateTime TimeStamp { get; private set; }

        /// <summary>
        /// Gets the message.
        /// </summary>
        /// <value>The message.</value>
        public string Message { get; private set; }
        #endregion
    }
}
