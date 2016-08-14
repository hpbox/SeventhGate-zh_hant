using System.Collections.Generic;
using System.Threading;

namespace wlan
{

	public class ThreadSafeQueue<T>
	{
		private int _capacity;
		private Queue<T> _items;
		private bool _end;
		private bool _empty;
		private bool _full;

		public ThreadSafeQueue(int capacity)
		{
			_capacity = capacity;
			_empty = true;
			_items = new Queue<T>(capacity);
		}


		// This sets the end flag, so once the queue is drained, RemoveItem will return false
		public void EmptyQueue()
		{
			lock(_items)
			{
				_end = true;
				if(_empty)
				{
					_empty = false;
					Monitor.PulseAll(_items);
				}
			}
		}

		// Add an item to the queue
		public void AddItem(T item)
		{
			lock(_items)
			{
				do
				{
					if(_items.Count >= _capacity)
					{
						_full = true;
						Monitor.Wait(_items);
						continue;
					}
					_items.Enqueue(item);

					if(_empty)
					{
						_empty = false;
						Monitor.PulseAll(_items);
					}
					break;
				}
				while(true);
			}
		}

		// Remove an item from the queue
		public bool RemoveItem(out T result)
		{
			lock(_items)
			{
				while(!_end)
				{
					if(_items.Count == 0)
					{
						_empty = true;
						Monitor.Wait(_items);
						continue;
					}

					result = _items.Dequeue();

					if(_full)
					{
						_full = false;
						Monitor.PulseAll(_items);
					}
					return true;
				}

				result = default(T);
				return false;
			}
		}
	}
}
