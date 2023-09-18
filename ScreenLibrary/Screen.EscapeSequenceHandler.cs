using System;
using System.Drawing;

namespace Terminal.ScreenLibrary
{
    public partial class Screen
    {
        // See https://www.man7.org/linux/man-pages/man4/console_codes.4.html
        // for the meaning of the following escape sequences.

        // We now only support color setting escape sequences, and other escape sequences
        // are ignored. A color setting escape sequence is of the form ESC [ ... m or
        // CSI ... m, where ... is a sequence of parameters separated by semicolons.

        // The first number of ... could be as follows:
        //        0          reset all attributes to their defaults
        //        30         set black foreground
        //        31         set red foreground
        //        32         set green foreground
        //        33         set brown foreground
        //        34         set blue foreground
        //        35         set magenta foreground
        //        36         set cyan foreground
        //        37         set white foreground
        //        38         256/24-bit foreground color follows, shoehorned into 16 basic colors
        //        40         set black background
        //        41         set red background
        //        42         set green background
        //        43         set brown background
        //        44         set blue background
        //        45         set magenta background
        //        46         set cyan background
        //        47         set white background
        //        48         256/24-bit background color follows, shoehorned into 8 basic colors
        //        90..97     set foreground to bright versions of 30..37
        //        100..107   set background, same as 40..47 (bright not supported)

        // Commands 38 and 48 require further arguments:
        // ;5;x       256 color: values 0..15 are IBGR(black, red, green,
        //            ... white), 16..231 a 6x6x6 color cube, 232..255 a
        //            grayscale ramp
        // ;2;r;g;b   24-bit color, r/g/b components are in the range 0..255

        internal class EscapeSequenceHandler
        {
            public bool HandleSuccessfully { get; private set; } = false;

            // This screen object reference is used to execute commands
            readonly Screen screen;

            // status:
            // 0: ESC- but not CSI-sequence has just been received
            // 1: CSI-sequence has just been received, or ESC [ has just been received
            // 2: Argument(s) of a CSI-sequence is being received
            int status;

            // At most 16 csi arguments are supported
            // An empty or absent parameter is taken to be 0
            readonly int[] csiArgs = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            int csiArgCount = 0;

            public EscapeSequenceHandler(char beginChar, Screen screen)
            {
                if (beginChar == '\x1b')
                    status = 0;
                else if (beginChar == '\x9b')
                    status = 1;
                else
                    throw new ArgumentException("beginChar must be either ESC or CSI");
                this.screen = screen;
            }

            /// <returns>
            /// Return true if the escape sequence is complete.
            /// </returns>
            public bool Continue(char c)
            {
                bool complete = false;
                // A status machine is used to parse escape sequences.
                switch (status)
                {
                    case 0:
                        // ESC- but not CSI-sequence has just been received
                        // THIS IS NOT IMPLEMENTED YET
                        switch (c)
                        {
                            case '[':
                                status = 1;
                                break;
                            default:
                                complete = true; // Unsupported escape sequence
                                break;
                        }
                        break;

                    case 1:
                        // CSI-sequence has just been received, or ESC [ has just been received
                        if (c >= '0' && c <= '9')
                        {
                            status = 2;
                            csiArgCount = 1;
                            csiArgs[0] = c - '0';
                        }
                        else
                            complete = true; // Illegal input
                        break;

                    case 2:
                        // Argument(s) of a CSI-sequence is being received
                        switch (c)
                        {
                            case '0':
                            case '1':
                            case '2':
                            case '3':
                            case '4':
                            case '5':
                            case '6':
                            case '7':
                            case '8':
                            case '9':
                                csiArgs[csiArgCount - 1] *= 10;
                                csiArgs[csiArgCount - 1] += c - '0';
                                break;

                            case ';':
                                if (csiArgCount >= 16)
                                {
                                    complete = true; // Illegal input
                                    break;
                                }
                                csiArgCount++;
                                csiArgs[csiArgCount - 1] = 0;
                                break;

                            case 'm':
                                Execute_m();
                                complete = true; // Escape sequence complete
                                break;

                            default:
                                complete = true; // Illegal input
                                break;
                        }
                        break;
                }
                return complete;
            }

