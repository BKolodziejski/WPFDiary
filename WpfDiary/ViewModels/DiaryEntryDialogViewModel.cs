using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WpfDiary.Models;
using WpfDiary.Services;

namespace WpfDiary.ViewModels
{
    public class DiaryEntryDialogViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private DiaryEntry entry;

        private string title;
        public string Title
        {
            get
            {
                return title;
            }

            set
            {
                title = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Title"));
            }
        }

        private string content;
        public string Content
        {
            get
            {
                return content;
            }

            set
            {
                content = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Content"));
            }
        }

        private string tags;
        public string Tags
        {
            get
            {
                return tags;
            }

            set
            {
                tags = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Tags"));
            }
        }

        private ICommand save;

        public DiaryEntryDialogViewModel(DiaryEntry entry)
        {
            this.entry = entry;
            if (entry != null)
            {
                Title = entry.Title;
                Content = entry.Content;
                Tags = Utils.TagsSetToString(entry.Tags);
            }
        }

        public ICommand Save
        {
            get
            {
                if (save == null)
                {
                    save = new RelayCommand(
                        p => !(string.IsNullOrWhiteSpace(Title) || string.IsNullOrWhiteSpace(Content)),
                        p => 
                        {
                            if (entry == null)
                                Diary.Instance.AddEntry(Title, Content, Tags);
                            else
                                Diary.Instance.EditEntry(entry, Title, Content, Tags, entry.Created);
                            WindowService.CloseEntryDialog();
                        }
                    );
                }
                return save;
            }
        }

    }
}
