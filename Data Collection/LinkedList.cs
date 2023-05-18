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
                return sentinel.Next;
            }
        }

        public LinkedListNode<T> Last
        {
            get
            {
                return sentinel.Previous;
            }
        }
        public bool IsReadOnly { get; }

        public void AddFirst(LinkedListNode<T> node)
        {
            ExceptionArgumentNullExceptionNode(node);
            AddAfter(sentinel, node);
        }

        public LinkedListNode<T> AddFirst(T value)
        {
            var newNode = new LinkedListNode<T>(value);
            AddAfter(sentinel, newNode);
            return newNode;
        }

        public void AddLast(LinkedListNode<T> node)
        {
            ExceptionArgumentNullExceptionNode(node);
            AddBefore(sentinel, node);
        }

        public LinkedListNode<T> AddLast(T value)
        {
            LinkedListNode<T> newNode = new LinkedListNode<T>(value);
            AddBefore(sentinel, newNode);
            return newNode;
        }

        public void AddAfter(LinkedListNode<T> node, LinkedListNode<T> newNode)
        {
            ExceptionArgumentNullExceptionNode(node);
            ExceptionArgumentNullExceptionNewNode(newNode);
            ExceptionNewNodeBelongsToAnotherList(newNode);
            ExceptionNodeIsNotInTheCurrentList(node);
            LinkedListNode<T> nextNode = node.Next;
            newNode.Next = nextNode;
            newNode.Previous = node;
            node.Next = newNode;
            nextNode.Previous = newNode;

            Count++;
        }

        public LinkedListNode<T> AddAfter(LinkedListNode<T> node, T value)
        {
            var newNode = new LinkedListNode<T>(value);
            AddAfter(node, newNode);
            return newNode;
        }

        public void AddBefore(LinkedListNode<T> currentNode, LinkedListNode<T> newNode)
        {
            ExceptionArgumentNullExceptionNode(currentNode);
            ExceptionArgumentNullExceptionNewNode(newNode);
            newNode.Previous = currentNode.Previous;
            newNode.Next = currentNode;
            currentNode.Previous = newNode;
            Count++;
        }

        public LinkedListNode<T> AddBefore(LinkedListNode<T> currentNode, T value)
        {
            ExceptionNodeIsNotInTheCurrentList(currentNode);
            var newNode = new LinkedListNode<T>(value);
            AddBefore(currentNode, newNode);
            return newNode;
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

        public void Add(T item)
        {
            AddLast(item);
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public bool Contains(T item)
        {
            throw new NotImplementedException();
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public bool Remove(T item)
        {
            throw new NotImplementedException();
        }

        public IEnumerator<T> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}
