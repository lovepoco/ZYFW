using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace ZY.Framework.Data.ICommon
{
    /// <summary>
    /// 扩展Linq表达式组合
    /// </summary>
    public static class PredicateExtensions
    {
        /// <summary>
        /// 逻辑初始 - 真
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static Expression<Func<T, bool>> True<T>() { return f => true; }
        /// <summary>
        /// 逻辑初始 - 假
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static Expression<Func<T, bool>> False<T>() { return f => false; }
        /// <summary>
        /// And条件组合
        /// <para>NHibernate请使用AndAlso</para>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="exp_left"></param>
        /// <param name="exp_right"></param>
        /// <returns></returns>
        public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> exp_left, Expression<Func<T, bool>> exp_right)
        {
            var candidateExpr = Expression.Parameter(typeof(T), "candidate");
            var parameterReplacer = new ParameterReplacer(candidateExpr);

            var left = parameterReplacer.Replace(exp_left.Body);
            var right = parameterReplacer.Replace(exp_right.Body);
            var body = Expression.And(left, right);

            return Expression.Lambda<Func<T, bool>>(body, candidateExpr);
        }
        /// <summary>
        /// Or条件组合
        /// <para>NHibernate请使用OrElse</para>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="exp_left"></param>
        /// <param name="exp_right"></param>
        /// <returns></returns>
        public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> exp_left, Expression<Func<T, bool>> exp_right)
        {
            var candidateExpr = Expression.Parameter(typeof(T), "candidate");
            var parameterReplacer = new ParameterReplacer(candidateExpr);

            var left = parameterReplacer.Replace(exp_left.Body);
            var right = parameterReplacer.Replace(exp_right.Body);
            var body = Expression.Or(left, right);

            return Expression.Lambda<Func<T, bool>>(body, candidateExpr);
        }

        /// <summary>
        /// AndAlso条件组合 - 适用NH
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="exp_left"></param>
        /// <param name="exp_right"></param>
        /// <returns></returns>
        public static Expression<Func<T, bool>> AndAlso<T>(this Expression<Func<T, bool>> exp_left, Expression<Func<T, bool>> exp_right)
        {
            var candidateExpr = Expression.Parameter(typeof(T), "candidate");
            var parameterReplacer = new ParameterReplacer(candidateExpr);

            var left = parameterReplacer.Replace(exp_left.Body);
            var right = parameterReplacer.Replace(exp_right.Body);
            var body = Expression.AndAlso(left, right);

            return Expression.Lambda<Func<T, bool>>(body, candidateExpr);
        }
        /// <summary>
        /// OrElse条件组合 - 适用NH
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="exp_left"></param>
        /// <param name="exp_right"></param>
        /// <returns></returns>
        public static Expression<Func<T, bool>> OrElse<T>(this Expression<Func<T, bool>> exp_left, Expression<Func<T, bool>> exp_right)
        {
            var candidateExpr = Expression.Parameter(typeof(T), "candidate");
            var parameterReplacer = new ParameterReplacer(candidateExpr);

            var left = parameterReplacer.Replace(exp_left.Body);
            var right = parameterReplacer.Replace(exp_right.Body);
            var body = Expression.OrElse(left, right);

            return Expression.Lambda<Func<T, bool>>(body, candidateExpr);
        }

        #region 老版本，不好用
        //public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> expression1,
        //   Expression<Func<T, bool>> expression2)
        //{
        //    var invokedExpression = Expression.Invoke(expression2, expression1.Parameters
        //            .Cast<Expression>());

        //    return Expression.Lambda<Func<T, bool>>(Expression.Or(expression1.Body, invokedExpression),
        //    expression1.Parameters);
        //}
        //public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> expression1,
        //      Expression<Func<T, bool>> expression2)
        //{
        //    var invokedExpression = Expression.Invoke(expression2, expression1.Parameters
        //         .Cast<Expression>());

        //    return Expression.Lambda<Func<T, bool>>(Expression.And(expression1.Body,
        //           invokedExpression), expression1.Parameters);
        //}
        #endregion


        /// <summary>
        /// 统一ParameterExpression
        /// </summary>
        internal class ParameterReplacer : ExpressionVisitor
        {
            public ParameterReplacer(ParameterExpression paramExpr)
            {
                this.ParameterExpression = paramExpr;
            }

            public ParameterExpression ParameterExpression { get; private set; }

            public Expression Replace(Expression expr)
            {
                return this.Visit(expr);
            }

            protected override Expression VisitParameter(ParameterExpression p)
            {
                return this.ParameterExpression;
            }
        }
    }
}
