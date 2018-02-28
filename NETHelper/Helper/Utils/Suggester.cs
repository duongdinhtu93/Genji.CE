using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;

namespace GenjiCore.Helper.Utils
{
    public static class Suggester
    {
        public static void RegisterSuggester<T>(this T ctrl, SuggestType suggestType = SuggestType.NaturalLanguage)
            where T : TextBox
        {
            if (!ctrl.IsNull())
            {
                new Thread(new ThreadStart(() =>
                {
                    frmSuggester frmS = new frmSuggester();
                    string currentText = ctrl.Text;
                    while (!ctrl.IsNull() && !ctrl.IsDisposed)
                    {
                        try
                        {
                            string newText = ctrl.Text;
                            if (newText != currentText)
                            {
                                if (!newText.IsNullOrEmpty())
                                {
                                    currentText = newText;
                                    List<string> suggestWords = new List<string>();
                                    string keyword = newText;
                                    switch (suggestType)
                                    {
                                        case SuggestType.Address:
                                            {
                                                var condinate = GenjiCore.Components.GPS.GeographyLocation.GetCoordinates(keyword);
                                                if (!condinate.IsNull() && !condinate.Address.IsNullOrEmpty())
                                                    suggestWords.Add(condinate.Address);
                                                break;
                                            }
                                        case SuggestType.NaturalLanguage:
                                            {
                                                suggestWords = GetSuggestion(keyword).ToList();
                                                break;
                                            }
                                    }

                                    ctrl.Invoke(new MethodInvoker(() =>
                                    {
                                        if (!ctrl.IsNull() && !ctrl.IsDisposed && !ctrl.ReadOnly && ctrl.Enabled
                                        && ctrl.Focused
                                        )
                                        {
                                            if (suggestWords.Any())
                                            {
                                                frmS.Location = new Point(ctrl.Top + ctrl.Height, ctrl.Left);
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
                            ctrl.Invoke(new MethodInvoker(() =>
                            {
                                if (!ctrl.Focused)
                                    frmS.Hide();
                            }));

                            Thread.Sleep(100);
                        }
                        catch
                        {

                        }
                    }
                    try
                    {
                        if (!frmS.IsDisposed)
                            frmS.Close();
                    }
                    catch { }
                })).Start();
            }
        }
        private static string GoogleApiSuggestion = "https://www.google.com/complete/search?output=toolbar&q=";
        private static string[] GetSuggestion(string keyword)
        {
            List<string> suggestWords = new List<string>();
            try
            {
                if (!keyword.IsNullOrEmpty())
                {
                    using (var responseStream = WebRequest.Create(GoogleApiSuggestion + keyword).GetResponse().GetResponseStream())
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

    public enum SuggestType
    {
        NaturalLanguage = 0,
        Address = 1
    }
}
