//*******************************************************************************************//
//                                                                                           //
// Get Your API Key: https://app.pdf.co/signup                                               //
// API Documentation: https://developer.pdf.co/api/index.html                                //
//                                                                                           //
// Note: Replace placeholder values in the code with your API Key                            //
// and file paths (if applicable)                                                            //
//                                                                                           //
//*******************************************************************************************//


using System;
using System.Collections.Generic;

/// <summary>
/// https://github.com/Efferent-Health/HL7-dotnetcore
/// </summary>
namespace HL7.Dotnetcore
{
    public class Component : MessageElement
    {
        internal List<SubComponent> SubComponentList { get; set; }

        public bool IsSubComponentized { get; set; } = false;

        private bool isDelimiter = false;

        public Component(HL7Encoding encoding, bool isDelimiter = false)
        {
            this.isDelimiter = isDelimiter;
            this.SubComponentList = new List<SubComponent>();
            this.Encoding = encoding;
        }
        public Component(string pValue, HL7Encoding encoding)
        {
            this.SubComponentList = new List<SubComponent>();
            this.Encoding = encoding;
            this.Value = pValue;
        }

        protected override void ProcessValue()
        {
            List<string> allSubComponents;
            
            if (this.isDelimiter)
                allSubComponents = new List<string>(new [] {this.Value});
            else
                allSubComponents = MessageHelper.SplitString(_value, this.Encoding.SubComponentDelimiter);

            if (allSubComponents.Count > 1)
            {
                this.IsSubComponentized = true;
            }

            this.SubComponentList = new List<SubComponent>();

            foreach (string strSubComponent in allSubComponents)
            {
                SubComponent subComponent = new SubComponent(this.Encoding.Decode(strSubComponent), this.Encoding);
                SubComponentList.Add(subComponent);
            }
        }

        public SubComponent SubComponents(int position)
        {
            position = position - 1;
            SubComponent sub = null;

            try
            {
                sub = SubComponentList[position];
            }
            catch (Exception ex)
            {
                throw new HL7Exception("SubComponent not availalbe Error-" + ex.Message);
            }

            return sub;
        }

        public List<SubComponent> SubComponents()
        {
            return SubComponentList;
        }
    }

    internal class ComponentCollection : List<Component>
    {
        /// <summary>
        /// Component indexer
        /// </summary>
        internal new Component this[int index]
        {
            get
            {
                Component component = null;

                if (index < base.Count)
                    component = base[index];

                return component;
            }
            set
            {
                base[index] = value;
            }
        }

        /// <summary>
        /// Add Component at next position
        /// </summary>
        /// <param name="component">Component</param>
        internal new void Add(Component component)
        {
            base.Add(component);
        }

        /// <summary>
        /// Add component at specific position
        /// </summary>
        /// <param name="component">Component</param>
        /// <param name="position">Position</param>
        internal void Add(Component component, int position)
        {
            int listCount = base.Count;
            position = position - 1;

            if (position < listCount)
            {
                base[position] = component;
            }
            else
            {
                for (int comIndex = listCount; comIndex < position; comIndex++)
                {
                    Component blankComponent = new Component(component.Encoding);
                    blankComponent.Value = string.Empty;
                    base.Add(blankComponent);
                }

                base.Add(component);
            }
        }
    }
}
