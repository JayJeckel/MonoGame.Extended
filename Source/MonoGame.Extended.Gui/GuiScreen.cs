﻿using System.Linq;
using Microsoft.Xna.Framework;
using MonoGame.Extended.Gui.Controls;
using Newtonsoft.Json;

namespace MonoGame.Extended.Gui
{
    public class GuiScreen
    {
        public GuiScreen(GuiSkin skin)
        {
            Skin = skin;
            Controls = new GuiControlCollection();
        }

        [JsonProperty(Order = 1)]
        public GuiSkin Skin { get; set; }

        [JsonProperty(Order = 2)]
        public GuiControlCollection Controls { get; }

        public T FindControl<T>(string name)
            where T : GuiControl
        {
            return FindControl<T>(Controls, name);
        }

        private static T FindControl<T>(GuiControlCollection controls, string name)
            where T : GuiControl
        {
            foreach (var control in controls)
            {
                if (control.Name == name)
                    return control as T;

                if (control.Controls.Any())
                {
                    var childControl = FindControl<T>(control.Controls, name);

                    if (childControl != null)
                        return childControl;
                }
            }

            return null;
        }

        public void Layout(IGuiContext context, RectangleF rectangle)
        {
            foreach (var control in Controls)
            {
                if (control.Size.IsEmpty)
                {
                    control.Position = rectangle.Position;
                    control.Size = control.GetDesiredSize(context, rectangle.Size);
                }

                var layoutControl = control as GuiLayoutControl;
                layoutControl?.Layout(context, rectangle);
            }
        }
    }
}