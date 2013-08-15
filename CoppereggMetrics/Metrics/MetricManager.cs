using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CopperEggLib;

namespace CoppereggMetrics
{
    class MetricManager
    {
        CopperEgg copperEgg;

        PausableTimer sampleTimer;

        List<IMetricProvider> metricProviders;
        ConcurrentDictionary<IMetricProvider, MetricGroup> providerMap;


        public MetricManager()
        {
            copperEgg = new CopperEgg( Settings.Current.CopperEggAPIKey );

            sampleTimer = new PausableTimer( OnSample, TimeSpan.FromSeconds( 60 ) );

            metricProviders = new List<IMetricProvider>();
            providerMap = new ConcurrentDictionary<IMetricProvider, MetricGroup>();

            // todo: reflection/plugin system?
            metricProviders.Add( new MySqlMetricProvider() );
        }


        public async void Start()
        {
            List<MetricGroup> existingGroups = await copperEgg.GetMetricGroups();

            IEnumerable<Task> setupTasks = metricProviders.Select( p => SetupMetricGroup( p ) );

            await Task.WhenAll( setupTasks );

            sampleTimer.Start();
        }


        public void Stop()
        {
            sampleTimer.Stop();
        }


        async Task SetupMetricGroup( IMetricProvider provider )
        {
            Log.WriteInfo( "MetricManager", "Initializing metric group for {0}", provider );

            var setupGroup = provider.SetupMetricGroup();

            List<MetricGroup> existingGroups = await copperEgg.GetMetricGroups();

            var existingGroup = existingGroups.Find( grp => grp.Name.StartsWith( setupGroup.Name ) );

            if ( existingGroup != null )
            {
                Log.WriteInfo( "MetricManager", "Metric group already exists!" );

                // if the group we're trying to create already exists, we're done!
                providerMap[ provider ] = existingGroup;
                return;
            }

            Log.WriteInfo( "MetricManager", "Creating metric group..." );

            var createdGroup = await copperEgg.CreateMetricGroup( setupGroup );

            providerMap[ provider ] = createdGroup;
        }

        async void OnSample( object state )
        {
            Log.WriteDebug( "MetricManager", "Sampling!" );

            IEnumerable<Task> sampleTasks = providerMap.Keys.Select( provider => SampleMetric( provider ) );

            await Task.WhenAll( sampleTasks );

            Log.WriteDebug( "MetricManager", "Done!" );
        }


        async Task SampleMetric( IMetricProvider provider )
        {
            MetricGroupSample sample = await provider.PerformSample();

            MetricGroup metricGroup;
            if ( !providerMap.TryGetValue( provider, out metricGroup ) )
            {
                Log.WriteError( "MetricManager", "Unable to sample {0}: provider not registered", provider );
                return;
            }

            await metricGroup.StoreSample( sample );
        }
    }
}
