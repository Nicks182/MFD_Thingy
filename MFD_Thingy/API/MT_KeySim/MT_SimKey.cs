using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WindowsInput;
using WindowsInput.Native;

namespace MFD_Thingy.API
{
    public partial class MT_SimKey
    {
        DB_Logic G_DB_Logic = null;
        InputSimulator G_InputSimulator = null;
        public MT_SimKey()
        {
            
            G_InputSimulator = new InputSimulator();
        }

        public void _SimInput(MT_Message P_MT_Message)
        {
            G_DB_Logic = new DB_Logic();
            MT_Message_Data L_Data_ControlId = P_MT_Message.Data.Where(d => d.Name.ToLower().Equals("controlid")).FirstOrDefault();

            List<DB.Context.Action> L_Actions = G_DB_Logic._Get_ControlActions(Convert.ToInt32(L_Data_ControlId.Value));

            for (int i = 0; i < L_Actions.Count; i++)
            {
                if (L_Actions[i].Type == 2 && L_Actions[i].ScreenId != null)
                {
                    P_MT_Message.MessageType = MT_PageType.LoadAppControls;
                    P_MT_Message.Data = new List<MT_Message_Data>
                    {
                        new MT_Message_Data
                        {
                            Name = "ScreenId",
                            Value = L_Actions[i].ScreenId.Value.ToString()
                        }
                    };
                }
                else
                {
                    _SimAction(L_Actions[i]);
                }
            }
        }

        private void _SimAction(DB.Context.Action P_Action)
        {
            switch (P_Action.Type)
            {
                case 0:
                    _SimKey(P_Action);
                    break;

                case 1:
                    _SimMouse(P_Action);
                    break;
            }

        }


        private void _SimMouse(DB.Context.Action P_Action)
        {
            switch (P_Action.Interaction)
            {
                case "MOUSEMIDDLE": // Middle Mouse
                    {
                        if (P_Action.HoldTime > 0)
                        {
                            G_InputSimulator.Mouse.MiddleButtonDown();
                            G_InputSimulator.Mouse.Sleep(P_Action.HoldTime.GetValueOrDefault(0));
                            G_InputSimulator.Mouse.MiddleButtonUp();
                        }
                        else
                        {
                            G_InputSimulator.Mouse.MiddleButtonClick();
                        }
                    }
                    break;

                case "MOUSELEFT": // Left Button
                    {
                        if (P_Action.HoldTime > 0)
                        {
                            G_InputSimulator.Mouse.LeftButtonDown();
                            G_InputSimulator.Mouse.Sleep(P_Action.HoldTime.GetValueOrDefault(0));
                            G_InputSimulator.Mouse.LeftButtonUp();
                        }
                        else
                        {
                            G_InputSimulator.Mouse.LeftButtonClick();
                        }
                    }
                    break;

                case "MOUSERIGHT": // Right Button
                    {
                        if (P_Action.HoldTime > 0)
                        {
                            G_InputSimulator.Mouse.RightButtonDown();
                            G_InputSimulator.Mouse.Sleep(P_Action.HoldTime.GetValueOrDefault(0));
                            G_InputSimulator.Mouse.RightButtonUp();
                        }
                        else
                        {
                            G_InputSimulator.Mouse.RightButtonClick();
                        }
                    }
                    break;

                case "SCROLLUP": // Scroll up
                    {
                        {
                            G_InputSimulator.Mouse.VerticalScroll(1);
                        }
                    }
                    break;

                case "SCROLLDOWN": // Scroll Down
                    {
                        G_InputSimulator.Mouse.VerticalScroll(-1);
                    }
                    break;

            }
        }

        private void _SimKey(DB.Context.Action P_Action)
        {
            VirtualKeyCode? L_VK = _GetKeyCode(P_Action.Interaction);
            VirtualKeyCode? L_Modifier = _GetModifierKey(P_Action.Modifier);

            if (L_VK != null)
            {
                if(L_Modifier != null)
                {
                    G_InputSimulator.Keyboard.ModifiedKeyStroke(L_Modifier.Value, L_VK.Value);
                }
                else if (P_Action.HoldTime > 0)
                {
                    G_InputSimulator.Keyboard.KeyDown(L_VK.Value);
                    G_InputSimulator.Keyboard.Sleep(P_Action.HoldTime.GetValueOrDefault(0));
                    G_InputSimulator.Keyboard.KeyUp(L_VK.Value);
                }
                else
                {
                    G_InputSimulator.Keyboard.KeyPress(L_VK.Value);
                }
            }
        }


