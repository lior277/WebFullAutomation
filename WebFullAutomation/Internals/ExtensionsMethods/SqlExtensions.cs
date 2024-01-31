// Ignore Spelling: Sql

using Microsoft.EntityFrameworkCore;
using System.Threading;
using AirSoftAutomationFramework.Models;

namespace AirSoftAutomationFramework.Internals.ExtensionsMethods
{
    public static class SqlExtensions
    {
        public static void VerifySaveForSqlManipulation(this QaAutomation01Context dbContext)
        {
            //var ff = dbContext.<SaveChangesEventArgs>();
            Thread.Sleep(1000);
            var saveConfirm = dbContext.SaveChangesAsync();
            //dbContext.Dispose();
            Thread.Sleep(1000);
        }
    }
}
