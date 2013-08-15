using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CopperEggLib;

namespace CoppereggMetrics
{
    interface IMetricProvider
    {
        MetricGroup SetupMetricGroup();

        Task<MetricGroupSample> PerformSample();
    }
}
