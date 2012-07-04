using System;
using System.Collections.Generic;

using SharpKit.JavaScript;
using SharpKit.Html4;
using SharpKit.jQuery;

namespace WebDE.Timekeeper
{

    /// <summary>
    /// The Execution object is an object that holds the contexts and properties of an execution job.
    /// </summary>
    [JsType(JsMode.Clr, Filename = "res/WebDE.Clock.js")]
    class Execution
    {

        private Action context;
        private bool delayed = false;

        /// <summary>
        /// Create a new execution object with the specified context.
        /// </summary>
        /// <param name="func">The context belonging to this execution.</param>
        public Execution(Action func)
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
        /// Delay this execution for the specefied number of seconds.
        /// </summary>
        /// <param name="seconds">The number of seconds to delay the execution.</param>
        public void Delay(int seconds, HtmlWindow window)
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
