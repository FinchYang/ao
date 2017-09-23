namespace mvc104.models
{
    public enum businessstatus
    {
        unknown,
        wait,//身份证正面
        process,//身份证反面
        finish, //户口簿本人信息变更页  
        failure  
    };
}