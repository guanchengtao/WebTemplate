
using SDAU.GCT.OA.DAL;
using SDAU.GCT.OA.DALFactory;
using SDAU.GCT.OA.IBLL;
using SDAU.GCT.OA.IDAL;
using SDAU.GCT.OA.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;


namespace SDAU.GCT.OA.BLL
{










    public partial class UserInfoService : BaseService<UserInfo>, IUserInfoService
    {
        public override void GetCurrentDal()
        {
            CurrentDal = DbSession.UserInfoDal;
        }
    }


}