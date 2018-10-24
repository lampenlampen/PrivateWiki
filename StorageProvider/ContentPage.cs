using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace StorageProvider
{
    public class ContentPage
    {
        /// <summary>
        /// The unique string id
        /// </summary>
        [Key]
        public string Id { get; set; }

        /// <summary>
        /// The markdown formatted content
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// Date of Creation
        /// </summary>
        public DateTimeOffset CreationTime { get; set; }

        /// <summary>
        /// Date of last Change
        /// </summary>
        public DateTimeOffset ChangeTime { get; set; }

        /// <summary>
        /// Date of last change (for versioning)
        /// </summary>
        public DateTimeOffset ValidFrom { get; set; }

        /// <summary>
        /// Date of next change (for versioning)
        /// </summary>
        public DateTimeOffset ValidTo { get; set; }

        public bool IsFavorite { get; set; }

        public List<Tag> Tags { get; set; } = new List<Tag>();

        /// <summary>
        /// Indicates whether this page can be edited
        /// </summary>
        public bool IsLocked { get; set; } = false;

        public ContentPage(string id, string content)
        {
            this.Id = id;
            Content = content;
        }

        public static ContentPage Create(string id)
        {
            string welcome = $"# {id}";
            return new ContentPage(id, welcome);
        }

        public ContentPage Clone()
        {
            var page = new ContentPage(Id, Content)
            {
                ChangeTime = ChangeTime,
                CreationTime = CreationTime,
                IsFavorite = IsFavorite,
                IsLocked = IsLocked,
                Tags = Tags,
                ValidFrom = ValidFrom,
                ValidTo = ValidTo
            };
            return page;
        }
    }
}