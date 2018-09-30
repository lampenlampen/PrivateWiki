using System;
using System.Collections.Generic;
using System.Text;

namespace StorageProvider
{
    public class ContentPage
    {
        public int _id { get; set; }
        public string Id { get; set; }
        public string Content { get; set; }

        public ContentPage(int id, string Id, string content)
        {
            _id = id;
            this.Id = Id;
            Content = content;
        }
    }
}
