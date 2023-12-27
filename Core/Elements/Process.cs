using CourserWork.Core.Elements.Internals;
using CourserWork.Core.Elements.Listeners;
using CourserWork.Core.ModelObjects;
using CourserWork.Core.Nodes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourserWork.Core.Elements
{
    public class Process : AbstractElement
    {
        private int maxqueue;
        private int failure;
        private double meanQueue;
        private double latestTnext;
        private double workTime;
        private double arrivedQuantity;
        private int quantity;
        private double averageInterval;
        private double lastInActT;
        private IProcessQueue queue;
        private List<ProcessWorker> workers;
        private INode? node;
        private ITimeEstimator? timeEstimator;
        private IModelObjectBearer? modelObjectBearer;
        private IOnActModelObjectMutator? onActObjectMutator;
        private IDequeueListener? dequeueListener;

        public Process(string name, double delay) : base(name, delay)
        {
            this.name = name;
            this.node = null;
            maxqueue = int.MaxValue;
            failure = 0;
            meanQueue = 0.0;
            workTime = 0;
            latestTnext = double.MinValue;
            quantity = 0;
            arrivedQuantity = 0;
            averageInterval = 0.0;
            lastInActT = 0;
            queue = new ProcessQueue();
            workers = new List<ProcessWorker> { new ProcessWorker(state: 0, tnext: double.MaxValue) };
        }

        public Process(string name) : base(name, 0)
        {
            this.name = name;
            this.node = null;
            maxqueue = int.MaxValue;
            failure = 0;
            meanQueue = 0.0;
            workTime = 0;
            latestTnext = double.MinValue;
            quantity = 0;
            arrivedQuantity = 0;
            averageInterval = 0.0;
            lastInActT = 0;
            queue = new ProcessQueue();
            workers = new List<ProcessWorker> { new ProcessWorker(state: 0, tnext: double.MaxValue) };
        }

        private ProcessWorker? GetFreeWorker()
        {
            return workers.Find(worker => worker.GetState() == 0);
        }

        private List<ProcessWorker> GetAllBusyWorkers()
        {
            return workers.FindAll(worker => worker.GetState() == 1);
        }

        public override double GetTnext()
        {
            List<ProcessWorker> busyWorkers = GetAllBusyWorkers();

            if (busyWorkers.Count == 0)
            {
                return double.MaxValue;
            }

            return busyWorkers.Min(worker => worker.GetTnext());
        }

        public ProcessWorker? GetNearestBusyWorker()
        {
            List<ProcessWorker> busyWorkers = GetAllBusyWorkers();

            if (busyWorkers.Count == 0)
            {
                return null;
            }

            double minTnext = busyWorkers.Min(worker => worker.GetTnext());

            return busyWorkers.Find(worker =>
              worker.GetTnext() == minTnext);
        }

        public override int GetState()
        {
            ProcessWorker? freeWorker = GetFreeWorker();

            if (freeWorker is not null)
            {
                return 0;
            }
            return 1;
        }

        public void SetNode(INode node)
        {
            this.node = node;
        }

        public void SetTimeEstimator(ITimeEstimator timeEstimator)
        {
            this.timeEstimator = timeEstimator;
        }

        public void SetModelObjectBearer(IModelObjectBearer modelObjectBearer)
        {
            this.modelObjectBearer = modelObjectBearer;
        }

        public void SetOnActObjectMutator(IOnActModelObjectMutator mutator)
        {
            this.onActObjectMutator = mutator;
        }

        private void DoInActStats(IModelObject modelObject)
        {
            double tcurr = GetTcurr();
            if (lastInActT == 0)
            {
                lastInActT = tcurr;
                arrivedQuantity++;
                return;
            }
            double interval = tcurr - lastInActT;

            averageInterval = ((averageInterval * arrivedQuantity) + interval) / (arrivedQuantity + 1);

            lastInActT = tcurr;
            arrivedQuantity++;
        }

        public double GetAverageInterval()
        {
            return averageInterval;
        }

        public override void InAct(IModelObject modelObject)
        {
            DoInActStats(modelObject);
            ProcessWorker? freeWorker = GetFreeWorker();

            if (modelObjectBearer is not null) {
                bool isModelObjectAllowed = modelObjectBearer.IsModelObjectAllowed(modelObject);
                if (!isModelObjectAllowed) {
                    failure++;
                    return;
                }
            }

            if (freeWorker is not null)
            {
                double delay = GetDelay(modelObject);
                double tnext = GetTcurr() + delay;

                double workDuration = 0;

                if (latestTnext == double.MinValue)
                {
                    workDuration = delay;
                    latestTnext = tnext;
                }
                else if (latestTnext < tnext)
                {
                    workDuration = tnext - latestTnext;
                    latestTnext = tnext;
                }
                modelObject.StartProcessing(GetName(), GetTcurr());
                freeWorker.StartWork(modelObject, tnext, workDuration);
            }
            else
            {
                if (GetQueueSize() < GetMaxqueue())
                {
                    var successEnqueue = queue.TryToEnqueue(modelObject);
                    if (!successEnqueue) {
                        failure++;
                    }
                }
                else
                {
                    failure++;
                }
            }
        }

        public override void OutAct()
        {
            ProcessWorker? worker = GetNearestBusyWorker();

            if (worker is null)
            {
                throw new InvalidOperationException("PROCESS doesn't have busy workers" + GetName());
            }

            IModelObject processedModelObject = worker.StopWork();
            processedModelObject.FinishProcessing(GetName(), GetTcurr());
            onActObjectMutator?.MutateModelObject(processedModelObject);
            quantity++;
            node?.TriggerAct(processedModelObject);

            ProcessWorker? busyWorker = GetNearestBusyWorker();
            if (busyWorker is null)
            {
                latestTnext = double.MinValue;
            }

            if (GetQueueSize() > 0)
            {
                IModelObject modelObject = queue.Dequeue();
                InAct(modelObject);
                dequeueListener?.OnDequeue(this);
            }
        }

        public IModelObject? Dequeue()
        {
            if (queue.Count > 0)
            {
                return queue.Dequeue();
            }
            return null;
        }

        public double GetDelay(IModelObject modelObject)
        {
            if (timeEstimator is not null)
            {
                return timeEstimator.GetEstimetedTime(modelObject);
            }
            return base.GetDelay();
        }

        public int GetFailure()
        {
            return failure;
        }
        public int GetQueueSize()
        {
            return queue.Count;
        }

        public int GetBusyWorkersAmount()
        {
            if (workers.Count > 0)
            {
                return GetAllBusyWorkers().Count;
            }
            return 0;
        }

        public IProcessQueue GetQueue()
        {
            return queue;
        }

        public void SetQueue(IProcessQueue queue)
        {
            this.queue = queue;
        }

        public List<IModelObject> GetAllObjectFromWorkers() {
            List<IModelObject> list = new List<IModelObject>();
            foreach (var worker in workers)
            {
                IModelObject? modelObject = worker.GetModelObject();
                if (modelObject is not null) {
                    list.Add(modelObject);
                }   
            }

            return list;
        }

        public List<IModelObject> GetAllObjects()
        {
            List<IModelObject> list = new List<IModelObject>();

            List<IModelObject> queueList = queue.GetListModelsObject();
            List<IModelObject> workersList = GetAllObjectFromWorkers();

            list.AddRange(queueList);
            list.AddRange(workersList);

            return list;
        }


        public void SetDequeueListener(IDequeueListener listener)
        {
            this.dequeueListener = listener;
        }

        public IDequeueListener? GetDequeueListener()
        {
            return dequeueListener;
        }

        public int GetMaxqueue()
        {
            return maxqueue;
        }
        public void SetMaxqueue(int maxqueue)
        {
            this.maxqueue = maxqueue;
        }

        public void SetWorkers(int workersAmount)
        {
            List<ProcessWorker> newWorkers = new List<ProcessWorker>();

            for (int i = 0; i < workersAmount; i++)
            {
                newWorkers.Add(new ProcessWorker(state: 0, tnext: double.MaxValue));
            }
            workers = newWorkers;
        }

        public int GetQuantity()
        {
            return quantity;
        }

        public double GetMeanQueue()
        {
            return meanQueue;
        }

        public double GetWorkTime()
        {
            return workTime;
        }

        public override void DoStatistics(double delta)
        {
            meanQueue = GetMeanQueue() + GetQueueSize() * delta;
            workTime = GetWorkTime() + GetState() * delta;
        }
    }
}
