using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WpfDiary.Models;
using WpfDiary.Services;

namespace WpfDiary.ViewModels
{
    public class DiaryViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private bool isFilterVisible;
        public bool IsFilterVisible
        {
            get
            {
                return isFilterVisible;
            }

            set
            {
                isFilterVisible = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsFilterVisible"));
            }
        }

        private string filterText;
        public string FilterText
        {
            get
            {
                return filterText;
            }

            set
            {
                filterText = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("FilterText"));
            }
        }

        public DiaryViewModel()
        {
            Diary.Instance.RegisterListener(HandleElementsChanged);
            Entries = new ObservableCollection<DiaryEntryViewModel>(Diary.Instance.GetAllEntries());
            IsFilterVisible = false;
            FilterText = ViewModelConstants.FILTERS_HIDDEN_TEXT;
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
                        p => WindowService.ShowEntry(new DiaryEntryViewModel(p as DiaryEntry, ViewModelConstants.DIARY_ENTRY_NON_TRIMMED, this))
                    );
                }
                return viewDetails;
            }
        }

        private ICommand applyFilter;
        public ICommand ApplyFilter
        {
            get
            {
                if (applyFilter == null)
                {
                    applyFilter = new RelayCommand(
                        p => true,
                        p =>
                        {
                            Entries.Clear();
                            activeFilter = new Filter(FilteredTags, FilteredText, FilterDates);
                            foreach (var i in Diary.Instance.GetFilteredEntries(activeFilter))
                            {
                                Entries.Add(i);
                            }
                        }
                    );
                }
                return applyFilter;
            }
        }

        private ICommand toggleFilters;
        public ICommand ToggleFilters
        {
            get
            {
                if (toggleFilters == null)
                {
                    toggleFilters = new RelayCommand(
                        p => true,
                        p => { IsFilterVisible = !IsFilterVisible; }
                    );
                }
                return toggleFilters;
            }
        }

        public HashSet<string> FilteredTags { get; set; }
        public string FilteredText { get; set; }
        
        private List<DateTime> dates = new List<DateTime>();
        private Filter activeFilter= new Filter(new HashSet<string>(), "", new List<DateTime>());

        public List<DateTime> FilterDates
        {
            get
            {
                return dates;
            }
            set
            {
                dates = value;
            }
        }

        private void HandleElementsChanged(CollectionChangedEventType type, List<DiaryEntry> changedElements)
        {
            changedElements = activeFilter.Apply(changedElements).ToList();
            if (type == CollectionChangedEventType.Added)
            {
                changedElements.ForEach(e => Entries.Add(new DiaryEntryViewModel(e)));
            }
            else if (type == CollectionChangedEventType.Updated)
            {
                foreach(var entry in Entries)
                {
                    entry.HandleElementsChanged(type, changedElements);
                }
            }
            else
            {
                changedElements.ForEach(e => Entries.Remove(Entries.Single(en => en.Entry == e)));
            }
        }

        
    }
}
