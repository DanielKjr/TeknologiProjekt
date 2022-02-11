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
        static Semaphore settlementsema = new Semaphore(10, 10);
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

        public delegate void TaskHandler(Task _task);

        /// <summary>
        /// Uses super-class to set its position as well as loading its texture, then starts its own thread.
        /// The overloads are purely to be able decide which sprite to use when we instantiate it.
        /// </summary>
        /// <param name="_pos"></param>
        /// <param name="_task"></param>
        public Worker(Vector2 _pos, Task _task) : base(_pos, "Workers/MinerGirth")
        {
            moveSpeed = 0.5f;
            isAlive = true;
            taskState = _task;
            taskThread = new Thread(ResourceGathering);
            taskThread.IsBackground = true;
            taskThread.Start();
        }
        public Worker(Vector2 _pos, Task _task, int empty) : base(_pos, "Workers/LumberGirth")
        {
            moveSpeed = 0.5f;
            isAlive = true;
            taskState = _task;
            taskThread = new Thread(ResourceGathering);
            taskThread.IsBackground = true;
            taskThread.Start();
        }
        public Worker(Vector2 _pos, Task _task, int empty, int empty2) : base(_pos, "Workers/FarmerGirth")
        {
            moveSpeed = 0.5f;
            isAlive = true;
            taskState = _task;
            taskThread = new Thread(ResourceGathering);
            taskThread.IsBackground = true;
            taskThread.Start();
        }

        /// <summary>
        /// Main thread/worker function, updates the workers position and where it is moving.
        /// The target is determined by the Task enum used when instantiating the worker, they will go to the resource assigned 
        /// and will start to withdraw resources from the ressource while also filling up the inventory that is used to determine when they are full/how much they can carry.
        /// When full the worker will return to the settlement and offload resources, the TaskHandler delegate then resets its loop
        /// </summary>
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

       

        /// <summary>
        /// This method determines the workers' behavior depending on the Task they're assigned.
        /// At the end of each loop of the ResourceGathering function the target will be reset to the corresponding ressource
        /// The collected ressources is then taken from the worker and added to the players ressource pool before reverting them to 0 to start over.
        /// </summary>
        /// <param name="_task"></param>
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