            void Execute_m()
            {
                switch (csiArgs[0])
                {
                    case 0:
                        screen.ResetColor();
                        break;

                    case 30:
                        screen.SetForegroundColor(Color.Black);
                        break;

                    case 31:
                        screen.SetForegroundColor(Color.Red);
                        break;

                    case 32:
                        screen.SetForegroundColor(Color.Green);
                        break;

                    case 33:
                        screen.SetForegroundColor(Color.Brown);
                        break;

                    case 34:
                        screen.SetForegroundColor(Color.Blue);
                        break;

                    case 35:
                        screen.SetForegroundColor(Color.Magenta);
                        break;

                    case 36:
                        screen.SetForegroundColor(Color.Cyan);
                        break;

                    case 37:
                        screen.SetForegroundColor(Color.White);
                        break;

                    case 38:
                        screen.SetForegroundColor(GetColorFor38And48());
                        break;

                    case 40:
                        screen.SetBackgroundColor(Color.Black);
                        break;

                    case 41:
                        screen.SetBackgroundColor(Color.Red);
                        break;

                    case 42:
                        screen.SetBackgroundColor(Color.Green);
                        break;

                    case 43:
                        screen.SetBackgroundColor(Color.Brown);
                        break;

                    case 44:
                        screen.SetBackgroundColor(Color.Blue);
                        break;

                    case 45:
                        screen.SetBackgroundColor(Color.Magenta);
                        break;

                    case 46:
                        screen.SetBackgroundColor(Color.Cyan);
                        break;

                    case 47:
                        screen.SetBackgroundColor(Color.White);
                        break;

                    case 48:
                        screen.SetBackgroundColor(GetColorFor38And48());
                        break;

                    case 90:
                        screen.SetForegroundColor(Color.Gray);
                        break;

                    case 91:
                        screen.SetForegroundColor(Color.Red);
                        break;

                    case 92:
                        screen.SetForegroundColor(Color.Green);
                        break;

                    case 93:
                        screen.SetForegroundColor(Color.Brown);
                        break;

                    case 94:
                        screen.SetForegroundColor(Color.Blue);
                        break;

                    case 95:
                        screen.SetForegroundColor(Color.Magenta);
                        break;

                    case 96:
                        screen.SetForegroundColor(Color.Cyan);
                        break;

                    case 97:
                        screen.SetForegroundColor(Color.White);
                        break;

                    case 100:
                        screen.SetBackgroundColor(Color.Gray);
                        break;

                    case 101:
                        screen.SetBackgroundColor(Color.Red);
                        break;

                    case 102:
                        screen.SetBackgroundColor(Color.Green);
                        break;

                    case 103:
                        screen.SetBackgroundColor(Color.Brown);
                        break;

                    case 104:
                        screen.SetBackgroundColor(Color.Blue);
                        break;

                    case 105:
                        screen.SetBackgroundColor(Color.Magenta);
                        break;

                    case 106:
                        screen.SetBackgroundColor(Color.Cyan);
                        break;

                    case 107:
                        screen.SetBackgroundColor(Color.White);
                        break;
                }

                Color GetColorFor38And48()
                {
                    Color tempColor;
                    if (csiArgs[1] == 5) // 256 color
                    {
                        if (csiArgs[2] < 16) // values 0..15 are IBGR (black, red, green, ... white)
                            switch (csiArgs[2])
                            {
                                case 0: tempColor = Color.Black; break;
                                case 1: tempColor = Color.Red; break;
                                case 2: tempColor = Color.Green; break;
                                case 3: tempColor = Color.Brown; break;
                                case 4: tempColor = Color.Blue; break;
                                case 5: tempColor = Color.Magenta; break;
                                case 6: tempColor = Color.Cyan; break;
                                case 7: tempColor = Color.White; break;
                                case 8: tempColor = Color.Gray; break;
                                case 9: tempColor = Color.Red; break;
                                case 10: tempColor = Color.Green; break;
                                case 11: tempColor = Color.Yellow; break;
                                case 12: tempColor = Color.Blue; break;
                                case 13: tempColor = Color.Magenta; break;
                                case 14: tempColor = Color.Cyan; break;
                                case 15: tempColor = Color.White; break;
                            }
                        else if (csiArgs[2] < 232) // 16..231 a 6x6x6 color cube
                        {
                            int r = (csiArgs[2] - 16) / 36;
                            int g = ((csiArgs[2] - 16) % 36) / 6;
                            int b = (csiArgs[2] - 16) % 6;
                            tempColor = Color.FromArgb(r * 51, g * 51, b * 51);
                        }
                        else if (csiArgs[2] < 256) // 232..255 a grayscale ramp
                        {
                            int gray = (csiArgs[2] - 232) * 10 + 8;
                            tempColor = Color.FromArgb(gray, gray, gray);
                        }
                    }
                    else if (csiArgs[1] == 2) // 24-bit color
                        tempColor = Color.FromArgb(csiArgs[2], csiArgs[3], csiArgs[4]);
                    return tempColor;
                }
            }
        }
    }
}
