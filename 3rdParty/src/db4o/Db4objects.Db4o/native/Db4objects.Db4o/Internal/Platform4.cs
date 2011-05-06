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
using System.Globalization;
using System.IO;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Config.Attributes;
using Db4objects.Db4o.Ext;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Internal.Activation;
using Db4objects.Db4o.Internal.Handlers;
using Db4objects.Db4o.Internal.Query;
using Db4objects.Db4o.Internal.Query.Processor;
using Db4objects.Db4o.Query;
using Db4objects.Db4o.Reflect;
using Db4objects.Db4o.Reflect.Generic;
using Db4objects.Db4o.Reflect.Net;
using Db4objects.Db4o.Typehandlers;
using Sharpen.IO;

namespace Db4objects.Db4o.Internal
{
    /// <exclude />
    public class Platform4
    {
		private static readonly LegacyDb4oAssemblyNameMapper _assemlbyNameMapper = new LegacyDb4oAssemblyNameMapper();

        private static List<ObjectContainerBase> _containersToBeShutdown;

		private static readonly object _shutdownStreamsLock = new object();

		public static object[] CollectionToArray(ObjectContainerBase stream, object obj)
        {
            Collection4 col = FlattenCollection(stream, obj);
            object[] ret = new object[col.Size()];
            col.ToArray(ret);
            return ret;
        }

    	internal static void AddShutDownHook(ObjectContainerBase container)
        {
            lock (_shutdownStreamsLock)
            {
                if (_containersToBeShutdown == null)
                {
					_containersToBeShutdown = new List<ObjectContainerBase>();
#if !CF && !SILVERLIGHT
                	AppDomain.CurrentDomain.ProcessExit += OnShutDown;
					AppDomain.CurrentDomain.DomainUnload += OnShutDown;
#endif
                }
                _containersToBeShutdown.Add(container);
            }
        }

        internal static byte[] Serialize(Object obj)
        {
            throw new NotSupportedException();
        }

        internal static Object Deserialize(byte[] bytes)
        {
            throw new NotSupportedException();
        }

        internal static bool CanSetAccessible()
        {
            return true;
        }

        internal static IReflector CreateReflector(Object config)
		{
#if USE_FAST_REFLECTOR && !CF && !SILVERLIGHT
			return new Db4objects.Db4o.Internal.Reflect.FastNetReflector();
#else
			return new NetReflector();
#endif
		}

        public static IReflector ReflectorForType(Type typeInstance)
		{
#if USE_FAST_REFLECTOR && !CF && !SILVERLIGHT
			return new Db4objects.Db4o.Internal.Reflect.FastNetReflector();
#else
			return new NetReflector();
#endif
		}

        internal static Object CreateReferenceQueue()
        {
            return new WeakReferenceHandlerQueue();
        }

        public static Object CreateWeakReference(Object obj)
        {
            return new WeakReference(obj, false);
        }

        internal static Object CreateActiveObjectReference(Object referenceQueue, Object yapObject, Object obj)
        {
            return new WeakReferenceHandler(referenceQueue, yapObject, obj);
        }

        internal static long DoubleToLong(double a_double)
		{
#if CF || SILVERLIGHT
			byte[] bytes = BitConverter.GetBytes(a_double);
            return BitConverter.ToInt64(bytes, 0);
#else
            return BitConverter.DoubleToInt64Bits(a_double);
#endif
        }

        internal static QConEvaluation EvaluationCreate(Transaction a_trans, Object example)
        {
            if (example is IEvaluation || example is EvaluationDelegate)
            {
                return new QConEvaluation(a_trans, example);
            }
            return null;
        }

        internal static void EvaluationEvaluate(Object a_evaluation, ICandidate a_candidate)
        {
            IEvaluation eval = a_evaluation as IEvaluation;
            if (eval != null)
            {
                eval.Evaluate(a_candidate);
            }
            else
            {
                // use starting _ for PascalCase conversion purposes
                EvaluationDelegate _ed = a_evaluation as EvaluationDelegate;
                if (_ed != null)
                {
                    _ed(a_candidate);
                }
            }
        }

