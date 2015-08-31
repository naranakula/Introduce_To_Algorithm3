using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Introduce_To_Algorithm3.Common.Utils.sqls.EF
{
    public static class DALHelper
    {
        /// <summary>
        /// 对DbContext执行Action
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="context"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public static bool Retrival<T>(this T context, Action<T> action) where T : DbContext, new()
        {
            try
            {
                using (context)
                {
                    action(context);
                }

                return true;
            }
            catch (Exception ex)
            {
                // Log Error
                return false;
            }
        }

        /// <summary>
        /// 获取通用结果
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="context"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        public static TResult Retrival<T, TResult>(this T context, Func<T, TResult> func) where T : DbContext, new()
        {
            try
            {
                using (context)
                {
                    TResult result = func(context);
                    return result;
                }
            }
            catch (Exception)
            {
                //Log Error
                return default(TResult);
            }
        }

        /// <summary>
        /// 异步调用
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="context"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        public static async Task<TResult> RetrivalAsync<T, TResult>(this T context, Func<T, Task<TResult>> func) where T : DbContext, new() where TResult:new()
        {
            try
            {
                using (context)
                {
                    return await func(context);
                }
            }
            catch (Exception ex)
            {
                return default(TResult);
            }
        }

        /// <summary>
        /// 无超时的异步执行
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="func"></param>
        /// <returns></returns>
        public static async Task<TResult> LongRetrivalAsync<T, TResult>(this T context,Func<T,Task<TResult>> func) where T:DbContext,new() where TResult:new()
        {
            try
            {
                using (context)
                {
                    ((IObjectContextAdapter) context).ObjectContext.CommandTimeout = 0;
                    //没有独占锁，也没有共享锁
                    using (var dbTransaction = context.Database.BeginTransaction(IsolationLevel.ReadUncommitted))
                    {
                        return await func(context);
                    }
                }
            }
            catch (Exception ex)
            {
                //Log Error
                return default(TResult);
            }
        }

        /// <summary>
        /// 原子事务
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="context"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public static bool SafeTransaction<T>(this T context, Action<T> action) where T : DbContext, new()
        {
            using (context)
            {
                using (DbContextTransaction dbTransaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        action(context);
                        dbTransaction.Commit();
                        return true;
                    }
                    catch (Exception)
                    {
                        dbTransaction.Rollback();
                        //Log Error
                        return false;
                    }
                }
            }
        }

        /// <summary>
        /// 异步任务
        /// </summary>
        /// <param name="context"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public static async Task<int?> SafeTransactionAsync<T>(this T context, Action<T> action) where T : DbContext, new()
        {
            using (context)
            {
                using (var dbTransaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        action(context);
                        int affectedRecords = await context.SaveChangesAsync();
                        dbTransaction.Commit();
                        return affectedRecords;
                    }
                    catch (Exception ex)
                    {
                        dbTransaction.Rollback();
                        //Log Error
                        return null;
                    }
                }
            }
        }

        /// <summary>
        /// 异步原子事务操作
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="context"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        public static async Task<int?> SafeTransactionAsync<T>(this T context, Func<T, Task<int>> func)
            where T : DbContext, new()
        {
            using (context)
            {
                using (var dbTransaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        int affectedRecords = await func(context);
                        dbTransaction.Commit();
                        return affectedRecords;
                    }
                    catch (Exception ex)
                    {
                        dbTransaction.Rollback();
                        //Log error
                        return null;
                    }
                }
            }
        }
    }
}
