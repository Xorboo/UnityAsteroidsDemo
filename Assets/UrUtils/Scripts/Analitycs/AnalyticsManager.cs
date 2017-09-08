//
// Copyright (c) Kirill Korepanov. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

using System;
using System.Collections.Generic;
using UnityEngine;


namespace UrUtils.Analytics
{
    public class AnalyticsManager : Singleton<AnalyticsManager>, IAnalytics, IAnalyticsTransaction
    {
        [SerializeField]
        List<AnalyticsBase> AnalyticsSystems = null;


        #region Behaviours
        protected new void Awake()
        {
            base.Awake();

            if (AnalyticsSystems.Count > 0)
            {
                foreach (var analytics in AnalyticsSystems)
                    analytics.Init();
            }
            else
                Debug.LogWarning("AnalyticsManager.Start no analytics systems provided, analytics won't do anyting");
        }
        #endregion


        public bool SendEvent(string eventName, Dictionary<string, object> parameters = null)
        {
            bool success = true;
            foreach (var analytics in AnalyticsSystems)
                success &= analytics.SendEvent(name, parameters);

            if (success)
                Debug.LogFormat("AnalyticsManager.SendEvent event '{0}' sent", eventName);
            else
                Debug.LogWarningFormat("AnalyticsManager.SendEvent error sending event '{0}'", eventName);
            return success;
        }

        public bool SendTransaction(string productID, decimal price, string currency, string reciept = null, string signature = null)
        {
            bool success = true;
            bool foundAnalytics = false;

            foreach (var analytics in AnalyticsSystems)
            {
                var transactionAnalytics = analytics as IAnalyticsTransaction;
                if (transactionAnalytics != null)
                {
                    success &= transactionAnalytics.SendTransaction(productID, price, currency, reciept, signature);
                    foundAnalytics = true;
                }
            }

            if (!foundAnalytics)
                Debug.LogWarning("AnalyticsManager.SendTransaction couldn't find any applicable systems for the call");


            if (success)
                Debug.LogFormat("AnalyticsManager.SendTransaction transaction '{0}' for '{1}' sent", productID, price);
            else
                Debug.LogWarningFormat("AnalyticsManager.SendTransaction error sending transaction '{0}' for '{1}'", productID, price);

            success &= foundAnalytics;
            return success;
        }
    }
}