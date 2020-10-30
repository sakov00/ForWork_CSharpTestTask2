using AngleSharp;
using AngleSharp.Dom;
using AngleSharp.Html.Parser;
using HtmlAgilityPack;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework.Internal;
using Sakov_Evgeni2.ViewModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Sakov_Evgeni2.Commands.CommandForMainWindow
{
    class InputLinkToVKCommand : ICommand
    {
        private Action<object> execute;
        private Func<object, bool> canExecute;

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public InputLinkToVKCommand(Action<object> execute, Func<object, bool> canExecute = null)
        {
            this.execute = execute;
            this.canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            return this.canExecute == null || this.canExecute(parameter);
        }
        public async void Execute(object parameter)
        {
            this.execute(parameter);
            try
            {
                string address = MainWindowViewModel.LinkTovk;
                Process.Start(address);

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(address);
                HttpWebResponse response = (HttpWebResponse)await request.GetResponseAsync();
                using (Stream stream = response.GetResponseStream())
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        string InfoUser = "";
                        Regex RegexStatus = new Regex(@"\w*pp_last_activity_text\w*", RegexOptions.IgnoreCase);
                        Regex RegexName = new Regex(@"\w*<title>\w*", RegexOptions.IgnoreCase);
                        string line="";
                        while (line != null)
                        {
                            line = reader.ReadLine();
                            MatchCollection MatchesStatus = RegexStatus.Matches(line);
                            MatchCollection MatchesName = RegexName.Matches(line);
                            if (MatchesStatus.Count > 0)
                            {
                                line=DeleteHtmlTags(line);
                                InfoUser += line;
                                break;
                            }
                            if (MatchesName.Count > 0)
                            {
                                line=DeleteHtmlTags(line);
                                line=line.Substring(0, line.Length - 11);
                                InfoUser += line;
                            }

                        }
                        MainWindowViewModel.mainwindow.Info = InfoUser;
                        MessageBox.Show(InfoUser);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private static string DeleteHtmlTags(string text)
        {
            text=Regex.Replace(text,"([^<]+(?=>))", "");
            text = text.Trim();
            text = text.Substring(2);
            text = text.Substring(0, text.Length-2);
            return text;
        }

    }
}
