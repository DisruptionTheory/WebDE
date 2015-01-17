using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using SharpKit.JavaScript;
using SharpKit.Html;
using SharpKit.jQuery;

namespace WebDE.Clock
{

    /// <summary>
    /// The Execution object is an object that holds the contexts and properties of an execution job.
    /// </summary>
    [JsType(JsMode.Clr, Filename = "scripts/WebDE.Clock.js")]
    class TimedExecution
    {
        private Action context;
        private int timerID;

        /// <summary>
        /// Create a new execution object with the specified context.
        /// </summary>
        /// <param name="func">The context belonging to this execution.</param>
        public TimedExecution(Action func)
        {
            context = func;
        }

        /// <summary>
        /// Executions the execution.
        /// </summary>
        public void Execute()
        {
            context.Invoke();
        }

        /// <summary>
        /// Set the javascript timer id of the Timed Execution.
        /// </summary>
        /// <param name="id">The javascript timer ID.</param>
        public void SetTimerID(int id)
        {
            timerID = id;
        }

        /// <summary>
        /// Get the javascript timer id of the Timed Execution.
        /// </summary>
        /// <returns>The javascript timer ID.</returns>
        public int GetTimerID()
        {
            return timerID;
        }

    }
}