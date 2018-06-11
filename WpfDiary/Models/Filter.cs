using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfDiary.Models
{
    public class Filter
    {
        private HashSet<string> tags;
        private string text;
        private List<DateTime> dateRanges;

        public Filter(HashSet<string> tags, string text, List<DateTime> dateRanges)
        {
            this.tags = tags;
            this.text = text;
            this.dateRanges = dateRanges;
        }

        public IEnumerable<DiaryEntry> Apply(IEnumerable<DiaryEntry> entries)
        {
            IEnumerable<DiaryEntry> tagFilteredEntries = new HashSet<DiaryEntry>();
            IEnumerable<DiaryEntry> textFilteredEntries = new HashSet<DiaryEntry>();
            IEnumerable<DiaryEntry> dateFilteredEntries = new HashSet<DiaryEntry>();

            bool anyFilterApplied = false;

            if (tags != null && tags.Any())
            {
                tagFilteredEntries = entries.Where(entry => entry.Tags.Intersect(tags).Any());
                anyFilterApplied = true;
            }
            if (!string.IsNullOrWhiteSpace(text))
            {
                textFilteredEntries = entries.Where(entry => entry.Content.Contains(text) || entry.Title.Contains(text));
                anyFilterApplied = true;
            }
            IEnumerator<DateTime> iter = dateRanges.GetEnumerator();
            for (int i = 0; i < dateRanges.Count / 2; ++i)
            {
                iter.MoveNext();
                DateTime earlierDate = iter.Current;
                iter.MoveNext();
                DateTime laterDate = iter.Current;
                dateFilteredEntries = dateFilteredEntries.Union(entries.Where(entry => earlierDate < entry.Created && entry.Created < laterDate));
                anyFilterApplied = true;
            }

            return anyFilterApplied
                ? tagFilteredEntries.Union(textFilteredEntries).Union(dateFilteredEntries)
                : entries;
        }
    }
}
