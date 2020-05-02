 
using SDAU.GCT.OA.DAL;
using SDAU.GCT.OA.IDAL;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SDAU.GCT.OA.DALFactory
{
    public partial class StaticDalFactory
    {
        public static string assemblyname = System.Configuration.ConfigurationManager.AppSettings["DalAssemblyName"];
	  
		
	 
	  
	
	
	
		  public static IUserInfoDal getUserInfoDal()
        {      
        return Assembly.Load(assemblyname).CreateInstance(assemblyname+".UserInfoDal") as IUserInfoDal;
        }
	  
}
}