using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfDiary.ViewModels;

namespace WpfDiary.Models
{
    public enum CollectionChangedEventType { Added, Removed, Updated };
    public delegate void OnCollectionChanged(CollectionChangedEventType type, List<DiaryEntry> changedElements);

    public class Diary
    {
        private static readonly Diary instance = new Diary();
        public static Diary Instance
        {
            get
            {
                return instance;
            }
        }

        private List<DiaryEntry> entries = new List<DiaryEntry>();

        private event OnCollectionChanged CollectionChangedEvent;

        public void RegisterListener(OnCollectionChanged handler)
        {
            CollectionChangedEvent += handler;
        }

        public void UnregisterListener(OnCollectionChanged handler)
        {
            CollectionChangedEvent -= handler;
        }

        public void RemoveEntry(DiaryEntry entry)
        {
            entries.Remove(entry);
            CollectionChangedEvent(CollectionChangedEventType.Removed, new List <DiaryEntry> { entry });
        }

        public void AddEntry(string title, string content, string tags)
        {
            DiaryEntry entry = new DiaryEntry(title, content, Utils.TagsStringToSet(tags), DateTime.Now);
            entries.Add(entry);
            CollectionChangedEvent(CollectionChangedEventType.Added, new List <DiaryEntry> { entry });
        }

        public void EditEntry(DiaryEntry entry, string title, string content, string tags, DateTime created)
        {
            entry.Update(title, content, Utils.TagsStringToSet(tags), created);
            CollectionChangedEvent(CollectionChangedEventType.Updated, new List<DiaryEntry> { entry });
        }

        public IEnumerable<DiaryEntry> GetEntriesBetween(DateTime start, DateTime end)
        {
            return new List<DiaryEntry>(entries.Where(entry => entry.Created > start && entry.Created < end));
        }

        public IEnumerable<DiaryEntry> GetEntriesTaggedWith(string tags)
        {
            HashSet<string> requiredTags = Utils.TagsStringToSet(tags);
            return new List<DiaryEntry>(entries.Where(entry => requiredTags.IsSubsetOf(entry.Tags)));
        }

        public IEnumerable<DiaryEntryViewModel> GetAllEntries()
        {
            return entries.Select(entry => new DiaryEntryViewModel(entry)).ToList();
        }
    }
}
