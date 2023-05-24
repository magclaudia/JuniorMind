using System;
using System.Collections.Generic;
using System.Text;

namespace DataCollection
{
    public class LinkedListNode<T>
    {
        public LinkedListNode(T value) => Value = value;

        public T Value { get; internal set; }

        public LinkedListNode<T> Next { get; internal set; }

        public LinkedListNode<T> Previous { get; internal set; }

        internal LinkedList<T> List { get; set; }
    }
}