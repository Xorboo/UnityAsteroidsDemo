//
// Copyright (c) Kirill Korepanov. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

using System.Collections.Generic;
using UnityEngine;


namespace UrUtils.Analytics
{
    public abstract class AnalyticsBase : ScriptableObject, IAnalytics
    {
        protected const string PathBase = "Analytics/";

        public virtual void Init() { }
        public abstract bool SendEvent(string eventName, Dictionary<string, object> parameters = null);
    }


    public interface IAnalytics
    {
        bool SendEvent(string eventName, Dictionary<string, object> parameters = null);
    }

    public interface IAnalyticsTransaction
    {
        bool SendTransaction(string productID, decimal price, string currency, string reciept = null, string signature = null);
    }
}