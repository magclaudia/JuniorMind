using System;
using System.Collections;
using System.Collections.Generic;

namespace DataCollection
{
    public class LinkedList<T> : ICollection<T>
    {
        private readonly LinkedListNode<T> sentinel;

        public LinkedList()
        {
            sentinel = new LinkedListNode<T>(default);
            sentinel.Next = sentinel;
            sentinel.Previous = sentinel;
            sentinel.List = this;
        }

        public int Count { get; protected set; } = 0;
        public LinkedListNode<T> First
        {
            get
            {
                return Count == 0 ? null : sentinel.Next;
            }
        }

        public LinkedListNode<T> Last
        {
            get
            {
                return Count == 0 ? null : sentinel.Previous;
            }
        }

        public bool IsReadOnly { get; }

        public void AddAfter(LinkedListNode<T> node, LinkedListNode<T> newNode)
        {
            ExceptionArgumentNullExceptionNode(node);
            ExceptionArgumentNullExceptionNode(newNode);
            ExceptionNodeIsNotInTheCurrentList(node);
            ExceptionNodeBelongsToAnotherList(newNode);
            newNode.Next = node.Next;
            newNode.Previous = node;
            node.Next.Previous = newNode;
            node.Next = newNode;
            newNode.List = this;
            Count++;
        }

        public void AddAfter(LinkedListNode<T> node, T value)
        {
            AddAfter(node, new LinkedListNode<T>(value));
        }

        public void AddBefore(LinkedListNode<T> node, LinkedListNode<T> newNode)
        {
            ExceptionArgumentNullExceptionNode(node);
            AddAfter(node.Previous, newNode);
        }

        public void AddBefore(LinkedListNode<T> node, T value)
        {
            AddBefore(node, new LinkedListNode<T>(value));
        }

        public void AddFirst(LinkedListNode<T> node)
        {
            AddAfter(sentinel, node);
        }

        public void AddFirst(T value)
        {
            AddFirst(new LinkedListNode<T>(value));
        }

        public void AddLast(LinkedListNode<T> node)
        {
            AddBefore(sentinel, node);
        }

        public void AddLast(T value)
        {
            AddLast(new LinkedListNode<T>(value));
        }

        public void Add(T value)
        {
            AddLast(value);
        }

        public LinkedListNode<T> Find(T value)
        {
            for (var node = sentinel.Next; node != sentinel; node = node.Next)
            {
                if (node.Value.Equals(value))
                {
                    return node;
                }
            }

            return null;
        }

        public bool Contains(T value)
        {
            return Find(value) != null;
        }

        public LinkedListNode<T> FindLast(T value)
        {
            for (var node = sentinel.Previous; node != sentinel; node = node.Previous)
            {
                if (node.Value.Equals(value))
                {
                    return node;
                }
            }

            return null;
        }

        public void Clear()
        {
            sentinel.Next = sentinel;
            sentinel.Previous = sentinel;
            sentinel.List = null;
            Count = 0;
        }

        public void Remove(LinkedListNode<T> node)
        {
            ExceptionArgumentNullExceptionNode(node);
            ExceptionNodeIsNotInTheCurrentList(node);
            if (node == sentinel)
            {
                throw new InvalidOperationException("Cannot remove sentinel node.");
            }

            node.Previous.Next = node.Next;
            node.Next.Previous = node.Previous;
            node.List = null;
            Count--;
        }

        public bool Remove(T value)
        {
            var element = Find(value);
            if (element != null)
            {
                Remove(element);
                return true;
            }

            return false;
        }

        public void RemoveFirst()
        {
            ExceptionInvalidOperationException();
            Remove(sentinel.Next);
        }

        public void RemoveLast()
        {
            ExceptionInvalidOperationException();
            Remove(sentinel.Previous);
        }

        public void CopyTo(T[] array, int index)
        {
            if (array == null)
            {
                throw new ArgumentNullException("Array is empty.");
            }

            if (index < 0)
            {
                throw new IndexOutOfRangeException("Index is not valid");
            }

            if (Count > (array.Length - index))
            {
                throw new ArgumentException("Number of elements should  not be bigger then available space.");
            }

            index = 0;
            for (var node = sentinel.Next; node != sentinel; node = node.Next)
            {
                array[index] = node.Value;
                index++;
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            for (var currentNode = sentinel.Previous; currentNode != sentinel; currentNode = currentNode.Previous)
            {
                yield return currentNode.Value;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        private void ExceptionArgumentNullExceptionNode(LinkedListNode<T> node)
        {
            if (node is null)
            {
                throw new ArgumentNullException("Node is null");
            }
        }

        private void ExceptionNodeIsNotInTheCurrentList(LinkedListNode<T> node)
        {
            if (node.List != this)
            {
                throw new InvalidOperationException("Node is not in the current LinkedList<T>.");
            }
        }

        private void ExceptionNodeBelongsToAnotherList(LinkedListNode<T> node)
        {
            if (node.List != null && node.List != this)
            {
                throw new InvalidOperationException("Node belongs to another LinkedList<T>.");
            }
        }

        private void ExceptionInvalidOperationException()
        {
            if (sentinel.Previous == sentinel || sentinel.Next == sentinel)
            {
                throw new InvalidOperationException("List is empty.");
            }
        }
    }
}