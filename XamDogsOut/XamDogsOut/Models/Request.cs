using Plugin.CloudFirestore.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace XamDogsOut.Models
{

    public static class RequestStatuses
    {
        public static readonly int Done = 1;
        public static readonly int Canceled = 2;
        public static readonly int StandBy = 3;
        public static readonly int InProgress = 4;


    }

    public class Request
    {
        [Id]
        public string Id { get; set; }

        public string SenderId { get; set; }
        public string ExecutorId { get; set; }

        public int Status { get; set; }

    }
}
