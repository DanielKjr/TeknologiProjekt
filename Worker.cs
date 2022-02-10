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
                    for (int i = 0; i < workerMaxInventory; i++)
                    {
                        gathering--;
                        workerInventory++;
                        Thread.Sleep(100);
                    }
                    
                }

                if (workerInventory >= workerMaxInventory)
                {
                    target = GameWorld.resourceLocations[0];
                }
                if (Vector2.Distance(GameWorld.resourceLocations[0], position) < 20f && workerInventory >= 0)
                {
                    for (int i = 0; i < workerMaxInventory; i++)
                    {
                        offloading++;
                        workerInventory--;
                        Thread.Sleep(500);
                    }
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
                    
                    break;
                case Task.Food:
                    target = GameWorld.resourceLocations[5];
                    foodResource += gathering;
                    Food += offloading;
                    break;
                case Task.Gold:
                    target = GameWorld.resourceLocations[2];
                    goldResource += gathering;
                    Gold += offloading;
                    break;
                default:
                    break;
            }
        }
    }
}