        internal static Config4Class ExtendConfiguration(IReflectClass clazz, IConfiguration config, Config4Class classConfig)
        {
            Type t = GetNetType(clazz);
            if (t == null)
            {
                return classConfig;
            }
            ConfigurationIntrospector a = new ConfigurationIntrospector(t, classConfig, config);
            a.Apply();
            return a.ClassConfiguration;
        }

		internal static Collection4 FlattenCollection(ObjectContainerBase stream, Object obj)
        {
            Collection4 collection41 = new Collection4();
            FlattenCollection1(stream, obj, collection41);
            return collection41;
        }

		internal static void FlattenCollection1(ObjectContainerBase stream, Object obj, Collection4 collection4)
        {
            Array arr = obj as Array;
            if (arr != null)
            {
                IReflectArray reflectArray = stream.Reflector().Array();

                Object[] flat = new Object[arr.Length];

                reflectArray.Flatten(obj, reflectArray.Dimensions(obj), 0, flat, 0);
                for (int i = 0; i < flat.Length; i++)
                {
                    FlattenCollection1(stream, flat[i], collection4);
                }
            }
            else
            {
                // If obj implements IEnumerable, add all elements to collection4
                IEnumerator enumerator = GetCollectionEnumerator(obj, true);

                // Add elements to collection if conversion was succesful
                if (enumerator != null)
                {
                    if (enumerator is IDictionaryEnumerator)
                    {
                        IDictionaryEnumerator dictEnumerator = enumerator as IDictionaryEnumerator;
                        while (enumerator.MoveNext())
                        {
                            FlattenCollection1(stream, dictEnumerator.Key, collection4);
                        }
                    }
                    else
                    {
                        while (enumerator.MoveNext())
                        {
                            // recursive call to flatten Collections in Collections
                            FlattenCollection1(stream, enumerator.Current, collection4);
                        }
                    }
                }
                else
                {
                    // If obj is not a Collection, it still needs to be collected.
                    collection4.Add(obj);
                }
            }
        }

        internal static void ForEachCollectionElement(Object obj, IVisitor4 visitor)
        {
            IEnumerator enumerator = GetCollectionEnumerator(obj, false);
            if (enumerator != null)
            {
                // If obj is a map (IDictionary in .NET speak) call Visit() with the key
                // otherwise use the element itself
                if (enumerator is IDictionaryEnumerator)
                {
                    IDictionaryEnumerator dictEnumerator = enumerator as IDictionaryEnumerator;
                    while (enumerator.MoveNext())
                    {
                        visitor.Visit(dictEnumerator.Key);
                    }
                }
                else
                {
                    while (enumerator.MoveNext())
                    {
                        visitor.Visit(enumerator.Current);
                    }
                }
            }
        }

        internal static String Format(DateTime date, bool showSeconds)
        {
            String fmt = "yyyy-MM-dd";
            if (showSeconds)
            {
                fmt += " HH:mm:ss";
            }
            return date.ToString(fmt);
        }

        internal static IEnumerator GetCollectionEnumerator(object obj, bool allowArray)
        {
			IEnumerable enumerable = obj as IEnumerable;
			if (enumerable == null) return null;
		    if (obj is string) return null;
            if (!allowArray && obj is Array) return null;
		    return enumerable.GetEnumerator();
		}

        internal static void GetDefaultConfiguration(Config4Impl config)
        {
            if (IsCompact())
            {
                config.SingleThreadedClient(true);
            }

            Translate(config, typeof(Delegate), new TNull());
            Translate(config, typeof(Type), new TType()); // TODO: unnecessary?
            Translate(config, typeof(Type).GetType(), new TType());

#if !CF && !SILVERLIGHT
            if (IsMono())
            {

				Translate(config, new Exception(), new TSerializable());
            }
#endif

#if !SILVERLIGHT
            Translate(config, new ArrayList(), new TList());
            Translate(config, new Hashtable(), new TDictionary());
            Translate(config, new Queue(), new TQueue());
            Translate(config, new Stack(), new TStack());
#endif
			Translate(config, CultureInfo.InvariantCulture, new TCultureInfo());

            if (!IsCompact())
            {
				Translate(config, "System.Collections.SortedList, mscorlib", new TDictionary());
            }

            new TypeHandlerConfigurationDotNet(config).Apply();

        	config.ObjectClass(typeof (ActivatableBase)).Indexed(false);
        }

