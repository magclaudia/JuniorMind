using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gitclient.model
{
    public class CommitsElements
    {
        public string Id { get;  }
        public string DateAndTime { get;  }
        public string Author { get; }
        public string Message { get; }   

        public CommitsElements(string id, string date, string author, string message) 
        {
            this.Id = id;
            this.DateAndTime = date;
            this.Author = author;
            this.Message = message;
        }

        public string Display()
        {
            return this.Id + "    " + this.DateAndTime + "       " + this.Author + "            " + this.Message;
        }

        public string GetCode()
        {
            return this.Id;
        }

        public string GetDate()
        {
            return this.DateAndTime;
        }

        public string GetAuthor() 
        {
            return this.Author;
        }

        public string GetMessage() 
        {
            return this.Message;
        }
    }
}
