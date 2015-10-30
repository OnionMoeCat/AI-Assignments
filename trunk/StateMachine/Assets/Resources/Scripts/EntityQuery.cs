using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace AISandbox
{
    class EntityQuery : MonoBehaviour
    {
        private bool running = false;
        public bool Running
        {
            get
            {
                return running;
            }
            set
            {
                running = value;
            }
        }
        void Update()
        {
            if (running)
            {
                EntityManager.QueryEveryActor();
            }   
        }
    }
}
