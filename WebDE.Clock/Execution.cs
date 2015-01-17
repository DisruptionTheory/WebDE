using System;
using System.Collections;
using System.Collections.Generic;

using SharpKit.JavaScript;
using SharpKit.Html;
using SharpKit.jQuery;

namespace WebDE.Clock
{

    /// <summary>
    /// The Execution object is an object that holds the contexts and properties of an execution job.
    /// </summary>
    [JsType(JsMode.Clr, Filename = "scripts/WebDE.Clock.js")]
    class Execution
    {
        private Action context;
        private Action<Dictionary<object, object>> contextParam;
        private Dictionary<object, object> stateObject;
        private bool delayed = false;

        public Execution(Action func)
        {
            context = func;
        }

        /// <summary>
        /// Create a new execution object with the specified context.
        /// </summary>
        /// <param name="func">The context belonging to this execution.</param>
        public Execution(Action<Dictionary<object, object>> func, Dictionary<object, object> stateObj)
        {
            contextParam = func;
            this.stateObject = stateObj;
        }

        /// <summary>
        /// Executions the execution.
        /// </summary>
        public void Execute()
        {
            if (stateObject != null)
            {
                contextParam.Invoke(stateObject);
                //contextParam(stateObject);
            }
            else
            {
                context.Invoke();
            }
        }

        /// <summary>
        /// Delay this execution for the specefied number of seconds.
        /// </summary>
        /// <param name="seconds">The number of seconds to delay the execution.</param>
        public void Delay(int seconds, Window window)
        {
            delayed = true;
            window.setTimeout(unDelay, seconds * 1000);
        }

        /// <summary>
        /// Whether or not the execution is delayed.
        /// </summary>
        /// <returns>Bool representing if the execution is delayed.</returns>
        public bool IsDelayed()
        {
            return delayed;
        }

        private void unDelay()
        {
            delayed = false;
        }
    }
}
