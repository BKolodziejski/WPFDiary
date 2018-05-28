using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WpfDiary.Models;
using WpfDiary.ViewModels;
using WpfDiary.Views;

namespace WpfDiary.Services
{
    class WindowService
    {
        private static DiaryEntryDialog entryDialog;

        public static void ShowEntryDialog(DiaryEntry entry)
        {
            entryDialog = new DiaryEntryDialog(new DiaryEntryDialogViewModel(entry));
            entryDialog.ShowDialog();
        }

        public static void CloseEntryDialog()
        {
            entryDialog.Close();
        }

        internal static void ShowEntry(DiaryEntryViewModel diaryEntry)
        {
            MainWindow.Instance.Content = new EntryDetails(diaryEntry);
        }

        public static void ShowList(DiaryViewModel diaryList)
        {
            MainWindow.Instance.Content = new DiaryEntryListUserControl(diaryList);
        }

        public static bool ConfirmChoice(string title, string text)
        {
            return MessageBox.Show(text, title, System.Windows.MessageBoxButton.YesNo) == MessageBoxResult.Yes;
        }
    }
}
