using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CoppereggMetrics
{
    class PausableTimer
    {
        Timer timer;

        TimeSpan period;
        Action<object> func;


        public PausableTimer( Action<object> callback, TimeSpan period )
        {
            timer = new Timer( OnTick );

            this.period = period;
            func = callback;
        }


        public void Start()
        {
            timer.Change( TimeSpan.Zero, period );
        }
        public void Stop()
        {
            Pause();
            timer.Dispose();
        }

        public void Pause()
        {
            timer.Change( Timeout.Infinite, Timeout.Infinite );
        }
        public void Resume()
        {
            timer.Change( period, period );
        }


        void OnTick( object state )
        {
            func( state );
        }
    }
}
