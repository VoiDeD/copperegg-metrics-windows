using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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

            var setupGroup = provider.GetMetricGroup();

            var existingGroups = new List<MetricGroup>();
            try
            {
                existingGroups = await copperEgg.GetMetricGroups();
            }
            catch ( HttpRequestException ex )
            {
                Log.WriteError( "MetricManager", "Unable to request existing metric groups: {0}", ex.Message );
                return;
            }

            var existingGroup = existingGroups.Find( grp => grp.Name.StartsWith( setupGroup.Name ) );

            if ( existingGroup != null )
            {
                Log.WriteInfo( "MetricManager", "Metric group for {0} already exists!", provider );

                // if the group we're trying to create already exists, we're done!
                providerMap[ provider ] = existingGroup;
                return;
            }

            Log.WriteInfo( "MetricManager", "Creating metric group..." );

            try
            {
                providerMap[ provider ] = await copperEgg.CreateMetricGroup( setupGroup );
            }
            catch ( HttpRequestException ex )
            {
                Log.WriteError( "MetricManager", "Unable to create metric group for {0}: {1}", provider, ex.Message );
            }
        }

        async void OnSample( object state )
        {
            sampleTimer.Pause();

            IEnumerable<Task> sampleTasks = providerMap.Keys.Select( provider => SampleMetric( provider ) );

            await Task.WhenAll( sampleTasks );

            sampleTimer.Resume();
        }


        async Task SampleMetric( IMetricProvider provider )
        {
            Log.WriteDebug( "MetricManager", "Sampling {0}", provider );

            MetricGroupSample sample;
            try
            {
                sample = await provider.PerformSample();
            }
            catch ( Exception ex )
            {
                Log.WriteWarn( "MetricManager", "Unable to sample {0}: {1}", provider, ex.Message );
                return;
            }

            Log.WriteDebug( "MetricManager", "Done sampling {0}", provider );

            MetricGroup metricGroup;
            if ( !providerMap.TryGetValue( provider, out metricGroup ) )
            {
                Log.WriteError( "MetricManager", "Unable to sample {0}: provider not registered", provider );
                return;
            }

            Log.WriteDebug( "MetricManager", "Storing sample for {0}", provider );

            try
            {
                await metricGroup.StoreSample( sample );
            }
            catch ( HttpRequestException ex )
            {
                Log.WriteWarn( "MetricManager", "Unable to store sample for {0}: {1}", provider, ex.Message );
            }

            Log.WriteDebug( "MetricManager", "Done storing sample for {0}", provider );
        }
    }
}
