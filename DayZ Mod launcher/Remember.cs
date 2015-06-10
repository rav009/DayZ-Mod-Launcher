using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace DayZ_Mod_Launcher
{
    public class Remember
    {
        public Remember()
        {

        }

        public bool HasRemember()
        {
            FileStream fs;
            try
            {
                fs = new FileStream("armapath.ini", FileMode.Open, FileAccess.Read);
                fs.Close();
            }
            catch
            {
                return false;
            }
            return true;
        }

        public void ReadRemember(ref string armapath, ref string armaoapath)
        {
            FileStream fs;
            try
            {
                fs = new FileStream("armapath.ini", FileMode.Open, FileAccess.Read);
                StreamReader m_streamReader = new StreamReader(fs);
                armapath = m_streamReader.ReadLine();
                armaoapath = m_streamReader.ReadLine();
                m_streamReader.Close();
                fs.Close();
            }
            catch
            {
                throw new FileLoadException();
            }
        }

        public void Write(string armapath, string armaoapath)
        {
            FileStream fs = new FileStream("armapath.ini", FileMode.Create, FileAccess.Write);
            StreamWriter sw = new StreamWriter(fs);
            sw.WriteLine(armapath);
            sw.WriteLine(armaoapath);
            sw.Flush();
            sw.Close();
            fs.Close();
        }


    }
}
