﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;

namespace System.Data.Store
{
    
    public sealed class UnitOfWork:IDisposable
    {
        private List<Func<int>> saveChangeHandlers = new List<Func<int>>();
        public UnitOfWork(params ISave[] stores)
        {
            this.Attach(stores);
           
        }
        public void Attach(params ISave[] stores)
        {
            foreach (var store in stores ?? Enumerable.Empty<ISave>())
            {
                if (store != null)
                {
                    var saveAction = store.GetSaveChangesMethod();
                    if (saveAction != null && !saveChangeHandlers.Contains(saveAction))
                    {
                        saveChangeHandlers.Add(saveAction);
                    }
                }
            }
        }

        public int SaveChanges()
        {
            int sum = 0;
            //Parallel.ForEach<Func<int>,int>(this.saveChangeHandlers,
            //    ()=> 0, 
            //    (item, state, before,result) => { return item.Invoke();},
            //    a => { sum = a; });
            foreach (var v in this.saveChangeHandlers)
            {
                sum += v.Invoke();
            }
            return sum;
        }

        

        #region IDisposable Support
        private bool disposedValue = false; // 要检测冗余调用

        public void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: 释放托管状态(托管对象)。
                }

                // TODO: 释放未托管的资源(未托管的对象)并在以下内容中替代终结器。
                // TODO: 将大型字段设置为 null。

                disposedValue = true;
            }
        }

        // TODO: 仅当以上 Dispose(bool disposing) 拥有用于释放未托管资源的代码时才替代终结器。
        // ~UnitOfWork() {
        //   // 请勿更改此代码。将清理代码放入以上 Dispose(bool disposing) 中。
        //   Dispose(false);
        // }

        // 添加此代码以正确实现可处置模式。
        void IDisposable.Dispose()
        {
            // 请勿更改此代码。将清理代码放入以上 Dispose(bool disposing) 中。
            Dispose(true);
            // TODO: 如果在以上内容中替代了终结器，则取消注释以下行。
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}
