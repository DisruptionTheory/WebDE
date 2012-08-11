/*Generated by SharpKit 5 v4.28.9000*/
if (typeof($CreateDelegate)=='undefined'){
    if(typeof($iKey)=='undefined') var $iKey = 0;
    if(typeof($pKey)=='undefined') var $pKey = String.fromCharCode(1);
    var $CreateDelegate = function(target, func){
        if (target == null || func == null) 
            return func;
        if(func.target==target && func.func==func)
            return func;
        if (target.$delegateCache == null)
            target.$delegateCache = {};
        if (func.$key == null)
            func.$key = $pKey + String(++$iKey);
        var delegate;
        if(target.$delegateCache!=null)
            delegate = target.$delegateCache[func.$key];
        if (delegate == null){
            delegate = function(){
                return func.apply(target, arguments);
            };
            delegate.func = func;
            delegate.target = target;
            delegate.isDelegate = true;
            if(target.$delegateCache!=null)
                target.$delegateCache[func.$key] = delegate;
        }
        return delegate;
    }
}
if(typeof(JsTypes) == "undefined")
    var JsTypes=[];
var WebDE$Timekeeper$Clock=
{
    fullname:"WebDE.Timekeeper.Clock",
    baseTypeName:"System.Object",
    staticDefinition:
    {
        cctor:function()
        {
            WebDE.Timekeeper.Clock.MaxFrameRate = 30;
            WebDE.Timekeeper.Clock.IDLength = 32;
            WebDE.Timekeeper.Clock.calculationList = new System.Collections.Generic.Dictionary$2.ctor(System.String.ctor,WebDE.Timekeeper.Execution.ctor);
            WebDE.Timekeeper.Clock.renderList = new System.Collections.Generic.Dictionary$2.ctor(System.String.ctor,WebDE.Timekeeper.Execution.ctor);
            WebDE.Timekeeper.Clock.intervals = new System.Collections.Generic.Dictionary$2.ctor(System.String.ctor,WebDE.Timekeeper.TimedExecution.ctor);
            WebDE.Timekeeper.Clock.timeouts = new System.Collections.Generic.Dictionary$2.ctor(System.String.ctor,WebDE.Timekeeper.TimedExecution.ctor);
        },
        Start:function()
        {
            window.setTimeout(WebDE.Timekeeper.Clock.loop,0);
        },
        AddCalculation:function(context)
        {
            var id=WebDE.Timekeeper.Clock.randomId(WebDE.Timekeeper.Clock.IDLength);
            WebDE.Timekeeper.Clock.calculationList.set_Item$$TKey(id,new WebDE.Timekeeper.Execution.ctor(context));
            return id;
        },
        AddRender:function(context)
        {
            var id=WebDE.Timekeeper.Clock.randomId(WebDE.Timekeeper.Clock.IDLength);
            WebDE.Timekeeper.Clock.renderList.set_Item$$TKey(id,new WebDE.Timekeeper.Execution.ctor(context));
            return id;
        },
        RemoveCalculation:function(id)
        {
            if(WebDE.Timekeeper.Clock.calculationList.ContainsKey(id))
            {
                WebDE.Timekeeper.Clock.calculationList.Remove(id);
            }
        },
        RemoveRender:function(id)
        {
            if(WebDE.Timekeeper.Clock.renderList.ContainsKey(id))
            {
                WebDE.Timekeeper.Clock.renderList.Remove(id);
            }
        },
        delayCalculation:function(id,delaySeconds)
        {
            if(WebDE.Timekeeper.Clock.calculationList.ContainsKey(id))
            {
                WebDE.Timekeeper.Clock.calculationList.get_Item$$TKey(id).Delay(delaySeconds,window);
            }
        },
        delayRender:function(id,delaySeconds)
        {
            if(WebDE.Timekeeper.Clock.renderList.ContainsKey(id))
            {
                WebDE.Timekeeper.Clock.renderList.get_Item$$TKey(id).Delay(delaySeconds,window);
            }
        },
        TimedExecute:function(execution,seconds)
        {
            var id=WebDE.Timekeeper.Clock.randomId(WebDE.Timekeeper.Clock.IDLength);
            var timedExecution=new WebDE.Timekeeper.TimedExecution.ctor(execution);
            var jsID=window.setTimeout($CreateDelegate(timedExecution,timedExecution.Execute),seconds * 1000);
            timedExecution.SetTimerID(jsID);
            WebDE.Timekeeper.Clock.timeouts.set_Item$$TKey(id,timedExecution);
            return id;
        },
        IntervalExecute:function(execution,seconds)
        {
            var id=WebDE.Timekeeper.Clock.randomId(WebDE.Timekeeper.Clock.IDLength);
            var timedExecution=new WebDE.Timekeeper.TimedExecution.ctor(execution);
            var jsID=window.setInterval($CreateDelegate(timedExecution,timedExecution.Execute),System.Math.Round$$Double(seconds * 1000));
            timedExecution.SetTimerID(jsID);
            WebDE.Timekeeper.Clock.timeouts.set_Item$$TKey(id,timedExecution);
            return id;
        },
        CancelIntervalExecute:function(id)
        {
            if(WebDE.Timekeeper.Clock.intervals.ContainsKey(id))
            {
                window.clearInterval(WebDE.Timekeeper.Clock.intervals.get_Item$$TKey(id).GetTimerID());
                WebDE.Timekeeper.Clock.intervals.Remove(id);
            }
        },
        loop:function()
        {
            var start=System.DateTime.get_Now();
            var $it1=WebDE.Timekeeper.Clock.calculationList.GetEnumerator();
            while($it1.MoveNext())
            {
                var exec=$it1.get_Current();
                if(!exec.get_Value().IsDelayed())
                {
                    exec.get_Value().Execute();
                }
            }
            var $it2=WebDE.Timekeeper.Clock.renderList.GetEnumerator();
            while($it2.MoveNext())
            {
                var exec=$it2.get_Current();
                if(!exec.get_Value().IsDelayed())
                {
                    exec.get_Value().Execute();
                }
            }
            var frameInterval=(1 / WebDE.Timekeeper.Clock.MaxFrameRate) * 1000;
            var end=System.DateTime.get_Now();
            var frameTime=end.Subtract$$DateTime(start).get_Milliseconds();
            if(frameTime > frameInterval)
            {
                window.setTimeout(WebDE.Timekeeper.Clock.loop,0);
            }
            else
            {
                window.setTimeout(WebDE.Timekeeper.Clock.loop,Cast(frameInterval,System.Int32.ctor) - frameTime);
            }
        },
        randomId:function(length)
        {
            var id="";
            for(var i=0;i < length;i++)
            {
                var alphonse=Math.floor(Math.random() * 10);
                id += alphonse.toString();
            }
            return id;
        }
    },
    assemblyName:"WebDE.Clock",
    Kind:"Class",
    definition:
    {
        ctor:function()
        {
            System.Object.ctor.call(this);
        }
    }
};
JsTypes.push(WebDE$Timekeeper$Clock);
var WebDE$Timekeeper$Execution=
{
    fullname:"WebDE.Timekeeper.Execution",
    baseTypeName:"System.Object",
    assemblyName:"WebDE.Clock",
    Kind:"Class",
    definition:
    {
        ctor:function(func)
        {
            this.context = null;
            this.delayed = false;
            System.Object.ctor.call(this);
            this.context = func;
        },
        Execute:function()
        {
            this.context();
        },
        Delay:function(seconds,window)
        {
            this.delayed = true;
            window.setTimeout($CreateDelegate(this,this.unDelay),seconds * 1000);
        },
        IsDelayed:function()
        {
            return this.delayed;
        },
        unDelay:function()
        {
            this.delayed = false;
        }
    }
};
JsTypes.push(WebDE$Timekeeper$Execution);
var WebDE$Timekeeper$TimedExecution=
{
    fullname:"WebDE.Timekeeper.TimedExecution",
    baseTypeName:"System.Object",
    assemblyName:"WebDE.Clock",
    Kind:"Class",
    definition:
    {
        ctor:function(func)
        {
            this.context = null;
            this.timerID = 0;
            System.Object.ctor.call(this);
            this.context = func;
        },
        Execute:function()
        {
            this.context();
        },
        SetTimerID:function(id)
        {
            this.timerID = id;
        },
        GetTimerID:function()
        {
            return this.timerID;
        }
    }
};
JsTypes.push(WebDE$Timekeeper$TimedExecution);
