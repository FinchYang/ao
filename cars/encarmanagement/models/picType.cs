namespace mvc104.models
{
  
    public enum picType
    {
        unknown,

        declaration_sign,//申告义务签名
        face1,//人脸识别拍照一
        face2,//人脸识别拍照二
        id_front,//身份证正面
        id_back,//身份证反面

        originalPlate, //摄原机动车号牌
        vehicle_pic,//机动车照片
        vehicle_info,//机动车信息页
        originalLicense,//原机动车行驶证
        assurance,//交通事故责任强制保险凭证副本

        originalTag,//原机动车检验合格标志
    };
}