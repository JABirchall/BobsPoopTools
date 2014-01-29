using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LuaInterface;
using System.Collections;
using Profiles;
using System.Windows.Forms;
using System.Reflection;
using System.IO;

namespace test
{
    class lau
    {
        static Lua pLuaVM = null;
        static Hashtable pLuaFuncs = null;
        static Hashtable pLuaPackages = null;
        static XDevkit.IXboxConsole xbc;

        public lau(XDevkit.IXboxConsole Newxbc)
        {
            xbc = Newxbc;
            pLuaVM = new Lua();
            pLuaFuncs = new Hashtable();
            pLuaPackages = new Hashtable();
            registerLuaFunctions(null, this, null);
        }

        public static void registerLuaFunctions(String strPackage, Object pTarget, String strPkgDoc)
        {
            if (pLuaVM == null || pLuaFuncs == null || pLuaPackages == null)
                return;

            LuaPackageDescriptor pPkg = null;
            
            if (strPackage != null)
            {
                pLuaVM.DoString(strPackage + " = {}");
                pPkg = new LuaPackageDescriptor(strPackage, strPkgDoc);
            }

            Type pTrgType = pTarget.GetType();

            foreach (MethodInfo mInfo in pTrgType.GetMethods())
            {
                foreach (Attribute attr in Attribute.GetCustomAttributes(mInfo))
                {
                    if (attr.GetType() == typeof(AttrLuaFunc))
                    {
                        AttrLuaFunc pAttr = (AttrLuaFunc) attr;
                        ArrayList pParams = new ArrayList();
                        ArrayList pParamDocs = new ArrayList();
                        String strFName = pAttr.getFuncName();
                        String strFDoc = pAttr.getFuncDoc();
                        String[] pPrmDocs = pAttr.getFuncParams();
                        ParameterInfo[] pPrmInfo = mInfo.GetParameters();
                        if (pPrmDocs != null && (pPrmInfo.Length != pPrmDocs.Length))
                        {
                            Console.WriteLine("Function " + mInfo.Name + " (exported as " + strFName + ") argument number mismatch. Declared " + pPrmDocs.Length + " but requires " + pPrmInfo.Length + ".");
                            break;
                        }

                        for (int i = 0; i < pPrmInfo.Length; i++)
                        {
                            pParams.Add(pPrmInfo[i].Name);
                            //pParamDocs.Add(pPrmDocs[i]);
                        }

                        LuaFuncDescriptor pDesc = new LuaFuncDescriptor(strFName, strFDoc, pParams, pParamDocs);
                        if (pPkg != null)
                        {
                            pPkg.AddFunc(pDesc);
                            pLuaVM.RegisterFunction(strPackage + strFName, pTarget, mInfo);
                            pLuaVM.DoString(strPackage + "." + strFName + " = " + strPackage + strFName);
                            pLuaVM.DoString(strPackage + strFName + " = nil");
                        }
                        else
                        {
                            pLuaFuncs.Add(strFName, pDesc);
                            pLuaVM.RegisterFunction(strFName, pTarget, mInfo);
                        }
                    }
                }
            }

            if (pPkg != null)
                pLuaPackages.Add(strPackage, pPkg);
        }

        [AttrLuaFunc("LauSendFriendRequest", "to SendFriendRequest.")]
        public void LauSendFriendRequest(ulong XUID)
        {
            if (Form1.IsDevKit == true)
            {
                ulong Xuid = 0L;
                DevUsers.GetXuidFromIndex(xbc, 0, out Xuid);
                DevUsers.SendFriendRequest(xbc, Xuid, XUID);
            }
        }

        [AttrLuaFunc("LauJoinParty", "JoinParty.")]
        public void LauJoinParty(ulong XUID)
        {
            if(Form1.IsDevKit == true)
                DevUsers.JoinParty(xbc, 0, XUID);
        }


        public void runSendFriendRequest()
        {
            if (File.Exists(@"lua\SendFriendRequest.lua") == false)
                MessageBox.Show("no 'SendFriendRequest.lua'");
            else
                pLuaVM.DoFile(@"lua\SendFriendRequest.lua");
        }

        public void runJoinParty()
        {
            if (File.Exists(@"lua\JoinParty.lua") == false)
                MessageBox.Show("no 'JoinParty.lua'");
            else
                pLuaVM.DoFile(@"lua\JoinParty.lua");
        }

        public void run()
        {
            if (File.Exists(@"lua\script.lua") == false)
                MessageBox.Show("no 'script.lua'");
            else
                pLuaVM.DoFile(@"lua\script.lua");
        }
    }
}
