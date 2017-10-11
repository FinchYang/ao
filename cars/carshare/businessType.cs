using System.Collections.Generic;

namespace carshare
{
   
    public enum businessType
    {
         unknown,
        scrap,//注销登记-车辆报废

        newplate,//补领机动车号牌
        changeplate,//换领机动车号牌
        newlicense,//补领机动车行驶证
        changelicense,//换领机动车行驶证

        newtag,//补领检验合格标志
        changetag,//换领检验合格标志
        changecontact

    };
}