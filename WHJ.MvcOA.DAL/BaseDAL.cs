using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using WHJ.MvcOA.DAL;

namespace WHJ.MvcOA.DAL
{
   public class BaseDAL<T>where T:class,new()
    {
       //实现线程内实例唯一
       DbContext db = DbContextFactory.CreateDbContext();      

        #region 1.0 新增 + AddEntity();
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
       public bool AddEntity(T entity)
       {
           db.Set<T>().Add(entity);
           return true;
       }

        #endregion

        #region 3.0 删除指定实体 + DeleteEntity();
        /// <summary>
        /// 删除指定实体
        /// </summary>
        /// <param name="entity"></param>
       public bool DeleteEntity(T entity)
       {
           DbEntityEntry entry = db.Entry<T>(entity);
           entry.State = System.Data.EntityState.Deleted;
           //db.Set<T>().Attach(entity);
           //db.Set<T>().Remove(entity);
           return true;
       }
        #endregion

        #region 4.0 根据指定条件删除 + DeleteList();
        /// <summary>
        /// 根据条件删除
        /// </summary>
        /// <param name="whereLambda">删除条件</param>
       public bool DeleteList(System.Linq.Expressions.Expression<Func<T, bool>> whereLambda)
       {
           var dbQuery = db.Set<T>();
           var delList = dbQuery.Where(whereLambda).ToList();
           foreach (var item in delList)
           {
               //将要删除的元素标记为删除状态
               dbQuery.Remove(item);
           }
           return true;
       }
        #endregion

        #region 5.0 修改实体 + int EditEntity();
        /// <summary>
        /// 修改实体
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
       public bool EditEntity(T entity,params string[] EditNames)
       {
           DbEntityEntry entry = db.Entry<T>(entity);
           entry.State = System.Data.EntityState.Unchanged;
           foreach (string editName in EditNames)
           {
               entry.Property(editName).IsModified = true;
           }
           db.Configuration.ValidateOnSaveEnabled = false;
           return true;
       }
        #endregion

        #region 6.0 根据条件修改 + EditList();
        /// <summary>
        /// 根据条件修改
        /// </summary>
        /// <param name="whereLambda">修改条件</param>
       public bool EditList(System.Linq.Expressions.Expression<Func<T, bool>> whereLambda,string[] editNames,object[] editValues)
       {
           //查询要修改的对象集合
           var editList = db.Set<T>().Where(whereLambda).ToList();
           //获取要修改对象的类型属性
           Type t = typeof(T);
           //循环要修改的实体对象
           foreach (var item in editList)
           {
               //循环要修改的属性名称并反射去除T中的属性对象
               for (int i = 0; i < editNames.Length;i++ )
               {
                   string eName = editNames[i];
                   //获取对象属性
                   PropertyInfo pi = t.GetProperty(eName);
                   //调用属性对象的Setvalue方法
                   pi.SetValue(item, editValues[i], null);
               }
           }
           return true;
       }
        #endregion

        #region 7.0 条件查询 + LoadEntities();
        /// <summary>
        /// 条件查询
        /// </summary>
        /// <param name="whereLambda">查询条件</param>
        /// <returns></returns>
       public IEnumerable<T> LoadEntities(System.Linq.Expressions.Expression<Func<T, bool>> whereLambda)
       {
           return db.Set<T>().Where(whereLambda);            
       }
        #endregion

        #region 8.0 条件查询 + 排序 + LoadOrderEntities<s>();
        /// <summary>
        /// 条件查询 + 排序
        /// </summary>
        /// <typeparam name="s">参数类型</typeparam>
        /// <param name="whereLambda">查询条件</param>
        /// <param name="orderbyLambda">排序条件</param>
        /// <param name="isAsc">true：升序，false：降序</param>
        /// <returns></returns>
       public IEnumerable<T> LoadOrderEntities<s>(System.Linq.Expressions.Expression<Func<T, bool>> whereLambda, System.Linq.Expressions.Expression<Func<T, s>> orderbyLambda, bool isAsc = true)
       {
           var temp = db.Set<T>().Where(whereLambda);
           if (isAsc)
               temp = temp.OrderBy(orderbyLambda);
           else
               temp = temp.OrderByDescending(orderbyLambda);
           return temp;
       }
        #endregion

        #region 9.0 条件查询 + Include + LoadIncludeEntities();
        /// <summary>
        /// 条件查询 + Include
        /// </summary>
        /// <param name="whereLambda">查询条件</param>
        /// <param name="includeNames">连接</param>
        /// <returns></returns>
       public IEnumerable<T> LoadIncludeEntities(System.Linq.Expressions.Expression<Func<T, bool>> whereLambda, params string[] includeNames)
       {
           DbQuery<T> dbQuery = db.Set<T>();
           foreach (string incName in includeNames)
           {
               dbQuery = dbQuery.Include(incName);
           }
           return dbQuery.Where(whereLambda);
       }

        #endregion

