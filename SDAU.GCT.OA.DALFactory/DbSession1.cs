 
using SDAU.GCT.OA.DAL;
using SDAU.GCT.OA.IDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDAU.GCT.OA.DALFactory
{
   public partial class DbSession:IDbSession
    { 
	  
	
	public IUserInfoDal UserInfoDal { get
            {
                return StaticDalFactory.getUserInfoDal();
            }
        }
}
}