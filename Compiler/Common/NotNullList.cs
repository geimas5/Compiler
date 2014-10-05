namespace Compiler.Common
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    public class NotNullList<T> : IList<T> where T : class
    {
        private readonly List<T> internalList = new List<T>();

        public IEnumerator<T> GetEnumerator()
        {
            return this.internalList.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public void Add(T item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }

            this.internalList.Add(item);
        }

        public void Clear()
        {
            this.internalList.Clear();
        }

        public bool Contains(T item)
        {
            return this.internalList.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            this.internalList.CopyTo(array, arrayIndex);
        }

        public bool Remove(T item)
        {
            return this.internalList.Remove(item);
        }

        public int Count
        {
            get
            {
                return this.internalList.Count;
            }
        }

        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        public int IndexOf(T item)
        {
            return this.internalList.IndexOf(item);
        }

        public void Insert(int index, T item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }

            this.internalList.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            this.internalList.RemoveAt(index);
        }

        public T this[int index]
        {
            get
            {
                return this.internalList[index];
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }


                this.internalList[index] = value;
            }
        }
    }
}
