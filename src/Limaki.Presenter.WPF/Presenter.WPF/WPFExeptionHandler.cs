using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Limaki.Common;
using System.Diagnostics;

namespace Limaki.Presenter.WPF {
    public class WPFExeptionHandler : IExceptionHandler {

       
        public void Catch(Exception e) {
            Catch (e, MessageType.OK);
        }


        public void Catch(Exception e, MessageType message) {
#if SILVERLIGHT
            try {
                string errorMsg = e.Message + e.StackTrace;
                errorMsg = errorMsg.Replace('"', '\'').Replace("\r\n", @"\n");

                System.Windows.Browser.HtmlPage.Window.Eval("throw new Error(\"Unhandled Error in Silverlight 2 Application " + errorMsg + "\");");
            } catch (Exception) {
            }
#else
            System.Console.WriteLine (e.Message);
            Debug.WriteLine (e.Message);
#endif
        }

       
    }
}
