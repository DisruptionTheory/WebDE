using System;
using System.Collections.Generic;
using SharpKit.JavaScript;
using SharpKit.Html4;
using SharpKit.jQuery;

namespace WebDE.Net
{
    [JsType(JsMode.Clr, Filename = "../scripts/WebDE.Net.GameClient.js")]
    public class MessageQueue
    {
        public int Count = 0;

        private List<object> queue = new List<object>();
 
        public void Enqueue(object newMember)
        {
            List<object> tQueue = queue;
            tQueue.Add(newMember);
            queue = tQueue;
            Count = queue.Count;
        }
        public object Dequeue()
        {
            int count = queue.Count;
            object top = queue[count - 1];
            List<object> tQueue = queue;
            tQueue.RemoveAt(count - 1);
            queue = tQueue;
            Count = queue.Count;
            return top;
        }
    }
}