        public static bool IsCompact()
		{
#if CF || SILVERLIGHT
			return true;
#else
            return false;
#endif
        }

        public static bool IsMono()
        {
            return null != Type.GetType("System.MonoType, mscorlib");
        }

        public static Object GetTypeForClass(Object obj)
        {
            return obj;
        }

        internal static Object GetYapRefObject(Object obj)
        {
			WeakReferenceHandler refHandler = obj as WeakReferenceHandler;
			if (refHandler != null)
            {
				return refHandler.Get();
            }
            return obj;
        }

        internal static bool HasCollections()
        {
            return true;
        }

        public static bool NeedsLockFileThread()
        {
            return false;
        }

        public static bool HasWeakReferences()
        {
            return true;
        }

        internal static bool IgnoreAsConstraint(Object obj)
        {
            Type t = obj.GetType();
            if (t.IsEnum)
            {
                if (System.Convert.ToInt32(obj) == 0)
                {
                    return true;
                }
            }
            return false;
        }

        internal static bool IsCollectionTranslator(Config4Class config4class)
        {
            if (config4class != null)
            {
                IObjectTranslator ot = config4class.GetTranslator();
                if (ot != null)
                {
#if SILVERLIGHT
					return false;
#else
					return ot is TList || ot is TDictionary || ot is TQueue || ot is TStack;
#endif
				}
            }
            return false;
        }

        public static bool IsConnected(Sharpen.Net.Socket socket)
        {
            if (socket == null)
            {
                return false;
            }
            return socket.IsConnected();
        }

        internal static bool IsStruct(IReflectClass claxx)
        {
            if (claxx == null)
            {
                return false;
            }
            System.Type netClass = GetNetType(claxx);
            if (netClass == null)
            {
                return false;
            }
            return netClass.IsValueType;
        }

        internal static void KillYapRef(Object obj)
        {
			WeakReferenceHandler yr = obj as WeakReferenceHandler;
            if (yr != null)
            {
                yr.ObjectReference = null;
            }
        }

        internal static double LongToDouble(long l)
		{
#if CF || SILVERLIGHT
			byte[] bytes = BitConverter.GetBytes(l);
            return BitConverter.ToDouble(bytes, 0);
#else
            return BitConverter.Int64BitsToDouble(l);
#endif
        }

        internal static void LockFile(string path, object file)
		{
#if !CF && !SILVERLIGHT
			try
            {
                FileStream stream = ((RandomAccessFile) file).Stream;
                stream.Lock(0, 1);
            }
            catch (IOException x)
            {
                throw new DatabaseFileLockedException(path,x);
            }
#endif
        }

        internal static void UnlockFile(string path, object file)
        {
            // do nothing. C# RAF is unlocked automatically upon closing
        }

        internal static void MarkTransient(String marker)
        {
            NetField.MarkTransient(marker);
        }

        internal static bool CallConstructor()
        {
            return false;
        }

        internal static void PollReferenceQueue(Object container, Object referenceQueue)
        {
            ((WeakReferenceHandlerQueue)referenceQueue).Poll((ObjectContainerBase)container);
        }

        public static void RegisterCollections(GenericReflector reflector)
        {
//            reflector.RegisterCollectionUpdateDepth(
//                typeof(IDictionary),
//                3);
        }

        internal static void RemoveShutDownHook(ObjectContainerBase container)
        {
            lock (_shutdownStreamsLock)
            {
                if (_containersToBeShutdown != null)
                {
                    _containersToBeShutdown.Remove(container);
                }
            }
        }

