using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

//爬虫的行为
namespace Spider
{
    public class Done
    {
        private int activeThreads = 0;
        //工作线程在开始处理各个URL之时，会调用WorkerBegin；处理结束时调用WorkerEnd。
        public void waitdone()//一直等待，直到不再有活动的线程(已经没有等待下载和正在下载的URL)
        {
            Monitor.Enter(this);
            while (activeThreads > 0) Monitor.Wait(this);
            Monitor.Exit(this);
        }
        public void workerbegin()
        {
            Monitor.Enter(this);
            activeThreads++;
            Monitor.Pulse(this);
            Monitor.Exit(this);
        }
        public void workerend()
        {
            Monitor.Enter(this);
            activeThreads--;
            Monitor.Pulse(this);
            Monitor.Exit(this);
        }
    }
}