using System;
using System.Collections.Generic;
using SharpKit.JavaScript;
using SharpKit.Html4;
using SharpKit.jQuery;

namespace UITK
{
    [JsType(JsMode.Clr, Filename = "scripts/UITK.js")]
    public class UITKStyles
    {
        internal UITKComponent Owner
        {
            get;
            private set;
        }

        private Dictionary<string, string> styles = new Dictionary<string, string>();
        internal UITKStyles(UITKComponent owner) 
        {
            styles.Add("position", "absolute");
            styles.Add("height", "50px");
            styles.Add("width", "50px");
            Owner = owner;
        }

        /// <summary>
        /// Get the full dictionary of every set rule in this UITKStyles.
        /// </summary>
        /// <returns>The dictionary of every set rule in this UITKStyles.</returns>
        public Dictionary<string, string> GetStyleDictionary()
        {
            return styles;
        }

        /// <summary>
        /// Sets the internal style dictionary for this UITKStyles.
        /// </summary>
        /// <param name="dictionary">The style dictionary to set.</param>
        internal void SetStyleDictionary(Dictionary<string, string> dictionary)
        {
            styles = dictionary;
        }

        /// <summary>
        /// Get the value of the rule with the specified identifier.
        /// </summary>
        /// <param name="identifier">The css identifier to ge tthe rule value of.</param>
        /// <returns>The string rule value of the specified identifier.</returns>
        public string GetRule(string identifier)
        {
            if(styles.ContainsKey(identifier)) return styles[identifier];
            return string.Empty;
        }

        /// <summary>
        /// Set the rule value for the specified identifier.
        /// </summary>
        /// <param name="identifier">The identifier to set the rule value for.</param>
        /// <param name="rule">The rule value to set.</param>
        internal void SetRule(string identifier, string rule)
        {
            if (styles.ContainsKey(identifier)) styles[identifier] = rule;
            styles.Add(identifier, rule);
        }

        /// <summary>
        /// Set the rule value for the specified identifier.
        /// </summary>
        /// <param name="identifier">The identifier to set the rule value for.</param>
        /// <param name="rule">The rule value to set.</param>
        internal void SetRule(string identifier, int rule)
        {
            if (styles.ContainsKey(identifier)) styles[identifier] = rule + "px";
            styles.Add(identifier, rule + "px");
        }

        internal void SetHeight(int height)
        {
            styles["height"] = height.ToString() + "px";
        }

        internal void SetWidth(int width)
        {
            styles["width"] = width.ToString() + "px";
        }

        
    }
}