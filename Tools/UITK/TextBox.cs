using System;
using System.Collections.Generic;
using SharpKit.JavaScript;
using SharpKit.Html;
using SharpKit.jQuery;

namespace UITK
{
    [JsType(JsMode.Clr, Filename = "scripts/UITK.js")]
    public class TextBox : UITKKeyedComponent
    {

        /// <summary>
        /// The text in the text field.
        /// </summary>
        public string Text
        {
            get
            {
                return currentText;
            }
            set
            {
                currentText = value;
                Validate();
            }
        }
        private string currentText = string.Empty;

        public TextBox()
        {
            Height = 30;
            Width = 150;
            KeyUp += new KeyUpEventHandler(keyUp);
        }

        private void keyUp(UITKComponent sender, UITKKeyboardEventArguments e)
        {
            string oldText = currentText;
            currentText = Surface.GetValue(Id);
            if (oldText != currentText) TextChanged(this);
        }

        public override void RecreateHTML()
        {
            string markup = "<input type='text' ";
            markup += "id='" + Id + "' ";
            markup += "value='" + currentText + "' ";
            markup += "/>";
            Markup.SetMarkup(markup);

            Styles.SetRule("left", Position.X);
            Styles.SetRule("top", Position.Y);
            Styles.SetHeight(Height);
            Styles.SetWidth(Width);
        }

        public event TextChangedEventHandler TextChanged;
    }
}