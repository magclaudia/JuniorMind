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
            ExceptionArgumentNullExceptionNewNode(newNode);
            ExceptionNewNodeBelongsToAnotherList(newNode);
            ExceptionNodeIsNotInTheCurrentList(node);
            newNode.Next = node.Next;
            newNode.Previous = node;
            node.Next.Previous = newNode;
            node.Next = newNode;
            newNode.List = this;
            Count++;
        }

        public void AddAfter(LinkedListNode<T> node, T value)
        {
            var newNode = new LinkedListNode<T>(value);
            AddAfter(node, newNode);
        }

        public void AddBefore(LinkedListNode<T> node, LinkedListNode<T> newNode)
        {
            AddAfter(node.Previous, newNode);
        }

        public void AddBefore(LinkedListNode<T> node, T value)
        {
            ExceptionNodeIsNotInTheCurrentList(node);
            var newNode = new LinkedListNode<T>(value);
            AddBefore(node, newNode);
        }

        public void AddFirst(LinkedListNode<T> node)
        {
            AddAfter(sentinel, node);
        }

        public void AddFirst(T value)
        {
            var newNode = new LinkedListNode<T>(value);
            AddFirst(newNode);
        }

        public void AddLast(LinkedListNode<T> node)
        {
            ExceptionArgumentNullExceptionNode(node);
            AddBefore(sentinel, node);
        }

        public void AddLast(T value)
        {
            var newNode = new LinkedListNode<T>(value);
            AddLast(newNode);
        }

        public void Add(T value)
        {
            AddLast(value);
        }

        public LinkedListNode<T> Find(T value)
        {
            for (LinkedListNode<T> node = sentinel.Next; node != sentinel; node = node.Next)
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
            for (LinkedListNode<T> node = sentinel.Previous; node != sentinel; node = node.Previous)
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
            Count = 0;
        }

        public void Remove(LinkedListNode<T> node)
        {
            ExceptionArgumentNullExceptionNode(node);
            ExceptionNodeIsNotInTheCurrentList(node);
            node.Previous.Next = node.Next;
            node.Next.Previous = node.Previous;
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

        public void CopyTo(T[] array, int arrayIndex)
        {
            if (array == null)
            {
                throw new ArgumentNullException("Array is empty.");
            }

            if (arrayIndex < 0)
            {
                throw new IndexOutOfRangeException("Index is not valid");
            }

            if (Count > (array.Length - arrayIndex))
            {
                throw new ArgumentException("Number of elements should  not be bigger then available space.");
            }

            arrayIndex = 0;
            LinkedListNode<T> node = First;
            for (int i = 0; i < Count; i++)
            {
                array[arrayIndex + i] = node.Value;
                node = node.Next;
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
                throw new ArgumentNullException(nameof(node), " is null");
            }
        }

        private void ExceptionArgumentNullExceptionNewNode(LinkedListNode<T> newNode)
        {
            if (newNode is null)
            {
                throw new ArgumentNullException(nameof(newNode), " is null");
            }
        }

        private void ExceptionNodeIsNotInTheCurrentList(LinkedListNode<T> node)
        {
            if (node == null)
            {
                throw new InvalidOperationException("Node is not in the current LinkedList<T>.");
            }
        }

        private void ExceptionNewNodeBelongsToAnotherList(LinkedListNode<T> node)
        {
            if (node.Previous != null || node.Next != null)
            {
                throw new InvalidOperationException("New node belongs to another LinkedList<T>.");
            }
        }

        private void ExceptionInvalidOperationException()
        {
            if (sentinel.Previous == sentinel.Next)
            {
                throw new InvalidOperationException("List is empty.");
            }
        }
    }
}