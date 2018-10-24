using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;
using System.Text;

namespace StorageProvider
{
    public class Tag
    {
        [Key]
        public string Name { get; set; }

        private int _alpha;
        private int _red;
        private int _green;
        private int _blue;

        [NotMapped]
        public Color Color
        {
            get => Color.FromArgb(_alpha, _red, _green, _blue);
            set
            {
                _alpha = value.A;
                _red = value.R;
                _green = value.G;
                _blue = value.B;
            }
        }

        public List<Tag> ChildTags { get; set; } = new List<Tag>();
    }
}
