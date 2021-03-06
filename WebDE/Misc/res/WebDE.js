/*Generated by SharpKit 5 v4.27.4000*/
if(typeof(JsTypes) == "undefined")
    JsTypes = [];
var WebDE$Color=
{
    fullname:"WebDE.Color",
    baseTypeName:"System.Object",
    staticDefinition:
    {
        cctor:function()
        {
            WebDE.Color.Black = new WebDE.Color.ctor(0,0,0);
            WebDE.Color.White = new WebDE.Color.ctor(255,255,255);
        },
        ToHex:function(val)
        {
            var hexstuff="ABCDEF";
            if(val < 10)
            {
                return val.toString();
            }
            else
            {
                val -= 10;
                return hexstuff.charAt(val).toString();
            }
        },
        FromHex:function(hexValue)
        {
            return new WebDE.Color.ctor(0,0,0);
        }
    },
    assemblyName:"WebDE",
    Kind:"Class",
    definition:
    {
        ctor:function(redVal,greenVal,blueVal)
        {
            this.name = "";
            this.red = 0;
            this.green = 0;
            this.blue = 0;
            System.Object.ctor.call(this);
            this.red = redVal;
            this.green = greenVal;
            this.blue = blueVal;
        },
        GetHex:function()
        {
            var returnString="";
            var redDub=this.red / 16;
            returnString += WebDE.Color.ToHex(Cast(System.Math.Round$$Double(redDub),System.Int32));
            redDub = this.red % 16;
            returnString += WebDE.Color.ToHex(Cast(System.Math.Round$$Double(redDub),System.Int32));
            var blueDub=this.blue / 16;
            returnString += WebDE.Color.ToHex(Cast(System.Math.Round$$Double(blueDub),System.Int32));
            blueDub = this.blue % 16;
            returnString += WebDE.Color.ToHex(Cast(System.Math.Round$$Double(blueDub),System.Int32));
            var greenDub=this.green / 16;
            returnString += WebDE.Color.ToHex(Cast(System.Math.Round$$Double(greenDub),System.Int32));
            greenDub = this.green % 16;
            returnString += WebDE.Color.ToHex(Cast(System.Math.Round$$Double(greenDub),System.Int32));
            return returnString;
        },
        Match:function(colorTomatch)
        {
            if(this.red != colorTomatch.red || this.green != colorTomatch.green || this.blue != colorTomatch.blue)
            {
                return false;
            }
            return true;
        }
    }
};
JsTypes.push(WebDE$Color);
var WebDE$Dimension=
{
    fullname:"WebDE.Dimension",
    baseTypeName:"System.Object",
    assemblyName:"WebDE",
    Kind:"Class",
    definition:
    {
        ctor:function(myWidth,myHeight,myDepth)
        {
            this.width = 0;
            this.height = 0;
            this.depth = 0;
            System.Object.ctor.call(this);
            this.width = myWidth;
            this.height = myHeight;
            this.depth = myDepth;
        },
        ToTuple:function()
        {
            var returnVal=new System.Tuple$2.ctor(System.Double,System.Double,this.width,this.height);
            return returnVal;
        }
    }
};
JsTypes.push(WebDE$Dimension);
var WebDE$Point=
{
    fullname:"WebDE.Point",
    baseTypeName:"System.Object",
    assemblyName:"WebDE",
    Kind:"Class",
    definition:
    {
        ctor:function(theX,theY)
        {
            this.x = 0;
            this.y = 0;
            System.Object.ctor.call(this);
            this.x = theX;
            this.y = theY;
        },
        Distance:function(point2)
        {
            return System.Math.Abs$$Double(point2.x - this.x) + System.Math.Abs$$Double(point2.y - this.y);
        },
        ToTuple:function()
        {
            var returnVal=new System.Tuple$2.ctor(System.Double,System.Double,this.x,this.y);
            return returnVal;
        }
    }
};
JsTypes.push(WebDE$Point);
var WebDE$Rectangle=
{
    fullname:"WebDE.Rectangle",
    baseTypeName:"System.Object",
    assemblyName:"WebDE",
    Kind:"Class",
    definition:
    {
        ctor:function(left,top,width,height)
        {
            this.x = 0;
            this.y = 0;
            this.width = 0;
            this.height = 0;
            System.Object.ctor.call(this);
            this.x = left;
            this.y = top;
            this.width = width;
            this.height = height;
        },
        Right:function()
        {
            return this.x + this.width;
        },
        Bottom:function()
        {
            return this.y + this.height;
        },
        Contains:function(point)
        {
            if(point.x < this.x || point.y < this.y || point.x > this.width || point.y > this.height)
            {
                return false;
            }
            return true;
        }
    }
};
JsTypes.push(WebDE$Rectangle);
var WebDE$Vector=
{
    fullname:"WebDE.Vector",
    baseTypeName:"System.Object",
    staticDefinition:
    {
        Distance:function(vector1,vector2)
        {
            return System.Math.Abs$$Double(vector2.y - vector1.y) + System.Math.Abs$$Double(vector2.x - vector1.x);
        }
    },
    assemblyName:"WebDE",
    Kind:"Class",
    definition:
    {
        ctor:function(xMagnitude,yMagnitude)
        {
            this.x = 0;
            this.y = 0;
            System.Object.ctor.call(this);
            this.x = xMagnitude;
            this.y = yMagnitude;
        }
    }
};
JsTypes.push(WebDE$Vector);
