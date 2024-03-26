using System;
using System.Text;

namespace CMSystem
{
    public class Device
    {
        public int ID { get; set; } = -1;
        public string Name { get; set; }
        public string ImagePath { get; set; }
        public string RichTextPath { get; set; }
        public DateTime CreationDate { get; set; }
    }
}
