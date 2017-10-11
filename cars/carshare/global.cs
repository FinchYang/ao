using System;
using System.Collections.Generic;
using System.Text;

namespace carshare
{
    public static  class global
    {
        public static Dictionary<businessType, short> businesscount = new Dictionary<businessType, short>();
        public static Dictionary<businessType, string> statistics = new Dictionary<businessType, string>();
        static global()
        {
            statistics.Add(businessType.unknown, "合计");

            //statistics.Add(businessType.ChangeLicense, "变更户籍姓名");//变更户籍姓名
            //statistics.Add(businessType.delay, "延期换证");//延期换证?
            //statistics.Add(businessType.lost, "遗失补证");//遗失补证
            //statistics.Add(businessType.damage, "损毁换证");//损毁换证
            //statistics.Add(businessType.overage, "超龄换证");//超龄换证?

            //statistics.Add(businessType.expire, "期满换证");//期满换证??6,7??
            //statistics.Add(businessType.changeaddr, "变更户籍地址");//变更户籍地址
            //statistics.Add(businessType.basicinfo, "基本信息证明");//基本信息证明
            //statistics.Add(businessType.first, "驾驶证自愿业务退办");//初领、增加机动车驾驶证自愿业务退办
            //statistics.Add(businessType.network, "网约车安全驾驶证明");// 网约车安全驾驶证明

            //statistics.Add(businessType.three, "三年无重大事故证明");//三年无重大事故证明
            //statistics.Add(businessType.five, "五年安全驾驶证明");//五年安全驾驶证明

            //statistics.Add(businessType.inspectDelay, "延期审验");//延期审验
            //statistics.Add(businessType.bodyDelay, "延期提交身体证明");//延期提交身体证明
            //statistics.Add(businessType.changeContact, "变更联系方式");//变更联系方式

            businesscount.Add(businessType.unknown, 5);

            businesscount.Add(businessType.newplate, 5);//补领机动车号牌
            businesscount.Add(businessType.scrap, 0);//注销登记-车辆报废
            businesscount.Add(businessType.changeplate, 6);//换领机动车号牌
            businesscount.Add(businessType.newlicense, 6);//补领机动车行驶证
            businesscount.Add(businessType.changelicense, 7);//换领机动车行驶证

            businesscount.Add(businessType.newtag, 8);//补领检验合格标志
            businesscount.Add(businessType.changetag, 9);//换领检验合格标志
            businesscount.Add(businessType.changecontact, 0);//
        }
    }
}
