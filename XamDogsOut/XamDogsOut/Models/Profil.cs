using Plugin.CloudFirestore.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace XamDogsOut.Models
{
    public class Profil
    {
        [Id]
        public string Id { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string UserSurname { get; set; }

        // adress
        public string City { get; set; }
        public string Street { get; set; }
        public string BuildingNumber { get; set; }
        public string FlatNumber { get; set; }
        public string ZipCode { get; set; }

        //position

        public double Lat { get; set; }
        public double Lon { get; set; }

    }
}
