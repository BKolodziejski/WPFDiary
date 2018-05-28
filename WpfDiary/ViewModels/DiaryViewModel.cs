using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WpfDiary.Models;
using WpfDiary.Services;

namespace WpfDiary.ViewModels
{
    public class DiaryViewModel
    {
        public DiaryViewModel()
        {
            Diary.Instance.RegisterListener(handleElementsAdded);
            Entries = new ObservableCollection<DiaryEntryViewModel>(Diary.Instance.GetAllEntries());
            /*Entries.Add(new DiaryEntryViewModel("First Title", "Some Content", DateTime.Now));
            Entries.Add(new DiaryEntryViewModel("Second Title", "Some Content", DateTime.Now));
            Entries.Add(new DiaryEntryViewModel("Third Title", "Some Content", DateTime.Now));
            Entries.Add(new DiaryEntryViewModel("Fourth Title", "Some Content", DateTime.Now));*/
        }

        public ObservableCollection<DiaryEntryViewModel> Entries { get; set; }

        private ICommand addNewEntry;
        public ICommand AddNewEntry
        {
            get
            {
                if (addNewEntry == null)
                {
                    addNewEntry = new RelayCommand(
                        p => true,
                        p => WindowService.ShowEntryDialog(null)
                    );
                }
                return addNewEntry;
            }
        }

        private ICommand viewDetails;
        public ICommand ViewDetails
        {
            get
            {
                if (viewDetails == null)
                {
                    viewDetails = new RelayCommand(
                        p => true,
                        p => WindowService.ShowEntry(new DiaryEntryViewModel(p as DiaryEntry, ViewModelConstants.DIARY_ENTRY_NON_TRIMMED))
                    );
                }
                return viewDetails;
            }
        }

        private void handleElementsAdded(CollectionChangedEventType type, List<DiaryEntry> changedElements)
        {
            changedElements.ForEach(e => Entries.Add(new DiaryEntryViewModel(e)));
        }

        
    }
}
