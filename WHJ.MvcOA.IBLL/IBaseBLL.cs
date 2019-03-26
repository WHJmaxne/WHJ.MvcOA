using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WHJ.MvcOA.IBLL
{
   public interface IBaseBLL<T>where T:class,new()
    {
        #region 1.0 新增 + AddEntity();
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        bool AddEntity(T entity);

        #endregion

        #region 3.0 删除指定实体 + DeleteEntity();
        /// <summary>
        /// 删除指定实体
        /// </summary>
        /// <param name="entity"></param>
        bool DeleteEntity(T entity);
        #endregion

        #region 4.0 根据指定条件删除 + DeleteList();
        /// <summary>
        /// 根据条件删除
        /// </summary>
        /// <param name="whereLambda">删除条件</param>
        bool DeleteList(System.Linq.Expressions.Expression<Func<T, bool>> whereLambda);
        #endregion

        #region 5.0 修改实体 + int EditEntity();
        /// <summary>
        /// 修改实体
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        bool EditEntity(T entity, params string[] EditNames);
        #endregion

        #region 6.0 根据条件修改 + EditList();
        /// <summary>
        /// 根据条件修改
        /// </summary>
        /// <param name="whereLambda">修改条件</param>
        /// <param name="editNames">修改的属性名</param>
        /// <param name="editValues">修改的值</param>
        bool EditList(System.Linq.Expressions.Expression<Func<T, bool>> whereLambda, string[] editNames, object[] editValues);
        #endregion

        #region 7.0 条件查询 + LoadEntities();
        /// <summary>
        /// 条件查询
        /// </summary>
        /// <param name="whereLambda">查询条件</param>
        /// <returns></returns>
        IEnumerable<T> LoadEntities(System.Linq.Expressions.Expression<Func<T, bool>> whereLambda);
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
        IEnumerable<T> LoadOrderEntities<s>(System.Linq.Expressions.Expression<Func<T, bool>> whereLambda, System.Linq.Expressions.Expression<Func<T, s>> orderbyLambda, bool isAsc = true);
        #endregion

        #region 9.0 条件查询 + Include + LoadIncludeEntities();
        /// <summary>
        /// 条件查询 + Include
        /// </summary>
        /// <param name="whereLambda">查询条件</param>
        /// <param name="includeNames">连接</param>
        /// <returns></returns>
        IEnumerable<T> LoadIncludeEntities(System.Linq.Expressions.Expression<Func<T, bool>> whereLambda, params string[] includeNames);

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
        IEnumerable<T> LoadOrderIncludeEntities<s>(System.Linq.Expressions.Expression<Func<T, bool>> whereLambda, System.Linq.Expressions.Expression<Func<T, s>> orderbyLambda, bool isAsc = true, params string[] includeNames);
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
        IEnumerable<T> LoadPageEntities<s>(int pageIndex, int pageSize, out int toalCount, out int pageCount, System.Linq.Expressions.Expression<Func<T, bool>> whereLambda, System.Linq.Expressions.Expression<Func<T, s>> orderbyLambda, bool isAsc = true, params string[] includeNames);
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
        IEnumerable<T> LoadPageModel<s>(Model.FormatModel.PageData pageData, System.Linq.Expressions.Expression<Func<T, bool>> whereLambda, System.Linq.Expressions.Expression<Func<T, s>> orderLambda, bool isAsc = true, params string[] includeNames);
        
        #endregion
    }
}
