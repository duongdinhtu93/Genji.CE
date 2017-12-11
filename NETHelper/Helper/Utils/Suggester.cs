using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;

namespace ApplicationCore.Helper.Utils
{
    public static class Suggester
    {
        public static void RegisterSuggester<T>(this T ctrl) where T : TextBox
        {
            if (!ctrl.IsNull())
            {
                new Thread(new ThreadStart(() =>
                {
                    frmSuggester frmS = new frmSuggester();
                    string currentText = ctrl.Text;
                    while (!ctrl.IsDisposed)
                    {
                        string newText = ctrl.Text;
                        if (newText != currentText)
                        {
                            if (!newText.IsNullOrEmpty())
                            {
                                ctrl.Invoke(new MethodInvoker(() =>
                                {
                                    currentText = newText;
                                    List<string> suggestWords = new List<string>();
                                    if (!ctrl.IsNull() && !ctrl.IsDisposed && !ctrl.ReadOnly && ctrl.Enabled
                                    && ctrl.Focused
                                    )
                                    {
                                        string keyword = newText;
                                        suggestWords = GetSuggestion(keyword).ToList();
                                        if (suggestWords.Any())
                                        {
                                            frmS.AppendData(suggestWords.ToArray());
                                            frmS.Show();
                                            ctrl.Focus();
                                        }
                                        else frmS.Hide();
                                    }
                                    else
                                        frmS.Hide();
                                }));

                            }
                            else frmS.Hide();
                        }

                        Thread.Sleep(500);
                    }
                    if (!frmS.IsDisposed)
                        frmS.Close();
                })).Start();
                //    frmSuggester frm = new frmSuggester() { TopMost = true };
                //    ctrl.Disposed += (sender, @event) =>
                //     {
                //         if (!frm.IsNull() && !frm.IsDisposed)
                //             frm.Close();
                //     };

                //    ctrl.LostFocus += (sender, @event) =>
                //    {
                //        try
                //        {
                //            if (!frm.Focused)
                //                frm.AppendData(null);
                //            else
                //            {
                //                ctrl.Focus();
                //            }
                //        }
                //        catch { }
                //    };


                //    ctrl.TextChanged += (sender, @event) =>
                //    {
                //        List<string> suggestWords = new List<string>();
                //        if (!ctrl.IsNull() && !ctrl.IsDisposed && !ctrl.ReadOnly && ctrl.Enabled && ctrl.Focused)
                //        {
                //            if (!ctrl.Text.IsNullOrEmpty())
                //            {
                //                string keyword = ctrl.Text;
                //                suggestWords = GetSuggestion(keyword).ToList();
                //                frm.AppendData(suggestWords.ToArray());
                //            }
                //            else
                //            {
                //                try
                //                {
                //                    frm.AppendData(null);
                //                }
                //                catch { }
                //            }
                //        }

                //    };
            }
        }

        private static string GoogleApiSuggestion = "http://suggestqueries.google.com/complete/search?client=chrome&q=";
        private static string GoogleApiSuggestion2 = "https://www.google.com/complete/search?output=toolbar&q=";
        public static string[] GetSuggestion(string keyword)
        {
            List<string> suggestWords = new List<string>();
            try
            {
                if (!keyword.IsNullOrEmpty())
                {
                    using (var responseStream = WebRequest.Create(GoogleApiSuggestion2 + keyword).GetResponse().GetResponseStream())
                    {
                        using (var streamReader = new StreamReader(responseStream, Encoding.GetEncoding("ISO-8859-9")))
                        {
                            using (var xmlReader = XmlReader.Create(streamReader))
                            {
                                while (xmlReader.Read())
                                {
                                    if (xmlReader.NodeType == XmlNodeType.Element)
                                    {
                                        Console.Write("<{0}>", xmlReader.Name);
                                        if (xmlReader.Name.Trim() == "suggestion")
                                            suggestWords.Add(xmlReader["data"]);
                                    }
                                }
                            }
                        }

                    }
                }
            }
            catch (Exception ex)
            {

            }

            return suggestWords.ToArray();
        }
    }
}
