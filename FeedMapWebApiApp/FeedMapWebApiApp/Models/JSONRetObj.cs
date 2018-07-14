using System;
namespace FeedMapWebApiApp.Models
{
    public class JSONRetObj<T>
    {
        public T ResponseObj { get; set; }
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
    }
}
