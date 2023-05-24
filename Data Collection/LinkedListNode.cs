using System;
using System.Collections.Generic;
using System.Text;

namespace DataCollection
{
    public class LinkedListNode<T>
    {
        public LinkedListNode(T value) => Value = value;

        public T Value { get; set; }

        public LinkedListNode<T> Next { get; internal set; }

        public LinkedListNode<T> Previous { get; internal set; }

        public LinkedList<T> List { get; internal set; }
    }
}