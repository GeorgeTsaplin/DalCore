using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace Itb.DalCore.NHibernate {
	/// <summary> Manager for NHibernate interceptors
	/// </summary>
	public sealed class InterceptorsManager : global::NHibernate.IInterceptor {
		private static readonly object StaticSyncObj = new object();
		private static InterceptorsManager _instance;
		/// <summary> Gets instance of interceptors manager
		/// </summary>
		public static InterceptorsManager Instance {
			get {
				if(null == _instance) {
					lock(StaticSyncObj) {
						if(null == _instance) {
							_instance = new InterceptorsManager();
						}
					}
				}
				return _instance;
			}
		}

		private Dictionary<string, global::NHibernate.IInterceptor> _interceptors;

		private InterceptorsManager() {
			_interceptors = new Dictionary<string, global::NHibernate.IInterceptor>(10);
			RegisterDefault();
		}

		private string GetKey(Type type) {
			return type.FullName + "," + type.AssemblyQualifiedName;
		}
		private string GetKey(global::NHibernate.IInterceptor interceptor) {
			return GetKey(interceptor.GetType());
		}
		private static readonly string DefaultKey = "default";
		private void RegisterDefault() {
			_interceptors.Add(DefaultKey, new global::NHibernate.EmptyInterceptor());
		}
		private void UnregisterDefault() {
			_interceptors.Remove(DefaultKey);
		}
		/// <summary> Register specified interceptor
		/// </summary>
		/// <param name="interceptor">interceptor</param>
        /// <param name="logger">logger</param>
		/// <exception cref="ArgumentException">
		/// Specified interceptor already registered
		/// </exception>
		public void Register(global::NHibernate.IInterceptor interceptor, ILogger logger = null) {
			if(null == interceptor) {
				throw new ArgumentNullException("interceptor");
			}
			UnregisterDefault();
			try {
				string key = GetKey(interceptor);
				_interceptors.Add(key, interceptor);

				if(logger?.IsEnabled(LogLevel.Information) == true)
                {
					logger.LogInformation($"Register interceptor: {key}");
				}
			} finally {
				RegisterDefault();
			}
		}
		/// <summary> Unregister specified interceptor type
		/// </summary>
		/// <param name="interceptorType">interceptor type</param>
        /// <param name="logger">logger</param>
		/// <returns>true if interceptor was previously registered</returns>
		public bool Unregister(Type interceptorType, ILogger logger = null) {
			if(null == interceptorType) {
				throw new ArgumentNullException("interceptorType");
			}
			string key = GetKey(interceptorType);
			bool res = _interceptors.Remove(key);
			if(res && logger?.IsEnabled(LogLevel.Information) == true) {
				logger.LogInformation($"Unregister interceptor: {key}");
			}
			return res;
		}

		#region IInterceptor
#pragma warning disable 1591

		void global::NHibernate.IInterceptor.AfterTransactionBegin(global::NHibernate.ITransaction tx) {
			foreach(var item in _interceptors) {
				item.Value.AfterTransactionBegin(tx);
			}
		}
		void global::NHibernate.IInterceptor.AfterTransactionCompletion(global::NHibernate.ITransaction tx) {
			foreach(var item in _interceptors) {
				item.Value.AfterTransactionCompletion(tx);
			}
		}
		void global::NHibernate.IInterceptor.BeforeTransactionCompletion(global::NHibernate.ITransaction tx) {
			foreach(var item in _interceptors) {
				item.Value.BeforeTransactionCompletion(tx);
			}
		}
		int[] global::NHibernate.IInterceptor.FindDirty(object entity, object id, object[] currentState, object[] previousState, string[] propertyNames, global::NHibernate.Type.IType[] types) {
			List<int> res = new List<int>(_interceptors.Count);
			foreach(var item in _interceptors) {
				int[] tmp = item.Value.FindDirty(entity, id, currentState, previousState, propertyNames, types);
				if(null != tmp) {
					res.AddRange(tmp);
				}
			}
			return ((res.Count == 0) ? null : res.ToArray());
		}
		object global::NHibernate.IInterceptor.GetEntity(string entityName, object id) {
			object res = null;
			foreach(var item in _interceptors) {
				res = item.Value.GetEntity(entityName, id);
				if(null != res) {
					break;
				}
			}
			return res;
		}
		string global::NHibernate.IInterceptor.GetEntityName(object entity) {
			string res = null;
			foreach(var item in _interceptors) {
				res = item.Value.GetEntityName(entity);
				if(null != res) {
					break;
				}
			}
			return res;
		}
		object global::NHibernate.IInterceptor.Instantiate(string entityName, object id) {
			object res = null;
			foreach(var item in _interceptors) {
				res = item.Value.Instantiate(entityName, id);
				if(null != res) {
					break;
				}
			}
			return res;
		}
		bool? global::NHibernate.IInterceptor.IsTransient(object entity) {
			bool? res = null;
			foreach(var item in _interceptors) {
				res = item.Value.IsTransient(entity);
				if(res.HasValue) {
					break;
				}
			}
			return res;
		}
		void global::NHibernate.IInterceptor.OnCollectionRecreate(object collection, object key) {
			foreach(var item in _interceptors) {
				item.Value.OnCollectionRecreate(collection, key);
			}
		}
		void global::NHibernate.IInterceptor.OnCollectionRemove(object collection, object key) {
			foreach(var item in _interceptors) {
				item.Value.OnCollectionRemove(collection, key);
			}
		}
		void global::NHibernate.IInterceptor.OnCollectionUpdate(object collection, object key) {
			foreach(var item in _interceptors) {
				item.Value.OnCollectionUpdate(collection, key);
			}
		}
		void global::NHibernate.IInterceptor.OnDelete(object entity, object id, object[] state, string[] propertyNames, global::NHibernate.Type.IType[] types) {
			foreach(var item in _interceptors) {
				item.Value.OnDelete(entity, id, state, propertyNames, types);
			}
		}
		bool global::NHibernate.IInterceptor.OnFlushDirty(object entity, object id, object[] currentState, object[] previousState, string[] propertyNames, global::NHibernate.Type.IType[] types) {
			bool res = false;
			foreach(var item in _interceptors) {
				res |= item.Value.OnFlushDirty(entity, id, currentState, previousState, propertyNames, types);
			}
			return res;
		}
		bool global::NHibernate.IInterceptor.OnLoad(object entity, object id, object[] state, string[] propertyNames, global::NHibernate.Type.IType[] types) {
			bool res = false;
			foreach(var item in _interceptors) {
				res |= item.Value.OnLoad(entity, id, state, propertyNames, types);
			}
			return res;
		}
		global::NHibernate.SqlCommand.SqlString global::NHibernate.IInterceptor.OnPrepareStatement(global::NHibernate.SqlCommand.SqlString sql) {
			global::NHibernate.SqlCommand.SqlString res = sql;
			foreach(var item in _interceptors) {
				res = item.Value.OnPrepareStatement(res);
			}
			return res;
		}
		bool global::NHibernate.IInterceptor.OnSave(object entity, object id, object[] state, string[] propertyNames, global::NHibernate.Type.IType[] types) {
			bool res = false;
			foreach(var item in _interceptors) {
				res |= item.Value.OnSave(entity, id, state, propertyNames, types);
			}
			return res;
		}
		void global::NHibernate.IInterceptor.PostFlush(System.Collections.ICollection entities) {
			foreach(var item in _interceptors) {
				item.Value.PostFlush(entities);
			}
		}
		void global::NHibernate.IInterceptor.PreFlush(System.Collections.ICollection entities) {
			foreach(var item in _interceptors) {
				item.Value.PreFlush(entities);
			}
		}
		void global::NHibernate.IInterceptor.SetSession(global::NHibernate.ISession session) {
			foreach(var item in _interceptors) {
				item.Value.SetSession(session);
			}
		}

#pragma warning restore 1591
		#endregion //IInterceptor
	}
}
