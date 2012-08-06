/*Generated by SharpKit 5 v4.27.4000*/
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
if(typeof(JsTypes) == "undefined")
    JsTypes = [];
var WebDE$Net$test=
{
    fullname:"WebDE.Net.test",
    baseTypeName:"System.Object",
    staticDefinition:
    {
        cctor:function()
        {
        },
        StartTest:function()
        {
            var client=new WebDE.Net.NetworkClient.ctor("localhost",81);
            client.Connect();
            client.OnDisconnect = $CombineDelegates(client.OnDisconnect,WebDE.Net.test.client_OnDisconnect);
        },
        client_OnDisconnect:function()
        {
            alert("hello!");
        }
    },
    assemblyName:"WebDE.Net",
    Kind:"Class",
    definition:
    {
        ctor:function()
        {
            System.Object.ctor.call(this);
        }
    }
};
JsTypes.push(WebDE$Net$test);
