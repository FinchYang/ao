namespace mvc104.models
{
    public enum businessType
    {
         unknown,
        ChangeLicense,//变更户籍姓名

        delay,//延期换证
        lost,//遗失补证
        damage,//损毁换证
        overage,//超龄换证

        expire,//期满换证
        changeaddr,//变更户籍地址
        basicinfo,//基本信息证明
        first,//初领、增加机动车驾驶证自愿业务退办
        network,// 网约车安全驾驶证明

        three,//三年无重大事故证明
        five,//五年安全驾驶证明
        inspectDelay//延期审验
, bodyDelay//延期提交身体证明
, changeContact//变更联系方式

    };
}