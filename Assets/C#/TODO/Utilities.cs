using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace TD
{
    class Utilities
    {
        public static void ReadBlock(TextReader tr, Action<string> readAction)
        {
            tr.ReadLine();
            string line = tr.ReadLine();
            while (line != "end")
            {
                readAction(line);
                line = tr.ReadLine();
            }
        }
    }
}
