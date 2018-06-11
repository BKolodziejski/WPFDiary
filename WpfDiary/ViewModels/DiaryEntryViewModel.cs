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
    public class DiaryEntryViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public DiaryEntry Entry { get; }
        private DiaryViewModel goBackModel;

        private bool isTrimmed;
        public bool IsTrimmed
        {
            get
            {
                return isTrimmed;
            }

            set
            {
                isTrimmed = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Trimmed"));
            }
        }

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

        private DateTime createdDate;
        public DateTime CreatedDate
        {
            get
            {
                return createdDate;
            }

            set
            {
                createdDate = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("CreatedDate"));
            }
        }

        private ICommand viewList;
        public ICommand ViewList
        {
            get
            {
                if (viewList == null)
                {
                    viewList = new RelayCommand(
                        p => true,
                        p => WindowService.ShowList(goBackModel)
                    );
                }
                return viewList;
            }
        }

        private ICommand editEntry;
        public ICommand EditEntry
        {
            get
            {
                if (editEntry == null)
                {
                    editEntry = new RelayCommand(
                        p => true,
                        p => WindowService.ShowEntryDialog(Entry)
                    );
                }
                return editEntry;
            }
        }

        private ICommand deleteEntry;
        public ICommand DeleteEntry
        {
            get
            {
                if (deleteEntry == null)
                {
                    deleteEntry = new RelayCommand(
                        p => true,
                        p =>
                        {
                            if (WindowService.ConfirmChoice("Delete Entry", "Are you sure you want to delete " + Title))
                                Diary.Instance.RemoveEntry(Entry);
                        }
                    );
                }
                return deleteEntry;
            }
        }
        public DiaryEntryViewModel(DiaryEntry entry, int trimLength = ViewModelConstants.DIARY_ENTRY_LIST_CONTENT_MAX_LENGTH, DiaryViewModel vm = null)
        {
            Diary.Instance.RegisterListener(HandleElementsChanged);
            Entry = entry;
            Title = entry.Title;
            IsTrimmed = trimLength > 0 && trimLength < entry.Content.Length;
            Content = isTrimmed ? entry.Content.Substring(0, trimLength) + "...": entry.Content;
            CreatedDate = entry.Created;
            Tags = Utils.TagsSetToString(entry.Tags);
            goBackModel = vm;
        }

        public void HandleElementsChanged(CollectionChangedEventType type, List<DiaryEntry> changedElements)
        {
            switch(type)
            {
                case CollectionChangedEventType.Updated:
                    if(changedElements.Contains(Entry))
                    {
                        Title = Entry.Title;
                        IsTrimmed = ViewModelConstants.DIARY_ENTRY_LIST_CONTENT_MAX_LENGTH < Entry.Content.Length;
                        Content = isTrimmed ? Entry.Content.Substring(0, ViewModelConstants.DIARY_ENTRY_LIST_CONTENT_MAX_LENGTH) + "..." : Entry.Content;
                        CreatedDate = Entry.Created;
                        Tags = Utils.TagsSetToString(Entry.Tags);
                    }
                    break;
                case CollectionChangedEventType.Removed:
                    if (changedElements.Contains(Entry))
                    {
                        WindowService.ShowList(goBackModel);
                    }
                    break;
                default:
                    return;
            }
        }

    }
}
