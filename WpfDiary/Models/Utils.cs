using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfDiary.Models
{
    class Utils
    {
        public const char TAGS_DELIMIER = ',';

        public static HashSet<string> TagsStringToSet(string tags)
        {
            if (string.IsNullOrEmpty(tags))
                return new HashSet<string>();

            return new HashSet<string>(tags.Split(TAGS_DELIMIER).Select(tag => tag.Trim()));
        }

        public static string TagsSetToString(HashSet<string> tags)
        {
            return string.Join(", ", tags.ToArray());
        }
    }
}
