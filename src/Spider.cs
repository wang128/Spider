using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Spider
{
    public class Spider
    {
        private Hashtable vis1, vis2;
        private Queue queue;
        private Uri uri;
        private string output;
        private MainForm mainForm;
        private int urlCount = 0;
        private long starttime = 0;
        private Done done;
        private bool quit;
        private int flag;
        private int cnt;
        public Uri Uri { get => uri; set => uri = value; }
        public string Output { get => output; set => output = value; }
        public MainForm MainForm { get => mainForm; set => mainForm = value; }
        public Done Done { get => done; set => done = value; }
        public bool Quit { get => quit; set => quit = value; }
        public int Flag { get => flag; set => flag = value; }
        public int Cnt { get => cnt; set => cnt = value; }

        enum Status { STATUS_FAILED, STATUS_QUEUED };
        public Spider()
        {
            vis1 = new Hashtable();
            vis2 = new Hashtable();
            queue = new Queue();
            done = new Done();
            quit = false;
            Cnt = 0;
        }
        public void addURI(Uri uri)
        {
            Monitor.Enter(this);
            if (!vis1.Contains(uri))
            {
                vis1.Add(uri, Status.STATUS_QUEUED);
                queue.Enqueue(uri);
            }
            Monitor.Pulse(this);
            Monitor.Exit(this);
        }
        public bool addIMG(Uri uri)
        {
            bool tmp = false;
            Monitor.Enter(this);
            if (!vis2.Contains(uri))
            {
                vis2.Add(uri, Status.STATUS_QUEUED);
                tmp = true;
            }
            Monitor.Pulse(this);
            Monitor.Exit(this);
            return tmp;
        }
        public Uri work()
        {
            Monitor.Enter(this);
            while (queue.Count < 1) Monitor.Wait(this);
            Uri next = (Uri)queue.Dequeue();
            if (mainForm != null)
            {
                mainForm.setlastURL(next.ToString());
                mainForm.setprocessedCount((urlCount++).ToString());
                long etime = (System.DateTime.Now.Ticks - starttime) / 10000000L;
                long urls = (etime == 0) ? 0 : urlCount / etime;
                mainForm.settime(etime / 60 + " minutes(" + urls + " urls/sec)");
            }
            Monitor.Pulse(this);
            Monitor.Exit(this);
            return next;
        }
        public void Start(Uri x, int threads)
        {
            uri = x;
            addURI(uri);
            starttime = System.DateTime.Now.Ticks;
            for (int i = 1; i < threads; i++)
            {
                DocumentWorker worker = new DocumentWorker(this);
                worker.start();
            }
            done.waitdone();
        }
    }
}
