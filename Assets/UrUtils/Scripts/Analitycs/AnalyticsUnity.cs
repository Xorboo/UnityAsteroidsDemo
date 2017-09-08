//
// Copyright (c) Kirill Korepanov. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

using System.Collections.Generic;
using UnityEngine;


namespace UrUtils.Analytics
{
    using UnityEngine.Analytics;

    [CreateAssetMenu(fileName = "UnityAnalytics", menuName = PathBase + "Unity Analytics")]
    public class AnalyticsUnity : AnalyticsBase, IAnalyticsTransaction
    {
        public override bool SendEvent(string eventName, Dictionary<string, object> parameters)
        {
            var result = Analytics.CustomEvent(name, parameters);
            return CheckResult(result, "SendEvent");
        }

        public bool SendTransaction(string productID, decimal price, string currency, string reciept = null, string signature = null)
        {
            var result = Analytics.Transaction(productID, price, currency, reciept, signature);
            return CheckResult(result, "SendTransaction");
        }


        bool CheckResult(AnalyticsResult result, string prefix)
        {
            switch (result)
            {
                case AnalyticsResult.Ok:
                    return true;

                default:
                    Debug.LogErrorFormat("AnalyticsUnity.{0} bad result: {1}", prefix, result);
                    return false;
            }
        }
    }
}