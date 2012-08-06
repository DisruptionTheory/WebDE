using System;
using System.Collections.Generic;

using SharpKit.JavaScript;
using SharpKit.Html4;
using SharpKit.jQuery;

namespace WebDE.Timekeeper
{
    [JsType(JsMode.Clr, Filename = "scripts/WebDE.Clock.js")]
    public class Clock : HtmlContextBase
    {
        /// <summary>
        /// The maximum frame rate, in frames/second, that the main loop should run.
        /// </summary>
        public static int MaxFrameRate = 30;
        /// <summary>
        /// The length of the IDs generated for the timers and functions added to the lists.
        /// </summary>
        public static int IDLength = 32;

        private static Dictionary<string, Execution> calculationList = new Dictionary<string, Execution>();
        private static Dictionary<string, Execution> renderList = new Dictionary<string, Execution>();
        private static Dictionary<string, TimedExecution> intervals = new Dictionary<string, TimedExecution>();
        private static Dictionary<string, TimedExecution> timeouts = new Dictionary<string, TimedExecution>();

        /// <summary>
        /// Instantiates a new clock object and starts its main loop.
        /// </summary>
        public static void Start()
        {
            window.setTimeout(loop, 0);
        }

        /// <summary>
        /// Adds a context to be run during the calculation phase of the main loop.
        /// </summary>
        /// <param name="context">The context to be run.</param>
        /// <returns>The id of the context for use later.</returns>
        public static string AddCalculation(Action context)
        {
            //generate a new random id
            string id = randomId(IDLength);
            calculationList[id] = new Execution(context);
            return id;
        }

        /// <summary>
        /// Adds a context to be run during the render phase of the main loop.
        /// </summary>
        /// <param name="context">The context to be run.</param>
        /// <returns>The id of the context for later use.</returns>
        public static string AddRender(Action context)
        {
            //generate a new random id
            string id = randomId(IDLength);
            renderList[id] = new Execution(context);
            return id;
        }

        /// <summary>
        /// Removes a calculation from the calculation phase of the main loop.
        /// </summary>
        /// <param name="id">The id of the calculation.</param>
        public static void RemoveCalculation(string id)
        {
            if (calculationList.ContainsKey(id))
            {
                calculationList.Remove(id);
            }
        }

        /// <summary>
        /// Removes a render context from the render phase of the main loop.
        /// </summary>
        /// <param name="id">The id of the calculation.</param>
        public static void RemoveRender(string id)
        {
            if (renderList.ContainsKey(id))
            {
                renderList.Remove(id);
            }
        }

        /// <summary>
        /// Delays the calculation of a calculation for the specefied time.
        /// </summary>
        /// <param name="id">The id of the calculation.</param>
        /// <param name="delaySeconds">The amount of time, in seconds, to delay the calculation.</param>
        public static void delayCalculation(string id, int delaySeconds)
        {
            if (calculationList.ContainsKey(id))
            {
                calculationList[id].Delay(delaySeconds, window);
            }
        }

        /// <summary>
        /// Delays the execution of the render for the specefied time.
        /// </summary>
        /// <param name="id">The id of the render.</param>
        /// <param name="delaySeconds">The amount of time, in seconds, to delay the render.</param>
        public static void delayRender(string id, int delaySeconds)
        {
            if (renderList.ContainsKey(id))
            {
                renderList[id].Delay(delaySeconds, window);
            }
        }

        /// <summary>
        /// Create a new single timeout context to be executed by the clock.
        /// </summary>
        /// <param name="execution">The context to be executed.</param>
        /// <param name="seconds">The amount of time, in seconds, to trigger the execution.</param>
        /// <returns>The string id of the execution.</returns>
        public static string TimedExecute(Action execution, int seconds)
        {
            string id = randomId(IDLength);
            TimedExecution timedExecution = new TimedExecution(execution);
            int jsID = window.setTimeout(timedExecution.Execute, seconds * 1000);
            timedExecution.SetTimerID(jsID);
            timeouts[id] = timedExecution;
            return id;
        }

        /// <summary>
        /// Create a new reoccuring execution.
        /// </summary>
        /// <param name="execution">The context to be executed.</param>
        /// <param name="seconds">The amount of time, in seconds, between those executions.</param>
        /// <returns>The string id of the execution.</returns>
        public static string IntervalExecute(Action execution, double seconds)
        {
            string id = randomId(IDLength);
            TimedExecution timedExecution = new TimedExecution(execution);
            int jsID = window.setInterval(timedExecution.Execute, Math.Round(seconds * 1000));
            timedExecution.SetTimerID(jsID);
            timeouts[id] = timedExecution;
            return id;
        }

        /// <summary>
        /// Cancels out and removes an interval execution.
        /// </summary>
        /// <param name="id">The id of the interval execution.</param>
        public static void CancelIntervalExecute(string id)
        {
            if (intervals.ContainsKey(id))
            {
                window.clearInterval(intervals[id].GetTimerID());
                intervals.Remove(id);
            }
        }

        /// <summary>
        /// Runs the game base loop, running through the executionList and then triggering the render.
        /// </summary>
        private static void loop()
        {
            //get starting time            
            DateTime start = DateTime.Now;

            //perform calculations
            foreach (KeyValuePair<string, Execution> exec in calculationList)
            {
                if (!exec.Value.IsDelayed())
                {
                    exec.Value.Execute();
                }
            }

            //perform renders
            //perform calculations
            foreach (KeyValuePair<string, Execution> exec in renderList)
            {
                if (!exec.Value.IsDelayed())
                {
                    exec.Value.Execute();
                }
            }

            //get frame interval millis
            double frameInterval = (1.0 / MaxFrameRate) * 1000;
            DateTime end = DateTime.Now;
            int frameTime = end.Subtract(start).Milliseconds;
            if (frameTime > frameInterval)
            {
                window.setTimeout(loop, 0);
            }
            else
            {
                window.setTimeout(loop, (int)frameInterval - frameTime);
            }
        }

        /// <summary>
        /// Generates a random string based on the passed in length.
        /// </summary>
        /// <param name="length">The number of digits in the returned string</param>
        /// <returns>A randomly generated string of numbers.</returns>
        private static string randomId(int length)
        {
            string id = "";
            for (int i = 0; i < length; i++)
            {
                int alphonse = JsMath.floor(JsMath.random() * 10);
                id += alphonse.ToString();

                //was
                //id += (Math.Floor(Math.Random() * 10)).ToString();
            }
            return id;
        }


    }
}