        public static void SetAccessible(Object obj)
        {
            // do nothing
        }

        private static void OnShutDown(object sender, EventArgs args)
        {
			lock (_shutdownStreamsLock)
			{
				foreach (ObjectContainerBase container in _containersToBeShutdown.ToArray())
				{
					container.ShutdownHook(); // this will remove the stream for the list
				}				
			}
        }

        public static bool StoreStaticFieldValues(IReflector reflector, IReflectClass clazz)
        {
            return false;
        }

		private static void Translate(IConfiguration config, object obj, IObjectTranslator translator)
		{
			config.ObjectClass(obj).Translate(translator);
		}

        public static byte[] UpdateClassName(byte[] nameBytes)
        {
            return _assemlbyNameMapper.MappedNameFor(nameBytes);
        }

        public static Object WeakReferenceTarget(Object weakRef)
        {
            WeakReference wr = weakRef as WeakReference;
            if (wr != null)
            {
                return wr.Target;
            }
            return weakRef;
        }

        internal static object WrapEvaluation(object evaluation)
		{
#if CF || SILVERLIGHT
			// FIXME: How to better support EvaluationDelegate on the CompactFramework?
			return evaluation;
#else
            return (evaluation is EvaluationDelegate)
                ? new EvaluationDelegateWrapper((EvaluationDelegate)evaluation)
                : evaluation;
#endif
        }

        public static bool IsTransient(IReflectClass clazz)
        {
            Type type = GetNetType(clazz);
            if (null == type) return false;
        	return IsTransient(type);
        }

    	public static bool IsTransient(Type type)
    	{
    		return type.IsPointer
    		       || type.IsSubclassOf(typeof(Delegate))
#if CF || SILVERLIGHT
;
#else
    		       || type == typeof(System.Reflection.Pointer);
#endif
    	}

    	private static Type GetNetType(IReflectClass clazz)
        {
            if (null == clazz)
            {
                return null;
            }

            NetClass netClass = clazz as NetClass;
            if (null != netClass)
            {
                return netClass.GetNetType();
            }
            IReflectClass claxx = clazz.GetDelegate();
            if (claxx == clazz)
            {
                return null;
            }
            return GetNetType(claxx);
        }

		public static NetTypeHandler[] Types(IReflector reflector)
        {
			return new NetTypeHandler[]
				{
					new SByteHandler(),
					new DecimalHandler(),
					new UIntHandler(),
					new ULongHandler(),
					new UShortHandler(),
				};
        }

        public static bool IsSimple(Type a_class)
        {
            for (int i1 = 0; i1 < SIMPLE_CLASSES.Length; i1++)
            {
                if (a_class == SIMPLE_CLASSES[i1])
                {
                    return true;
                }
            }
            return false;
        }

        private static Type[] SIMPLE_CLASSES = {
		                                        	typeof(Int32),
		                                        	typeof(Int64),
		                                        	typeof(Single),
		                                        	typeof(Boolean),
		                                        	typeof(Double),
		                                        	typeof(Byte),
		                                        	typeof(Char),
		                                        	typeof(Int16),
		                                        	typeof(String),
		                                        };

        public static DateTime Now()
        {
            return DateTime.Now;
        }

		internal static bool IsJavaEnum(IReflector genericReflector, IReflectClass iReflectClass)
		{
			return false;
		}

		internal static bool IsEnum(IReflector genericReflector, IReflectClass iReflectClass)
		{
		    Type type = GetNetType(iReflectClass);
            if(type == null)
            {
                return false;
            }
			return type.IsEnum;
		}

        public static bool UseNativeSerialization() 
        {
            return false;
        }

