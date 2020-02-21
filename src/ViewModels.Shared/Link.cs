using System;

namespace ViewModels.Shared
{
    public class Link
    {
        public string Uri { get; set; }

        public DateTimeOffset Expires { get; set; }
    }
}