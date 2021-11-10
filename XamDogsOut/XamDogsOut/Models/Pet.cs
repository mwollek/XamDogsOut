using Plugin.CloudFirestore.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace XamDogsOut.Models
{
    public abstract class Pet
    {
        [Id]
        public string Id { get; set; }
        public string Name { get; set; }
        public int? Weight { get; set; }
        public string PhotoUrl { get; set; }
    }
}