        #region 10.0 条件查询 + 排序 + Include + LoadOrderIncludeEntities<s>();
        /// <summary>
        /// 条件查询 + 排序 + Include
        /// </summary>
        /// <typeparam name="s">参数类型</typeparam>
        /// <param name="whereLambda">查询条件</param>
        /// <param name="orderbyLambda">排序条件</param>
        /// <param name="isAsc">true：升序，false：降序</param>
        /// <param name="includeNames">要连接查询的外键属性</param>
        /// <returns></returns>
       public IEnumerable<T> LoadOrderIncludeEntities<s>(System.Linq.Expressions.Expression<Func<T, bool>> whereLambda, System.Linq.Expressions.Expression<Func<T, s>> orderbyLambda, bool isAsc = true, params string[] includeNames)
       {
           DbQuery<T> dbQuery = db.Set<T>();
           foreach (string incName in includeNames)
           {
               dbQuery = dbQuery.Include(incName);
           }
           if (isAsc)
               return dbQuery.Where(whereLambda).OrderBy(orderbyLambda);
           else
               return dbQuery.Where(whereLambda).OrderByDescending(orderbyLambda);
       }
        #endregion

        #region 11.0 条件查询 + 分页 + 排序 + Include + LoadPageEntities<s>();
        /// <summary>
        /// 条件查询 + 分页 + 排序 + Include
        /// </summary>
        /// <typeparam name="s">参数类型</typeparam>
        /// <param name="pageIndex">当前页码</param>
        /// <param name="pageSize">页容量</param>
        /// <param name="toalCount">总行数</param>
        /// <param name="pageCount">总页数</param>
        /// <param name="whereLambda">查询条件</param>
        /// <param name="orderbyLambda">排序类型</param>
        /// <param name="isAsc">true：升序，false：降序</param>
        /// <param name="includeNames">要连接查询的外键属性</param>
        /// <returns></returns>
       public IEnumerable<T> LoadPageEntities<s>(int pageIndex, int pageSize, out int toalCount, out int pageCount, System.Linq.Expressions.Expression<Func<T, bool>> whereLambda, System.Linq.Expressions.Expression<Func<T, s>> orderbyLambda, bool isAsc = true, params string[] includeNames)
       {
           DbQuery<T> dbQuery = db.Set<T>();
           foreach (string incName in includeNames)
           {
               dbQuery = dbQuery.Include(incName);
           }
           IOrderedQueryable<T> orderQuery = null;
           if (isAsc)
               orderQuery = dbQuery.OrderBy(orderbyLambda);
           else
               orderQuery = dbQuery.OrderByDescending(orderbyLambda);
           var temp = orderQuery.Where(whereLambda).Skip((pageIndex - 1) * pageSize).Take(pageSize);
           toalCount = orderQuery.Count();
           pageCount = Convert.ToInt32(Math.Ceiling(toalCount * 1.0 / pageSize));
           return temp;
       }
        #endregion

        #region 12.0 条件查询 + 分页 + 排序 + Include + LoadPageModel()
       /// <summary>
       /// 条件查询 + 分页 + 排序 + Include + LoadPageModel()
       /// </summary>
       /// <typeparam name="s">参数类型</typeparam>
       /// <param name="pageData">分页实体模型</param>
       /// <param name="whereLambda">查询条件</param>
       /// <param name="orderLambda">排序类型</param>
       /// <param name="isAsc">true：升序，false：降序</param>
       /// <param name="includeNames">要连接查询的外键属性</param>
       /// <returns></returns>
       public IEnumerable<T> LoadPageModel<s>(Model.FormatModel.PageData pageData, System.Linq.Expressions.Expression<Func<T, bool>> whereLambda, System.Linq.Expressions.Expression<Func<T, s>> orderLambda, bool isAsc = true, params string[] includeNames)
       {
           DbQuery<T> dbQuery = db.Set<T>();
           foreach (string incName in includeNames)
           {
               dbQuery = dbQuery.Include(incName);
           }
           IOrderedQueryable<T> ordQuery = null;
           if (isAsc)
               ordQuery = dbQuery.OrderBy(orderLambda);
           else
               ordQuery = dbQuery.OrderByDescending(orderLambda);
           //查询
           pageData.rows = ordQuery.Where(whereLambda).Skip((pageData.pageIndex-1)*pageData.pageSize).Take(pageData.pageSize);
           pageData.total = ordQuery.Where(whereLambda).Count();
           return pageData.rows as IEnumerable<T>;
       }
       #endregion

        #region 13.0 执行sql语句
       /// <summary>
       /// 执行sql语句
       /// </summary>
       /// <param name="strSql">sql语句</param>
       /// <param name="paras">SqlParameter数组</param>
       /// <returns></returns>
       public bool ExecuteSQL(string strSql, params SqlParameter[] paras)
       {
           db.Database.ExecuteSqlCommand(strSql, paras);
           return true;
       }
       #endregion
    }
}
