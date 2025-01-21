using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using GitClient.ui;

namespace Gitclient.model
{
    public class CommitsElements
    {
        public string Id { get;  }
        public string DateAndTime { get;  }
        public string Author { get; }
        public string Message { get; }

        private string space;
        private string timeSpace;


        public CommitsElements(string id, string date, string author, string message) 
        {
            this.Id = id;
            this.DateAndTime = date;
            this.Author = author;
            this.Message = message;
            space = new string(' ', 2);
            timeSpace = SetSpace();
        }

        public string Display()
        {
            return this.Id + timeSpace + this.DateAndTime + space + this.Author + "          " + this.Message;
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

        public string SetSpace()
        {
            return DateAndTime.Contains(":") ? timeSpace += space + "  " : space;
        }
    }
}