        public VirtualKeyCode? _GetKeyCode(string P_Key)
        {
            foreach (VirtualKeyCode L_VK in Enum.GetValues(typeof(VirtualKeyCode)))
            {
                if (L_VK.ToString().Equals(P_Key))
                {
                    return L_VK;
                }
            }

            return null;
        }

        public List<MT_Interaction> _GetKeyboardInteractions()
        {
            List<MT_Interaction> L_Items = new List<MT_Interaction>();
            foreach (VirtualKeyCode L_VK in Enum.GetValues(typeof(VirtualKeyCode)))
            {
                L_Items.Add(new MT_Interaction
                {
                    Id = L_Items.Count,
                    Name = L_VK.ToString()
                });
            }

            return L_Items;
        }

        public List<MT_Interaction> _GetMouseInteractions()
        {
            List<MT_Interaction> L_Items = new List<MT_Interaction>();
            L_Items.Add(new MT_Interaction
            {
                Id = L_Items.Count,
                Name = "NONE"
            });
            L_Items.Add(new MT_Interaction
            {
                Id = L_Items.Count,
                Name = "MOUSEMIDDLE"
            });
            L_Items.Add(new MT_Interaction
            {
                Id = L_Items.Count,
                Name = "MOUSELEFT"
            });
            L_Items.Add(new MT_Interaction
            {
                Id = L_Items.Count,
                Name = "MOUSERIGHT"
            });
            L_Items.Add(new MT_Interaction
            {
                Id = L_Items.Count,
                Name = "SCROLLUP"
            });
            L_Items.Add(new MT_Interaction
            {
                Id = L_Items.Count,
                Name = "SCROLLDOWN"
            });

            return L_Items;
        }

        private VirtualKeyCode? _GetModifierKey(string P_ModifierId)
        {
            MT_Modifier L_MT_Modifier = _GetModifiers().Where(m => m.Id.ToString().Equals(P_ModifierId)).FirstOrDefault();

            if(L_MT_Modifier != null && L_MT_Modifier.Id != MT_ModifierId.None)
            {
                switch(L_MT_Modifier.Id)
                {
                    case MT_ModifierId.Caps:
                        return VirtualKeyCode.CAPITAL;

                    case MT_ModifierId.LAlt:
                        return VirtualKeyCode.LMENU;

                    case MT_ModifierId.LCtrl:
                        return VirtualKeyCode.LCONTROL;

                    case MT_ModifierId.LShift:
                        return VirtualKeyCode.LSHIFT;

                    case MT_ModifierId.RAlt:
                        return VirtualKeyCode.RMENU;

                    case MT_ModifierId.RCtrl:
                        return VirtualKeyCode.RCONTROL;

                    case MT_ModifierId.RShift:
                        return VirtualKeyCode.RSHIFT;
                }
            }

            return null;
        }

        public List<MT_Modifier> _GetModifiers()
        {
            List<MT_Modifier> L_Items = new List<MT_Modifier>();
            L_Items.Add(new MT_Modifier
            {
                Id = MT_ModifierId.None,
                Name = "None"
            });
            L_Items.Add(new MT_Modifier
            {
                Id =  MT_ModifierId.LCtrl,
                Name = "CTRL Left"
            });
            L_Items.Add(new MT_Modifier
            {
                Id = MT_ModifierId.RCtrl,
                Name = "CTRL Right"
            });
            L_Items.Add(new MT_Modifier
            {
                Id = MT_ModifierId.LShift,
                Name = "SHIFT Left"
            });
            L_Items.Add(new MT_Modifier
            {
                Id = MT_ModifierId.RShift,
                Name = "SHIFT Right"
            });
            L_Items.Add(new MT_Modifier
            {
                Id = MT_ModifierId.LAlt,
                Name = "ALT Left"
            });
            L_Items.Add(new MT_Modifier
            {
                Id = MT_ModifierId.RAlt,
                Name = "Alt Right"
            });
            L_Items.Add(new MT_Modifier
            {
                Id = MT_ModifierId.Caps,
                Name = "CAPS"
            });

            return L_Items;
        }
    }

    public class MT_InteractionType
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class MT_Interaction
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class MT_Modifier
    {
        public MT_ModifierId Id { get; set; }
        public string Name { get; set; }
    }

    public enum MT_ModifierId
    {
        None,
        LCtrl,
        RCtrl,
        LShift,
        RShift,
        LAlt,
        RAlt,
        Caps
    }
}
