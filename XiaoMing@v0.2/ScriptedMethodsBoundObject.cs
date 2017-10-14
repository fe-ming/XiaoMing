using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XiaoMing_v0._2
{
    class ScriptedMethodsBoundObject
    {
        /// <summary>
        /// Raised when a Javascript event arrives.
        /// </summary>
        public event Action<string, object> EventArrived;

        /// <summary>
        /// This method will be exposed to the Javascript environment. It is
        /// invoked in the Javascript environment when some event of interest
        /// happens.
        /// </summary>
        /// <param name="eventName">The name of the event.</param>
        /// <param name="eventData">Data provided by the invoker pertaining to the event.</param>
        /// <remarks>
        /// By default RaiseEvent will be translated to raiseEvent as a javascript function.
        /// This is configurable when calling RegisterJsObject by setting camelCaseJavascriptNames;
        /// </remarks>
        public void RaiseEvent(string eventName, object eventData = null)
        {
            if (EventArrived != null)
            {
                EventArrived(eventName, eventData);
            }
        }
    }
}
