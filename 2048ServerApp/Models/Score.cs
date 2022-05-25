using System;

namespace _2048ServerApp.Models
{
    public class Score
    {
        public Guid Id { get; set; }
        public DateTime Date { get; set; }
        public int Result { get; set; }
    }
}