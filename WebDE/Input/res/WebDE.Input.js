/*Generated by SharpKit 5 v4.27.4000*/
if(typeof(JsTypes) == "undefined")
    JsTypes = [];
var WebDE$InputManager$Input=
{
    fullname:"WebDE.InputManager.Input",
    baseTypeName:"System.Object",
    staticDefinition:
    {
        cctor:function()
        {
            WebDE.InputManager.Input.disableAdvancedMouseInput = false;
            WebDE.InputManager.Input.onscreenKeyboard = false;
        },
        Init:function()
        {
            WebDE.InputManager.Input.Bind_Input_DOM();
            WebDE.InputManager.InputDevice.InitializeDevices();
        },
        LoadConfig:function(configKeyVals)
        {
        },
        ProcessMouseButtonEvent:function(buttonId,clickX,clickY)
        {
            var buttonFunction=WebDE.InputManager.InputDevice.Mouse.GetFunctionFromButton("",buttonId);
            var $it22=WebDE.GUI.GuiLayer.GetActiveLayers().GetEnumerator();
            while($it22.MoveNext())
            {
                var activeLayer=$it22.get_Current();
                if(clickX > activeLayer.GetArea().Right() || clickX < activeLayer.GetArea().x || clickY > activeLayer.GetArea().Bottom() || clickY < activeLayer.GetArea().y)
                {
                    continue;
                }
                var actionLocation=new WebDE.Point.ctor(clickX,clickY);
                activeLayer.GUI_Event(buttonFunction,actionLocation);
            }
        },
        ProcessKeyboardEvent:function(buttonId)
        {
            var buttonFunction=WebDE.InputManager.InputDevice.Keyboard.GetFunctionFromButton("",buttonId);
            var $it23=WebDE.GUI.GuiLayer.GetActiveLayers().GetEnumerator();
            while($it23.MoveNext())
            {
                var activeLayer=$it23.get_Current();
                var actionLocation=new WebDE.Point.ctor(0,0);
                activeLayer.GUI_Event(buttonFunction,actionLocation);
            }
        },
        Bind_Input_DOM:function()
        {
            $(document.body).bind("click",WebDE.InputManager.Input.handlejQueryClick);
            $(document.body).bind("keydown",WebDE.InputManager.Input.handlejQueryKeyboard);
        },
        handlejQueryClick:function(eventData)
        {
            var clickX=eventData.clientX - System.Int32.Parse$$String(document.getElementById("gameWrapper").style.left);
            var clickY=eventData.clientY - System.Int32.Parse$$String(document.getElementById("gameWrapper").style.top);
            WebDE.InputManager.Input.ProcessMouseButtonEvent(eventData.which,clickX,clickY);
        },
        handlejQueryKeyboard:function(eventData)
        {
            WebDE.InputManager.Input.ProcessKeyboardEvent(eventData.which);
        }
    },
    assemblyName:"WebDE",
    Kind:"Class",
    definition:
    {
        ctor:function()
        {
            System.Object.ctor.call(this);
        }
    }
};
JsTypes.push(WebDE$InputManager$Input);
var WebDE$InputManager$InputDevice=
{
    fullname:"WebDE.InputManager.InputDevice",
    baseTypeName:"System.Object",
    staticDefinition:
    {
        cctor:function()
        {
            WebDE.InputManager.InputDevice.Mouse = null;
            WebDE.InputManager.InputDevice.Keyboard = null;
        },
        InitializeDevices:function()
        {
            WebDE.InputManager.InputDevice.Mouse = new WebDE.InputManager.InputDevice.ctor("mouse");
            WebDE.InputManager.InputDevice.Keyboard = new WebDE.InputManager.InputDevice.ctor("keyboard");
            WebDE.InputManager.InputDevice.Mouse.SetButtonName(1,"mouse0");
            WebDE.InputManager.InputDevice.Keyboard.SetButtonName(187,"Plus");
            WebDE.InputManager.InputDevice.Keyboard.SetButtonName(107,"NumPlus");
        }
    },
    assemblyName:"WebDE",
    Kind:"Class",
    definition:
    {
        ctor:function(name)
        {
            this.deviceName = "";
            this.buttonNames = new System.Collections.Generic.Dictionary$2.ctor(System.Int32,System.String);
            this.buttonFunctions = new System.Collections.Generic.Dictionary$2.ctor(System.Int32,WebDE.GUI.GUIFunction);
            System.Object.ctor.call(this);
            this.deviceName = name;
        },
        SetButtonName:function(buttonId,buttonName)
        {
            this.buttonNames.set_Item$$TKey(buttonId,buttonName);
        },
        GetButtonName:function(buttonId)
        {
            return this.buttonNames.get_Item$$TKey(buttonId);
        },
        GetButtonId:function(buttonName)
        {
            var $it24=this.buttonNames.get_Keys().GetEnumerator();
            while($it24.MoveNext())
            {
                var buttonKey=$it24.get_Current();
                if(this.buttonNames.get_Item$$TKey(buttonKey) == buttonName)
                {
                    return buttonKey;
                }
            }
            return -1;
        },
        Bind:function(buttonName,buttonId,buttonFunction)
        {
            if(buttonName != "" && buttonName != null)
            {
                buttonId = this.GetButtonId(buttonName);
            }
            this.buttonFunctions.set_Item$$TKey(buttonId,buttonFunction);
        },
        UnBind:function(buttonName,buttonId)
        {
        },
        GetFunctionFromButton:function(buttonName,buttonId)
        {
            if(buttonName != "" && buttonName != null)
            {
                buttonId = this.GetButtonId(buttonName);
            }
            return this.buttonFunctions.get_Item$$TKey(buttonId);
        }
    }
};
JsTypes.push(WebDE$InputManager$InputDevice);
