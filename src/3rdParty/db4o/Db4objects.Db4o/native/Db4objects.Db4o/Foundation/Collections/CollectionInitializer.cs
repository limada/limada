/* This file is part of the db4o object database http://www.db4o.com

Copyright (C) 2004 - 2010  Versant Corporation http://www.versant.com

db4o is free software; you can redistribute it and/or modify it under
the terms of version 3 of the GNU General Public License as published
by the Free Software Foundation.

db4o is distributed in the hope that it will be useful, but WITHOUT ANY
WARRANTY; without even the implied warranty of MERCHANTABILITY or
FITNESS FOR A PARTICULAR PURPOSE.  See the GNU General Public License
for more details.

You should have received a copy of the GNU General Public License along
with this program.  If not, see http://www.gnu.org/licenses/. */
using System;
using System.Collections;
using System.Collections.Generic;

#if CF
using System.Reflection;
#endif

namespace Db4objects.Db4o.Foundation.Collections
{
	public interface ICollectionInitializer
	{
		void Clear();
		void Add(object o);
		void FinishAdding();
	    int Count();
	}

	public sealed class CollectionInitializer
	{
		private static readonly Dictionary<Type, Type> _initializerByType = new Dictionary<Type, Type>();

		static CollectionInitializer()
		{
			_initializerByType[typeof (ICollection<>)] = typeof (CollectionInitializerImpl<>);
			_initializerByType[typeof(Stack<>)] = typeof(StackInitializer<>);
			_initializerByType[typeof(Queue<>)] = typeof(QueueInitializer<>);
#if NET_3_5 && ! CF
		    _initializerByType[typeof (HashSet<>)] = typeof (HashSetInitializer<>);
#endif
		}

		public static ICollectionInitializer For(object destination)
		{
			if (IsNonGenericList(destination))
			{
			    return new ListInitializer((IList)destination);
			}

			return InitializerFor(destination);
		}

		private static ICollectionInitializer InitializerFor(object destination)
		{
			Type destinationType = destination.GetType();
			if (!destinationType.IsGenericType)
			{
				throw new ArgumentException("Unknown collection: " + destination);
			}

			Type containerType = GenericContainerTypeFor(destination);
			if (containerType != null)
			{
				return GetInitializer(destination, _initializerByType[containerType]);
			}

			throw new ArgumentException("Unknown collection: " + destination);
		}

		private static Type GenericContainerTypeFor(object destination)
		{
			Type containerType = destination.GetType().GetGenericTypeDefinition();
			while (containerType != null && !_initializerByType.ContainsKey(containerType))
			{
				foreach (Type interfaceType in containerType.GetInterfaces())
				{
					if (!interfaceType.IsGenericType)
					{
						continue;
					}

					Type genericInterfaceType = interfaceType.GetGenericTypeDefinition();
					if (_initializerByType.ContainsKey(genericInterfaceType))
					{
						return genericInterfaceType;
					}
				}

				containerType = containerType.BaseType;
			}

			return containerType;
		}

		private static ICollectionInitializer GetInitializer(object destination, Type initializerType)
		{
			ICollectionInitializer initializer = null;
			Type containedElementType = ContainerElementTypeFor(destination);
			if (containedElementType != null)
			{
				Type genericProtocolType = initializerType.MakeGenericType(containedElementType);
				initializer = InstantiateInitializer(destination, genericProtocolType);
			}
			return initializer;
		}

		private static bool IsNonGenericList(object destination)
		{
			return !destination.GetType().IsGenericType && destination is IList;
		}

		private static ICollectionInitializer InstantiateInitializer(object destination, Type genericProtocolType)
	    {
#if !CF
            return (ICollectionInitializer) Activator.CreateInstance(genericProtocolType, destination);
#else
	        ConstructorInfo constructor = genericProtocolType.GetConstructors()[0];
	        return (ICollectionInitializer) constructor.Invoke(new object[] {destination});
#endif
	    }

	    private static Type ContainerElementTypeFor(object destination)
		{
	    	Type containerType = destination.GetType();
	    	return containerType.GetGenericArguments()[0];
		}

		private sealed class ListInitializer : ICollectionInitializer
		{
			private readonly IList _list;

			public ListInitializer(IList list)
			{
				_list = list;
			}

			public void Clear()
			{
				_list.Clear();
			}

			public void Add(object o)
			{
				_list.Add(o);
			}

            public int Count()
            {
                return _list.Count;
            }

			public void FinishAdding()
			{
			}
		}

		private sealed class CollectionInitializerImpl<T> : ICollectionInitializer
		{
			private readonly ICollection<T> _collection;

			public CollectionInitializerImpl(ICollection<T> collection)
			{
				_collection = collection;
			}

			public void Clear()
			{
				_collection.Clear();
			}

            public int Count()
            {
                return _collection.Count;
            }

			public void Add(object o)
			{
				_collection.Add((T)o);
			}

			public void FinishAdding()
			{
			}
		}

		private sealed class StackInitializer<T> : ICollectionInitializer
		{
			private readonly Stack<T> _stack;
			private readonly Stack<T> _tempStack;

			public StackInitializer(Stack<T> stack)
			{
				_stack= stack;
				_tempStack = new Stack<T>();
			}

			public void Clear()
			{
				_tempStack.Clear();
				_stack.Clear();
			}

            public int Count()
            {
                return _stack.Count;
            }

			public void Add(object o)
			{
				_tempStack.Push((T) o);
			}

			public void FinishAdding()
			{
				foreach(T item in _tempStack)
				{
					_stack.Push(item);
				}

				_tempStack.Clear();
			}
		}

		private sealed class QueueInitializer<T> : ICollectionInitializer
		{
			private readonly Queue<T> _queue;

			public QueueInitializer(Queue<T> queue)
			{
				_queue = queue;
			}

			public void Clear()
			{
				_queue.Clear();
			}

            public int Count()
            {
                return _queue.Count;
            }

			public void Add(object o)
			{
				_queue.Enqueue((T) o);
			}

			public void FinishAdding()
			{
			}
		}

#if NET_3_5 && ! CF
        private sealed class HashSetInitializer<T> : ICollectionInitializer
        {
            private readonly HashSet<T> _hashSet;

            public HashSetInitializer(HashSet<T> stack)
            {
                _hashSet = stack;
            }

            public void Clear()
            {
                _hashSet.Clear();
            }

            public void Add(object o)
            {
                _hashSet.Add((T)o);
            }

            public int Count()
            {
                return _hashSet.Count;
            }

            public void FinishAdding()
            {
            }
        }
#endif

	}
}
