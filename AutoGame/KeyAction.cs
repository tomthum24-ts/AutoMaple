using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoGame
{
    public class KeyAction
    {
        public Keys Key { get; set; }           // phím nhấn
        public bool IsKeyDown { get; set; }     // true: nhấn, false: thả
        public TimeSpan TimeOffset { get; set; } // thời gian kể từ lúc bắt đầu ghi
    }
}
