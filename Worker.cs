using Microsoft.Xna.Framework;
using System.Threading;

namespace TeknologiProjekt
{
    public enum Task
    {
        Wood,
        Food,
        Gold
    }
    public class Worker : GameObject
    {
        static Semaphore settlementsema = new Semaphore(2, 2);
        static Semaphore goldsema = new Semaphore(2, 2); //would be nice with semaphore array here...
        static Semaphore woodsema = new Semaphore(2, 2);
        static Semaphore foodsema = new Semaphore(2, 2);
        private Semaphore activeSema;
        static readonly object settlementlock = new object();
        static readonly object goldlock = new object();
        static readonly object woodlock = new object();
        static readonly object foodlock = new object();
        private object activeLock;
        private Vector2 target;
        protected TaskHandler _taskHandler;
        protected int gathering;
        protected int offloading;


        public Worker(Vector2 _pos, Task _task) : base(_pos, "Workers/MinerGirth")
        {

            moveSpeed = 0.5f;
            isAlive = true;
            taskState = _task;
            taskThread = new Thread(ResourceGathering);
            taskThread.IsBackground = true;
            taskThread.Start();

        }

        private void ResourceGathering()
        {
            _taskHandler = new TaskHandler(TaskHandlerMethod);
            _taskHandler(taskState);
            while (isAlive)
            {

                moveDir = target - position;
                moveDir.Normalize();
                position += moveDir * moveSpeed;
                Thread.Sleep(1);

                if (Vector2.Distance(target, position) < 40f && workerInventory < workerMaxInventory)
                {
                    activeSema.WaitOne(); //allow 2 in at same time.
                    for (int i = 0; i < workerMaxInventory; i++)
                    {
                        lock (activeLock) //Only allow 1 to manipulate data.
                        {
                            gathering--;
                            workerInventory++;
                        }
                        Thread.Sleep(500);
                    }
                    activeSema.Release();
                }

                if (workerInventory >= workerMaxInventory)
                {
                    target = GameWorld.resourceLocations[0];
                }
                if (Vector2.Distance(GameWorld.resourceLocations[0], position) < 20f && workerInventory >= 0)
                {
                    settlementsema.WaitOne();
                    for (int i = 0; i < workerMaxInventory; i++)
                    {
                        lock (settlementlock)
                        {
                            offloading++;
                            workerInventory--;
                        }
                        Thread.Sleep(500);
                    }
                    settlementsema.Release();
                    if (workerInventory == 0)
                    {
                        _taskHandler(taskState);
                    }
                }
                

            }
        }

        public delegate void TaskHandler(Task _task);

        private void TaskHandlerMethod(Task _task)
        {
            switch (_task)
            {
                case Task.Wood:
                    target = GameWorld.resourceLocations[3];
                    woodResource += gathering;
                    Wood += offloading;
                    gathering = 0;
                    offloading = 0;
                    activeSema = woodsema;
                    activeLock = woodlock;
                    break;
                case Task.Food:
                    target = GameWorld.resourceLocations[5];
                    foodResource += gathering;
                    Food += offloading;
                    gathering = 0;
                    offloading = 0;
                    activeSema = foodsema;
                    activeLock = foodlock;
                    break;
                case Task.Gold:
                    target = GameWorld.resourceLocations[2];
                    goldResource += gathering;
                    Gold += offloading;
                    gathering = 0;
                    offloading = 0;
                    activeSema = goldsema;
                    activeLock = goldlock;
                    break;
                default:
                    break;
            }
        }
    }
}
