using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace lib.update
{
    public class Actions
    {
        private int current_action = 0;

        public void Execute(object arg)
        {
            if (GetType().GetMethod("Action_" + (current_action)) != null)
            {
                MethodInfo theMethod = GetType().GetMethod("Action_" + (current_action));
                theMethod.Invoke(this, new object[] { arg });
            }
        }

        public void Elevate(object arg)
        {
            MethodInfo theMethod = arg.GetType().GetMethod("Elevate");
            theMethod.Invoke(arg, null);
        }

        private void NextAction(object arg)
        {
            current_action++;
            if (GetType().GetMethod("Action_" + (current_action)) != null)
            {
                MethodInfo theMethod = this.GetType().GetMethod("Action_" + (current_action));
                theMethod.Invoke(this, new object[] { arg });
            }
        }

        private void ChangeStatus(string text,object arg)
        {
            MethodInfo theMethod = arg.GetType().GetMethod("ChangeStatus");
            theMethod.Invoke(arg, new object[] { text });
        }

        /*

        //
        //      SPECIFIED ACTIONS FOR EACH APPLICATION.
        //

        */

        public void Action_0(object arg)
        {
            ChangeStatus("Initialize...",arg);
            Thread.Sleep(1000);
            NextAction(arg);
        }
        public void Action_1(object arg)
        {
            ChangeStatus("Loading Libraries...", arg); 
            Thread.Sleep(1500);

          
              NextAction(arg);
        }
        public void Action_2(object arg)
        {
            ChangeStatus("Waiting for server response...", arg);
            Thread.Sleep(200);
            NextAction(arg);
        }
        public void Action_3(object arg)
        {
            ChangeStatus("Checking for available updates...", arg);
            Thread.Sleep(200);
            NextAction(arg);
        }
        public void Action_4(object arg)
        {
            ChangeStatus("Application Start..", arg);
            Thread.Sleep(1000);
            NextAction(arg);
        }
    }
}
