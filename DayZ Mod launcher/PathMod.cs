using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace DayZ_Mod_Launcher
{
    public class PathMod
    {
        public readonly string _armapath;

        public readonly string _oapath;

        public readonly string _arma2oaexename = "ArmA2OA.exe";

        public readonly string _arma2exename = "arma2.exe";

        public readonly string _arma2oaexebename = "ArmA2OA_BE.exe";

        private string dayzmodname = "@DayZ";

        public PathMod(string armapath, string oapath)
        {
            if(!armapath.EndsWith("\\"))
            {
                armapath += "\\";
            }
            if (!oapath.EndsWith("\\"))
            {
                oapath += "\\";
            }
            _armapath = armapath;
            _oapath = oapath;
        }

        public bool TestPath(string path, string exename)
        {
            System.IO.DirectoryInfo dir = new DirectoryInfo(path);
            if (dir.Exists)
            {
                FileInfo[] fiList = dir.GetFiles();
                List<string> _pfs = new List<string>();
                foreach(var f in fiList)
                {
                    _pfs.Add(f.Name.ToLower());
                }
                return _pfs.Contains(exename.ToLower());
            }
            else
            {
                return false;
            }
        }

        public List<string> LoadModNames()
        {
            System.IO.DirectoryInfo dir = new DirectoryInfo(this._oapath);
            if (dir.Exists)
            {
                DirectoryInfo[] fiList = dir.GetDirectories();
                List<string> _fns = new List<string>();
                foreach (var f in fiList)
                {
                    if (f.Name.StartsWith("@") && f.Name != "@DayZ")
                    {
                        _fns.Add(f.Name.Substring(1));
                    }
                }
                return _fns;
            }
            else
            {
                return null;
            }
        }


        public string[] GetRunCmd(List<string> mods, bool? BE)
        {
            string exepath = "";
            string modparam = "";
            if (BE == true)
            {
                exepath = this._oapath  + this._arma2oaexebename;
                modparam = "0 0 \"-mod=" + this._armapath + ";Expansion;";
            }
            else
            {
                exepath = this._oapath + this._arma2oaexename;
                modparam = "\"-mod=" + this._armapath + ";Expansion;";
            }
            foreach(string mod in mods)
            {
                modparam += this._oapath + @"@" + mod.Trim() + ";";
            }
            modparam += "\"";
            string[] rs = new string[2];
            rs[0] = exepath;
            rs[1] = modparam;
            return rs;
        }
    }
}
