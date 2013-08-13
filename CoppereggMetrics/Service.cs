using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;

namespace CoppereggMetrics
{
    public partial class Service : ServiceBase
    {
        public Service()
        {
            InitializeComponent();
        }


        public void Start( string[] args )
        {
            OnStart( args );
        }


        protected async override void OnStart( string[] args )
        {
        }
        protected override void OnStop()
        {
        }
    }
}
