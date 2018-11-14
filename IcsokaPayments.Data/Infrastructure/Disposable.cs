using System;
using System.Collections;
using System.Collections.Generic;

namespace IcsokaPayments.Data.Infrastructure
{
    public  class Disposable : IDisposable
    {
        private bool _isDisposed;

        ~Disposable()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            if (!_isDisposed)
            {
                Dispose(true);
                GC.SuppressFinalize(this);
            }
        }
        private void Dispose(bool disposing)
        {
            if (!_isDisposed && disposing)
            {
                DisposeCore(true);
            }

            _isDisposed = true;
        }

        protected virtual void DisposeCore(bool disposing)
        {
        }

        protected static void DisposeEnumerable(IEnumerable enumerable)
        {
            if (enumerable != null)
            {
                foreach (object obj2 in enumerable)
                {
                    DisposeMember(obj2);
                }
                DisposeMember(enumerable);
            }
        }

        protected static void DisposeDictionary<TK, TV>(IDictionary<TK, TV> dictionary)
        {
            if (dictionary != null)
            {
                foreach (KeyValuePair<TK, TV> pair in dictionary)
                {
                    DisposeMember(pair.Value);
                }
                DisposeMember(dictionary);
            }
        }

        protected static void DisposeDictionary(IDictionary dictionary)
        {
            if (dictionary != null)
            {
                foreach (IDictionaryEnumerator pair in dictionary)
                {
                    DisposeMember(pair.Value);
                }
                DisposeMember(dictionary);
            }
        }

        protected static void DisposeMember(object member)
        {
            IDisposable disposable = member as IDisposable;
            if (disposable != null)
            {
                disposable.Dispose();
            }
        }

        
    }   
}

