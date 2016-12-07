using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BLToolkit.Fluent.Test
{
    /// <summary>
    /// Класс для работы с асертами обладающий дополнительными методами
    /// </summary>
    public static class AssertExceptionEx
    {
        /// <summary>
        /// Проверка на обязательность исключения
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="action"></param>
        /// <param name="message"></param>
        public static void AreException<T>(Action action, string message)
        {
            bool isOk = false;
            try
            {
                action();
            }
            catch (Exception e)
            {
                if (e is T)
                {
                    isOk = true;
                }
            }
            if (!isOk)
            {
                Assert.Fail(message);
            }
        }
        /// <summary>
        /// Проверка на обязательность отсутсвия исключения
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="action"></param>
        /// <param name="message"></param>
        public static void AreNotException<T>(Action action, string message)
        {
            bool isOk = true;
            try
            {
                action();
            }
            catch (Exception e)
            {
                if (e is T)
                {
                    isOk = false;
                }
            }
            if (!isOk)
            {
                Assert.Fail(message);
            }
        }
    }
}