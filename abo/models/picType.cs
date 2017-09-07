namespace mvc104.models
{
    public enum picType
    {
        id_front,//身份证正面
         id_back,//身份证反面
          id_inhand, delay_pic, driver,
        health,//机动车驾驶人身体条件证明
         overage, expire,
         hukou_pic, //户口簿本人信息变更页
        sign_pic, //驾驶证遗失声明
        declaration_sign,//申告义务签名
        face1,//人脸识别拍照一
        face2,//人脸识别拍照二
        face3,
        passport//护照
        ,visa//签证
        ,serviceNote//入伍通知书
    };
}