        public static void RegisterPlatformHandlers(ObjectContainerBase container)
        {
            EnumTypeHandler enumTypeHandler = new EnumTypeHandler();
            container.ConfigImpl.RegisterTypeHandler(new EnumTypeHandlerPredicate(), enumTypeHandler);
			container.Handlers.RegisterHandlerVersion(enumTypeHandler, 4, new StandardReferenceTypeHandler());
			container.Handlers.RegisterHandlerVersion(enumTypeHandler, 0, new StandardReferenceTypeHandler0());

			GuidTypeHandler guidTypeHandler = new GuidTypeHandler();
			container.ConfigImpl.RegisterTypeHandler(new SingleClassTypeHandlerPredicate(typeof(Guid)), guidTypeHandler);
			container.Handlers.RegisterHandlerVersion(guidTypeHandler, 8, new StandardReferenceTypeHandler());

            DateTimeHandler dateTimeHandler = new DateTimeHandler();
            container.Handlers.RegisterNetTypeHandler(dateTimeHandler);
            container.Handlers.RegisterHandlerVersion(dateTimeHandler, 6, new DateTimeHandler6());

#if !CF
        	DateTimeOffsetTypeHandler dateTimeOffsetHandler = new DateTimeOffsetTypeHandler();
        	container.ConfigImpl.RegisterTypeHandler(new SingleClassTypeHandlerPredicate(typeof(DateTimeOffset)), dateTimeOffsetHandler);
			container.Handlers.RegisterHandlerVersion(dateTimeOffsetHandler, 9, new StandardReferenceTypeHandler());
#endif
        }

		public static Type[] PrimitiveTypes()
		{
			return PRIMITIVE_TYPES;
		}

        public static object NullValue(Type type) 
        {
            if(_nullValues == null) 
            { 
                InitNullValues();
			}
			
			return _nullValues.Get(type);                
		}
		
        private static void InitNullValues()
        {
    	    _nullValues = new Hashtable4();
            _nullValues.Put(typeof(int), 0);
            _nullValues.Put(typeof(uint), (uint)0);
            _nullValues.Put(typeof(byte), (byte)0);
    	    _nullValues.Put(typeof(short), (short)0);
    	    _nullValues.Put(typeof(float), (float)0);
    	    _nullValues.Put(typeof(double), (double)0);
            _nullValues.Put(typeof(ulong), (ulong)0);
            _nullValues.Put(typeof(long), (long)0);
    	    _nullValues.Put(typeof(bool), false);
            _nullValues.Put(typeof(char), (char)0);
            _nullValues.Put(typeof(sbyte), (sbyte)0);
            _nullValues.Put(typeof(decimal), (decimal)0);
            _nullValues.Put(typeof(ushort), (ushort)0);
            _nullValues.Put(typeof(DateTime), DateTime.MinValue);
        	
        }

        private static Hashtable4 _nullValues;
		
        public static Type NullableTypeFor(Type primitiveType) 
        {
            if(_primitive2Wrapper == null)
                InitPrimitive2Wrapper();
            Type wrapperClazz = (Type)_primitive2Wrapper.Get(primitiveType);
            if(wrapperClazz==null)        
                throw new NotImplementedException();
            return wrapperClazz;
        }
    
        private static void InitPrimitive2Wrapper()
        {
    	    _primitive2Wrapper = new Hashtable4();

        	foreach (Type type in PRIMITIVE_TYPES)
        	{
				_primitive2Wrapper.Put(type, ConcreteNullableTypeFor(type));
        	}
        }

    	private static Type ConcreteNullableTypeFor(Type type)
    	{
    		return typeof (Nullable<>).MakeGenericType(type);
    	}

    	private static Hashtable4 _primitive2Wrapper;

    	private static readonly Type[] PRIMITIVE_TYPES = new Type[]
    	                                        	{
														typeof(int),
														typeof(uint), 
														typeof(byte), 
														typeof(short),
														typeof(float),
														typeof(double),
														typeof(ulong),
														typeof(long), 
														typeof(bool), 
														typeof(char), 
														typeof(sbyte),
														typeof(decimal),
														typeof(ushort),
														typeof(DateTime),
    	                                        	};

        public static void ThrowUncheckedException(Exception exc)
        {
            throw exc;
        }

        public static sbyte ToSByte(byte b)
        {
		    return (sbyte)b;
	    }

    }
}