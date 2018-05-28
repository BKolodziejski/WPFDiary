using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfDiary.Models
{
    public class DiaryEntry
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public HashSet<string> Tags { get; set; }
        public DateTime Created { get; set; }

        public DiaryEntry(string title, string content, HashSet<string> tags, DateTime created)
        {
            Title = title;
            Tags = tags;
            Content = content;
            Created = created;
        }

        public DiaryEntry()
        {
        }

        public void Update(string title, string content, HashSet<string> tags, DateTime created)
        {
            Title = title;
            Tags = tags;
            Content = content;
            Created = created;
        }
    }
}
