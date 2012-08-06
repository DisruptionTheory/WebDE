/*Generated by SharpKit 5 v4.28.1000*/
function $CombineDelegates(del1,del2)
{
    if(del1 == null)
        return del2;
    if(del2 == null)
        return del1;
    var del=$CreateMulticastDelegateFunction();
    del.delegates = [];
    if(del1.isMulticastDelegate)
    {
        for(var i=0;i < del1.delegates.length;i++)
            del.delegates.push(del1.delegates[i]);
    }
    else
    {
        del.delegates.push(del1);
    }
    if(del2.isMulticastDelegate)
    {
        for(var i=0;i < del2.delegates.length;i++)
            del.delegates.push(del2.delegates[i]);
    }
    else
    {
        del.delegates.push(del2);
    }
    return del;
};
function $CreateMulticastDelegateFunction()
{
    var del=function()
    {
        var del2=arguments.callee;
        var x=undefined;
        for(var i=0;i < del2.delegates.length;i++)
        {
            var del3=del2.delegates[i];
            x = del3.apply(null,arguments);
        }
        return x;
    };
    del.isMulticastDelegate = true;
    return del;
};
if (typeof($CreateDelegate)=='undefined'){
    if(typeof($iKey)=='undefined') var $iKey = 0;
    if(typeof($pKey)=='undefined') var $pKey = String.fromCharCode(1);
    $CreateDelegate = function(target, func){
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
    JsTypes = [];
var WebDE$Net$GameClient=
{
    fullname:"WebDE.Net.GameClient",
    baseTypeName:"System.Object",
    assemblyName:"WebDE",
    Kind:"Class",
    definition:
    {
        ctor:function(host,port)
        {
            this.client = null;
            this.requestQueue = new WebDE.Net.MessageQueue.ctor();
            this.messageInTransit = false;
            this.messageCallBack = null;
            System.Object.ctor.call(this);
            this.client = new WebDE.Net.NetworkClient.ctor(host,port);
            this.client.OnConnect = $CombineDelegates(this.client.OnConnect,$CreateDelegate(this,this.client_OnConnect));
            this.client.OnDisconnect = $CombineDelegates(this.client.OnDisconnect,$CreateDelegate(this,this.client_OnDisconnect));
            this.client.OnReceive = $CombineDelegates(this.client.OnReceive,$CreateDelegate(this,this.client_OnReceive));
            this.client.Connect();
        },
        Host$$:"System.String",
        get_Host:function()
        {
            return this.client.get_Host();
        },
        Port$$:"System.Int32",
        get_Port:function()
        {
            return this.client.get_Port();
        },
        ApiKey$$:"System.String",
        get_ApiKey:function(){return this._ApiKey;},
        set_ApiKey:function(value){this._ApiKey = value;},
        client_OnConnect:function()
        {
        },
        client_OnDisconnect:function()
        {
        },
        client_OnReceive:function(message)
        {
            this.messageInTransit = false;
            var callback=this.messageCallBack;
            if(this.requestQueue.Count > 0)
            {
                var queueItem=Cast(this.requestQueue.Dequeue(),Array);
                this.send(queueItem[0],Cast(queueItem[1],System.Action$1));
            }
            callback(message);
        },
        send:function(message,callback)
        {
            if(this.messageInTransit)
            {
                var queueItem=[message,callback];
                this.requestQueue.Enqueue(queueItem);
            }
            else
            {
                this.messageInTransit = true;
                this.messageCallBack = callback;
                this.client.Send(message);
            }
        },
        GetApikey:function(username,password,callback)
        {
            var request={action:2,user:username,pass:password};
            this.send(request,callback);
        },
        GetMaps:function(callback)
        {
            var request={action:0,type:0,mapid:0,apikey:this.get_ApiKey()};
            this.send(request,callback);
        },
        GetMap:function(mapid,callback)
        {
            var request={action:0,type:0,mapid:mapid,apikey:this.get_ApiKey()};
            this.send(request,callback);
        },
        GetGroups:function(callback)
        {
            var request={action:3,apikey:this.get_ApiKey()};
            this.send(request,callback);
        }
    }
};
JsTypes.push(WebDE$Net$GameClient);
var WebDE$Net$MessageQueue=
{
    fullname:"WebDE.Net.MessageQueue",
    baseTypeName:"System.Object",
    assemblyName:"WebDE",
    Kind:"Class",
    definition:
    {
        ctor:function()
        {
            this.Count = 0;
            this.queue = new System.Collections.Generic.List$1.ctor(System.Object);
            System.Object.ctor.call(this);
        },
        Enqueue:function(newMember)
        {
            var tQueue=this.queue;
            tQueue.Add(newMember);
            this.queue = tQueue;
            this.Count = this.queue.get_Count();
        },
        Dequeue:function()
        {
            var count=this.queue.get_Count();
            var top=this.queue.get_Item$$Int32(count - 1);
            var tQueue=this.queue;
            tQueue.RemoveAt(count - 1);
            this.queue = tQueue;
            this.Count = this.queue.get_Count();
            return top;
        }
    }
};
JsTypes.push(WebDE$Net$MessageQueue);
