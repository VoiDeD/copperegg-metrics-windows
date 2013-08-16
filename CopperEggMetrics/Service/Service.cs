using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using CopperEggLib;

namespace CoppereggMetrics
{
    public partial class Service : ServiceBase
    {
        MetricManager metricManager;


        public Service()
        {
            InitializeComponent();

            Settings.Load();

            metricManager = new MetricManager();
        }


        public void Start( string[] args )
        {
            OnStart( args );
        }


        protected override void OnStart( string[] args )
        {
            metricManager.Start();
        }
        protected override void OnStop()
        {
            metricManager.Stop();
        }
    }
}
