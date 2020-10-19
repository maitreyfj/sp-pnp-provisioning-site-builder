using System;
using System.Diagnostics;
using System.Net;
using System.Reflection;
using System.Threading;
using OfficeDevPnP.Core;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.SharePoint.Client;

namespace SiteMigrationProject
{
    public static partial class ClientContextExtensions
    {
        internal class ExceptionScopeInfo<T>
        {
            internal ExceptionHandlingScope Scope { get; set; }
            internal T ItemToProcess { get; set; }
        }
        public static void ExecuteQueryBatch<T>(this ClientRuntimeContext context,
           List<T> itemsToProcess, Action<T> action, int batchSize)
        {
            ExecuteQueryBatch<T>(context, itemsToProcess, action, null, batchSize);
        }

        public static void ExecuteQueryBatch<T>(this ClientRuntimeContext context,
            List<T> itemsToProcess, Action<T> action, Action<string, T> actionError, int batchSize)
        {
            int idx = 0;
            int length = itemsToProcess.Count;

            List<ExceptionScopeInfo<T>> listScopes = new List<ExceptionScopeInfo<T>>();

            while (idx < length)
            {
                for (var i = idx; i < Math.Min(idx + batchSize, length); ++i)
                {
                    T item = itemsToProcess[i];
                    ExceptionHandlingScope scope = new ExceptionHandlingScope(context);
                    listScopes.Add(new ExceptionScopeInfo<T>()
                    {
                        Scope = scope,
                        ItemToProcess = item
                    });
                    using (scope.StartScope())
                    {
                        using (scope.StartTry())
                        {
                            action(item);
                        }
                        using (scope.StartCatch())
                        {
                        }
                    }
                }
                context.ExecuteQueryRetry();
                idx += batchSize;
            }

            if (null != actionError)
            {
                listScopes.ForEach((info) =>
                {
                    if (-1 != info.Scope.ServerErrorCode)
                    {
                        actionError(info.Scope.ErrorMessage, info.ItemToProcess);
                    }
                });
            }
        }
    }
}
